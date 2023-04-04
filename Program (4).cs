using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using ConsoleApp25;

public class Program
{
    public static void PrintTask(List<IRoom> listofroom)
    {
        List<IClass> list = new List<IClass>();
        foreach (var room in listofroom)
        {
            foreach (var clas in room.classes)
            {
                int k = 0;
                IStudent tmpstud = null;
                ITeacher tmpteach = null;
                if (clas.students.Count >= 1)
                {
                    k++;
                    foreach (var stud in clas.students)
                    {
                        if (stud.names.Count == 2)
                        {
                            k++;
                            tmpstud = stud;
                            break;
                        }
                    }
                    foreach (var teac in clas.listofTeachers)
                    {
                        if (teac.names.Count == 2)
                        {
                            k++;
                            tmpteach = teac;
                            break;
                        }
                    }
                    if (k > 2 && list.Contains(clas) == false)
                    {
                        list.Add(clas);
                        Console.Write(clas);
                        Console.Write(tmpstud);
                        Console.Write(tmpteach);
                        Console.WriteLine();
                        Console.WriteLine();
                        
                    }
                }
            }
           
        }
    }
    public static void Main()
    {
        List<IRoom> rooms = new();
        Room room1 = new Room(107, typeOfRoom.Lecture, new List<IClass> { });
        Room room2 = new Room(204, typeOfRoom.Training, new List<IClass> { });
        Room room3 = new Room(21, typeOfRoom.Lecture, new List<IClass> { });
        Room room4 = new Room(123, typeOfRoom.Laboratory, new List<IClass> { });
        Room room5 = new Room(404, typeOfRoom.Lecture, new List<IClass> { });
        Room room6 = new Room(504, typeOfRoom.Training, new List<IClass> { });
        Room room7 = new Room(73, typeOfRoom.Laboratory, new List<IClass> { });

        var MD2 = new Class("Diabolical Mathematics 2", "MD2", 2, new List<ITeacher> { }, new List<IStudent> { });
        var RD = new Class("Routers descriptions", "RD", 1, new List<ITeacher> { }, new List<IStudent> { });
        var WDK = new Class("Introduction to cables", "WDK", 5, new List<ITeacher> { }, new List<IStudent> {  });
        var AC3 = new Class("Advanced Cooking 3", "AC3", 3, new List<ITeacher> { }, new List<IStudent> { });

        List<IClass> classes1 = new List<IClass> { AC3 };
        List<IClass> classes2 = new List<IClass> { MD2 };
        List<IClass> classes3 = new List<IClass> { RD, WDK };
        List<IClass> classes4 = new List<IClass> { WDK };
        List<IClass> classes5 = new List<IClass> { AC3 };
        List<IClass> classes6 = new List<IClass> { MD2, WDK };
        List<IClass> classes7 = new List<IClass> { MD2, WDK, AC3 };
        List<IClass> classes8 = new List<IClass> { RD, WDK };
        List<IClass> classes9 = new List<IClass> { RD, WDK, AC3 };
        List<IClass> classes10 = new List<IClass> { MD2, WDK, AC3 };

        Teacher P1 = new Teacher(new List<string> { "Tomas" }, "Cherrmann", ranks.MiB, "P1", classes1);
        Teacher P2 = new Teacher(new List<string> { "Jon" }, "Tron", ranks.TiB, "P2", classes2);
        Teacher P3 = new Teacher(new List<string> { "William", "Joseph" }, "Blazkowicz", ranks.GiB, "P3", classes3);
        Teacher P4 = new Teacher(new List<string> { "Arkadiusz", "Amadeusz" }, "Kamiński", ranks.KiB, "P4", classes4);
        Teacher P5 = new Teacher(new List<string> { "Cooking" }, "Mama", ranks.GiB, "P5", classes5);

        Student S1 = new Student(new List<string> { "Robert" }, "Kielbica", 3, "S1", classes6);
        Student S2 = new Student(new List<string> { "Archibald", "Agapios" }, "Linux", 7, "S2", classes7);
        Student S3 = new Student(new List<string> { "Angrboða" }, "Kára", 1, "S3", classes8);
        Student S4 = new Student(new List<string> { "Olympos" }, "Andronikos", 5, "S4", classes9);
        Student S5 = new Student(new List<string> { "Mac", "Rhymes" }, "Pickuppicker", 6, "S5", classes10);

        room1.classes.Add(MD2);
        room1.classes.Add(RD);
        room1.classes.Add(WDK);
        room1.classes.Add(AC3);
        room2.classes.Add(WDK);
        room2.classes.Add(AC3);
        room3.classes.Add(RD);
        room4.classes.Add(RD);
        room4.classes.Add(WDK);
        room5.classes.Add(MD2);
        room5.classes.Add(WDK);
        room5.classes.Add(RD);
        room6.classes.Add(MD2);
        room7.classes.Add(AC3);
        MD2.listofTeachers.Add(P2);
        MD2.students.Add(S1);
        MD2.students.Add(S2);
        MD2.students.Add(S5);
        RD.listofTeachers.Add(P3);
        RD.students.Add(S3);
        RD.students.Add(S4);
        WDK.listofTeachers.Add(P4);
        WDK.listofTeachers.Add(P3);
        WDK.students.Add(S1);
        WDK.students.Add(S2);
        WDK.students.Add(S3);
        WDK.students.Add(S4);
        WDK.students.Add(S2);
        AC3.listofTeachers.Add(P5);
        AC3.listofTeachers.Add(P1);
        AC3.students.Add(S2);
        AC3.students.Add(S4);
        AC3.students.Add(S5);
        rooms.Add(room1);
        rooms.Add(room2);
        rooms.Add(room3);
        rooms.Add(room4);
        rooms.Add(room5);
        rooms.Add(room6);
        rooms.Add(room7);
        List<ClassListAdapt> t1 = new();
        List<RoomListAdapt> t2 = new();
        List<TeacherListAdapt> t3 = new();
        List<StudentListAdapt> t4 = new();
        List<ClassTextAdapt> t5 = new();
        List<RoomTextAdapt> t6 = new();
        List<TeacherTextAdapt> t7 = new();
        List<StudentTextAdapt> t8 = new();

        foreach (var item in rooms)
        {
            var k = new RoomList(item.number, item.type, item.classes);
            var kt = new RoomRep(item.number, item.type, item.classes);
            t2.Add(new RoomListAdapt(k));
            t6.Add(new RoomTextAdapt(kt));
            Slownik.Dict.Add(item.number.ToString(), k);
            foreach (var item1 in item.classes)
            {
                var t = new ClassList(item1.Name,item1.Code,item1.Duration,item1.listofTeachers,item1.students);
                var tk = new ClassRep(item1.Name, item1.Code, item1.Duration, item1.listofTeachers, item1.students);
                var tt = new ClassListAdapt(t);
                if (t1.Exists(e => e.Code == tt.Code) == false)
                {
                    t1.Add(tt);
                    t5.Add(new ClassTextAdapt(tk));
                    Slownik.Dict.Add(tt.Code, item1);
                }
                foreach (var item2 in item1.students)
                {
                    var ttt = new StudentList(item2.names,item2.surname,item2.semester,item2.code,item2.classes);
                    var tttk = new StudentRep(item2.names, item2.surname, item2.semester, item2.code, item2.classes);
                    var tttt = new StudentListAdapt(ttt);
                    if (t4.Exists(e => e.code == tttt.code) == false)
                    {
                        t4.Add(tttt);
                        t8.Add(new StudentTextAdapt(tttk));
                        Slownik.Dict.Add(tttt.code, item2);

                    }
                }
                foreach (var item2 in item1.listofTeachers)
                {
                    var ttt = new TeacherList(item2.names, item2.surname, item2.rank, item2.code, item2.classes);
                    var tttk = new TeacherRep(item2.names, item2.surname, item2.rank, item2.code, item2.classes);
                    var tttt = new TeacherListAdapt(ttt);
                    if (t3.Exists(e => e.code == tttt.code) == false)
                    {
                        t3.Add(tttt);
                        t7.Add(new TeacherTextAdapt(tttk));
                        Slownik.Dict.Add(tttt.code, item2);

                    }
                }
            }
        }
        Console.WriteLine("Base Rep\n");
        Program.PrintTask(rooms);
        Console.WriteLine("List Rep\n");
        Program.PrintTask(new List<IRoom>(t2));
        Console.WriteLine("Text Rep\n");
        Program.PrintTask(new List<IRoom>(t6));


    }
}