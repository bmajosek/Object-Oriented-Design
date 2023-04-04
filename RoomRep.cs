using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp25
{
    public class RoomRep
    {
        public string Room;

        public RoomRep(int number, typeOfRoom type, List<IClass> classes)
        {
            var classestmp = string.Join(",", classes.Select(e => $"(<{e.Code}>)"));
            Room = $"<{number}>(<{type}>),{classestmp}";
        }

    }
    public class RoomTextAdapt : IRoom
    {
        public RoomRep roomrep;

        public RoomTextAdapt(RoomRep roomrep)
        {
            this.roomrep = roomrep;
        }

        public int number
        {
            get
            {
                var fields = roomrep.Room.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var roomNumber = int.Parse(fields[0]);
                return roomNumber;
            }
        }
        public typeOfRoom type
        {
            get
            {
                var fields = roomrep.Room.Split(new[] { "*", "(", ")", "^", "$", "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                var roomType = (typeOfRoom)Enum.Parse(typeof(typeOfRoom), fields[1].Trim('(', ')'));
                return roomType;
            }
        }
        public List<IClass> classes
        {
            get
            {
                var fields = roomrep.Room.Split(new[] { "*", "(", ")", "^", "$", "@", "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; ++i)
                {
                    fields[i] = fields[i].Replace("<", "").Replace(">", "");
                }
                var classes = new List<IClass>();
                for (int i = 2; i < fields.Length; i++)
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
            return $"number: {number}, type of room: {type}\n";

        }

    }
}
