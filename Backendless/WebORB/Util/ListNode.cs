using System;

namespace Weborb.Util
{
	public class ListNode
	{
        private object obj;
        public LinkedList list;
        public ListNode next;
        public ListNode prev;

		public ListNode( object obj )
		{
            this.obj = obj;
		}

        public bool Remove()
        {
            if( list != null )
            {
                list.Remove( this );
                return true;
            }
            else
            {
                return false;
            }
        }

        public Object GetObject()
        {
            return obj;
            }

        public LinkedList GetList()
        {
            return list;
        }

        public void SetNext( ListNode node )
        {
            list.Add( this, node );
        }

        public void SetPrevious( ListNode node )
        {
            list.Insert( this, node );
        }
	}
}
