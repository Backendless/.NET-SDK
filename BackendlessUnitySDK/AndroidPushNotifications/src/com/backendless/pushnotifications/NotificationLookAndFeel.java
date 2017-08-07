/*
Copyright 2015 Backendless Corp. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

package com.backendless.pushnotifications;

import android.app.Notification;
import android.content.Context;
import android.util.DisplayMetrics;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.LinearLayout;
import android.widget.TextView;

public class NotificationLookAndFeel
{
  private static final String COLOR_SEARCH_RECURSE_TIP = "SOME_SAMPLE_TEXT";
  private static float notification_text_size;
  private static Integer notification_text_color = null;
  private static float notification_title_size_factor = (float) 1.0;

  private boolean recurseGroup( Context context, ViewGroup gp )
  {
    final int count = gp.getChildCount();

    for ( int i = 0; i < count; ++i )
    {
      if( gp.getChildAt( i ) instanceof TextView )
      {
        final TextView text = (TextView) gp.getChildAt( i );
        final String szText = text.getText().toString();

        if( COLOR_SEARCH_RECURSE_TIP.equals( szText ) )
        {
          notification_text_color = text.getTextColors().getDefaultColor();
          notification_text_size = text.getTextSize();
          DisplayMetrics metrics = new DisplayMetrics();
          WindowManager systemWM = (WindowManager) context.getSystemService( Context.WINDOW_SERVICE );
          systemWM.getDefaultDisplay().getMetrics( metrics );
          notification_text_size /= metrics.scaledDensity;
          return true;
        }
      }
      else if( gp.getChildAt( i ) instanceof ViewGroup )
      {
        return recurseGroup( context, (ViewGroup) gp.getChildAt( i ) );
      }
    }
    return false;
  }

  void extractColors( Context context )
  {
    if( notification_text_color != null )
      return;

    try
    {
      Notification ntf = new Notification();
      ntf.setLatestEventInfo( context, COLOR_SEARCH_RECURSE_TIP, "Utest", null );
      LinearLayout group = new LinearLayout( context );
      ViewGroup event = (ViewGroup) ntf.contentView.apply( context, group );
      recurseGroup( context, event );
      group.removeAllViews();
    }
    catch ( Exception e )
    {
      notification_text_color = android.R.color.black;
    }
  }

  public int getTextColor()
  {
    return notification_text_color;
  }

  public float getTextSize()
  {
    return notification_title_size_factor * notification_text_size;
  }
}
