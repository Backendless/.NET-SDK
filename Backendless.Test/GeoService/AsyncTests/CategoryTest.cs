using System;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.AsyncTests
{
  [TestClass]
  public class CategoryTest : TestsFrame
  {
    [TestMethod]
    public void TestAddNullCategory()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.AddCategory( null,
                                     new AsyncCallback<GeoCategory>(
                                       response => FailCountDownWith( "Client have send a null category" ),
                                       fault => CheckErrorCode( ExceptionMessage.NULL_CATEGORY_NAME, fault ) ) ) );
    }

    [TestMethod]
    public void TestDeleteNullCategory()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.DeleteCategory( null,
                                        new AsyncCallback<Boolean>(
                                          response => FailCountDownWith( "Client have send a null category" ),
                                          fault => CheckErrorCode( ExceptionMessage.NULL_CATEGORY_NAME, fault ) ) ) );
    }

    [TestMethod]
    public void TestAddEmptyCategory()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.AddCategory( "",
                                     new AsyncCallback<GeoCategory>(
                                       response => FailCountDownWith( "Client have send an empty category" ),
                                       fault => CheckErrorCode( ExceptionMessage.NULL_CATEGORY_NAME, fault ) ) ) );
    }

    [TestMethod]
    public void TestDeleteEmptyCategory()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.DeleteCategory( "",
                                        new AsyncCallback<Boolean>(
                                          response => FailCountDownWith( "Client have send an empty category" ),
                                          fault => CheckErrorCode( ExceptionMessage.NULL_CATEGORY_NAME, fault ) ) ) );
    }

    [TestMethod]
    public void TestAddDefaultCategory()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.AddCategory( DEFAULT_CATEGORY_NAME,
                                     new AsyncCallback<GeoCategory>(
                                       response => FailCountDownWith( "Client have send a default category" ),
                                       fault => CheckErrorCode( ExceptionMessage.DEFAULT_CATEGORY_NAME, fault ) ) ) );
    }

    [TestMethod]
    public void TestDeleteDefaultCategory()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.DeleteCategory( DEFAULT_CATEGORY_NAME,
                                        new AsyncCallback<Boolean>(
                                          response => FailCountDownWith( "Client have send a default category" ),
                                          fault => CheckErrorCode( ExceptionMessage.DEFAULT_CATEGORY_NAME, fault ) ) ) );
    }

    [TestMethod]
    public void TestAddProperCategory()
    {
      string categoryName = GetRandomCategory();
      RunAndAwait(
        () =>
        Backendless.Geo.AddCategory( categoryName,
                                     new ResponseCallback<GeoCategory>( this )
                                       {
                                         ResponseHandler = geoCategory =>
                                           {
                                             Assert.IsNotNull( geoCategory, "Server returned a null category" );
                                             Assert.AreEqual( categoryName, geoCategory.Name,
                                                              "Server returned a category with a wrong name" );
                                             Assert.IsNotNull( geoCategory.Id, "Server returned a category with a null id" );
                                             Assert.IsTrue( geoCategory.Size == 0,
                                                            "Server returned a category with a wrong size" );
                                             CountDown();
                                           }
                                       } ) );
    }

    [TestMethod]
    public void TestAddSameCategoryTwice()
    {
      string categoryName = GetRandomCategory();
      RunAndAwait(
        () =>
        Backendless.Geo.AddCategory( categoryName,
                                     new ResponseCallback<GeoCategory>( this )
                                       {
                                         ResponseHandler =
                                           response =>
                                           Backendless.Geo.AddCategory( categoryName,
                                                                        new ResponseCallback<GeoCategory>( this )
                                                                          {
                                                                            ResponseHandler = geoCategory =>
                                                                              {
                                                                                Assert.IsNotNull( geoCategory,
                                                                                                  "Server returned a null category" );
                                                                                Assert.AreEqual( categoryName,
                                                                                                 geoCategory.Name,
                                                                                                 "Server returned a category with a wrong name" );
                                                                                Assert.IsNotNull( geoCategory.Id,
                                                                                                  "Server returned a category with a null id" );
                                                                                Assert.IsTrue( geoCategory.Size == 0,
                                                                                               "Server returned a category with a wrong size" );
                                                                                CountDown();
                                                                              }
                                                                          } )
                                       } ) );
    }

    [TestMethod]
    public void TestRemoveCategory()
    {
      string categoryName = GetRandomCategory();
      RunAndAwait(
        () =>
        Backendless.Geo.AddCategory( categoryName,
                                     new ResponseCallback<GeoCategory>( this )
                                       {
                                         ResponseHandler =
                                           geoCategory =>
                                           Backendless.Geo.DeleteCategory( categoryName,
                                                                           new ResponseCallback<Boolean>( this )
                                                                             {
                                                                               ResponseHandler = response =>
                                                                                 {
                                                                                   Assert.IsTrue( response,
                                                                                                  "Server returned wrong status" );
                                                                                   CountDown();
                                                                                 }
                                                                             } )
                                       } ) );
    }

    [TestMethod]
    public void TestRemoveUnexistingCategory()
    {
      string categoryName = GetRandomCategory();

      RunAndAwait(
        () =>
        Backendless.Geo.DeleteCategory( categoryName,
                                        new AsyncCallback<bool>(
                                          response => FailCountDownWith( "Server deleted unexisting category" ),
                                          fault => CheckErrorCode( 4001, fault ) ) ) );
    }
  }
}