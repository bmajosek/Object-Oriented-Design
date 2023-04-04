using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    [Flags]
    public enum ranks
    {
        KiB = 0,
        MiB = 1,
        GiB = 2,
        TiB = 3
    }
    public class Teacher : ITeacher
    {
        public Teacher(List<string> names, string surname, ranks rank, string code, List<IClass> classes)
        {
            this.names = names;
            this.surname = surname;
            this.rank = rank;
            this.code = code;
            this.classes = classes;
        }

        public List<string> names { get; }
        public string surname {  get; }
        public ranks rank { get; }
        public string code { get; }
        public List<IClass> classes { get; }

        public override string ToString()
        {
            string name = "";
            foreach (var c in names)
            {
                name += c + " ";
            }
            return $"names: {name}, surname: {this.surname}, rank: {this.rank}, code: {this.code}\n";

        }
        public string CodeToString()
        {
            var classestmp = string.Join("^", classes.Select(e => $"<{e}>"));
            var namestmp = string.Join(",", names.Select(e => $"<{e}>"));
            return $"<{surname}>,{namestmp}*<{rank}>(<{code}>)^{classestmp}";
        }
    }
}
