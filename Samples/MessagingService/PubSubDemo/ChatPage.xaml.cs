using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Messaging;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.PubSubDemo
{
  public partial class ChatPage : PhoneApplicationPage
  {
    private string _userName;

    public ChatPage()
    {
      InitializeComponent();
    }

    protected override void OnNavigatedTo( NavigationEventArgs e )
    {
      base.OnNavigatedTo( e );
      _userName = NavigationContext.QueryString[Defaults.NAME_TAG];

      if( string.IsNullOrEmpty( _userName ) )
        NavigationService.GoBack();

      Backendless.Messaging.Subscribe( new AsyncCallback<List<Message>>( response => Dispatcher.BeginInvoke( () =>
        {
          foreach( Message message in response )
            HistoryField.Text = message.PublisherId + ": " + message.Data + "\n" + HistoryField.Text;
        } ), HandleFault ), new AsyncCallback<Subscription>( response =>
          {
            /*NO ACTION*/
          }, HandleFault ) );
    }

    private void MessageField_OnKeyUp( object sender, KeyEventArgs e )
    {
      if( e.Key.Equals( Key.Enter ) )
        SendMesssage();
    }

    private void SendButton_Click( object sender, RoutedEventArgs e )
    {
      SendMesssage();
    }

    private void SendMesssage()
    {
      if( string.IsNullOrEmpty( MessageField.Text ) )
        return;

      Backendless.Messaging.Publish( MessageField.Text, new PublishOptions( _userName ),
                                     new AsyncCallback<MessageStatus>( response => Dispatcher.BeginInvoke( () => { } ),
                                                                       HandleFault ) );
      MessageField.Text = "";
    }

    private void HandleFault( BackendlessFault fault )
    {
      Dispatcher.BeginInvoke( () => MessageBox.Show( fault.Message ) );
    }
  }
}