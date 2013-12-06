using System;
using System.Windows;
using BackendlessAPI;
using BackendlessAPI.Async;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.UserServiceDemo
{
  public partial class RegistrationPage : PhoneApplicationPage
  {
    private const string NAME_KEY = "name";
    private const string GENDER_KEY = "gender";
    private const string DATE_OF_BIRTH_KEY = "dateofbirth";
    private const string LOGIN_KEY = "login";
    private const string EMAIL_KEY = "email";

    public RegistrationPage()
    {
      InitializeComponent();
    }

    private void RegisterButton_Click( object sender, RoutedEventArgs e )
    {
      string name = NameField.Text;
      if( string.IsNullOrEmpty( name ) )
      {
        MessageBox.Show( "Name cannot be empty" );
        return;
      }

      string password = PasswordField.Password;
      if( string.IsNullOrEmpty( password ) )
      {
        MessageBox.Show( "Password cannot be empty" );
        return;
      }

      string verifyPassword = verifyPasswordField.Password;
      if( !verifyPassword.Equals( password ) )
      {
        MessageBox.Show( "Passwords does not match" );
        return;
      }

      string email = EmailField.Text;
      if( string.IsNullOrEmpty( email ) )
      {
        MessageBox.Show( "Email cannot be empty" );
        return;
      }

      DateTime? dateOfBirth = DateOfBirthField.Value;
      if( dateOfBirth == null )
      {
        MessageBox.Show( "Date of birth cannot be empty" );
        return;
      }

      Gender gender = MaleRadioButton.IsChecked != null && (bool) MaleRadioButton.IsChecked
                        ? Gender.MALE
                        : Gender.FEMALE;

      var user = new BackendlessUser();
      user.Password = password;
      user.AddProperty(EMAIL_KEY, email);
      user.AddProperty(NAME_KEY, name);
      user.AddProperty(GENDER_KEY, gender);
      user.AddProperty(DATE_OF_BIRTH_KEY, dateOfBirth);
      user.AddProperty(LOGIN_KEY, email);
      
      Backendless.UserService.Register( user,
                                        new AsyncCallback<BackendlessUser>(
                                          response =>
                                          Dispatcher.BeginInvoke(
                                            () =>
                                            NavigationService.Navigate( new Uri( "/RegisteredPage.xaml",
                                                                                 UriKind.Relative ) ) ),
                                          fault => Dispatcher.BeginInvoke( () => MessageBox.Show( fault.Message ) ) ) );
    }

    private enum Gender
    {
      MALE,
      FEMALE
    }
  }
}