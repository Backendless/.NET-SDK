using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackendlessAPI;
using BackendlessAPI.Property;
using BackendlessAPI.Messaging;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Persistence;
using BackendlessAPI.Data;
using System.IO;
using BackendlessAPI.Geo;
using BackendlessAPI.Counters;
using BackendlessAPI.Logging;
using System.Reflection;
using System.Threading.Tasks;

class CDObject
{
  public String title;
  public String objectId;
  public CDObject relation;
}

class ParentObject
{
  public CDObject obj1;
  public CDObject obj2;
  public CDObject[] relations1;
  public CDObject[] relations2;
  public CDObject[] relations3;
}

namespace BackendlessConsoleDemo
{

  class Friend
  {
    public string Name { get; set; }
    public String PhoneNumber { get; set; }
    public GeoPoint Coordinates { get; set; }
  }



  class TaxiCab
  {
    public String CarMake;
    public String CarModel;
    public GeoPoint Location { get; set; }
    public List<GeoPoint> PreviousDropOffs { get; set; }
  }

  class invoice
  {
    public List<Object> items;
  }


  class Order
  {
    public String name;
    public Customer customer;
  }

  class Manufacturer
  {
    public string name;
  }

  class OrderItem
  {
    public string product;
    public Manufacturer vendor;
  }

  class Weather
  {
    public int Humidity { get; set; }
    public int Temperature { get; set; }
  }

  class Customer
  {
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public Address customerAddress { get; set; }
    public int Orders { get; set; }
    public string objectId { get; set; }
  }

  class Address
  {
    public string street { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zipCode { get; set; }
  }

  public class Person
  {
    // using public fields, but you can 
    // use getter/setter methods too
    public string name { get; set; }
    public int age { get; set; }
  }

  class Program
  {
    static void Main( string[] args )
    {
            String appId = "A3D96FA2-7314-2543-FF66-0B60549D7300"; //"834D7918-F1D4-6BF0-FFDE-0AFC804FFD00";
            String secretKey = "97ABCC00-2E19-E4F9-FFCA-B3721842F100"; //"E6148FDD -4B08-91CA-FFE9-7ED1AB986200";
    //  String version = "v1";
    //  Backendless.URL = "http://api.test.backendless.com";
     // Backendless.URL = "http://10.0.1.101:9000";
      Backendless.URL = "http://api.backendless.com";
    //   Backendless.URL = "http://192.168.15.7:9000";
     // Backendless.URL = "http://192.168.16.4:9000";
      //Backendless.URL = "http://api.backendless.com";
      //Backendless.URL = "http://10.0.1.28:9000";

      Backendless.InitApp( appId, secretKey );

      Person person = new Person();
person.name = "Joe";
person.age = 25;
Backendless.Data.Of<Person>().Save( person );

      //LoginAndCreateObject();
      //RetrieveObjectWithSavedId();

      //isValidSession();

     // CacheTest();

      // COUNTERS
      //GetAndIncrement();
      //IncAndGet();
      //GetAndDec();
      //DecAndGet();
      //AddAndGet();
      //GetAndAdd();
      //CondUpdate();
      //reset();

      //SendEmail();

      //CustomEvent();
      //LoadRelations();
      //AddRelations();
      //ObjectUpdate();

      //LoginAndUpdate();
      //LoginAndLoadRelations();
      //AddUserToObject();
     // LoginAndGetDataObjects();

      //TestSerialization();
     // TestFind();

      //SendEmail();
      //SearchPoints();
      //SavePoint();
      //CreateCategory();
      //UploadFile();

      //DescribeUser();
      //RegisterSync();
      //LoginSync();
      //UpdateUserSync();
      //LogoutSync();
      //PasswordRecovery();

     // SaveCustomer();
      //AddMoreCustomers( 30 );
     // FindCustomers();

      //UpdateCustomer();

     // SavePerson();
     // DescribePerson();

      //LoginUser();
      //GetUserRoles();

      //SaveFile();

     // GrantToUser();
      //TestClusteringSync();

      //TestDistanceSearch();

     // TestIsValidLogin();

      //SearchByDateInCategorySync();
      //SearchByDateInCategoryAsync();
      //SearchByDateInRadiusSync();
      //SearchByDateInRadiusAsync();
      //SearchByDateInRectangularAreaSync();
      //SearchByDateInRectangularAreaAsync();

      //SaveDataWithGeo();
     // PartialMatch();
      //GetUsersSorted();
      //ChangeDataPermission();

      //GetClusterPoints();
      //registerUser();
      //loginUserAndGetProperties();
      //GetGeoFencePoints();
      //UsersToGeoOneToOne();
      //UsersToGeoOneToMany();
     // RunOnStayActionInGeoFence();
     // LoggingTests();
      //LoadMetaForClusterSync();
      //DeleteGeoPointSync();
     // DeleteGeoPointAsync();
    //  registerUser();
      LoadData();



      try
      {
        var status = PublishMessage();
        CheckMessageStatus(status);
        CheckMessageStatusAsync(status);
        
        /*
        PublishMessageAsync();
        PublishMessageWithHeaders();
        PublishMessageWithHeadersAsync();
        PublishMessageToSubtopic();
        PublishMessageToSubtopicAsync();
        PublishMessageAsPush();
        PublishMessageAsPushAsync();
        PublishToAndroidNotificationCenter();
        PublishToAndroidNotificationCenterAsync();
        PublishMessageDelayed();
        PublishMessageDelayedAsync();*/
      }
      catch( Exception e )
      {
        System.Console.WriteLine( e.ToString() + "\n" + e.StackTrace );
      }

 
      System.Console.WriteLine( "Done.." );
      System.Console.ReadLine();
   }

    static void LoadData()
    {
      Person person = Backendless.Data.Of<Person>().FindFirst();
      long l = Backendless.Data.Of<Person>().Remove( person );
      System.Console.WriteLine( l );
      /*
      BackendlessCollection<Person> people = Backendless.Data.Of<Person>().Find();

      foreach( Person person in people.GetCurrentPage() )
      {
        System.Console.WriteLine( person.name );
      }
       * */
    }

    static void GetFencePoints()
    {
BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
geoQuery.IncludeMeta = true;
geoQuery.WhereClause = "businessType='restaurant' and rating > 3";

AsyncCallback<IList<GeoPoint>> getPointsCallback = new AsyncCallback<IList<GeoPoint>>(
  points =>
  {
    // "points" collection contains geopoints which satisfy the query
  },
  error =>
  {
  }
  );

Backendless.Geo.GetPoints( "Manhattan", geoQuery, getPointsCallback );
    }

    static void DeleteGeoPointSync()
    {
GeoPoint geoPoint = new GeoPoint( -31.96, 115.84 );
GeoPoint savedGeoPoint = Backendless.Geo.SavePoint( geoPoint );

Backendless.Geo.RemovePoint( savedGeoPoint );
    }

    static void DeleteGeoPointAsync()
    {
AsyncCallback<GeoPoint> removeCallback = new AsyncCallback<GeoPoint>(
  r =>
  {
    System.Console.WriteLine( "Geo point has been removed." );
  },

  error =>
  {
    System.Console.WriteLine( "Server reported an error - " + error.Message );
  } );

AsyncCallback<GeoPoint> saveCallback = new AsyncCallback<GeoPoint>(
  savedGeoPoint =>
  {
    System.Console.WriteLine( "Geo point has been saved. Object ID - " + savedGeoPoint.ObjectId );
    Backendless.Geo.RemovePoint( savedGeoPoint, removeCallback );
  },

  error =>
  {
  } );

GeoPoint geoPoint = new GeoPoint( -31.96, 115.84 );
Backendless.Geo.SavePoint( geoPoint, saveCallback );
    }

    static void LoadMetaForClusterSync()
    {
BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
geoQuery.Radius = 290;
geoQuery.Units = Units.MILES;
geoQuery.Latitude = 33.217;
geoQuery.Longitude = -97.573;
geoQuery.Categories.Add( "geoservice_sample" );
geoQuery.SetClusteringParams( -144.726, -45.322, 1125 );

IList<GeoPoint> points = Backendless.Geo.GetPoints( geoQuery );

foreach( GeoPoint point in points )
{
  GeoPoint pointWithMeta = Backendless.Geo.LoadMetadata( point );
  System.Console.WriteLine( "GeoPoint/Cluster info - " + pointWithMeta );
}
    }

    static void LoadMetaForClusterAsync()
    {
BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
geoQuery.Radius = 290;
geoQuery.Units = Units.MILES;
geoQuery.Latitude = 33.217;
geoQuery.Longitude = -97.134;
geoQuery.Categories.Add( "geoservice_sample" );
geoQuery.SetClusteringParams( -146.748, -47.519, 1125 );

AsyncCallback<GeoPoint> loadMetadataResponder = new AsyncCallback<GeoPoint>(
  geoCluster =>
  {
    System.Console.WriteLine( "GeoPoint/Cluster info - " + geoCluster );
  },
  error =>
  {
    System.Console.WriteLine( "Server reported an error " + error.Message );
  } );

AsyncCallback<IList<GeoPoint>> responder = new AsyncCallback<IList<GeoPoint>>(
  points =>
  {
    foreach( GeoPoint point in points )
      Backendless.Geo.LoadMetadata( point, loadMetadataResponder );
  },
  error =>
  {
    System.Console.WriteLine( "Server reported an error " + error.Message );
  }
  );

Backendless.Geo.GetPoints( geoQuery, responder );
    }

