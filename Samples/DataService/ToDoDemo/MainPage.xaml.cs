using System;
using System.Windows;
using System.Windows.Input;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using Examples.MessagingService.ToDoDemo;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;

namespace ToDoDemo
{
  public partial class MainPage : PhoneApplicationPage
  {
    public static IDataStore<ToDoEntity> DataStore;
    private readonly string _deviceId;
    private readonly ToDoList _toDoList = new ToDoList();

    public MainPage()
    {
      InitializeComponent();
      _deviceId =
        Convert.ToBase64String( DeviceExtendedProperties.GetValue( "DeviceUniqueId" ) as byte[] ??
                                Guid.NewGuid().ToByteArray() );

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
      DataStore = Backendless.Persistence.Of<ToDoEntity>();

      EntitiesDataGrid.DataContext = _toDoList;
      _toDoList.CollectionChanged +=
        ( senderObj, args ) => Footer.Visibility = _toDoList.Count == 0 ? Visibility.Collapsed : Visibility.Visible;

      AsyncStartedEvent += () =>
      {
        ProgressBar.Visibility = Visibility.Visible;
        ContentPanel.Opacity = 0.1;
      };
      AsyncFinishedEvent += () =>
      {
        ProgressBar.Visibility = Visibility.Collapsed;
        ContentPanel.Opacity = 1;
      };

      AsyncStartedEvent.Invoke();
      DataStore.Find( new AsyncCallback<BackendlessCollection<ToDoEntity>>( response => Dispatcher.BeginInvoke( () =>
      {
        _toDoList.AddAll( response.GetCurrentPage() );
        AsyncFinishedEvent.Invoke();
      } ), fault => Dispatcher.BeginInvoke( () => AsyncFinishedEvent.Invoke() ) ) );
    }

    private void NewToDoFIeld_KeyDown( object sender, KeyEventArgs e )
    {
      if( !e.Key.Equals( Key.Enter ) || string.IsNullOrEmpty( NewToDoField.Text ) )
        return;

      AsyncStartedEvent.Invoke();
      DataStore.Save( new ToDoEntity {DeviceId = _deviceId, Text = NewToDoField.Text},
                      new AsyncCallback<ToDoEntity>( response => Dispatcher.BeginInvoke( () =>
                        {
                          _toDoList.Add( new ToDoEntityProxy( response ) );
                          NewToDoField.Text = "What needs to be done?";
                          AsyncFinishedEvent.Invoke();
                          Focus();
                        } ), fault => Dispatcher.BeginInvoke( () =>
                          {
                            MessageBox.Show( fault.Message );
                            AsyncFinishedEvent.Invoke();
                          } ) ) );
    }

    private void NewToDoField_Click( object sender, ManipulationStartedEventArgs e )
    {
      NewToDoField.Text = "";
    }

    internal event AsyncStartedEventHandler AsyncStartedEvent;

    internal event AsyncFinishedEventHandler AsyncFinishedEvent;

    internal delegate void AsyncFinishedEventHandler();

    internal delegate void AsyncStartedEventHandler();
  }
}