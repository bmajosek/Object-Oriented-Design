using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public class StudentRep
    {
        public string StudentCoded;

        public StudentRep(List<string> names, string surname, int semester, string code, List<IClass> classes)
        {
            var classestmp = string.Join("^", classes.Select(e => $"<{e.Code}>"));
            var namestmp = string.Join(",", names.Select(e => $"<{e}>"));
            StudentCoded = $"<{surname}>,{namestmp}@<{semester}>(<{code}>)^{classestmp}";
        }

    }
    public class StudentTextAdapt : IStudent
    {
        public StudentRep studentrep;

        public StudentTextAdapt(StudentRep studentrep)
        {
            this.studentrep = studentrep;
        }
        public List<string> names
        {
            get
            {
                var fields = studentrep.StudentCoded.Split(new[] { "@", "(", ")", "^", "$" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var names = fields[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                names.RemoveAt(0);
                return names;
            }
        }
        public string surname
        {
            get
            {
                var fields = studentrep.StudentCoded.Split(new[] { "@", "(", ")", "^", "$" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var names = fields[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                var surname = names.First();
                return surname;
            }
        }
        public int semester
        {
            get
            {
                var fields = studentrep.StudentCoded.Split(new[] { "@", "(", ")", "^", "$" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var semester = int.Parse(fields[1]);
                return semester;
            }
        }

        public string code
        {
            get
            {
                var fields = studentrep.StudentCoded.Split(new[] { "@", "(", ")", "^", "$" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                return fields[2];
            }
        }
        public List<IClass> classes
        {
            get
            {
                var fields = studentrep.StudentCoded.Split(new[] { "@", "(", ")", "^", "$" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var classes = new List<string>();
                List<IClass> clas = new List<IClass>();
                for (int i = 3; i < fields.Length; i++)
                {
                    object classref;
                    Slownik.Dict.TryGetValue(fields[i], out classref);
                    clas.Add((IClass)classref);
                }
                return clas;
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