    static void test()
    {
String whereClause = "updated after " + DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
BackendlessDataQuery dataQuery = new BackendlessDataQuery();
dataQuery.WhereClause = whereClause;
IList<Person> result = Backendless.Data.Of<Person>().Find( dataQuery );
    }

    static void LoggingTests()
    {
      Backendless.Logging.SetLogReportingPolicy( 5, 23000 );
      Logger myLogger = Backendless.Logging.GetLogger( "com.mbaas.LoggingSample" );
      int i = 0;
      while( true )
      {
        myLogger.Info( "FOOBAR - " + i++ );
        System.Threading.Thread.Sleep( 1000 );

        if( i % 30 == 0 )
        {
          System.Console.WriteLine( "Manual flush" );
          Backendless.Logging.Flush();
        }
      }
    }

    static void RunOnStayActionInGeoFence()
    {
      AsyncCallback<int> callback = new AsyncCallback<int>(
        pointsAffected =>
        {
          System.Console.WriteLine( String.Format( "Action has been executed in {0} geo points", pointsAffected ) );
        },
        error =>
        {
          System.Console.WriteLine( String.Format( "Server reported an error {0}", error.Message ) );
        });
      
      Backendless.Geo.RunOnStayAction( "texas", callback );
    }

    static void UsersToGeoOneToMany()
    {
BackendlessUser user = Backendless.Data.Of<BackendlessUser>().FindFirst();
GeoPoint geoPoint1 = new GeoPoint( 48.85, 2.35 );
GeoPoint geoPoint2 = new GeoPoint( 40.40, 3.68 );
List<GeoPoint> locations = new List<GeoPoint>();
locations.Add( geoPoint1 );
locations.Add( geoPoint2 );
user.AddProperty( "locations", locations );
Backendless.Data.Of<BackendlessUser>().Save( user );
    }


    static void UsersToGeoOneToManyAsync()
    {
AsyncCallback<BackendlessUser> saveUserCallback = new AsyncCallback<BackendlessUser>(
  user =>
  {
    // user object has been updated with geo point
  },
    error =>
    {
    }
  );
AsyncCallback<BackendlessUser> findUserCallback = new AsyncCallback<BackendlessUser>(
  user =>
  {
    GeoPoint geoPoint1 = new GeoPoint( 48.85, 2.35 );
    GeoPoint geoPoint2 = new GeoPoint( 40.40, 3.68 );
    List<GeoPoint> locations = new List<GeoPoint>();
    locations.Add( geoPoint1 );
    locations.Add( geoPoint2 );
    user.AddProperty( "locations", locations );
    Backendless.Data.Of<BackendlessUser>().Save( user, saveUserCallback );
  },
    error =>
    {
    }
  );
Backendless.Data.Of<BackendlessUser>().FindFirst( findUserCallback );
    }

    static void UsersToGeoOneToOne()
    {
BackendlessUser user = Backendless.Data.Of<BackendlessUser>().FindFirst();
GeoPoint geoPoint = new GeoPoint( 48.85, 2.35 );
user.AddProperty( "location", geoPoint );
Backendless.Data.Of<BackendlessUser>().Save( user );
    }

    static void UsersToGeoOneToOneAsync()
    {
AsyncCallback<BackendlessUser> saveUserCallback = new AsyncCallback<BackendlessUser>(
  user =>
  {
    // user object has been updated with geo point
  },
    error =>
    {
    }
  );
AsyncCallback<BackendlessUser> findUserCallback = new AsyncCallback<BackendlessUser>(
  user =>
  {
    GeoPoint geoPoint = new GeoPoint( 48.85, 2.35 );
    user.AddProperty( "location", geoPoint );
    Backendless.Data.Of<BackendlessUser>().Save( user, saveUserCallback );
  },
    error =>
    {
    }
  );
Backendless.Data.Of<BackendlessUser>().FindFirst( findUserCallback );
    }

    static void GetGeoFencePoints()
    {
      AsyncCallback<IList<GeoPoint>> callback = new AsyncCallback<IList<GeoPoint>>(
        result =>
        {
          foreach( GeoPoint point in result )
          {
            System.Console.WriteLine( "Latitiude - " + point.Latitude );
            System.Console.WriteLine( "Longitude - " + point.Longitude );
          }
        },
        error =>
        {
        }
        );

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
      geoQuery.Categories.Add( "geoservice_sample" );
      geoQuery.WhereClause = "city = 'DALLAS'";
      geoQuery.SetClusteringParams( -164.458, -29.809, 1532, 400 );
      geoQuery.Radius = 1000;
      geoQuery.Units = Units.MILES;
      geoQuery.Latitude = 33.217;
      geoQuery.Longitude = -97.134;
      Backendless.Geo.GetPoints( "texas", geoQuery, callback );
    }

    static void GetClusterPoints()
    {
BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
geoQuery.Radius = 1000;
geoQuery.Units = Units.MILES;
geoQuery.Latitude = 33.217;
geoQuery.Longitude = -97.134;
geoQuery.Categories.Add( "geoservice_sample" );
geoQuery.SetClusteringParams( -164.458, -29.809, 1532, 400 );

AsyncCallback<IList<GeoPoint>> loadGeoPointsResponder;
loadGeoPointsResponder = new AsyncCallback<IList<GeoPoint>>(
  result => 
  {
        System.Console.WriteLine( "points in cluster:" );
        foreach( GeoPoint point in result )
        {
          System.Console.WriteLine( "Latitude - " + point.Latitude );
          System.Console.WriteLine( "Longitude - " + point.Longitude );
        }
        System.Console.WriteLine( "-------------------------" );
  },
  error => 
  {
    System.Console.WriteLine( String.Format( "Server reported an error - {0}" ), error.Message );
  });

AsyncCallback<IList<GeoPoint>> responder;
responder = new AsyncCallback<IList<GeoPoint>>(
  result =>
  {
    foreach( GeoPoint point in result )
    {
      if( point is GeoCluster )
      {
        GeoCluster geoCluster = (GeoCluster) point;
        System.Console.WriteLine( String.Format( "Cluster. Number of points - {0}", geoCluster.TotalPoints ) );

        Backendless.Geo.GetPoints( geoCluster, loadGeoPointsResponder );
      }
    }
  },
  error =>
  {
    System.Console.WriteLine( String.Format( "Server reported an error - {0}" ), error.Message );
  } );

Backendless.Geo.GetPoints( geoQuery, responder );
    }

  public static void registerUser()
{
  try
  {
    BackendlessUser user = new BackendlessUser();
    user.Email = "spidey@backendless.com";
    user.Password ="greeng0blin";
    user.SetProperty( "phoneNumber", "214-555-1212" );

    BackendlessUser registeredUser = Backendless.UserService.Register( user );
    System.Console.WriteLine( String.Format( "User has been registered: {0}", registeredUser.Email ) );
  }
  catch( BackendlessException e )
  {
    System.Console.WriteLine( String.Format( "Server reported an error: {0}", e.Message ));
  }
}

  public static void loginUserAndGetProperties()
{
  try
  {
    BackendlessUser loggedUser = Backendless.UserService.Login( "spidey@backendless.com", "greeng0blin" );
    System.Console.WriteLine( String.Format( "User has been logged in: {0} ", loggedUser.Email ) );

    BackendlessUser user = Backendless.UserService.CurrentUser;
    if( user != null )
    {
      // get user's email (i.e. mandatory/predefined property)
      String email = user.Email;
      // get user's phone number (i.e. custom property)
      String phoneNumber = (String) user.Properties[ "phoneNumber" ];
      System.Console.WriteLine( String.Format( "User email: {0}, phone number: {1}", email, phoneNumber ) );
    }
    else
    {
      System.Console.WriteLine( "User hasn't been logged" );
    }
  }
  catch( BackendlessException e )
  {
    System.Console.WriteLine( String.Format( "Server reported an error: {0}", e ) );
  }
}

public static void registerUserAsync()
{
  AsyncCallback<BackendlessUser> registerCallback = new AsyncCallback<BackendlessUser>(
    result =>
    {
      System.Console.WriteLine( String.Format( "User has been registered: {0}", result.Email ) );
    },

    error =>
    {
      System.Console.WriteLine( String.Format( "Server reported an error: {0}", error.Message ) );
    } );

  BackendlessUser user = new BackendlessUser();
  user.Email = "spidey@backendless.com";
  user.Password = "greeng0blin";
  user.SetProperty( "phoneNumber", "214-555-1212" );
  Backendless.UserService.Register( user, registerCallback );
}

public static void loginUserAndGetPropertiesAsync()
{
  AsyncCallback<BackendlessUser> loginCallback = new AsyncCallback<BackendlessUser>(
    result =>
    {
      System.Console.WriteLine( String.Format( "User has been logged in: {0} ", result.Email ) );
      BackendlessUser user = Backendless.UserService.CurrentUser;
      if( user != null )
      {
        // get user's email (i.e. mandatory/predefined property)
        String email = user.Email;
        // get user's phone number (i.e. custom property)
        String phoneNumber = (String) user.Properties[ "phoneNumber" ];
        System.Console.WriteLine( String.Format( "User email: {0}, phone number: {1}", email, phoneNumber ) );
      }
      else
      {
        System.Console.WriteLine( "User hasn't been logged" );
      }
    },
    error =>
    {
      System.Console.WriteLine( String.Format( "Server reported an error: {0}", error.Message ) );
    } );

  Backendless.UserService.Login( "spidey@backendless.com", "greeng0blin" );
}

