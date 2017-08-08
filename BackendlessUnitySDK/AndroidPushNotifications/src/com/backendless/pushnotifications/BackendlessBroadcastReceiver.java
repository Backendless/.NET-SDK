/*
Copyright 2015 Backendless Corp. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */

package com.backendless.pushnotifications;

import java.util.Random;
import java.util.concurrent.TimeUnit;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.media.Ringtone;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.PowerManager;
import android.os.SystemClock;
import android.util.Log;
import android.widget.RemoteViews;

public class BackendlessBroadcastReceiver extends BroadcastReceiver
{
	private static final String TAG = "BackendlessBroadcastReceiver";
	private static final Random random = new Random();

	private static final int MAX_BACKOFF_MS = (int) TimeUnit.SECONDS.toMillis( 3600 );

	private static final String TOKEN = Long.toBinaryString( random.nextLong() );
	private static final String EXTRA_TOKEN = "token";

	private static final String WAKELOCK_KEY = "GCM_LIB";
	private static PowerManager.WakeLock wakeLock;
	private static final Object LOCK = BackendlessBroadcastReceiver.class;

	//Fields are placed here because this class is most strongly referenced by android
	private static String persistedSenderId;
	private static long persistedRegistrationExpiration;
	private static int customLayout;
	private static int customLayoutTitle;
	private static int customLayoutDescription;
	private static int customLayoutImageContainer;
	private static int customLayoutImage;

	private static int notificationId = 1;

	public final static String ANDROID_TICKER_TEXT_TAG = "android-ticker-text";
	public final static String ANDROID_CONTENT_TITLE_TAG = "android-content-title";
	public final static String ANDROID_CONTENT_TEXT_TAG = "android-content-text";
	public final static String ANDROID_ACTION_TAG = "android-action";

	public BackendlessBroadcastReceiver()
	{
	}

	public BackendlessBroadcastReceiver( String senderId )
	{
		BackendlessBroadcastReceiver.persistedSenderId = senderId;
	}

	protected static void setSenderId( String senderId )
	{
		BackendlessBroadcastReceiver.persistedSenderId = senderId;
	}

	protected static void setRegistrationExpiration( long registrationExpiration )
	{
		BackendlessBroadcastReceiver.persistedRegistrationExpiration = registrationExpiration;
	}

	private static String getSenderId()
	{
		return persistedSenderId;
	}

	private static long getRegistrationExpiration()
	{
		if( persistedRegistrationExpiration == 0 )
			return System.currentTimeMillis() + GCMRegistrar.DEFAULT_ON_SERVER_LIFESPAN_MS;

		return persistedRegistrationExpiration;
	}

	@Override
	public final void onReceive( Context context, Intent intent )
	{
		synchronized( LOCK )
		{
			if( wakeLock == null )
			{
				PowerManager pm = (PowerManager) context.getSystemService( Context.POWER_SERVICE );
				wakeLock = pm.newWakeLock( PowerManager.PARTIAL_WAKE_LOCK, WAKELOCK_KEY );
			}
		}

		wakeLock.acquire();
		this.handleIntent( context, intent );
		setResult( Activity.RESULT_OK, null, null );
	}

	//API block
	public void onRegistered( Context context, String registrationId )
	{
	}

	public void onUnregistered( Context context, Boolean unregistered )
	{
	}

	public boolean onMessage( Context context, Intent intent )
	{
		return true;
	}

	public void onError( Context context, String message )
	{
		throw new RuntimeException( message );
	}

	//Internal block
	private void handleIntent( Context context, Intent intent )
	{
		try
		{
			String action = intent.getAction();

			if( action.equals( GCMConstants.INTENT_FROM_GCM_REGISTRATION_CALLBACK ) )
				handleRegistration( context, intent );
			else if( action.equals( GCMConstants.INTENT_FROM_GCM_MESSAGE ) )
				handleMessage( context, intent );
			else if( action.equals( GCMConstants.INTENT_FROM_GCM_LIBRARY_RETRY ) )
			{
				String token = intent.getStringExtra( EXTRA_TOKEN );
				if( !TOKEN.equals( token ) )
					return;
				// retry last call
				if( GCMRegistrar.isRegistered( context ) )
					GCMRegistrar.internalUnregister( context );
				else
					GCMRegistrar.internalRegister( context, getSenderId() );
			}
		}
		finally
		{
			synchronized( LOCK )
			{
				if( wakeLock != null )
					wakeLock.release();
			}
		}
	}

	public static void registerResources( Context context )
	{
		customLayout = context.getResources().getIdentifier( "notification", "layout", context.getPackageName() );
		customLayoutTitle = context.getResources().getIdentifier( "title", "id", context.getPackageName() );
		customLayoutDescription = context.getResources().getIdentifier( "text", "id", context.getPackageName() );
		customLayoutImageContainer = context.getResources().getIdentifier( "image", "id", context.getPackageName() );
		customLayoutImage = context.getResources().getIdentifier( "push_icon", "drawable", context.getPackageName() );
	}

