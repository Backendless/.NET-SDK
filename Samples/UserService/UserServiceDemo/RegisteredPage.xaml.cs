using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Examples.MessagingService.UserServiceDemo
{
  public partial class RegisteredPage : PhoneApplicationPage
  {
    public RegisteredPage()
    {
      InitializeComponent();
    }

    protected override void OnBackKeyPress( CancelEventArgs e )
    {
      NavigationService.Navigate( new Uri( "/MainPage.xaml", UriKind.Relative ) );
    }
  }
}