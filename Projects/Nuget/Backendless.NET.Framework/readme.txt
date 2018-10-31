==============================================================
            Welcome to Backendless SDK for .NET/Xamarin
==============================================================

Backendless is a cloud-based serverless platform which significantly
simplifies the process of development of web and mobile applications.

This SDK provides the client-side APIs which you can use in your app
to take advantage of the Backendless platform functionality.

To get started you need to create a Backendless developer account at:

https://develop.backendless.com
                
Once logged in, you will be prompted to create a backend application.
Every application has a unique application ID, you will see it right 
on the main dashboard. Additionally, there is also API Key which must
be used in your client application. The API key is also available
from the main dashboard of the Backendless backend app. Make sure to use
the ".NET API Key". 

When you obtain both application ID and API key, make sure to use them
in the following API call to initialize your .NET/Xamarin app:

using BackendlessAPI;

Backendless.InitApp( "YOUR-APP-ID", "YOUR-API-KEY" );

Backendless has a ton of awesome functionality including:
 - Real-time Database
 - Real-time Publish/Subscribe Messaging
 - User Management (user registration, login, logout, password recovery)
 - Push Notification
 - File operations (upload, download, copy, move, delete)
 - Geo location
 
All the APIs can be secured with roles-based security. Also, you can create
your own API services and deploy them into Backendless.

Be sure to check out API documentation available at:
https://backendless.com/docs/dotnet/doc.html

If you run into any problems or have any questions, you can contact us at:
 - Slack http://slack.backendless.com
 - Support Forum http://support.backendless.com
   
HAPPY CODING!!!!