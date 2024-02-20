# Backendless SDK for .NET/Xamarin

version 6.7.1
## What's new
* Fixed problem with BulkUpsert for transactions.
* Added methods Append and AppendText for FileService.
* Added new overrides with urlToFile parameter for method Upload in FileService.
* EmailEnvelop fields changed to properties.
* Added new property 'UniqueEmails' to EmailEnvelope.
* Added BackendlessExpression support.

## About Backendless
Backendless is a cloud-based serverless platform which significantly
simplifies the process of development of web and mobile applications.

This SDK provides the client-side APIs which you can use in your app
to take advantage of the Backendless platform functionality.

Backendless has a ton of awesome functionality including:
* Real-time Database (except for the .netstandard2.0 build)
* Real-time Publish/Subscribe Messaging (except for the .netstandard2.0 build)
* User Management (user registration, login, logout, password recovery)
* Push Notification
* File operations (upload, download, copy, move, delete)
* Geo location
* Custom business logic
 
All APIs can be secured with roles-based security. Also, you can create
your own API services and deploy them into Backendless.

Be sure to check out API documentation available at:
[https://backendless.com/docs/dotnet/](https://backendless.com/docs/dotnet/)

## Getting Started
To get started you need to create a Backendless developer account at:

[https://develop.backendless.com](https://develop.backendless.com)
                
Once logged in, you will be prompted to create a backend application. 
You will be able to select between different "hosting zones". A hosting
zone determines where the backend of your app runs. There are two hosting
zones at this time: North America and Europe.

If you decide to use the European hosting zone, add the following line of code 
in your application. Make sure it is done in the application initialization logic.
For the applications running in the North American data center, you do not need 
to set the URL - the SDK comes pre-configured with the North American API endpoint URL.

```
using BackendlessAPI;

Backendless.URL = "https://eu-api.backendless.com";
```

Every application has a unique application ID, you will see it right 
on the main dashboard. Additionally, there is also API Key which must
be used in your client application. The API key is also available
from the main dashboard of the Backendless backend app. Make sure to use
the `.NET API Key`. 

When you obtain both application ID and API key, make sure to use them
in the following API call to initialize your .NET/Xamarin app:
```
using BackendlessAPI;

Backendless.InitApp( "YOUR-APP-ID", "YOUR-API-KEY" );
```

If you run into any problems or have any questions, you can contact us at:
* [Slack](http://slack.backendless.com)
* [Support Forum](http://support.backendless.com)
   
HAPPY CODING!!!!