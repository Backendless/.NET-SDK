using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class PublishOptions
  {
    public const string MESSAGE_TAG = "message";

    public const string IOS_ALERT_TAG = "ios-alert";
    public const string IOS_BADGE_TAG = "ios-badge";
    public const string IOS_SOUND_TAG = "ios-sound";

    public const string ANDROID_TICKER_TEXT_TAG = "android-ticker-text";
    public const string ANDROID_CONTENT_TITLE_TAG = "android-content-title";
    public const string ANDROID_CONTENT_TEXT_TAG = "android-content-text";
    public const string ANDROID_ACTION_TAG = "android-action";

    public const string WP_TYPE_TAG = "wp-type";
    public const string WP_TITLE_TAG = "wp-title";
    public const string WP_TOAST_SUBTITLE_TAG = "wp-subtitle";
    public const string WP_TOAST_PARAMETER_TAG = "wp-parameter";
    public const string WP_TILE_BACKGROUND_IMAGE = "wp-backgroundImage";
    public const string WP_TILE_COUNT = "wp-count";
    public const string WP_TILE_BACK_TITLE = "wp-backTitle";
    public const string WP_TILE_BACK_BACKGROUND_IMAGE = "wp-backImage";
    public const string WP_TILE_BACK_CONTENT = "wp-backContent";
    public const string WP_RAW_DATA = "wp-raw";

    public PublishOptions()
    {
    }

    public PublishOptions( string publisherId )
    {
      this.PublisherId = publisherId;
    }

    public PublishOptions( string publisherId, string subtopic )
    {
      this.PublisherId = publisherId;
      this.Subtopic = subtopic;
    }

    public PublishOptions( string publisherId, Dictionary<string, string> headers, string subtopic )
    {
      this.PublisherId = publisherId;
      this.Headers = headers;
      this.Subtopic = subtopic;
    }

    [SetClientClassMemberName( "publisherId" )]
    public string PublisherId { get; set; }

    [SetClientClassMemberName( "headers" )]
    public Dictionary<string, string> Headers { get; set; }

    [SetClientClassMemberName( "subtopic" )]
    public string Subtopic { get; set; }

    public void AddHeader( string key, string value )
    {
      if( Headers == null )
        Headers = new Dictionary<string, string>();

      Headers[ key ] = value;
    }
  }
}