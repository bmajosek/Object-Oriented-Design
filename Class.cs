using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp25
{
    public class Class : IClass
    {

        public string Name { get; }
        public string Code { get; }
        public int Duration { get; }
        public List<ITeacher> listofTeachers { get; }
        public List<IStudent> students { get; }

        public Class(string name, string code, int duration, List<ITeacher> listofTeachers, List<IStudent> students)
        {
            Name = name;
            Code = code;
            Duration = duration;
            this.listofTeachers = listofTeachers;
            this.students = students;
        }

        public override string ToString()
        {
            return $"name: {Name}, code: {Code}, duration: {Duration}\n";

        }
    }
}
