package jp.funx.backendlessunitysdk;

import jp.funx.backendlessunitysdk.pushnotifications.Messaging;
import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;

public class ExtendedUnityPlayerActivity extends UnityPlayerActivity {
	public static final String UNITY_PREFS_FILE_SUFFIX = ".v2.playerprefs";

	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Messaging.setContext(this);
	}

	public void registerDevice(String GCMSenderID, long expiration) {
		Messaging.registerDevice(GCMSenderID, expiration);
	}

	public void setUnityGameObject(String gameObject) {
		Messaging.setUnityGameObject(gameObject);
	}

	public void unregisterDevice() {
		Messaging.unregisterDevice();
	}
}
