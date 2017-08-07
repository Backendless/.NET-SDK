package com.backendless.pushnotifications;

import com.backendless.pushnotifications.BackendlessBroadcastReceiver;
import com.backendless.pushnotifications.Messaging;

import android.content.Context;
import android.content.Intent;
import android.util.Log;

public class PushReceiver extends BackendlessBroadcastReceiver
{
	public final static String MESSAGE_TAG = "message";

	@Override
	public boolean onMessage( Context context, Intent intent )
	{
		String message = intent.getStringExtra( MESSAGE_TAG );
		Messaging.onPushMessage(message);
		return true;
	}

	@Override
	public void onError( Context context, String messageError )
	{
	}
}