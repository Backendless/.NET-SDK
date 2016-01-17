using System;

namespace Weborb.V3Types
{
	public class UpdateCollRange
	{
		private int _position;
		private int _updateType;
		private object[] _identities;

        public UpdateCollRange()
        {
        }

		public UpdateCollRange( int position, int updateType, object[] identities )
		{
			this._position = position;
			this._updateType = updateType;
			this._identities = identities;
		}

		public int position
		{
			get
			{
				return _position;
			}
            set
            {
                _position = value;
            }
		}

		public int updateType
		{
			get
			{
				return _updateType;
			}
            set
            {
                _updateType = value;
            }
		}

		public object[] identities
		{
			get
			{
				return _identities;
			}
            set
            {
                _identities = value;
            }
		}
	}
}
