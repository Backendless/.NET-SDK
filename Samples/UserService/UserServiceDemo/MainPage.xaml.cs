using System;
using System.Windows;
using BackendlessAPI;
using BackendlessAPI.Async;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.UserServiceDemo
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();
      this.Loaded += new RoutedEventHandler( MainPage_Loaded );
    }

    void MainPage_Loaded( object sender, RoutedEventArgs e )
    {
      if( string.IsNullOrEmpty( Defaults.APPLICATION_ID ) || string.IsNullOrEmpty( Defaults.SECRET_KEY ) ||
          string.IsNullOrEmpty( Defaults.VERSION ) )
      {
        NavigationService.Navigate( new Uri( "/ErrorPage.xaml", UriKind.Relative ) );
        return;
      }

      Backendless.InitApp( Defaults.APPLICATION_ID, Defaults.SECRET_KEY, Defaults.VERSION );
    }

    private void LoginButton_Click( object sender, RoutedEventArgs e )
    {
      if( string.IsNullOrEmpty( EmailField.Text ) || string.IsNullOrEmpty( PasswordField.Password ) )
      {
        MessageBox.Show( "Enter your email and password" );
        return;
      }

      Backendless.UserService.Login( EmailField.Text, PasswordField.Password,
                                     new AsyncCallback<BackendlessUser>(
                                       response =>
                                       Dispatcher.BeginInvoke(
                                         () =>
                                         NavigationService.Navigate( new Uri( "/LoggedInPage.xaml", UriKind.Relative ) ) ),
                                       fault => Dispatcher.BeginInvoke( () => MessageBox.Show( fault.Message ) ) ) );
    }

    private void RegisterButton_Click( object sender, RoutedEventArgs e )
    {
      NavigationService.Navigate( new Uri( "/RegistrationPage.xaml", UriKind.Relative ) );
    }
  }
}