/*
Copyright 2015 Acrodea, Inc. All Rights Reserved.

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

package jp.funx.backendlessunitysdk.pushnotifications;

import java.net.InetAddress;
import java.net.NetworkInterface;
import java.util.*;

import com.unity3d.player.UnityPlayer;

import android.content.Context;
import android.os.Build;
import android.util.Log;

public class Messaging {
	private final static String DEVICE_ID;
	private final static String OS;
	private final static String OS_VERSION;
	private static String UNITY_GAMEOBJECT;
	private static Context m_context;
	
	static {
		String id = null;
		id = Build.SERIAL;
		OS_VERSION = String.valueOf(Build.VERSION.SDK_INT);
		OS = "ANDROID";

		if (id == null || id.equals("") || id.equals("unknown"))
			try {
				InetAddress inetAddress = InetAddress.getLocalHost();
				id = UUID.nameUUIDFromBytes(NetworkInterface.getByInetAddress(inetAddress).getHardwareAddress()).toString();
			} catch (Exception e) {
				StringBuilder builder = new StringBuilder();
				builder.append(System.getProperty("os.name"));
				builder.append(System.getProperty("os.arch"));
				builder.append(System.getProperty("os.version"));
				builder.append(System.getProperty("user.name"));
				builder.append(System.getProperty("java.home"));
				id = UUID.nameUUIDFromBytes(builder.toString().getBytes()).toString();
			}
		
		DEVICE_ID = id;
	}

	public static void setContext(Context context) {
		m_context = context;
	}

	
	public static void registerDevice(String GCMSenderID, long expiration) {
		final String fGCMSenderID = GCMSenderID;
		Date _expiration = null;
		if(expiration != 0) {
			_expiration = new Date();
			_expiration.setTime(expiration);
		}
		
		final Date fExpiration = _expiration;
		
		new Thread(new Runnable() {

			@Override
			public void run() {
				GCMRegistrar.checkDevice(m_context);
				GCMRegistrar.checkManifest(m_context);
				GCMRegistrar.register(m_context, fGCMSenderID, fExpiration);
			}
			
		}).start();
	}
	
	public static void unregisterDevice() {
		new Thread(new Runnable() {

			@Override
			public void run() {
				GCMRegistrar.unregister( m_context );
			}
			
		}).start();
	}
	
	public static String getDeviceID() {
		return DEVICE_ID;
	}
	
	public static void setUnityGameObject(String gameObject) {
		UNITY_GAMEOBJECT = gameObject;
		UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "setDeviceId", DEVICE_ID);
	}
	
	public static void registerDeviceOnServer(String deviceToken, long expiration) {
		try {
			UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "setDeviceToken", deviceToken);
			UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "setOs", OS);
			UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "setOsVersion", OS_VERSION);
			UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "registerDeviceOnServer", "");
		} catch (UnsatisfiedLinkError e) {
			Log.w("BackendlessUnitySDK", "Not registerDeviceOnServer for Push Notification to Unity layer because app isn't loaded");
		}
	}
	
	public static void unregisterDeviceOnServer() {
		UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "unregisterDeviceOnServer", "");
	}
	
	public static void onPushMessage(String message){
		try {
			UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, "onPushMessage", message);
		} catch (UnsatisfiedLinkError e) {
			Log.w("BackendlessUnitySDK", "Not relaying Push Notification to Unity layer because app isn't loaded");
		}
	}
}
