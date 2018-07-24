Unity SDK for Backendless mBaaS
===============================

This is an unofficial Backendless SDK for Unity based off of their .NET SDK. It has been has been tested with Unity 2017.1.0p1 and Backendless v4 on Android and iOS. The User and Data services have been tested. The code for Push Notifications for both iOS and Android is there but hasn't been extensively tested with Backendless v4 (it is based on code that was used in production with an older version of this SDK which was based on Backendless v3 and its .NET SDK.)

To see how a sample project would be setup:

1. Download the BackendlessUnitySDK folder and open it as a new project in Unity.
2. Open the TestDataService scene in Unity.
3. Locate the BackendlessPlugin prefab in the scene and set your app's App ID and Windows Phone API Key.
Backendless v4 no longer has versions for apps, so if you have an app for production and a separate one for development on Backendless v4, you can configure both.
4. Run the scene and see in the console an example of how some test data is saved and then retrieved.

Please note: This is an unofficial SDK and not provided by Backendless. They, nor we, are liable for any problems you may have from using it. We can't guarantee support but we'll try our best to fix any bugs.