    static void ChangeDataPermission()
    {
AsyncCallback<Object> denyCallback = new AsyncCallback<Object>(
  result =>
  {
    System.Console.WriteLine( "Permission has been denied for all roles" );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

AsyncCallback<Address> searchCallback = new AsyncCallback<Address>(
  result =>
  {
    DataPermission.FIND.DenyForAllRoles( result, denyCallback );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

Backendless.Data.Of<Address>().FindFirst( searchCallback );
    }

    static void GetUsersSorted()
    {
      BackendlessDataQuery dataQuery = new BackendlessDataQuery();
      dataQuery.Offset = 1;
      dataQuery.QueryOptions = new QueryOptions()
      {
        SortBy = new List<string> { "email" }
      };

      IList<BackendlessUser> freeUsers = Backendless.Data.Of<BackendlessUser>().Find( dataQuery );
      IList<BackendlessUser> users = freeUsers;

      foreach( BackendlessUser user in users )
      {
        System.Console.Out.WriteLine( user.Email );
      }
    }

    static void PartialMatch()
    {
      BackendlessGeoQuery query = new BackendlessGeoQuery();

      query.Categories.Add( "personals" );
      query.IncludeMeta = true;

      query.RelativeFindMetadata.Add( "burgers", "true" );
      query.RelativeFindMetadata.Add( "hiking", "true" );
      query.RelativeFindMetadata.Add( "chinese", "true" );
      query.RelativeFindPercentThreshold = 30.0;

      AsyncCallback<IList<SearchMatchesResult>> searchCallback = new AsyncCallback<IList<SearchMatchesResult>>(
        result =>
        {
          System.Console.WriteLine( String.Format( "\nSearchByDateInCategory GETPOINTS: {0}", String.Join( ",", result ) ) );
          foreach( SearchMatchesResult matchResult in result )
          {
            System.Console.WriteLine( "MATCH: %{0}", matchResult.Matches );
            System.Console.WriteLine( matchResult.GeoPoint );
            System.Console.WriteLine( "==================================" );
          }

        },
        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Geo.RelativeFind( query, searchCallback );
    }

    static void PartialMatchInRadius()
    {
      BackendlessGeoQuery query = new BackendlessGeoQuery();

      query.Categories.Add( "City" );
      query.Metadata.Add( "Name", "Starbucks" );
      query.Latitude = 47.60657;
      query.Longitude = -122.33180;
      query.Radius = 10.0;
      query.Units = Units.KILOMETERS;

      query.RelativeFindMetadata.Add( "BusinessRating", "5 stars" );
      query.RelativeFindMetadata.Add( "DriveThrough", "Yes" );
      query.RelativeFindPercentThreshold = 50.0;

      AsyncCallback<IList<SearchMatchesResult>> searchCallback = new AsyncCallback<IList<SearchMatchesResult>>(
        result =>
        {
          System.Console.WriteLine( String.Format( "\nSearchByDateInCategory GETPOINTS: {0}", String.Join( ",", result ) ) );
          foreach( SearchMatchesResult matchResult in result )
          {
            System.Console.WriteLine( "MATCH: %{0}", matchResult.Matches );
            System.Console.WriteLine( matchResult.GeoPoint );
            System.Console.WriteLine( "==================================" );
          }

        },
        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Geo.RelativeFind( query, searchCallback );
    }

    static void PartialMatchInRectangle()
    {
      BackendlessGeoQuery query = new BackendlessGeoQuery();

      query.Categories.Add( "City" );
      query.Metadata.Add( "Name", "Starbucks" );
      query.SearchRectangle = new double[] {48.302, -125.173, 47.007, -117.620 };

      query.RelativeFindMetadata.Add( "BusinessRating", "5 stars" );
      query.RelativeFindMetadata.Add( "DriveThrough", "Yes" );
      query.RelativeFindPercentThreshold = 50.0;

      AsyncCallback<IList<SearchMatchesResult>> searchCallback = new AsyncCallback<IList<SearchMatchesResult>>(
        result =>
        {
          System.Console.WriteLine( String.Format( "\nSearchByDateInCategory GETPOINTS: {0}", String.Join( ",", result ) ) );
          foreach( SearchMatchesResult matchResult in result )
          {
            System.Console.WriteLine( "MATCH: %{0}", matchResult.Matches );
            System.Console.WriteLine( matchResult.GeoPoint );
            System.Console.WriteLine( "==================================" );
          }

        },
        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Geo.RelativeFind( query, searchCallback );
    }

    static void PartialMatchWithWhere()
    {
      BackendlessGeoQuery query = new BackendlessGeoQuery();

      query.Categories.Add( "City" );
      query.Metadata.Add( "Name", "Starbucks" );
      query.WhereClause = "ManagerName LIKE 'James%'";

      query.RelativeFindMetadata.Add( "BusinessRating", "5 stars" );
      query.RelativeFindMetadata.Add( "DriveThrough", "Yes" );
      query.RelativeFindPercentThreshold = 50.0;

      AsyncCallback<IList<SearchMatchesResult>> searchCallback = new AsyncCallback<IList<SearchMatchesResult>>(
        result =>
        {
          System.Console.WriteLine( String.Format( "\nSearchByDateInCategory GETPOINTS: {0}", String.Join( ",", result ) ) );
          foreach( SearchMatchesResult matchResult in result )
          {
            System.Console.WriteLine( "MATCH: %{0}", matchResult.Matches );
            System.Console.WriteLine( matchResult.GeoPoint );
            System.Console.WriteLine( "==================================" );
          }

        },
        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Geo.RelativeFind( query, searchCallback );
    }

    static void SaveDataWithGeo()
    {
      /*
      TaxiCab taxi = new TaxiCab();
      taxi.CarMake = "Toyota";
      taxi.CarModel = "Prius";

      // one-to-one relation between a data object and a geo point
      taxi.Location = new GeoPoint( 40.7148, -74.0059 );
      taxi.Location.Categories.Add( "taxi" );
      taxi.Location.Metadata.Add( "service-area", "NYC" );

      // one-to-many relation between a data object and geo points
      List<GeoPoint> previousDropOffs = new List<GeoPoint>();

      GeoPoint droppOff1 = new GeoPoint( 40.757977, -73.98557 );
      droppOff1.Metadata.Add( "name", "Times Square" );
      droppOff1.Categories.Add( "DropOffs" );
      previousDropOffs.Add( droppOff1 );

      GeoPoint droppOff2 = new GeoPoint( 40.748379, -73.985565 );
      droppOff2.Metadata.Add( "name", "Empire State Building" );
      droppOff2.Categories.Add( "DropOffs" );
      previousDropOffs.Add( droppOff2 );

      taxi.PreviousDropOffs = previousDropOffs;

      Backendless.Data.Of<TaxiCab>().Save( taxi );*/


      /// Geo to Data

      // one to one relation between geo and data
      TaxiCab cab = new TaxiCab();
      cab.CarMake = "Ford";
      cab.CarModel = "Crown Victoria";
      GeoPoint pickupLocation = new GeoPoint( 40.750549, -73.994232 );
      pickupLocation.Categories.Add( "Pickups" );
      pickupLocation.Metadata.Add( "TaxiCab", cab );
      Backendless.Geo.AddPoint( pickupLocation );



      // one to many relations between geo and data
      TaxiCab cab1 = new TaxiCab();
      cab1.CarMake = "Ford";
      cab1.CarModel = "Crown Victoria";

      TaxiCab cab2 = new TaxiCab();
      cab2.CarMake = "Toyota";
      cab2.CarModel = "Prius";

      List<TaxiCab> availableCabs = new List<TaxiCab>();
      availableCabs.Add( cab1 );
      availableCabs.Add( cab2 );

      GeoPoint pickupLocation1 = new GeoPoint( 40.750549, -73.994232 );
      pickupLocation1.Categories.Add( "Pickups" );
      pickupLocation1.Metadata.Add( "AvailableCabs", availableCabs );
      Backendless.Geo.AddPoint( pickupLocation1 );


     // TaxiCab cab = Backendless.Data.Of<TaxiCab>().FindFirst();
     // System.Console.WriteLine( "cab " + cab.CarMake );
    }

static void SearchByDateInCategorySync()
{
  // date
  DateTime updated = DateTime.Parse( "2015-01-17 12:00:00Z" );
 
  // create
  GeoPoint geoPoint = new GeoPoint( 21.306944, -157.858333 );
  geoPoint.Categories.Add( "Coffee" );
  geoPoint.Metadata.Add( "Name", "Starbucks" );
  geoPoint.Metadata.Add( "Parking", true );
  geoPoint.Metadata.Add( "updated", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond );

  geoPoint = Backendless.Geo.SavePoint( geoPoint );
  System.Console.WriteLine( String.Format( "SearchByDateInCategory -> point: {0}", geoPoint ) );

  // search
  BackendlessGeoQuery query = new BackendlessGeoQuery();
  query.Categories.Add( "Coffee" );
  query.WhereClause = String.Format( "updated > {0}", updated.Ticks / TimeSpan.TicksPerMillisecond );
  query.IncludeMeta = true;
  IList<GeoPoint> points = Backendless.Geo.GetPoints( query );

  System.Console.WriteLine( String.Format( "SearchByDateInCategory GETPOINTS: {0}", String.Join( ",", points ) ) );
}

static void SearchByDateInCategoryAsync()
{
  // date
  DateTime updated = DateTime.Parse( "2015-01-17 12:00:00Z" );

  AsyncCallback<IList<GeoPoint>> searchCallback = new AsyncCallback<IList<GeoPoint>>(
    result =>
    {
      System.Console.WriteLine( String.Format( "\nSearchByDateInCategory GETPOINTS: {0}", String.Join( ",", result ) ) );
    },
    fault =>
    {
      System.Console.WriteLine( "Error - " + fault );
    } );

  AsyncCallback<GeoPoint> saveGeoPointCallback = new AsyncCallback<GeoPoint>(
    result =>
    {
      System.Console.WriteLine( String.Format( "SearchByDateInCategory -> point: {0}", result ) );

      // search
      BackendlessGeoQuery query = new BackendlessGeoQuery();
      query.Categories.Add( "Coffee" );
      query.WhereClause = String.Format( "updated > {0}", updated.Ticks / TimeSpan.TicksPerMillisecond );
      query.IncludeMeta = true;
      Backendless.Geo.GetPoints( query, searchCallback );
    },
    fault =>
    {
      System.Console.WriteLine( "Error - " + fault );
    } );

  // create
  GeoPoint geoPoint = new GeoPoint( 21.306944, -157.858333 );
  geoPoint.Categories.Add( "Coffee" );
  geoPoint.Metadata.Add( "Name", "Starbucks" );
  geoPoint.Metadata.Add( "Parking", true );
  geoPoint.Metadata.Add( "updated", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond );

  Backendless.Geo.SavePoint( geoPoint, saveGeoPointCallback );
}

static void SearchByDateInRadiusSync()
{
  // date
  DateTime updated = DateTime.Parse( "2015-01-17 12:00:00Z" );

  // create
  GeoPoint geoPoint = new GeoPoint( 21.306944, -157.858333 );
  geoPoint.Categories.Add( "Coffee" );
  geoPoint.Categories.Add( "City" );
  geoPoint.Metadata.Add( "Name", "Starbucks" );
  geoPoint.Metadata.Add( "Parking", true );
  geoPoint.Metadata.Add( "updated", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond );

  geoPoint = Backendless.Geo.SavePoint( geoPoint );
  System.Console.WriteLine( String.Format( "SearchByDateInRadius -> point: {0}", geoPoint ) );

  // search
  BackendlessGeoQuery query = new BackendlessGeoQuery( 21.30, -157.85, 50, Units.KILOMETERS );
  query.Categories.Add( "City" );
  query.WhereClause = String.Format( "updated > {0}", updated.Ticks / TimeSpan.TicksPerMillisecond );
  query.IncludeMeta = true;
  IList<GeoPoint> points = Backendless.Geo.GetPoints( query );

  System.Console.WriteLine( String.Format( "SearchByDateInRadius GETPOINTS: {0}", String.Join( ",", points ) ) );
}

static void SearchByDateInRadiusAsync()
{
  // date
  DateTime updated = DateTime.Parse( "2015-01-17 12:00:00Z" );

  AsyncCallback<IList<GeoPoint>> searchCallback = new AsyncCallback<IList<GeoPoint>>(
    result =>
    {
      System.Console.WriteLine( String.Format( "\nSearchByDateInRadius GETPOINTS: {0}", String.Join( ",", result ) ) );
    },
    fault =>
    {
      System.Console.WriteLine( "Error - " + fault );
    } );

  AsyncCallback<GeoPoint> saveGeoPointCallback = new AsyncCallback<GeoPoint>(
    result =>
    {
      System.Console.WriteLine( String.Format( "SearchByDateInRadius -> point: {0}", result ) );

      // search
      BackendlessGeoQuery query = new BackendlessGeoQuery( 21.30, -157.85, 50, Units.KILOMETERS );
      query.Categories.Add( "City" );
      query.WhereClause = String.Format( "updated > {0}", updated.Ticks / TimeSpan.TicksPerMillisecond );
      query.IncludeMeta = true;
      Backendless.Geo.GetPoints( query, searchCallback );
    },
    fault =>
    {
      System.Console.WriteLine( "Error - " + fault );
    } );

  // create
  GeoPoint geoPoint = new GeoPoint( 21.306944, -157.858333 );
  geoPoint.Categories.Add( "Coffee" );
  geoPoint.Metadata.Add( "Name", "Starbucks" );
  geoPoint.Metadata.Add( "Parking", true );
  geoPoint.Metadata.Add( "updated", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond );

  Backendless.Geo.SavePoint( geoPoint, saveGeoPointCallback );
}

static void SearchByDateInRectangularAreaSync()
{
  // date
  DateTime openTime = DateTime.Parse( "2015-01-17 07:00:00" );
  DateTime closeTime = DateTime.Parse( "2015-01-17 23:00:00" );
  DateTime now = DateTime.Parse( "2015-01-17 15:20:00" );

  // create
  GeoPoint geoPoint = new GeoPoint( 21.306944, -157.858333 );
  geoPoint.Categories.Add( "Coffee" );
  geoPoint.Metadata.Add( "Name", "Starbucks" );
  geoPoint.Metadata.Add( "openTime", openTime.Ticks / TimeSpan.TicksPerMillisecond );
  geoPoint.Metadata.Add( "closeTime", closeTime.Ticks / TimeSpan.TicksPerMillisecond );
  geoPoint = Backendless.Geo.SavePoint( geoPoint );
  System.Console.WriteLine( String.Format( "SearchByDateInRectangularAreaSync -> point: {0}", geoPoint ) );

  // search
  GeoPoint northWestCorner = new GeoPoint( 21.306944 + 0.5, -157.858333 - 0.5 );
  GeoPoint southEastCorner = new GeoPoint( 21.306944 - 0.5, -157.858333 + 0.5 );
  BackendlessGeoQuery query = new BackendlessGeoQuery( northWestCorner, southEastCorner );
  query.Categories.Add( "Coffee" );
  query.WhereClause = String.Format( "openTime < {0} AND closeTime > {0}", now.Ticks / TimeSpan.TicksPerMillisecond );
  query.IncludeMeta = true;
  IList<GeoPoint> points = Backendless.Geo.GetPoints( query );

  System.Console.WriteLine( String.Format( "SearchByDateInRectangularAreaSync GETPOINTS: {0}", String.Join( ",", points ) ) );
}

static void SearchByDateInRectangularAreaAsync()
{
  // date
  DateTime openTime = DateTime.Parse( "2015-01-17 07:00:00" );
  DateTime closeTime = DateTime.Parse( "2015-01-17 23:00:00" );
  DateTime now = DateTime.Parse( "2015-01-17 15:20:00" );

  AsyncCallback<IList<GeoPoint>> searchCallback = new AsyncCallback<IList<GeoPoint>>(
    result =>
    {
      System.Console.WriteLine( String.Format( "\nSearchByDateInRectangularAreaAsync GETPOINTS: {0}", String.Join( ",", result ) ) );
    },
    fault =>
    {
      System.Console.WriteLine( "Error - " + fault );
    } );

  AsyncCallback<GeoPoint> saveGeoPointCallback = new AsyncCallback<GeoPoint>(
    result =>
    {
      System.Console.WriteLine( String.Format( "SearchByDateInRectangularAreaAsync -> point: {0}", result ) );

      // search
      GeoPoint northWestCorner = new GeoPoint( 21.306944 + 0.5, -157.858333 - 0.5 );
      GeoPoint southEastCorner = new GeoPoint( 21.306944 - 0.5, -157.858333 + 0.5 );
      BackendlessGeoQuery query = new BackendlessGeoQuery( northWestCorner, southEastCorner );
      query.Categories.Add( "Coffee" );
      query.WhereClause = String.Format( "openTime < {0} AND closeTime > {0}", now.Ticks / TimeSpan.TicksPerMillisecond );
      query.IncludeMeta = true;
      Backendless.Geo.GetPoints( query, searchCallback );
    },
    fault =>
    {
      System.Console.WriteLine( "Error - " + fault );
    } );

  // create
  GeoPoint geoPoint = new GeoPoint( 21.306944, -157.858333 );
  geoPoint.Categories.Add( "Coffee" );
  geoPoint.Metadata.Add( "Name", "Starbucks" );
  geoPoint.Metadata.Add( "openTime", openTime.Ticks / TimeSpan.TicksPerMillisecond );
  geoPoint.Metadata.Add( "closeTime", closeTime.Ticks / TimeSpan.TicksPerMillisecond );
  Backendless.Geo.SavePoint( geoPoint, saveGeoPointCallback );
}
  

    static void TestIsValidLogin()
    {
// Login user
Backendless.UserService.Login( "batman@backendless.com", "superm@n", true );

// Check if login is valid with the sync API call
bool isValidLogin = Backendless.UserService.IsValidLogin();
System.Console.WriteLine( "[SYNC] Is login valid? - " + isValidLogin );

// Check if login is valid with the async API call
AsyncCallback<Boolean> isLoginValidCallback = new AsyncCallback<Boolean>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC] Is login valid? - " + result );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

Backendless.UserService.IsValidLogin( isLoginValidCallback );
    }

    static void TestDistanceSearch()
    {
      Friend bob = new Friend();
      bob.Name = "Bob";
      bob.PhoneNumber = "512-555-1212";
      bob.Coordinates = new GeoPoint( 30.26715, -97.74306 );
      bob.Coordinates.Categories.Add( "Home" );
      bob.Coordinates.Metadata.Add( "description", "Bob's home" );
      Backendless.Data.Of<Friend>().Save( bob );

      Friend jane = new Friend();
      jane.Name = "Jane";
      jane.PhoneNumber = "281-555-1212";
      jane.Coordinates = new GeoPoint( 29.76328, -95.36327 );
      jane.Coordinates.Categories.Add( "Home" );
      jane.Coordinates.Metadata.Add( "description", "Jane's home" );
      Backendless.Data.Of<Friend>().Save( jane );

      Friend fred = new Friend();
      fred.Name = "Jane";
      fred.PhoneNumber = "210-555-1212";
      fred.Coordinates = new GeoPoint( 29.42412, -98.49363 );
      fred.Coordinates.Categories.Add( "Home" );
      fred.Coordinates.Metadata.Add( "description", "Fred's home" );
      Backendless.Data.Of<Friend>().Save( fred );

      String whereClause = "distance( 30.26715, -97.74306, Coordinates.latitude, Coordinates.longitude ) < ml(200)";
      BackendlessDataQuery dataQuery = new BackendlessDataQuery( whereClause );
      dataQuery.QueryOptions = new QueryOptions()
      {
        RelationsDepth = 1
      };
      IList<Friend> friends = Backendless.Data.Of<Friend>().Find( dataQuery );

      foreach( Friend friend in friends )
        System.Console.WriteLine( String.Format( "{0} lives at {1}, {2} tagged as '{3}'", friend.Name, friend.Coordinates.Latitude, friend.Coordinates.Longitude, friend.Coordinates.Metadata[ "description" ] ) ); 
    }

    static void TestClusteringSync()
    {
BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
geoQuery.Radius = 1000;
geoQuery.Units = Units.MILES;
geoQuery.Latitude = 33.217;
geoQuery.Longitude = -97.134;
geoQuery.Categories.Add( "geoservice_sample" );
geoQuery.SetClusteringParams( -164.458, -29.809, 1532, 400 );
IList<GeoPoint> points = Backendless.Geo.GetPoints( geoQuery );

foreach( GeoPoint point in points )
{
  System.Console.WriteLine( "latitude - " + point.Latitude );
  System.Console.WriteLine( "longitude - " + point.Longitude );

  if( point is GeoCluster )
  {
    GeoCluster geoCluster = (GeoCluster) point;
    System.Console.WriteLine( "Cluster. Number of points - " + geoCluster.TotalPoints );
  }

  GeoPoint updatedPoint = Backendless.Geo.LoadMetadata( point );

  foreach( string key in updatedPoint.Metadata.Keys )
  {
    var value = updatedPoint.Metadata[ key ];

    if( value is Array )
      value = string.Join( ", ", ( (Object[]) value ).Select( v => v.ToString() ) );

    Console.WriteLine( String.Format( "\t{0}: {1}", key, value ) );
  }

  System.Console.WriteLine( "-------------------------------" );
}
    }

    static void TestClusteringAsync()
    {
      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
      geoQuery.Radius = 1000;
      geoQuery.Units = Units.MILES;
      geoQuery.Latitude = 33.217;
      geoQuery.Longitude = -97.134;
      geoQuery.Categories.Add( "geoservice_sample" );
      geoQuery.SetClusteringParams( -164.458, -29.809, 1532, 400 );

      AsyncCallback<GeoPoint> loadMetaCallback = new AsyncCallback<GeoPoint>(
         result =>
         {
           GeoPoint updatedPoint = result;
           foreach( string key in updatedPoint.Metadata.Keys )
           {
             var value = updatedPoint.Metadata[ key ];

             if( value is Array )
               value = string.Join( ", ", ( (Object[]) value ).Select( v => v.ToString() ) );

             Console.WriteLine( String.Format( "\t{0}: {1}", key, value ) );
           }
         },
         fault =>
         {
           System.Console.WriteLine( "Error - " + fault );
         } );

      AsyncCallback<IList<GeoPoint>> callback = new AsyncCallback<IList<GeoPoint>>(
         result =>
         {
           IList<GeoPoint> points = result;
           foreach( GeoPoint point in points )
           {
             System.Console.WriteLine( "latitude - " + point.Latitude );
             System.Console.WriteLine( "longitude - " + point.Longitude );

             if( point is GeoCluster )
             {
               GeoCluster geoCluster = (GeoCluster) point;
               System.Console.WriteLine( "Cluster. Number of points - " + geoCluster.TotalPoints );
             }

            Backendless.Geo.LoadMetadata( point, loadMetaCallback );

            

             System.Console.WriteLine( "-------------------------------" );
           }
         },
         fault =>
         {
           System.Console.WriteLine( "Error - " + fault );
         } );

      Backendless.Geo.GetPoints( geoQuery, callback );

    }

    static void GrantToUser()
    {
      Address address = Backendless.Data.Of<Address>().FindFirst();
      DataPermission.FIND.GrantForUser( "mpiller@gmail.com", address );
    }

    static void SaveFile()
    {
      String content = "The quick brown fox jumps over the lazy dog";
      byte[] bytes = UTF8Encoding.UTF8.GetBytes( content );
      String savedFileURL = Backendless.Files.SaveFile( "tempfolder", "fox.txt", bytes, true );
      System.Console.WriteLine( "File saved. File URL - " + savedFileURL );
    }

    static void LoginUser()
    {
      Backendless.UserService.Login( "joe@developer.com", "password" );
    }

    static void GetUserRoles()
    {
      IList<String> userRoles = Backendless.UserService.GetUserRoles();

      foreach( String roleName in userRoles )
        System.Console.WriteLine( roleName );
    }

    static void SavePerson()
    {
      Person person = new Person();
      person.name = "James Bond";
      person.age = 42;
      Backendless.Persistence.Of<Person>().Save( person );
    }

    static void DescribePerson()
    {
      List<ObjectProperty> properties = Backendless.Persistence.Describe( "Person" );

      foreach( ObjectProperty property in properties )
      {
        System.Console.WriteLine( "Property name - " + property.Name );
        System.Console.WriteLine( "Required property? - " + property.IsRequired );
        System.Console.WriteLine( "Type - " + property.Type );
        System.Console.WriteLine( "Is the property the primary key? - " + property.PrimaryKey );
        System.Console.WriteLine( "Default value - " + property.DefaultValue );
        System.Console.WriteLine( "Custom validator - " + property.CustomRegex );
        System.Console.WriteLine( "Is it a relation? (related table name) - " + property.RelatedTable );
        System.Console.WriteLine( "=====================================" );
      }
    }

    static void GetAndIncrement()
    {
      AsyncCallback<byte> callback = new AsyncCallback<byte>(
       result =>
       {
         System.Console.WriteLine( "[ASYNC] counter value is - " + result );
       },
       fault =>
       {
         System.Console.WriteLine( "Error - " + fault );
       } );

      Backendless.Counters.GetAndIncrement( "my counter", callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      int counter = myCounter.GetAndIncrement();
      System.Console.WriteLine( "[SYNC] counter value through IAtomic is - " + counter );

      counter = Backendless.Counters.GetAndIncrement( "my counter" );
      System.Console.WriteLine( "[SYNC] counter value is - " + counter );

      long longCounter = Backendless.Counters.GetAndIncrement<long>( "my counter" );
      System.Console.WriteLine( "[SYNC] counter value is - " + longCounter );
    }

    static void IncAndGet()
    {
      AsyncCallback<int> callback = new AsyncCallback<int>(
        result =>
        {
          System.Console.WriteLine( "[ASYNC] counter value is - " + result );
        },
        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Counters.IncrementAndGet( "my counter", callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      int counter = myCounter.IncrementAndGet();
      System.Console.WriteLine( "[SYNC] counter value through IAtomic is - " + counter );

      counter = Backendless.Counters.IncrementAndGet( "my counter" );
      System.Console.WriteLine( "[SYNC] counter value is - " + counter );

      long longCounter = Backendless.Counters.IncrementAndGet<long>( "my counter" );
      System.Console.WriteLine( "[SYNC] counter value is - " + longCounter );
    }

    static void GetAndDec()
    {
      AsyncCallback<int> callback = new AsyncCallback<int>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC] previous counter value is - " + result );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

      Backendless.Counters.GetAndDecrement( "my counter", callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      int counter = myCounter.GetAndDecrement();
      System.Console.WriteLine( "[SYNC] previous counter value through IAtomic is - " + counter );

      counter = Backendless.Counters.GetAndDecrement( "my counter" );
      System.Console.WriteLine( "[SYNC] previous counter value is - " + counter );

      long longCounter = Backendless.Counters.GetAndDecrement<long>( "my counter" );
      System.Console.WriteLine( "[SYNC] previous counter value is - " + longCounter );
    }

    static void DecAndGet()
    {
      AsyncCallback<int> callback = new AsyncCallback<int>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC] current counter value is - " + result );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

      Backendless.Counters.DecrementAndGet( "my counter", callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      int counter = myCounter.DecrementAndGet();
      System.Console.WriteLine( "[SYNC] current counter value through IAtomic is - " + counter );

      counter = Backendless.Counters.DecrementAndGet( "my counter" );
      System.Console.WriteLine( "[SYNC] current counter value is - " + counter );

      long longCounter = Backendless.Counters.DecrementAndGet<long>( "my counter" );
      System.Console.WriteLine( "[SYNC] current counter value is - " + longCounter );
    }

    static void AddAndGet()
    {
      AsyncCallback<int> callback = new AsyncCallback<int>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC] current counter value is - " + result );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

      Backendless.Counters.AddAndGet( "my counter", 1000, callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      int counter = myCounter.AddAndGet( 1000 );
      System.Console.WriteLine( "[SYNC] current counter value through IAtomic is - " + counter );

      counter = Backendless.Counters.AddAndGet( "my counter", 1000 );
      System.Console.WriteLine( "[SYNC] current counter value is - " + counter );

      long longCounter = Backendless.Counters.AddAndGet<long>( "my counter", 1000 );
      System.Console.WriteLine( "[SYNC] current counter value is - " + longCounter );
    }

    static void GetAndAdd()
    {
      AsyncCallback<int> callback = new AsyncCallback<int>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC] previous counter value is - " + result );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

      Backendless.Counters.GetAndAdd( "my counter", 1000, callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      int counter = myCounter.GetAndAdd( 1000 );
      System.Console.WriteLine( "[SYNC] previous counter value through IAtomic is - " + counter );

      counter = Backendless.Counters.GetAndAdd( "my counter", 1000 );
      System.Console.WriteLine( "[SYNC] previous counter value is - " + counter );

      long longCounter = Backendless.Counters.GetAndAdd<long>( "my counter", 1000 );
      System.Console.WriteLine( "[SYNC] previous counter value is - " + longCounter );
    }

    static void CondUpdate()
    {
      AsyncCallback<bool> callback = new AsyncCallback<bool>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC] valud has been updated? - " + result );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

      Backendless.Counters.CompareAndSet( "my counter", 1000, 2000, callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      bool updateResult = myCounter.CompareAndSet( 1000, 2000 );
      System.Console.WriteLine( "[SYNC] value has been updated? - " + updateResult );

      updateResult = Backendless.Counters.CompareAndSet( "my counter", 2000, 3000 );
      System.Console.WriteLine( "[SYNC] value has been updated? - " + updateResult );
    }

    static void reset()
    {
      AsyncCallback<Object> callback = new AsyncCallback<Object>(
  result =>
  {
    System.Console.WriteLine( "[ASYNC]counter has been reset" );
  },
  fault =>
  {
    System.Console.WriteLine( "Error - " + fault );
  } );

      Backendless.Counters.Reset( "my counter", callback );

      IAtomic<int> myCounter = Backendless.Counters.Of<int>( "my counter" );
      myCounter.Reset();
      System.Console.WriteLine( "[SYNC] counter has been reset" );

      Backendless.Counters.Reset( "my counter" );
      System.Console.WriteLine( "[SYNC] counter has been reset" );
    }



    static void CacheTest()
    {
      long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 72000000;
      try
      {
        AsyncCallback<Order> callback = new AsyncCallback<Order>(
        result =>
        {
          System.Console.WriteLine( "[ASYNC] received order object from cache - " + result.name );
        },
        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

        Order order = Backendless.Persistence.Of<Order>().FindFirst();
        Backendless.Cache.Put( "firstorder", order );

        // get object from cache asynchronously
        Backendless.Cache.Get<Order>( "firstorder", callback );

        // get object from cache synchronously
        order = Backendless.Cache.Get<Order>( "firstorder" );
        System.Console.WriteLine( "[SYNC] received order object from cache - " + order.name );
      }
      catch( System.Exception e )
      {
        System.Console.WriteLine( e );
      }
      //Backendless.Cache.ExpireIn( "foo", 10000000 );
    }

    static void LoginAndCreateObject()
    {
      Backendless.UserService.Login( "mark@backendless.com", "12345", true );
      Address a = new Address();
      a.city = "Dallas";
      Backendless.Persistence.Of<Address>().Save( a );
    }

    static void isValidSession()
    {
      System.Console.WriteLine( "Login is " + ( Backendless.UserService.IsValidLogin() ? " valid " : " not valid " ) );
    }

    static void RetrieveObjectWithSavedId()
    {
      Backendless.UserService.Logout();
      IList<Address> addresses = Backendless.Persistence.Of<Address>().Find();
      System.Console.WriteLine( addresses[ 0 ].city );
    }

    static void CustomEvent()
    {
      Dictionary<String, String> d = new Dictionary<string, string>();
      d.Add( "foo", "bar" );

        AsyncCallback<IDictionary> callback = new AsyncCallback<IDictionary>(
       result =>
       {
         System.Console.WriteLine( "received result - " + result );
       },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

        Backendless.Events.Dispatch( "test1", d, callback );

    }

    static void LoadRelations()
    {
      /*invoice invoice = Backendless.Persistence.Of<invoice>().FindFirst();
      List<String> relations = new List<String>();
      relations.Add( "items" );
      Backendless.Persistence.Of<invoice>().LoadRelations( invoice, relations );
      System.Console.WriteLine( "got relations" ); ;*/


      BackendlessDataQuery query = new BackendlessDataQuery();
      QueryOptions queryOptions = new QueryOptions();
      queryOptions.Related = new List<String>();
      queryOptions.Related.Add( "items" );
      query.QueryOptions = queryOptions;
      IList<invoice> invoices = Backendless.Persistence.Of<invoice>().Find( query );
      System.Console.WriteLine( "got relations " + invoices );
    }

    static void TestFind()
    {
      BackendlessDataQuery query = new BackendlessDataQuery( "customerName = 'John Doe'" );
      DateTime now = DateTime.Now;
      IList<Order> collection = Backendless.Persistence.Of<Order>().Find( query );
      double diff =  DateTime.Now.Subtract( now ).TotalMilliseconds;
      System.Console.WriteLine( collection.Count + "  " + diff );
    }

    static void TestSerialization()
    {
      CDObject obj = new CDObject();
      ParentObject parent = new ParentObject();
      parent.relations1 = new CDObject[] { obj };
      parent.relations2 = new CDObject[] { obj };
      Backendless.Persistence.Of<ParentObject>().Save( parent );
    }

    static void LoginAndGetDataObjects()
    {
      BackendlessUser user = Backendless.UserService.Login( "mpiller@gmail.com", "12345" );
      IList<Order> orders = Backendless.Persistence.Of<Order>().Find();
        System.Console.WriteLine( "got orders - " + orders );
    }

    static void LoginAndUpdate()
    {
      BackendlessUser user = Backendless.UserService.Login( "foo@foo.com", "12345" );
      Order order = Backendless.Persistence.Of<Order>().FindFirst();
      user.AddProperty( "order1", order );
      Backendless.Persistence.Of< BackendlessUser>().Save( user );
    }

    static void LoginAndLoadRelations()
    {
      Backendless.Persistence.MapTableToType( "Order", typeof( Order ) );
      Backendless.Persistence.MapTableToType( "OrderItem", typeof( OrderItem ) );
      BackendlessUser user = Backendless.UserService.Login( "foo@foo.com", "12345" );
      LoadRelationsQueryBuilder<Order> loadRelationsQueryBuilder = LoadRelationsQueryBuilder<Order>.Create();
      loadRelationsQueryBuilder.SetRelationName( "order1" );

      Backendless.Persistence.Of<BackendlessUser>().LoadRelations( user.ObjectId, loadRelationsQueryBuilder);
      Order order = (Order) user.GetProperty( "order1" );
     // System.Console.WriteLine( order.items[ 0 ].product ); 
    }

    static void AddUserToObject()
    {
      BackendlessUser user = Backendless.UserService.Login( "mpiller@gmail.com", "12345" );

      Manufacturer manufacturer = new Manufacturer();
      manufacturer.name = "Nintendo";

      OrderItem item1 = new OrderItem();
      item1.vendor = manufacturer;
      item1.product = "Wii";

      Order order1 = new Order();
      //order1.items = new List<OrderItem>() { item1 };
      //order1.user = user;

      Backendless.Persistence.Of<Order>().Save( order1 );
    }

    static void AddRelations()
    {
      for( int i = 0; i < 10; i++ )
      {
        Manufacturer manufacturer = new Manufacturer();
        manufacturer.name = "Sony";

        OrderItem item1 = new OrderItem();
        item1.vendor = manufacturer;
        item1.product = "Playstation";

        Order order1 = new Order();
       // order1.items = new List<OrderItem>() { item1 };

        Backendless.Persistence.Of<Order>().Save( order1 );
      }
    }

    static void ObjectUpdate()
    {
      Order order = Backendless.Persistence.Of<Order>().FindFirst();
     // System.Console.WriteLine( "loaded " + order.items[ 0 ].vendor.name );
      //order.items[ 0 ].product = "ABC";

      AsyncCallback<Order> callback = new AsyncCallback<Order>(
       result =>
       {
         System.Console.WriteLine( "object has been placed into cache" );
       },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Persistence.Of<Order>().Save( order, callback );
    }

    static void SendEmail()
    {
      AsyncCallback<object> responder = new AsyncCallback<object>(
        result =>
        {
          System.Console.WriteLine( "message sent" );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      // async request. Plain text message to one recipient
      Backendless.Messaging.SendTextEmail( "Reminder", "Hey JB! Your car will be ready by 5pm", "mpiller@gmail.com", responder );

      // sync request. HTML messahe to multiple recipients
      List<String> recipients = new List<String>();
      recipients.Add( "mpiller@gmail.com" );

      String mailBody = "Guys, the dinner last night was <b>awesome</b>";

      Backendless.Messaging.SendHTMLEmail( "Dinner", mailBody, recipients );

      System.Console.WriteLine( "[SYNC] email has been sent" );
    }

    static void SearchPoints()
    {
      AsyncCallback<IList<GeoPoint>> callback = new AsyncCallback<IList<GeoPoint>>(
        result =>
        {
          System.Console.WriteLine( "Found geo points - " + result );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
      geoQuery.Categories.Add( "Tizen" );
      geoQuery.Latitude = 41.48;
      geoQuery.Longitude = 2.15;
      geoQuery.Radius = 100000;
      geoQuery.SearchRectangle = new double[] {32.78, -96.8, 25.79, -80.22};
      geoQuery.Units = BackendlessAPI.Geo.Units.METERS;

      geoQuery.IncludeMeta = true;

      Backendless.Geo.GetPoints( geoQuery, callback );
    }

    static void SavePoint()
    {
      AsyncCallback<BackendlessAPI.Geo.GeoPoint> callback = new AsyncCallback<BackendlessAPI.Geo.GeoPoint>(
        result =>
        {
          System.Console.WriteLine( "Geo point saved - " + result.ObjectId );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      List<String> categories = new List<string>();
      categories.Add( "restaurants" );
      categories.Add( "cool_places" );

      Dictionary<String, String> metadata = new Dictionary<string, string>();
      metadata.Add( "name", "Eatzi's" );
      GeoPoint geoPoint = new GeoPoint( 32.81, -96.80, categories, metadata );

      BackendlessAPI.Backendless.Geo.SavePoint( geoPoint, callback );
    }

    static void CreateCategory()
    {
      AsyncCallback<List<BackendlessAPI.Geo.GeoCategory>> callback = new AsyncCallback<List<BackendlessAPI.Geo.GeoCategory>>(
        result =>
        {
          System.Console.WriteLine( "Categories received - " + result.Count );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      BackendlessAPI.Backendless.Geo.GetCategories( callback );
    }

    static void UploadFile()
    {
      AsyncCallback<BackendlessAPI.File.BackendlessFile> callback = new AsyncCallback<BackendlessAPI.File.BackendlessFile>(
        result =>
        {
          System.Console.WriteLine( "File uploaded. URL - " + result.FileURL );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      FileStream fs = new FileStream( "test.txt", FileMode.Open, FileAccess.Read );
      BackendlessAPI.Backendless.Files.Upload( fs, "myfiles/test", callback );
    }

    static void SaveCustomer()
    {
      Customer customer = new Customer();
      customer.Name = "ACME Corp";
      customer.PhoneNumber = "314 555-1212";

      Address address = new Address();
      address.street = "123 Main St";
      address.city = "Dallas";
      address.state = "TX";
      address.zipCode = "75034";

      customer.customerAddress = address;

      Customer savedCustomer = Backendless.Persistence.Of<Customer>().Save( customer );
      System.Console.WriteLine( String.Format( "Customer saved. Object ID - {0}", savedCustomer.objectId ) );
    }

    static void UpdateCustomer()
    {
      Customer c = Backendless.Persistence.Of<Customer>().FindFirst();
      c.Name = "Bob Marley";
      Customer savedCustomer = Backendless.Persistence.Of<Customer>().Save( c );
      System.Console.WriteLine( savedCustomer.Name );
    }

    static void AddMoreCustomers( int n )
    {
      for( int i = 0; i < n; i++ )
      {
        Customer customer = new Customer();
        customer.Name = "ACME Corp " + i;
        customer.PhoneNumber = "314 555-1212";
        customer.Orders = i * 10;

        Address address = new Address();
        address.street = "123 Main St";
        address.city = "Dallas";
        address.state = "TX";
        address.zipCode = "75034";

        customer.customerAddress = address;

        Customer savedCustomer = Backendless.Persistence.Of<Customer>().Save( customer );
        System.Console.WriteLine( String.Format( "Customer saved. Object ID - {0}", savedCustomer.objectId ) );
      }
    }

    static void FindCustomers()
    {
      BackendlessDataQuery query = new BackendlessDataQuery();
      query.WhereClause = "Orders >= 100";
      QueryOptions options = new QueryOptions();
      options.Related = new List<String>();
      options.Related.Add( "customerAddress" );
      query.QueryOptions = options;
      IList<Customer> customers = Backendless.Persistence.Of<Customer>().Find( query );

      foreach( Customer c in customers )
        System.Console.WriteLine( String.Format( "Found customer. Orders - {0}. Customer ID - {1}. Customer city - {2}", c.Orders, c.objectId, c.customerAddress.city ) ); 
    }

    static MessageStatus PublishMessage()
    {
      // synchronous publishing
      MessageStatus status = Backendless.Messaging.Publish( "hello world" );
      System.Console.WriteLine( "Message published. Message ID - " + status.MessageId + ". Message Status - " + status.Status );
      return status;
    }

    static void PublishMessageAsync()
    {
      // asynchronous publishing
      AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        result =>
        {
          System.Console.WriteLine( "Message published. Message ID - " + result.MessageId + ". Message Status - " + result.Status );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      Backendless.Messaging.Publish( "Hello world", callback );
      System.Console.WriteLine( "Press [Enter] to continue" );
      System.Console.ReadLine();
    }

      static void CheckMessageStatus(MessageStatus status)
      {
          MessageStatus receivedStatus = Backendless.Messaging.GetMessageStatus(status.MessageId);
          System.Console.WriteLine("Message status. Message ID - " + receivedStatus.MessageId + ". Message Status - " + receivedStatus.Status +". Error - " + (string.IsNullOrEmpty(receivedStatus.ErrorMessage)? receivedStatus.ErrorMessage:string.Empty));
      }

      static void CheckMessageStatusAsync(MessageStatus status)
      {
          AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
              result =>
          {
              System.Console.WriteLine("Message status. Message ID - " + result.MessageId + ". Message Status - " + result.Status + ". Error - " + (string.IsNullOrEmpty(result.ErrorMessage) ? result.ErrorMessage : string.Empty));
          },

          fault =>
          {
              System.Console.WriteLine("Error - " + fault);
          });

          Backendless.Messaging.GetMessageStatus(status.MessageId, callback);
          System.Console.WriteLine("Press [Enter] to continue");
          System.Console.ReadLine();
        }

        static void PublishMessageWithHeaders()
     {
      // synchronous publishing
      PublishOptions publishOptions = new PublishOptions();
      publishOptions.AddHeader( "city", "Tokyo" );

      Weather weather = new Weather();
      weather.Humidity = 80;
      weather.Temperature = 78;

      MessageStatus status = Backendless.Messaging.Publish( weather, publishOptions );
      System.Console.WriteLine( "Message published. Message ID - " + status.MessageId + ". Message Status - " + status.Status );
    }

    static void PublishMessageWithHeadersAsync()
    {
      // asynchronous publishing
      AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        result =>
        {
          System.Console.WriteLine( "Message published. Message ID - " + result.MessageId + ". Message Status - " + result.Status );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );
      
      PublishOptions publishOptions = new PublishOptions();
      publishOptions.AddHeader( "city", "Tokyo" );

      Weather weather = new Weather();
      weather.Humidity = 80;
      weather.Temperature = 78;

      Backendless.Messaging.Publish( weather, publishOptions, callback );
      System.Console.WriteLine( "Press [Enter] to continue" );
      System.Console.ReadLine();
    }

    static void PublishMessageToSubtopic()
    {
      // synchronous publishing
      PublishOptions publishOptions = new PublishOptions();
      publishOptions.Subtopic = "news.business.newyork";

      MessageStatus status = Backendless.Messaging.Publish( "Get free coffee at Moonbucks today", publishOptions );
      System.Console.WriteLine( "Message published. Message ID - " + status.MessageId + ". Message Status - " + status.Status );
    }

    static void PublishMessageToSubtopicAsync()
    {
      // asynchronous publishing
      AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        result =>
        {
          System.Console.WriteLine( "Message published. Message ID - " + result.MessageId + ". Message Status - " + result.Status );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      PublishOptions publishOptions = new PublishOptions();
      publishOptions.Subtopic = "news.business.newyork";

      Backendless.Messaging.Publish( "Get free coffee at Moonbucks today", publishOptions, callback );
      System.Console.WriteLine( "Press [Enter] to continue" );
      System.Console.ReadLine();
    }

    static void PublishMessageAsPush()
    {
      // synchronous publishing
      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PushBroadcast = DeliveryOptions.ALL;
      deliveryOptions.PushPolicy = PushPolicyEnum.ONLY;

      MessageStatus status = Backendless.Messaging.Publish( "Hi Android!", deliveryOptions );
      System.Console.WriteLine( "Message published. Message ID - " + status.MessageId + ". Message Status - " + status.Status );

      if( status.Status == PublishStatusEnum.FAILED )
        System.Console.WriteLine( "Message published failed with error " + status.ErrorMessage );
    }

    static void PublishMessageAsPushAsync()
    {
      // asynchronous publishing
      AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        result =>
        {
          System.Console.WriteLine( "Message published. Message ID - " + result.MessageId + ". Message Status - " + result.Status );

          if( result.Status == PublishStatusEnum.FAILED )
            System.Console.WriteLine( "Message published failed with error " + result.ErrorMessage );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PushBroadcast = DeliveryOptions.ALL;
      deliveryOptions.PushPolicy = PushPolicyEnum.ONLY;

      Backendless.Messaging.Publish( "Hi Android!", deliveryOptions, callback );
      System.Console.WriteLine( "Press [Enter] to continue" );
      System.Console.ReadLine();
    }

    static void PublishToAndroidNotificationCenter()
    {
      // synchronous publishing
      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PushBroadcast = DeliveryOptions.ANDROID;
      deliveryOptions.PushPolicy = PushPolicyEnum.ONLY;

      PublishOptions publishOptions = new PublishOptions();
      publishOptions.AddHeader( "android-ticker-text", "You just got a push notification!" );
      publishOptions.AddHeader( "android-content-title", "This is a notification title" );
      publishOptions.AddHeader( "android-content-text", "Push Notifications are cool" );

      MessageStatus status = Backendless.Messaging.Publish( "Hi Android!", publishOptions, deliveryOptions );
      System.Console.WriteLine( "Message published. Message ID - " + status.MessageId + ". Message Status - " + status.Status );

      if( status.Status == PublishStatusEnum.FAILED )
        System.Console.WriteLine( "Message published failed with error " + status.ErrorMessage );
    }

    static void PublishToAndroidNotificationCenterAsync()
    {
      // asynchronous publishing
      AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        result =>
        {
          System.Console.WriteLine( "Message published. Message ID - " + result.MessageId + ". Message Status - " + result.Status );

          if( result.Status == PublishStatusEnum.FAILED )
            System.Console.WriteLine( "Message published failed with error " + result.ErrorMessage );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PushBroadcast = DeliveryOptions.ANDROID;
      deliveryOptions.PushPolicy = PushPolicyEnum.ONLY;

      PublishOptions publishOptions = new PublishOptions();
      publishOptions.AddHeader( "android-ticker-text", "You just got a push notification!" );
      publishOptions.AddHeader( "android-content-title", "This is a notification title" );
      publishOptions.AddHeader( "android-content-text", "Push Notifications are cool" );

      Backendless.Messaging.Publish( "Hi Android!", publishOptions, deliveryOptions, callback );
      System.Console.WriteLine( "Press [Enter] to continue" );
      System.Console.ReadLine();
    }

    static void PublishMessageDelayed()
    {
      // synchronous publishing
      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PublishAt = DateTime.Now.AddSeconds( 20 );
      deliveryOptions.RepeatEvery = 20;
      deliveryOptions.RepeatExpiresAt = DateTime.Now.AddHours( 1 );

      MessageStatus status = Backendless.Messaging.Publish( "This message was scheduled 20 seconds ago", deliveryOptions );
      System.Console.WriteLine( "Message published. Message ID - " + status.MessageId + ". Message Status - " + status.Status );

      if( status.Status == PublishStatusEnum.FAILED )
        System.Console.WriteLine( "Message published failed with error " + status.ErrorMessage );
    }

    static void PublishMessageDelayedAsync()
    {
      // asynchronous publishing
      AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        result =>
        {
          System.Console.WriteLine( "Message published. Message ID - " + result.MessageId + ". Message Status - " + result.Status );

          if( result.Status == PublishStatusEnum.FAILED )
            System.Console.WriteLine( "Message published failed with error " + result.ErrorMessage );
        },

        fault =>
        {
          System.Console.WriteLine( "Error - " + fault );
        } );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PublishAt = DateTime.Now.AddSeconds( 20 );

      Backendless.Messaging.Publish( "Hi Android!", deliveryOptions, callback );
      System.Console.WriteLine( "Press [Enter] to continue" );
      System.Console.ReadLine();
    }

    static void Register()
    {
      AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
        user =>
        {
          System.Console.WriteLine( "User registered. Assigned ID - " + user.ObjectId );

          Dictionary<string, object> props = user.Properties;

          foreach( KeyValuePair<string, object> pair in props )
            System.Console.WriteLine( String.Format( "Property: {0} - {1}", pair.Key, pair.Value ) );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );


      BackendlessUser newUser = new BackendlessUser();
      newUser.SetProperty( "login", "james.bond" );
      newUser.SetProperty( "email", "jb@mi6.co.uk" );
      newUser.Password = "guessIt";
      Backendless.UserService.Register( newUser, callback );
      System.Console.In.ReadLine();
    }

    static void RegisterSync()
    {
      BackendlessUser newUser = new BackendlessUser();
      newUser.SetProperty( "login", "james.bond123" );
      newUser.SetProperty( "email", "jb@mi6.co.uk" );
      newUser.Password = "guessIt";

      try
      {
        BackendlessUser user = Backendless.UserService.Register( newUser );

        System.Console.WriteLine( "User registered. Assigned ID - " + user.GetProperty( "objectId" ) );

        Dictionary<string, object> props = user.Properties;

        foreach( KeyValuePair<string, object> pair in props )
          System.Console.WriteLine( String.Format( "Property: {0} - {1}", pair.Key, pair.Value ) );
      }
      catch( BackendlessException ex )
      {
        System.Console.WriteLine( ex.BackendlessFault.ToString() );
      }
    }

    static void Login()
    {
      AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
        user =>
        {
          System.Console.WriteLine( "User logged in. Assigned ID - " + user.GetProperty( "objectId" ) );

          Dictionary<string, object> props = user.Properties;

          foreach( KeyValuePair<string, object> pair in props )
            System.Console.WriteLine( String.Format( "Property: {0} - {1}", pair.Key, pair.Value ) );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      String login = "james.bond123";
      String password = "guessIt";
      Backendless.UserService.Login( login, password, callback );
      System.Console.In.ReadLine();
    }

    static BackendlessUser LoginSync()
    {
      String login = "james.bond123";
      String password = "guessIt";
      BackendlessUser user = Backendless.UserService.Login( login, password );
      System.Console.WriteLine( "User logged in. Assigned ID - " + user.GetProperty( "objectId" ) );

      Dictionary<string, object> props = user.Properties;

      foreach( KeyValuePair<string, object> pair in props )
        System.Console.WriteLine( String.Format( "Property: {0} - {1}", pair.Key, pair.Value ) );

      return user;
    }

    static void UpdateUser()
    {
      AsyncCallback<BackendlessUser> updateCallback = new AsyncCallback<BackendlessUser>(
        user =>
        {
          System.Console.WriteLine( "User account has been updated" );

          Dictionary<string, object> props = user.Properties;

          foreach( KeyValuePair<string, object> pair in props )
            System.Console.WriteLine( String.Format( "Property: {0} - {1}", pair.Key, pair.Value ) );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      AsyncCallback<BackendlessUser> loginCallback = new AsyncCallback<BackendlessUser>(
        user =>
        {
          user.SetProperty( "login", "james.bond.123" );
          user.SetProperty( "email", "james.bond@mi6.co.uk" );
          Backendless.UserService.Update( user, updateCallback );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      String login = "james.bond.www";
      String password = "guessIt";
      Backendless.UserService.Login( login, password, loginCallback );
      System.Console.In.ReadLine();
    }

    static void UpdateUserSync()
    {
      String login = "james.bond.www";
      String password = "guessIt";
      BackendlessUser user = Backendless.UserService.Login( login, password );

      user.SetProperty( "email", "foo@bar.com" );

      Customer customer = new Customer();
      customer.Name = "Backendless Corp";
      customer.PhoneNumber = "972 555-1212";

      Address address = new Address();
      address.street = "2591 Dallas Pkwy, Ste 300";
      address.city = "Frisco";
      address.state = "TX";
      address.zipCode = "75034";
      customer.customerAddress = address;

      List<Customer> customers = new List<Customer>();
      customers.Add( customer );
      user.SetProperty( "customers", customers );
      Backendless.UserService.Update( user );
   
      System.Console.WriteLine( "User account has been updated" );

      Dictionary<string, object> props = user.Properties;

      foreach( KeyValuePair<string, object> pair in props )
        System.Console.WriteLine( String.Format( "Property: {0} - {1}", pair.Key, pair.Value ) );
    }

    static void Logout()
    {
      AsyncCallback<Object> logoutCallback = new AsyncCallback<Object>(
        user =>
        {
          System.Console.WriteLine( "User has been logged out" );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      AsyncCallback<BackendlessUser> loginCallback = new AsyncCallback<BackendlessUser>(
        user =>
        {
          Backendless.UserService.Logout( logoutCallback );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      String login = "james.bond.www";
      String password = "guessIt";
      Backendless.UserService.Login( login, password, loginCallback );
      System.Console.In.ReadLine();
    }

    static void LogoutSync()
    {
      String login = "james.bond.www";
      String password = "guessIt";
      Backendless.UserService.Login( login, password );
      Backendless.UserService.Logout();
      System.Console.WriteLine( "User has been logged out" );
    }

    static void PasswordRecovery()
    {
      AsyncCallback<Object> pwRecoveryCallback = new AsyncCallback<Object>(
        user =>
        {
          System.Console.WriteLine( "Password recovery email has been sent" );
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      Backendless.UserService.RestorePassword( "james.bond.www", pwRecoveryCallback );
      System.Console.In.ReadLine();
    }

    static void DescribeUser()
    {
      AsyncCallback<List<UserProperty>> callback = new AsyncCallback<List<UserProperty>>(
        props =>
        {
          foreach( UserProperty p in props )
          {
            System.Console.WriteLine( "prop name " + p.Name );
            System.Console.WriteLine( "\tis identity " + p.IsIdentity );
            System.Console.WriteLine( "\tis required " + p.IsRequired );
            System.Console.WriteLine( "\tprop type " + p.Type );
            System.Console.WriteLine( "\tdefault value " + p.DefaultValue );
          }
        },

        fault =>
        {
          System.Console.WriteLine( fault.ToString() );
        } );

      Backendless.UserService.DescribeUserClass( callback );
      System.Console.In.ReadLine();
    }
  }
}
