using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp25
{
    public class StudentList
    {
        public List<Tuple<string, Object>> list = new();

        public StudentList(List<string> names, string surname, int semester, string code, List<IClass> classes)
        {
            list.Add(new Tuple<string, object>("names", names));
            list.Add(new Tuple<string, object>("surname", surname));
            list.Add(new Tuple<string, object>("semester", semester));
            list.Add(new Tuple<string, object>("code", code));
            list.Add(new Tuple<string, object>("classes", classes));
        }

    }
    public class StudentListAdapt : IStudent
    {
        public StudentList listOfStudent;
        public StudentListAdapt(StudentList list)
        {
            this.listOfStudent = list;
        }
        public List<string> names
        {
            get
            {
                List<string> name = new();
                foreach (var item in listOfStudent.list)
                {
                    if (item.Item1 == "names")
                        return item.Item2 as List<string>;
                }
                return name;
            }
        }
        public string surname
        {
            get
            {
                foreach (var item in listOfStudent.list)
                {
                    if (item.Item1 == "surname")
                        return item.Item2 as string;
                }
                return null;
            }
        }
        public int semester
        {
            get
            {
                foreach (var item in listOfStudent.list)
                {
                    if (item.Item1 == "semester")
                        return (int)item.Item2;
                }
                return 0;
            }
        }

        public string code
        {
            get
            {
                foreach (var item in listOfStudent.list)
                {
                    if (item.Item1 == "code")
                        return item.Item2 as string;
                }
                return null;
            }
        }
        public List<IClass> classes
        {
            get
            {
                List<IClass> Classes = new();
                foreach (var item in listOfStudent.list)
                {
                    if (item.Item1 == "students")
                        return item.Item2 as List<IClass>;
                }
                return Classes;
            }
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
