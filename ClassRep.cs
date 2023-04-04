using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp25
{
    public class ClassRep
    {
        public string Class;
        public ClassRep(string name, string code, int duration, List<ITeacher> listofTeachers, List<IStudent> students)
        {
            var teacherstmp = string.Join(",", listofTeachers.Select(e => $"<{e.code}>"));
            var studentstmp = string.Join(",", students.Select(e => $"<{e.code}>"));
            Class = $"<{name}>#<{code}>(<{duration}>)^{teacherstmp}${studentstmp}";
        }
    }
    public class ClassTextAdapt : IClass
    {
        public ClassRep classrep;

        public ClassTextAdapt(ClassRep classrep)
        {
            this.classrep = classrep;
        }
        
        public string Name
        {
            get
            {
                var fields = classrep.Class.Split(new[] { "#", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var className = fields[0];
                return className;
            }
        }

        public string Code
        {
            get
            {
                var fields = classrep.Class.Split(new[] { "#", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var classCode = fields[1];
                return classCode;
            }
        }

        public int Duration
        {
            get
            {
                var fields = classrep.Class.Split(new[] { "#", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var duration = int.Parse(fields[2]);
                return duration;
            }
        }

        public List<ITeacher> listofTeachers
        {
            get
            {
                var fields = classrep.Class.Split(new[] { "#", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var teachers = fields[3].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                List<ITeacher> teach = new List<ITeacher>();
                foreach (var item in teachers)
                {
                    object teacref;
                    Slownik.Dict.TryGetValue(item, out teacref);
                    teach.Add((ITeacher)teacref);
                }
                return teach;
            }
        }
        public List<IStudent> students
        {
            get
            {
                var fields = classrep.Class.Split(new[] { "#", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var students = fields[4].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                List<IStudent> stud = new List<IStudent>();
                foreach (var item in students)
                {
                    object studref;
                    Slownik.Dict.TryGetValue(item, out studref);
                    stud.Add((IStudent)studref);
                }
                return stud;
            }
        }
        public override string ToString()
        {
            return $"name: {Name}, code: {Code}, duration: {Duration}\n";
        }
    }
}
