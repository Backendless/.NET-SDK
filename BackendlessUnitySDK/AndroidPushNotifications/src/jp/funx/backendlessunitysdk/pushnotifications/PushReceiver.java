package jp.funx.backendlessunitysdk.pushnotifications;

import jp.funx.backendlessunitysdk.pushnotifications.BackendlessBroadcastReceiver;
import jp.funx.backendlessunitysdk.pushnotifications.Messaging;
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