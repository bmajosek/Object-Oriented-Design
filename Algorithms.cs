using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public static class Algorithms<T>
    {
        public static T Find(IMyCollections<T> collection, Func<T, bool> predicate, bool direction)
        {
            var iterator = direction ? collection.GetIterator() : collection.GetReverseIterator();
            while (iterator.MoveNext())
            {
                T element = iterator.Current;
                if (predicate(element))
                {
                    return element;
                }
            }
            return default(T);
        }

        public static void ForEach(IMyCollections<T> collection, Func<T, T> function, bool direction)
        {
            var iterator = direction ? collection.GetIterator() : collection.GetReverseIterator();
            while (iterator.HasNext())
            {
                T element = function(iterator.Current);
                Console.Write(element + " ");
                iterator.MoveNext();
            }
            Console.WriteLine();
        }

        public static int CountIf(IMyCollections<T> collection, Func<T, bool> predicate)
        {
            int count = 0;
            var iterator = collection.GetIterator();
            while (iterator.HasNext())
            {
                T element = iterator.Current;
                if (predicate(element))
                {
                    count++;
                }
                iterator.MoveNext();
            }
            return count;
        }
        public static void Print(IMyCollections<T> collection, Func<T, bool> fun, bool direction)
        {
            var iterator = direction ? collection.GetIterator() : collection.GetReverseIterator();
            while (iterator.HasNext())
            {
                if (fun(iterator.Current))
                {
                    Console.Write(iterator.Current + " ");
                }
                iterator.MoveNext();
            }
            Console.WriteLine();
        }
    }
}
