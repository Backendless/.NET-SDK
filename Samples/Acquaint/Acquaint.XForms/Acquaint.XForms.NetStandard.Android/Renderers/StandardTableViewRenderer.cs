using System;
using Acquaint.XForms;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer( typeof( TableView ), typeof( Acquaint.XForms.Droid.StandardTableViewRenderer ) )]

namespace Acquaint.XForms.Droid
{
  public class StandardTableViewRenderer : TableViewRenderer
  {
    public StandardTableViewRenderer( Context context ) : base( context )
    {

    }
    protected override void OnElementChanged( ElementChangedEventArgs<TableView> e )
    {
      base.OnElementChanged( e );

      if( Control == null )
        return;

      if( (e.NewElement != null && e.NewElement.StyleId == "NoSeparator") || (e.OldElement != null && e.OldElement.StyleId == "NoSeparator") )
      {
        (Control as global::Android.Widget.ListView).DividerHeight = 0;
      }
    }
  }
}

