using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Weborb.Util.Codegen
  {
  public class VBConverter
    {
    internal static void ConvertToVB( CodeItem codeItem )
      {
      if( codeItem.IsDirectory() )
        {
        foreach( CodeItem item in ( (CodeDirectory) codeItem).Items )
          ConvertToVB( item );
        }
      else if( codeItem.IsFile() && codeItem.Name.EndsWith( ".cs" ) )
        {
        CodeFile codeFile = (CodeFile) codeItem;

        String vbFileName = codeFile.Name.Replace( ".cs", ".vb" );

        codeFile.Name = vbFileName;
        codeFile.Content = CSharpToVBConverter.Convert( codeFile.Content );
        }
      else if ( codeItem.IsFile() && codeItem.Name.EndsWith( ".csproj" ) )
        {
        CodeFile codeFile = (CodeFile) codeItem;        
        codeFile.Name = codeFile.Name.Replace( ".csproj", ".vbproj" );

        codeFile.Content = codeFile.Content.Replace(".cs'", ".vb'");
        codeFile.Content = codeFile.Content.Replace(".cs\"", ".vb\"");
        codeFile.Content = codeFile.Content.Replace(".csproj", ".vbproj");
        codeFile.Content = codeFile.Content.Replace("fae04ec0-301f-11d3-bf4b-00c04f79efbc", "F184B08F-C81C-45F6-A57F-5ABD9991F28F");
        codeFile.Content = codeFile.Content.Replace("Microsoft.Silverlight.CSharp.targets", "Microsoft.Silverlight.VisualBasic.targets");        
        }
      else if (codeItem.IsFile() && codeItem.Name.EndsWith(".sln"))
      {
        CodeFile codeFile = (CodeFile)codeItem;
        codeFile.Content = codeFile.Content.Replace(".csproj", ".vbproj");
      }

      if ( codeItem.IsFile() && ( codeItem.Name.StartsWith("MainPage.xaml.") || codeItem.Name.StartsWith( "App.xaml.") ) )
        {
        CodeFile codeFile = (CodeFile) codeItem;                
        Regex regex = new Regex(@"Namespace \w+");
        codeFile.Content = regex.Replace(codeFile.Content, "", 2);
        codeFile.Content = codeFile.Content.Replace("End Namespace", "");
        }
      if (codeItem.IsFile() && ((CodeFile)codeItem).Content != CodeFile.HIDDEN_CONTENT)
      {
        CodeFile codeFile = (CodeFile)codeItem;
        codeFile.BinaryData = System.Text.ASCIIEncoding.ASCII.GetBytes(codeFile.Content);
      }
      }
    }
  }