	private void handleMessage( final Context context, Intent intent )
	{
		try
		{
			boolean showPushNotification = onMessage( context, intent );

			if( showPushNotification )
			{
				CharSequence tickerText = intent.getStringExtra( ANDROID_TICKER_TEXT_TAG );
				CharSequence contentTitle = intent.getStringExtra( ANDROID_CONTENT_TITLE_TAG );
				CharSequence contentText = intent.getStringExtra( ANDROID_CONTENT_TEXT_TAG );

				try {
					Uri notification = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
					Ringtone r = RingtoneManager.getRingtone(context, notification);
					r.play();
				} catch (Exception e) {
					e.printStackTrace();
				}

				if( tickerText != null && tickerText.length() > 0 )
				{
					Resources res = context.getResources();
					int appIcon = res.getIdentifier("a_notification_icon", "drawable", context.getPackageName());
					if( appIcon == 0 )
						appIcon = android.R.drawable.sym_def_app_icon;

					Intent notificationIntent = context.getPackageManager().getLaunchIntentForPackage( context.getApplicationInfo().packageName );
					PendingIntent contentIntent = PendingIntent.getActivity( context, 0, notificationIntent, 0 );
					Notification notification = new Notification( appIcon, tickerText, System.currentTimeMillis() );
					notification.flags |= Notification.FLAG_AUTO_CANCEL;
					notification.setLatestEventInfo( context, contentTitle, contentText, contentIntent );

					registerResources( context );
					if( customLayout > 0 && customLayoutTitle > 0 && customLayoutDescription > 0 && customLayoutImageContainer > 0 )
					{
						NotificationLookAndFeel lookAndFeel = new NotificationLookAndFeel();
						lookAndFeel.extractColors( context );
						RemoteViews contentView = new RemoteViews( context.getPackageName(), customLayout );
						contentView.setTextViewText( customLayoutTitle, contentTitle );
						contentView.setTextViewText( customLayoutDescription, contentText );
						contentView.setTextColor( customLayoutTitle, lookAndFeel.getTextColor() );
						contentView.setFloat( customLayoutTitle, "setTextSize", lookAndFeel.getTextSize() );
						contentView.setTextColor( customLayoutDescription, lookAndFeel.getTextColor() );
						contentView.setFloat( customLayoutDescription, "setTextSize", lookAndFeel.getTextSize() );
						contentView.setImageViewResource( customLayoutImageContainer, customLayoutImage );
						notification.contentView = contentView;
					}

					NotificationManager notificationManager = (NotificationManager) context.getSystemService( Context.NOTIFICATION_SERVICE );
					notificationManager.notify( notificationId++, notification );
				}
			}
		}
		catch( Throwable throwable )
		{
			Log.e( TAG, "Error processing push notification", throwable );
		}
	}

	private void handleRegistration( final Context context, Intent intent )
	{
		String registrationId = intent.getStringExtra( GCMConstants.EXTRA_REGISTRATION_ID );
		String error = intent.getStringExtra( GCMConstants.EXTRA_ERROR );
		String unregistered = intent.getStringExtra( GCMConstants.EXTRA_UNREGISTERED );
		boolean isInternal = intent.getBooleanExtra( GCMConstants.EXTRA_IS_INTERNAL, false );

		// registration succeeded
		if( registrationId != null )
		{
			if( isInternal )
			{
				onRegistered( context, registrationId );
				return;
			}

			GCMRegistrar.resetBackoff( context );
			GCMRegistrar.setGCMdeviceToken( context, registrationId );
			registerFurther( context, registrationId );
			return;
		}

		// unregistration succeeded
		if( unregistered != null )
		{
			// Remember we are unregistered
			GCMRegistrar.resetBackoff( context );
			GCMRegistrar.setGCMdeviceToken( context, "" );
			unregisterFurther( context );
			return;
		}

		// Registration failed
		if( error.equals( GCMConstants.ERROR_SERVICE_NOT_AVAILABLE ) )
		{
			int backoffTimeMs = GCMRegistrar.getBackoff( context );
			int nextAttempt = backoffTimeMs / 2 + random.nextInt( backoffTimeMs );
			Intent retryIntent = new Intent( GCMConstants.INTENT_FROM_GCM_LIBRARY_RETRY );
			retryIntent.putExtra( EXTRA_TOKEN, TOKEN );
			PendingIntent retryPendingIntent = PendingIntent.getBroadcast( context, 0, retryIntent, 0 );
			AlarmManager am = (AlarmManager) context.getSystemService( Context.ALARM_SERVICE );
			am.set( AlarmManager.ELAPSED_REALTIME, SystemClock.elapsedRealtime() + nextAttempt, retryPendingIntent );
			// Next retry should wait longer.
			if( backoffTimeMs < MAX_BACKOFF_MS )
				GCMRegistrar.setBackoff( context, backoffTimeMs * 2 );
		}
		else
		{
			onError( context, error );
		}
	}

	private void registerFurther( final Context context, String GCMregistrationId )
	{
		Messaging.registerDeviceOnServer(GCMregistrationId, getRegistrationExpiration());

		GCMRegistrar.setRegistrationId(context, GCMregistrationId, getRegistrationExpiration());
		onRegistered(context, GCMregistrationId);
	}

	private void unregisterFurther( final Context context )
	{
		Messaging.unregisterDeviceOnServer();
		GCMRegistrar.setRegistrationId(context, "", 0);
		onUnregistered(context, true);
	}
}
