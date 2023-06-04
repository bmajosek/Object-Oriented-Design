using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    
    public interface IMyCollections<T>
    {
        void Add(T item);
        void Remove(T item);
        IMyIter<T> GetIterator();
        IMyIter<T> GetReverseIterator();

    }
    public class Node<T>
    {
        public T Data;
        public Node<T> Prev;
        public Node<T> Next;

        public Node(T data)
        {
            Data = data;
            Prev = null;
            Next = null;
        }
    }
    public class DoublyLinkedList<T> : IMyCollections<T>
    {
        public Node<T> head;
        public Node<T> tail;
        public void Add(T data)
        {
            var newNode = new Node<T>(data);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
        }
        public Node<T> FindNode(T data)
        {
            var current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    return current;
                }
                current = current.Next;
            }
            return null;
        }
        public void Remove(T data)
        {
            var nodeToRemove = FindNode(data);
            if (nodeToRemove == null)
            {
                return;
            }

            if (nodeToRemove.Prev == null)
            {
                head = nodeToRemove.Next;
            }
            else
            {
                nodeToRemove.Prev.Next = nodeToRemove.Next;
            }

            if (nodeToRemove.Next == null)
            {
                tail = nodeToRemove.Prev;
            }
            else
            {
                nodeToRemove.Next.Prev = nodeToRemove.Prev;
            }
        }

        public IMyIter<T> GetIterator()
        {
            return new DoublyLinkedListIterator<T>(this);
        }
        public IMyIter<T> GetReverseIterator()
        {
            return new DoublyLinkedListIterator<T>(this, true);
        }
    }
    public class Vector<T> : IMyCollections<T>
    {
        public T[] array = new T[16];
        public int count;

        public void Add(T item)
        {
            if (count == array.Length)
            {
                Array.Resize(ref array, array.Length * 2);
            }
            array[count++] = item;
        }

        public void Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
            {
                return;
            }
            for (int i = index + 1; i < count; i++)
            {
                array[i - 1] = array[i];
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(array[i], item))
                {
                    return i;
                }
            }
            return -1;
        }
        public IMyIter<T> GetIterator()
        {
            return new VectorIterator<T>(this);
        }
        public IMyIter<T> GetReverseIterator()
        {
            return new VectorIterator<T>(this, true);
        }
    }
    public class MyHashmap<T> : IMyCollections<T>
    {
        public List<T>[] data;
        public int capacity;

        public MyHashmap(int capacity)
        {
            this.capacity = capacity;
            this.data = new List<T>[capacity];
            for (int i = 0; i < capacity; i++)
            {
                data[i] = new List<T>();
            }
        }

        public void Add(T value)
        {
            int hash = value.GetHashCode();
            int index = hash % capacity;
            data[index].Add(value);
        }

        public void Remove(T value)
        {
            int hash = value.GetHashCode();
            int index = hash % capacity;
            data[index].Remove(value);
        }

        public IMyIter<T> GetIterator()
        {
            return new HashMapIterator<T>(this);
        }

        public IMyIter<T> GetReverseIterator()
        {
            return new HashMapIterator<T>(this, true);
        }
    }




}
