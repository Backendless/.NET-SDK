using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;

using Weborb.Config;
using Weborb.Client;
using Weborb.Util;
using Weborb.Exceptions;

namespace Weborb.Util.License
{
  public class LicensingService
  {
    private static ProductInfo WEBORBPRODUCT = new ProductInfo(
        LicenseManager.WEBORB_PRODUCT_ID,
        "WebORB for .NET",
        "weborblicense.key.config",
        typeof( LicensingService ).Assembly.FullName );
    private const String GATEWAY_URL = "http://www.themidnightcoders.com/licensing/weborb.aspx";

    public static ProductInfo[] getInstalledProducts()
    {
      if( DeploymentMode.IsCloud() )
        return null;

      List<ProductInfo> products = new List<ProductInfo>();
      products.Add( WEBORBPRODUCT );

      AppDomain domain = AppDomain.CurrentDomain;
      Assembly[] assemblies = domain.GetAssemblies();

      foreach( Assembly assembly in assemblies )
      {
        AssemblyName assName = new AssemblyName( assembly.FullName );
        Type t = assembly.GetType( assName.Name + ".ProductInfo", false );

        if( t != null )
        {
          object prodInfo = ObjectFactories.CreateServiceObject( t );
          MethodInfo method = prodInfo.GetType().GetMethod( "GetProductInfo" );

          try
          {
            products.Add( (ProductInfo) method.Invoke( prodInfo, null ) );
          }
          catch( Exception )
          {
          }
        }
      }

      return products.ToArray();
    }

    public static ProductInfo getProduct( int productID )
    {
      if( productID == LicenseManager.WEBORB_PRODUCT_ID )
        return WEBORBPRODUCT;

      ProductInfo[] products = getInstalledProducts();

      foreach( ProductInfo product in products )
        if( product.id == productID )
          return product;

      return null;
    }
#if !CLOUD
    public LicenseKeyInfo getLicenseKeyInfo( int productId )
    {
      return LicenseManager.GetInstance( productId ).getLicenseKeyInfo();
    }

    public LicenseKeyInfo activateByKey( String licenseKey, String activationKey )
    {
      try
      {
        LicenseKeyInfo licenseKeyInfo = LicenseKeyDecoder.decode( licenseKey, activationKey );
        String licensingPath = Paths.GetLicensesPath();

        if( !Directory.Exists( licensingPath ) )
          Directory.CreateDirectory( licensingPath ).Create();

        ProductInfo product = getProduct( licenseKeyInfo.product );

        if( product == null )
          throw new LicenseException( "Unknown product ID - " + licenseKeyInfo.product +
                                     ". Contact Midnight Coders customer support. License key - " + licenseKey );

        String licensePath = Path.Combine( licensingPath, product.licenseFileName );
        LicenseFile licenseFile = LicenseFile.Load( licensePath );
        licenseFile.licenseKey = licenseKey;
        licenseFile.activationKey = activationKey;
        licenseFile.Save();
        LicenseManager.GetInstance( product.id ).reset();
        return licenseKeyInfo;
      }
      catch( Exception )
      {
        throw new LicenseException( "Product cannot be activated. Contact Midnight Coders customer support. License key - " + licenseKey );
      }
    }

    public String activate( RegistrationInfo registrationInfo, String licenseKey )
    {
      WeborbClient weborbClient = new WeborbClient( GATEWAY_URL );
      String result = String.Empty;
      Object signal = new Object();

      weborbClient.Invoke<String>( "com.tmc.license.manager.LicenseManagerService",
          "activate",
          new object[] { registrationInfo, licenseKey },
          new Responder<string>(
            delegate( String value )
            {
              result = value;

              if( value.StartsWith( "OK" ) )
              {
                String activationKey = value.Substring( 3 );

                try
                {
                  activateByKey( licenseKey, activationKey );
                }
                catch( Exception exception )
                {
                  result = "FAULT:" + exception.ToString();
                }
              }

              lock( signal )
                Monitor.Pulse( signal );
            },
            delegate( Fault fault )
            {
              result = "FAULT:" + fault.Message;

              lock( signal )
                Monitor.Pulse( signal );
            } ) );


      lock( signal )
        Monitor.Wait( signal );

      return result;
    }
#endif
  }
}
