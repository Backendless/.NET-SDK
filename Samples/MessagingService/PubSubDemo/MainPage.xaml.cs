using System;
using System.Windows;
using BackendlessAPI;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.PubSubDemo
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

    private void StartChat_Click( object sender, RoutedEventArgs e )
    {
      if( string.IsNullOrEmpty( NameField.Text ) )
        Dispatcher.BeginInvoke( () => MessageBox.Show( "Please enter your name" ) );
      else
        NavigationService.Navigate( new Uri( "/ChatPage.xaml?" + Defaults.NAME_TAG + "=" + NameField.Text,
                                             UriKind.Relative ) );
    }
  }
}