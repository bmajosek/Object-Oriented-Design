using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp25
{
    public class ClassList
    {
        public List<Tuple<string, Object>> list = new();

        public ClassList(string name, string code, int duration, List<ITeacher> listofTeachers, List<IStudent> students)
        {
            list.Add(new Tuple<string, object>("Name", name));
            list.Add(new Tuple<string, object>("Code", code));
            list.Add(new Tuple<string, object>("Duration", duration));
            list.Add(new Tuple<string, object>("listofTeachers", listofTeachers));
            list.Add(new Tuple<string, object>("students", students));
        }

    }
    public class ClassListAdapt : IClass
    {
        public ClassList listofClass;
        public ClassListAdapt(ClassList list)
        {
            this.listofClass = list;
        }
        public string Name
        {
            get
            {
                foreach (var item in listofClass.list)
                {
                    if (item.Item1 == "Name")
                        return item.Item2 as string;
                }
                return null;
            }
        }
        public string Code
        {
            get
            {
                foreach (var item in listofClass.list)
                {
                    if (item.Item1 == "Code")
                        return item.Item2 as string;
                }
                return null;
            }
        }
        public int Duration
        {
            get
            {
                foreach (var item in listofClass.list)
                {
                    if (item.Item1 == "Duration")
                        return (int)item.Item2;
                }
                return 0;
            }
        }

        public List<ITeacher> listofTeachers
        {
            get
            {
                List<ITeacher> teachers = new List<ITeacher>();
                foreach (var item in listofClass.list)
                {
                    if (item.Item1 == "listofTeachers")
                        return item.Item2 as List<ITeacher>;
                }
                return teachers;
            }
        }
        public List<IStudent> students
        {
            get
            {
                List<IStudent> students = new List<IStudent>();
                foreach (var item in listofClass.list)
                {
                    if (item.Item1 == "students")
                        return item.Item2 as List<IStudent>;
                }
                return students;
            }
        }
        public override string ToString()
        {
            return $"name: {Name}, code: {Code}, duration: {Duration}\n";
        }
    }
}
