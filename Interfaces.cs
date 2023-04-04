using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public interface IClass
    {
        public string Name { get; }
        public string Code { get; }
        public int Duration { get; }
        public List<ITeacher> listofTeachers { get; }
        public List<IStudent> students { get; }
    }
    public interface ITeacher
    {
        public List<string> names { get; }
        public string surname { get; }
        public ranks rank { get; }
        public string code { get; }
        public List<IClass> classes { get; }
    }
    public interface IStudent
    {
        public List<string> names { get; }
        public string surname { get; }
        public int semester { get; }
        public string code { get; }
        public List<IClass> classes { get; }
    }
    public interface IRoom
    {
        public int number { get; }
        public typeOfRoom type { get; }
        public List<IClass> classes { get; }
    }
}
