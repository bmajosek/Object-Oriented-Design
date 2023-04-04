using ConsoleApp25;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    [Flags]
    public enum typeOfRoom
    {
        Laboratory = 0,
        Training = 1,
        Lecture = 2,
        Other = 3
    }

    public class Room : IRoom
    {
        public Room(int number, typeOfRoom type, List<IClass> classes)
        {
            this.number = number;
            this.type = type;
            this.classes = classes;
        }

        public int number { get;  }
        public typeOfRoom type { get; }
        public List<IClass> classes { get; }

        public override string ToString()
        {
            return $"number: {number}, type of room: {type}\n";

        }
        public string CodetoString()
        {
            var classestmp = string.Join(",", classes.Select(e => $"(<{e}>)"));
            return $"<{number}>(<{type}>),{classestmp}";
        }
    }
}
