using System;
using System.Collections;

namespace Weborb.Util
{
    public class LinkedList : IEnumerable
    {
        private ListNode first;
        private ListNode last;

        public void Add( ListNode node )
        {
            lock( this )
            {
                node.Remove();
                node.list = this;
                node.prev = last;

                if( first == null )
                    first = node;
                else
                    last.next = node;

                last = node;
            }
        }

        public void Add( ListNode node, ListNode newNode )
        {
            lock( this )
            {
                newNode.Remove();
                newNode.list = this;

                newNode.next = node.next;
                newNode.prev = node;

                if( node.next == null )
                    last = newNode;
                else
                    node.next.prev = newNode;

                node.next = newNode;
            }
        }

        public void Remove( ListNode node )
        {
            lock( this )
            {
                if( node.prev == null )
                    first = node.next;
                else
                    node.prev.next = node.next;

                if( node.next == null )
                    last = node.prev;
                else
                    node.next.prev = node.prev;

                node.list = null;
            }
        }

        public void Replace( ListNode oldNode, ListNode newNode )
        {
            lock( this )
            {
                newNode.Remove();
                newNode.list = this;

                if( oldNode.prev == null )
                    first = newNode;
                else
                    oldNode.prev.next = newNode;

                if( oldNode.next == null )
                    last = newNode;
                else
                    oldNode.next.prev = newNode;

                newNode.prev = oldNode.prev;
                newNode.next = oldNode.next;
            }
        }

        public void Insert( ListNode node, ListNode newNode )
        {
            lock( this )
            {
                newNode.Remove();
                newNode.list = this;

                newNode.prev = node.prev;
                newNode.next = node;

                if( node.prev == null )
                    first = newNode;
                else
                    node.prev.next = newNode;

                node.prev = newNode;
            }
        }

        public ListNode GetFirst()
        {
            return first;
        }

        public ListNode LetLast()
        {
            return last;
        }

        public int Size()
        {
            int count = 0;

            lock( this )
            {
                for( ListNode node = first; node != null; node = node.next )
                    ++count;
            }
            
            return count;
        }

        public void Clear()
        {
            lock( this )
            {
                first = null;
                last = null;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new ListEnumerator( this );
        }
    }
}
