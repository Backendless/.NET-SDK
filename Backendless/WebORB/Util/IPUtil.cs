using System;
using System.Net;

namespace Weborb.Util
{
	/// <summary>
	/// Summary description for IPUtil.
	/// </summary>
	public class IPUtil
	{
		public static string getIPAddress()
		{
			string hostName = Dns.GetHostName();
			IPHostEntry hostEntry = Dns.GetHostEntry( hostName );

			//Return the first one
			foreach(IPAddress ipAddress in hostEntry.AddressList)
				return ipAddress.ToString();

			return hostName;
		}
	}
}
