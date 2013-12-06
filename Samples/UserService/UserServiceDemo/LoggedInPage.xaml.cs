using System;
using System.Windows;
using BackendlessAPI;
using BackendlessAPI.Async;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.UserServiceDemo
{
  public partial class LoggedInPage : PhoneApplicationPage
  {
    public LoggedInPage()
    {
      InitializeComponent();
    }

    private void LogoutButton_Click( object sender, RoutedEventArgs e )
    {
      Backendless.UserService.Logout(
        new AsyncCallback<object>(
          response =>
          Dispatcher.BeginInvoke( () => NavigationService.Navigate( new Uri( "/MainPage.xaml", UriKind.Relative ) ) ),
          fault => Dispatcher.BeginInvoke( () => MessageBox.Show( fault.Message ) ) ) );
    }
  }
}