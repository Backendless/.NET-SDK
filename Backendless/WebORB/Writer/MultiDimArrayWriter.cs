using System;
using System.Collections;

namespace Weborb.Writer
{
	public class MultiDimArrayWriter : AbstractReferenceableTypeWriter
	{
		#region ITypeWriter Members

		public override void write( object obj, IProtocolFormatter writer )
		{
            Array array = (Array) obj;
			int[] coord = new int[ array.Rank ];
			serialize( coord, 0, array, writer );
		}

		#endregion

		private void serialize( int[] coord, int dim, Array array, IProtocolFormatter writer )
		{
			int dimLength = array.GetLength( dim );
            writer.BeginWriteArray( dimLength );

			for( int i = 0; i < dimLength; i++ )
			{
				coord[ dim ] = i;

				if( dim == (array.Rank - 1) )
					MessageWriter.writeObject( array.GetValue( coord ), writer );					
				else
					serialize( coord, dim + 1, array, writer );
			}			

            writer.EndWriteArray();
		}
	}
}
