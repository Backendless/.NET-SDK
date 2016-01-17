using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Weborb.Util
{
    public partial class DiagnosticsPage : System.Web.UI.Page
    {
		protected void Page_Load(object sender, System.EventArgs e)
		{
      
            if( !IsPostBack && Request.HttpMethod != "POST" )
            {
                Diagnostics.RunDiagnostics( Response );
            }
		}

		#region Web Form Designer generated code
		override protected void OnInit(System.EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

        }
		#endregion
    }
}
