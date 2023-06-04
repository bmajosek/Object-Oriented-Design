using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public interface IMyIter<out T>
    {
        T Current { get; }
        bool MoveNext();
        bool HasNext();
    }
    public class DoublyLinkedListIterator<T> : IMyIter<T>
    {
        public Node<T> current;
        public DoublyLinkedList<T> list;
        readonly bool reverse;

        public DoublyLinkedListIterator(DoublyLinkedList<T> list, bool reverse = false)
        {
            this.list = list;
            if (reverse == false)
                current = list.head;
            else
                current = list.tail;
            this.reverse = reverse;
        }

        public T Current
        {
            get
            {
                if (current == null)
                {
                    throw new InvalidOperationException();
                }
                return current.Data;
            }
        }

        public bool MoveNext()
        {
            if (reverse == false)
                current = current.Next;
            else
                current = current.Prev;
            return current != null;
        }

        public bool HasNext()
        {
            return current != null;
        }
    }

    public class VectorIterator<T> : IMyIter<T>
    {
        public readonly Vector<T> vector;
        public int index;
        bool reverse;
        public VectorIterator(Vector<T> vector, bool reverse = false)
        {
            this.vector = vector;
            this.reverse = reverse;
            if (reverse == true)
                index = vector.count - 1;
            else
                index = 0;
        }

        public T Current => vector.array[index];

        public bool MoveNext()
        {
            if (reverse == false)
            {
                index++;
                return index < vector.count;
            }
            else
            {
                index--;
                return index >= 0;
            }
        }

        public bool HasNext()
        {
            if (reverse == false)
                return index <= vector.count - 1;
            else
                return index >= 0;
        }
    }
    public class HashMapIterator<T> : IMyIter<T>
    {
        public MyHashmap<T> hashmap;
        public int listIndex;
        public int itemIndex;
        public readonly bool reverse;

        public HashMapIterator(MyHashmap<T> hashmap, bool reverse = false)
        {
            this.hashmap = hashmap;
            this.reverse = reverse;
            if (reverse)
            {
                listIndex = hashmap.capacity - 1;
                itemIndex = hashmap.data[listIndex].Count - 1;
            }
            else
            {
                listIndex = 0;
                itemIndex = 0;
            }
        }

        public bool HasNext()
        {
            if (reverse)
            {
                while (listIndex >= 0 && itemIndex < 0)
                {
                    listIndex--;
                    if (listIndex >= 0)
                    {
                        itemIndex = hashmap.data[listIndex].Count - 1;
                    }
                }
                return listIndex >= 0 && itemIndex >= 0;
            }
            else
            {
                while (listIndex < hashmap.capacity && itemIndex >= hashmap.data[listIndex].Count)
                {
                    listIndex++;
                    if (listIndex < hashmap.capacity)
                    {
                        itemIndex = 0;
                    }
                }
                return listIndex < hashmap.capacity;
            }
        }

        public bool MoveNext()
        {
            if (reverse)
            {
                itemIndex--;
                while (listIndex >= 0 && itemIndex < 0)
                {
                    listIndex--;
                    if (listIndex >= 0)
                    {
                        itemIndex = hashmap.data[listIndex].Count - 1;
                    }
                }
                return listIndex >= 0 && itemIndex >= 0;
            }
            else
            {
                itemIndex++;
                while (listIndex < hashmap.capacity && itemIndex >= hashmap.data[listIndex].Count)
                {
                    listIndex++;
                    if (listIndex < hashmap.capacity)
                    {
                        itemIndex = 0;
                    }
                }
                return listIndex < hashmap.capacity;
            }
        }

        public T Current
        {
            get 
            {
                return hashmap.data[listIndex][itemIndex]; 
            }
        }
    }

}
