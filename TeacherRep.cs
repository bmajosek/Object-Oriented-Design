using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp25
{
    public class TeacherRep
    {
        public string Teacher;

        public TeacherRep(List<string> names, string surname, ranks rank, string code, List<IClass> classes)
        {
            var classestmp = string.Join("^", classes.Select(e => $"<{e}>"));
            var namestmp = string.Join(",", names.Select(e => $"<{e}>"));
            Teacher = $"<{surname}>,{namestmp}*<{rank}>(<{code}>)^{classestmp}";
        }
    }

    public class TeacherTextAdapt : ITeacher
    {
        public TeacherRep teacherrep;

        public TeacherTextAdapt(TeacherRep teacherre)
        {
            this.teacherrep = teacherre;
        }
        public List<string> names
        {
            get
            {
                var fields = teacherrep.Teacher.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var names = fields[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                names.RemoveAt(names.Count - 1);
                return names;
            }
        }
        public string surname
        {
            get
            {
                var fields = teacherrep.Teacher.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var names = fields[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                return names.First();
            }
        }

        public ranks rank
        {
            get
            {
                var fields = teacherrep.Teacher.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var rank = (ranks)Enum.Parse(typeof(ranks), fields[1].Trim('(', ')'));
                return rank;
            }
        }
        public string code
        {
            get
            {
                var fields = teacherrep.Teacher.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var code = fields[2];
                return code;
            }
        }
        public List<IClass> classes
        {
            get
            {
                var fields = teacherrep.Teacher.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var classes = new List<IClass>();

                for (int i = 3; i < fields.Length; i++)
                {
                    object classref;
                    Slownik.Dict.TryGetValue(fields[i], out classref);
                    classes.Add((IClass)classref);
                }
                return classes;
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
