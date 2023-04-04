using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public class TeacherList
    {
        public List<Tuple<string, Object>> list = new();

        public TeacherList(List<string> names, string surname, ranks rank, string code, List<IClass> classes)
        {
            list.Add(new Tuple<string, object>("names", names));
            list.Add(new Tuple<string, object>("surname", surname));
            list.Add(new Tuple<string, object>("rank", rank));
            list.Add(new Tuple<string, object>("code", code));
            list.Add(new Tuple<string, object>("classes", classes));
        }
    }

    public class TeacherListAdapt : ITeacher
    {
        public TeacherList listOfTeacher;
        public TeacherListAdapt(TeacherList list)
        {
            this.listOfTeacher = list;
        }
        public List<string> names
        {
            get
            {
                foreach (var item in listOfTeacher.list)
                {
                    if (item.Item1 == "names")
                        return item.Item2 as List<string>;
                }
                return null;
            }
        }
        public string surname
        {
            get
            {
                foreach (var item in listOfTeacher.list)
                {
                    if (item.Item1 == "surname")
                        return item.Item2 as string;
                }
                return null;
            }
        }

        public ranks rank
        {
            get
            {
                foreach (var item in listOfTeacher.list)
                {
                    if (item.Item1 == "rank")
                        return (ranks)item.Item2;
                }
                return 0;
            }
        }
        public string code
        {
            get
            {
                foreach (var item in listOfTeacher.list)
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
                foreach (var item in listOfTeacher.list)
                {
                    if (item.Item1 == "classes")
                        return item.Item2 as List<IClass>;
                }
                return null;
            }
        }
        public override string ToString()
        {
            string name = "";
            foreach (var c in names)
            {
                name += c + " ";
            }
            return $"names: {name}, surname: {this.surname}, rank: {this.rank}, code: {this.code}\n";

        }
    }
}
