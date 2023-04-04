using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
    public class RoomList
    {
        public List<Tuple<string, Object>> list = new();

        public RoomList(int number, typeOfRoom type, List<IClass> classes)
        {
            list.Add(new Tuple<string, object>("number", number));
            list.Add(new Tuple<string, object>("type", type));
            list.Add(new Tuple<string, object>("classes", classes));
        }
    }
    public class RoomListAdapt : IRoom
    {
        public RoomList listOfRoom;
        public RoomListAdapt(RoomList list)
        {
            this.listOfRoom = list;
        }
        public int number
        {
            get
            {
                foreach (var item in listOfRoom.list)
                {
                    if (item.Item1 == "number")
                        return (int)item.Item2;
                }
                return 0;
            }
        }
        public typeOfRoom type
        {
            get
            {
                foreach (var item in listOfRoom.list)
                {
                    if (item.Item1 == "type")
                        return (typeOfRoom)item.Item2;
                }
                return 0;
            }
        }
        public List<IClass> classes
        {
            get
            {
                foreach (var item in listOfRoom.list)
                {
                    if (item.Item1 == "classes")
                        return item.Item2 as List<IClass>;
                }
                return null;
            }
        }
        public override string ToString()
        {
            return $"number: {number}, type of room: {type}\n";

        }
    }
}
