using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public class Collection
    {
        public class linkedlist
        {
            public object item;
            public linkedlist next, prev;

            public linkedlist(object item)
            {
                this.item = item;
                this.next = null;
                this.prev = null;
            }
            public void AddItem(object item)
            {
                linkedlist New = new(item);
                New.prev = this;
                this.next = New;
            }
            public object DeleteItem()
            {
                var head = item;
                this.next.prev = null;
                this = this.next;
                return head;
            }
        }
        public class vector
        {

        }
    }
}
