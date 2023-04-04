using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public class Student : IStudent
    {


        public List<string> names { get; }
        public string surname { get; }
        public int semester { get;  }
        public string code { get; }
        public List<IClass> classes { get; }

        public Student(List<string> names, string surname, int semester, string code, List<IClass> classes)
        {
            this.names = names;
            this.surname = surname;
            this.semester = semester;
            this.code = code;
            this.classes = classes;
        }

        public override string ToString()
        {
            string name = "";
            foreach (var c in names)
            {
                name += c.ToString() + " ";
            }
            return $"names: {name}, surname: {surname}, semester {semester}, code: {code}\n";
        }

    }
}
