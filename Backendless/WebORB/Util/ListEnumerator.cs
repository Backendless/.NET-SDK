using System;
using System.Collections;

namespace Weborb.Util
{
	public class ListEnumerator : IEnumerator
	{
        private LinkedList list;
        private static ListNode UNINITIALIZED = new ListNode( null );
        private ListNode curNode = UNINITIALIZED;

		public ListEnumerator( LinkedList list )
		{
            this.list = list;
        }
        #region IEnumerator Members

        public void Reset()
        {
            curNode = UNINITIALIZED;
        }

        public object Current
        {
            get
            {
                return curNode;                
            }
        }

        public bool MoveNext()
        {
            if( curNode == null )
                return false;

            lock( list )
            {
                if( curNode == UNINITIALIZED )
                {
                    curNode = list.GetFirst();
                }
                else
                {
                    curNode = curNode.next;

                    while( curNode != null && curNode.list == null )
                        curNode = curNode.next;
                }
            }

            return curNode != null;
        }

        #endregion
    }
}
