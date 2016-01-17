using System;
using System.IO;
using System.Collections;

namespace Weborb.Util
{
	public class FileInfoComparer : IComparer
	{
		public FileInfoComparer()
		{
		}

		#region IComparer Members

		public int Compare( object x, object y )
		{
			FileInfo file1 = (FileInfo)x;
			FileInfo file2 = (FileInfo)y;
			return file1.LastWriteTime.CompareTo( file2.LastWriteTime );
		}

		#endregion
	}
}
