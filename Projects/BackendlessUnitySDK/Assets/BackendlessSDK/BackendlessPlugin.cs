/*
Copyright 2017 Backendless Corporation. and FUNX Corporation., All Rights Reserved.

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

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using BackendlessAPI;
using BackendlessAPI.Engine;
using Weborb.Client;
using Weborb.Util;

public class BackendlessPlugin : MonoBehaviour
{
#if ENABLE_PUSH_PLUGIN
#if UNITY_ANDROID
    private static AndroidJavaObject activity = null;
#elif UNITY_IOS || UNITY_TVOS 
    [System.Runtime.InteropServices.DllImport("__Internal")]
    extern static public void setListenerGameObject(string listenerName);
    [System.Runtime.InteropServices.DllImport("__Internal")]
    extern static public void registerForRemoteNotifications();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    extern static public void unregisterForRemoteNotifications();
#endif
#endif
    
    private static BackendlessPlugin _instance = null;
    public static BackendlessPlugin Instance {
        get {
            return _instance;
        }
    }
    
    public enum ENVIRONMENT {
        dev,
        prod
    }

    [SerializeField]
    private ENVIRONMENT version;

    public static string sVersion = ENVIRONMENT.dev.ToString();
    
    [SerializeField]
    private string ProdApplicationId;
     
    [SerializeField]
    private string ProdWindowsPhoneKey;

    [SerializeField]
    private string DevApplicationId;

    [SerializeField]
    private string DevWindowsPhoneKey;

    public string applicationId {
        get {
            if (version == ENVIRONMENT.prod)
                return ProdApplicationId;
             else
                return DevApplicationId;
            }
        set {}
    }

    public string WindowsPhoneKey {
        get {
            if (version == ENVIRONMENT.prod)
                return ProdWindowsPhoneKey;
             else
                return DevWindowsPhoneKey;
            }
        set {}
    }
                    
    void Awake ()
    {
        sVersion = version.ToString();

        DontDestroyOnLoad (this);
        if (_instance == null) {
            _instance = this;
        } else {
            DestroyImmediate (this.gameObject); // avoid duplicate BackendlessPlugin GameObjects
            return;
        }

#if UNITY_ANDROID
        /* The following is needed to avoid the following errors on Android when using HTTPS.
        The authentication or decryption has failed.
        */
        ServicePointManager.ServerCertificateValidationCallback = (p1, p2, p3, p4) => true;
#endif
        Backendless.URL = "https://api.backendless.com";

        // Initialize Backendless
        Backendless.InitApp (applicationId, WindowsPhoneKey);

        // Default network timeout (this must be set after Backendless.InitApp)
        Backendless.Timeout = 30000; // 30 secs

//        Backendless.Data.MapTableToType("Devices", typeof(/* type of a class that models one of your tables on Backendless and inherits Backendless Entity */));

#if ENABLE_PUSH_PLUGIN
        Backendless.Messaging.SetUnityRegisterDevice(UnityRegisterDevice, UnityUnregisterDevice);
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("setUnityGameObject", this.gameObject.name);
#elif (UNITY_IOS || UNITY_TVOS ) && !UNITY_EDITOR
        setListenerGameObject(this.gameObject.name);
#endif
#endif

#if UNITY_IOS || UNITY_TVOS  || UNITY_ANDROID
        /* In order to use the .NET SDK we needed some hardcoded logic here to make sure some of its code was properly compiled for AOT on iOS.
        We'd get errors like the following when trying to delete the a record from a table.

        Error: Code = , Message = Attempting to call method 'Weborb.Client.HttpEngine::SendRequest<System.Int64>' for which no ahead of time (AOT) code was generated., Detail = 
        UnityEngine.DebugLogHandler:Internal_Log(LogType, String, Object)
        UnityEngine.DebugLogHandler:LogFormat(LogType, Object, String, Object[])
        UnityEngine.Logger:Log(LogType, Object)
        UnityEngine.Debug:LogError(Object)
        <FinalizeSellItem>c__AnonStoreyA5:<>m__C1(BackendlessFault)
        BackendlessAPI.Async.ErrorHandler:Invoke(BackendlessFault)
        BackendlessAPI.Engine.Invoker:InvokeAsync(String, String, Object[], Boolean, AsyncCallback`1)
        BackendlessAPI.Engine.Invoker:InvokeAsync(String, String, Object[], AsyncCallback`1)
        BackendlessAPI.Service.PersistenceService:Remove(T, AsyncCallback`1)
        BackendlessAPI.Data.DataStoreImpl`1:Remove(T, AsyncCallback`1)
        <FinalizeSellItem>c__AnonStoreyA5:<>m__BF()
        System.Action:Invoke()
        Loom:Update()
         */

        try {
            IdInfo idInfo = new IdInfo();
            HttpEngine httpEngine = new Weborb.Client.HttpEngine("http://api.backendless.com", idInfo);
            httpEngine.SendRequest<Boolean>(null, null, null, null, null);
            httpEngine.SendRequest<Int64>(null, null, null, null, null);
        } catch (Exception e) {
            // ignore
        }
#endif
    }
    
#if ENABLE_PUSH_PLUGIN
    public static void UnityRegisterDevice(string GCMSenderID, List<String> channels, DateTime? timestamp)
    {
#if UNITY_ANDROID
        DateTime expiration = (timestamp == null ? new DateTime(0) : (DateTime) timestamp);
        activity.Call("registerDevice", GCMSenderID, expiration.Ticks);
#elif UNITY_IOS
        registerForRemoteNotifications();
#endif
    }
    
    public static void UnityUnregisterDevice()
    {
#if UNITY_ANDROID
        activity.Call("unregisterDevice");
#elif UNITY_IOS
        unregisterForRemoteNotifications();
#endif
    }
    
    void setDeviceToken(string deviceToken)
    {
        Backendless.Messaging.DeviceRegistration.DeviceToken = deviceToken;
    }
    
    void setDeviceId(string deviceId)
    {
        Backendless.Messaging.DeviceRegistration.DeviceId = deviceId;
    }
    
    void setOs(string os)
    {
        Backendless.Messaging.DeviceRegistration.Os = os;
    }
    
    void setOsVersion(string osVersion)
    {
        Backendless.Messaging.DeviceRegistration.OsVersion = osVersion;
    }
    
    void setExpiration(long expiration)
    {
        Backendless.Messaging.DeviceRegistration.Expiration = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(expiration);
    }
    
    void registerDeviceOnServer(string dummy)
    {
        Debug.Log("registerDeviceOnServer()");
        Backendless.Messaging.RegisterDeviceOnServer();
    }
    
    void unregisterDeviceOnServer(string dummy)
    {
        Debug.Log("unregisterDeviceOnServer()");
        Backendless.Messaging.UnregisterDeviceOnServer();
    }
    
    public void onDidFailToRegisterForRemoteNotificationsWithError(string error)
    {
        Debug.LogError("onDidFailToRegisterForRemoteNotificationsWithError() error=" + error);
    }
    
    void onPushMessage(string message)
    {
        Debug.Log("onPushMessage() message=" + message);
    }
#endif
}
