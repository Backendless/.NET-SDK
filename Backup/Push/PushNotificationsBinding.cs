using System;
using Microsoft.Phone.Notification;

namespace BackendlessAPI.Push
{
  public class PushNotificationsBinding
  {
    internal bool? IsBindedToShellTile { get; set; }
    internal bool? IsBindedToShellToast { get; set; }
    internal EventHandler<HttpNotificationEventArgs> OnHttpNotificationReceived { get; set; }
    internal EventHandler<NotificationEventArgs> OnShellToastNotificationReceived { get; set; }

    private PushNotificationsBinding()
    {
    }

    public static PushNotificationsBinding CreateBinding()
    {
      return new PushNotificationsBinding();
    }

    public PushNotificationsBinding BindToShellTile()
    {
      IsBindedToShellTile = true;
      return this;
    }

    public PushNotificationsBinding BindToShellToast()
    {
      IsBindedToShellToast = true;
      return this;
    }

    public PushNotificationsBinding HttpNotificationReceivedHandler( EventHandler<HttpNotificationEventArgs> handler )
    {
      OnHttpNotificationReceived += handler;
      return this;
    }

    public PushNotificationsBinding ShellToastNotificationHandler( EventHandler<NotificationEventArgs> handler )
    {
      OnShellToastNotificationReceived += handler;
      return this;
    }

    internal void ApplyTo( HttpNotificationChannel httpNotificationChannel )
    {
      if( IsBindedToShellTile != null )
        if( IsBindedToShellTile.Value )
        {
          if( !httpNotificationChannel.IsShellTileBound )
            httpNotificationChannel.BindToShellTile();
        }
        else
        {
          if( httpNotificationChannel.IsShellTileBound )
            httpNotificationChannel.UnbindToShellTile();
        }

      if( IsBindedToShellToast != null )
        if( IsBindedToShellToast.Value )
        {
          if( !httpNotificationChannel.IsShellToastBound )
            httpNotificationChannel.BindToShellToast();
        }
        else
        {
          if( httpNotificationChannel.IsShellToastBound )
            httpNotificationChannel.UnbindToShellToast();
        }

      if( OnHttpNotificationReceived != null )
        httpNotificationChannel.HttpNotificationReceived += OnHttpNotificationReceived;

      if( OnShellToastNotificationReceived != null )
        httpNotificationChannel.ShellToastNotificationReceived += OnShellToastNotificationReceived;
    }
  }
}