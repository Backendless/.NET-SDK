Backendless SDK for .NET and Windows Phone
==========================================

Welcome to Backendless! In this document you will find the instructions for getting up and running with Backendless quickly. The SDK you downloaded contains a library with the APIs, which provide access to the Backendless services. These services enable the server-side functionality for developing and running mobile and desktop applications. Follow the steps below to get started with Backendless:

1. **Create Developer Account.** An account is required in order to create and manage your Backendless backend. You can login to our console at: http://backendless.com/develop
2. **Locate Application ID and Secret Key.** The console is where you can manage the applications, their configuration settings and data. Before you start using any of the APIs, make sure to select an application in the console and open the “Manage” section. The “App Settings” screen contains the application ID and secret API keys, which you will need to use in your code.
3. **Open Backendless Examples.** The SDK includes several examples demonstrating some of the Backendless functionality. The /examples folder contains a Visual Studio solution combining all the samples. Locate the /examples/BackendlessSamples.sln file and open it in Visual Studio 2010 or newer.
4. **Copy/Paste Application ID and Secret Key.**  Each example must be configured with the application ID and secret key generated for your application. Make sure to copy/paste these values into the following source code files:
 * User Service Demo - /examples/UserService/UserServiceDemo/Defaults.cs
 * Data Service Demo - /examples/DataService/ToDoDemo/Defaults.cs
 * Messaging Service Demo - /examples/MessagingService/PubSubDemo/Defaults.cs
 * Geo Service Demo - /examples/GeoService/GeoServiceDemo/Default.js
5. **Run Sample Apps.**

Check the BackendlessUnitySDK folder for Unity specific info.
