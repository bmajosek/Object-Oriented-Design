using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Windows.Input;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace ConsoleApp25
{
    public static class readerFile
    {
        public static StreamReader reader = new StreamReader(Console.OpenStandardInput());
    }
    public interface ICommand
    {
        void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections);
    }

    public class ListCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments. Usage: list <class_name>");
                return;
            }

            var className = args[1];
            commandQueue.Enqueue(new ListCommandQueueItem(className));
        }
    }

    public class FindCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments. Usage: find <class_name> [requirements ...]");
                return;
            }

            var className = args[1];
            var requirements = args.Skip(2).ToList();
            commandQueue.Enqueue(new FindCommandQueueItem(className, requirements));
        }
    }

    public class AddCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid arguments. Usage: add <class_name> base|secondary");
                return;
            }

            var className = args[1];
            var representation = args[2];
            bool flag = false;
            var type = Type.GetType("ConsoleApp25." + className);
            if (representation == "secondary")
            {
                type = Type.GetType("ConsoleApp25." + className + "ListAdapt");
            }
            else if (representation != "base")
            {
                Console.WriteLine("Invalid arguments. Usage: add <class_name> base|secondary");
                return;
            }
            if (type == null)
            {
                Console.WriteLine($"Class {className} not found.");
                return;
            }
            var availableFields = type.GetProperties();
            Dictionary<string, string> properties = new Dictionary<string, string>();

            foreach (var item in availableFields)
            {
                properties.Add(item.Name, "1");
            }
            List<string> k = new List<string>();
            while (true)
            {
                if (ReferenceEquals(readerFile.reader.BaseStream, Console.OpenStandardInput()))
                    Console.Write("ByTE> ");
                string input = readerFile.reader.ReadLine();
                string[] lineTokens = input.Split('=');
                if (lineTokens.Length != 2)
                {
                    if (lineTokens[0] == "Done")
                    {
                        k.Add(input);
                        flag = true;
                        break;
                    }
                    else if (lineTokens[0] == "Exit")
                    {
                        k.Add(input); 
                        if (ReferenceEquals(readerFile.reader.BaseStream, Console.OpenStandardInput()))
                            Console.WriteLine("[Creation abandoned]");
                        commandQueue.Enqueue(new AddCommandQueueItem(className, null, flag, k, representation == "base"));
                        return;
                    }
                    Console.WriteLine("Invalid input: " + input);
                    continue;
                }

                k.Add(input);
                string fieldName = lineTokens[0];
                string value = lineTokens[1];

                var field = availableFields.FirstOrDefault(f => f.Name == fieldName);
                if (field == null)
                {
                    Console.WriteLine("Unknown field: " + fieldName);
                    continue;
                }
                properties[fieldName] = value;
            }

            Dictionary<string, Func<string[], object>> TypeConstructors = new Dictionary<string, Func<string[], object>>
            {
                { "ConsoleApp25.Class", (string[] a) => new Class(a[0], a[1], Int32.Parse(a[2]), null, null) },
                { "ConsoleApp25.Room",  (string[] a) => new Room((Int32.Parse(a[0])),(typeOfRoom)(Int32.Parse(a[1])), null) },
                { "ConsoleApp25.Teacher",  (string[] a) => new Teacher(a[0].Split(' ').ToList(), (string)a[1], (ranks)(Int32.Parse(a[2])), a[3], null) },
                { "ConsoleApp25.Student",  (string[] a) => new Student(a[0].Split(' ').ToList(), (string)a[1], (Int32.Parse(a[2])), (string)a[3], null) },
                { "ConsoleApp25.ClassListAdapt", (string[] a) => new Class(a[0], a[1], Int32.Parse(a[2]), null, null) },
                { "ConsoleApp25.RoomListAdapt",  (string[] a) => new Room((Int32.Parse(a[0])), (typeOfRoom)(Int32.Parse(a[1])), null) },
                { "ConsoleApp25.TeacherListAdapt",  (string[] a) => new Teacher(a[0].Split(' ').ToList(), (string)a[1], (ranks)(Int32.Parse(a[2])), a[3], null) },
                { "ConsoleApp25.StudentListAdapt",  (string[] a) => new Student(a[0].Split(' ').ToList(), (string)a[1], (Int32.Parse(a[2])), (string)a[3], null) }
                // add more here
            };

            List<string> objs = new List<string>();
            foreach (var item in properties)
            {
                objs.Add(item.Value);
            }
            string[] objArray = objs.ToArray();
            Func<string[], object> Construct = TypeConstructors[type.ToString()];
            var newElem = Construct(objArray);
            commandQueue.Enqueue(new AddCommandQueueItem(className, newElem, flag, k, representation == "base"));
        }
    }

    public class EditCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments. Usage: edit <name_of_the_class> [<requirement> ...]");
                return;
            }

            var className = args[1];
            if (!collections.ContainsKey(className))
            {
                Console.WriteLine($"Class {className} not found.");
                return;
            }

            var requirements = args.Skip(2).ToList();
            if (requirements.Count == 0)
            {
                Console.WriteLine("At least one requirement must be provided.");
                return;
            }
            var inputAll = new List<string>();
            while (true)
            {
                if (ReferenceEquals(readerFile.reader.BaseStream, Console.OpenStandardInput()))
                    Console.Write("ByTE> ");
                string input = readerFile.reader.ReadLine();
                string[] lineTokens = input.Split('=');

                if (lineTokens.Length != 2)
                {
                    if (lineTokens[0] == "Done")
                    {
                        break;
                    }
                    else if (lineTokens[0] == "Exit")
                    {
                        var cm = new EditCommandQueueItem(className, requirements, inputAll, false);
                        commandQueue.Enqueue(cm);
                        if (ReferenceEquals(readerFile.reader.BaseStream, Console.OpenStandardInput()))
                            Console.WriteLine("[Creation abandoned]");
                        return;
                    }
                    Console.WriteLine("Invalid input: " + input);
                    continue;
                }

                inputAll.Add(lineTokens[0]);
                inputAll.Add(lineTokens[1]);
            }
            var command = new EditCommandQueueItem(className, requirements, inputAll, true);
            commandQueue.Enqueue(command);
        }
    }

    public class QueuePrintCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments. Usage: queue print");
                return;
            }

            commandQueue.Print();
        }
    }

    public class QueueExportCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length < 3 || args.Length > 4)
            {
                Console.WriteLine("Invalid arguments. Usage: queue export {filename} [format]");
                return;
            }

            var filename = args[1];
            var format = args[2];

            commandQueue.Export(filename, format);
        }
    }

    public class QueueCommitCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments. Usage: queue commit");
                return;
            }

            commandQueue.Commit(collections);
        }
    }
    public class DeleteCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments. Usage: delete <name_of_the_class> [<requirement> ...]");
                return;
            }

            var className = args[1];
            if (!collections.ContainsKey(className))
            {
                Console.WriteLine($"Class {className} not found.");
                return;
            }

            var requirements = args.Skip(2).ToList();
            var command = new DeleteCommandQueueItem(className, requirements, true);
            commandQueue.Enqueue(command);
        }
    }

    public class QueueDismissCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments. Usage: queue dismiss");
                return;
            }

            commandQueue.Clear();
            Console.WriteLine("Command queue cleared.");
        }
    }
    public class QueueLoadCommand : ICommand
    {
        Dictionary<string, ICommand> _commands;
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid arguments. Usage: queue load {filename} {format}");
                return;
            }

            var filename = args[1];
            var format = args[2];

            commandQueue.Load(filename, collections, format);
            Console.WriteLine("Commands loaded from file.");
        }
    }
    public class ExitCommand : ICommand
    {
        public void Execute(string[] args, CommandQueue commandQueue, Dictionary<string, IMyCollections<object>> collections)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments. Usage: exit");
                return;
            }
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }

    public class CommandProcessor
    {
        private readonly Dictionary<string, ICommand> _commands;
        private readonly CommandQueue _commandQueue;
        private readonly Dictionary<string, IMyCollections<object>> _collections;

        public CommandProcessor(Dictionary<string, IMyCollections<object>> allObjects)
        {
            _collections = allObjects;
            _commandQueue = new CommandQueue();

            _commands = new Dictionary<string, ICommand>
            {
                { "list", new ListCommand() },
                { "find", new FindCommand() },
                { "add", new AddCommand() },
                { "edit", new EditCommand() },
                { "delete", new DeleteCommand() },
                { "clear", new QueueDismissCommand() },
                { "queue_print", new QueuePrintCommand() },
                { "queue_export", new QueueExportCommand() },
                { "queue_commit", new QueueCommitCommand() },
                { "queue_load", new QueueLoadCommand() },
                { "exit", new ExitCommand() }
            };

        }

        public void ProcessCommand(string command)
        {
            var tokens = command.Split(' ');
            if (tokens.Length == 0)
            {
                Console.WriteLine("Empty command.");
                return;
            }

            var commandName = tokens[0];

            if (_commands.TryGetValue(commandName, out var cmd))
            {
                cmd.Execute(tokens, _commandQueue, _collections);
            }
            else
            {
                Console.WriteLine($"Unknown command: {commandName}");
            }
        }
    }

    public class CommandQueue
    {
        public Queue<ICommandQueueItem> _queue;

        public CommandQueue()
        {
            _queue = new Queue<ICommandQueueItem>();
        }

        public void Enqueue(ICommandQueueItem command)
        {
            _queue.Enqueue(command);
        }
        public void Load(string filename, Dictionary<string, IMyCollections<object>> _collections, string format)
        {
            var _commands = new Dictionary<string, ICommand>
            {
                { "list", new ListCommand() },
                { "find", new FindCommand() },
                { "add", new AddCommand() },
                { "edit", new EditCommand() },
                { "delete", new DeleteCommand() },
                { "clear", new QueueDismissCommand() },
                { "queue_print", new QueuePrintCommand() },
                { "queue_export", new QueueExportCommand() },
                { "queue_commit", new QueueCommitCommand() },
                { "queue_load", new QueueLoadCommand() },
                { "exit", new ExitCommand() }
            };
            switch (format)
            {
                case "xml":
                    ImportFromXml(filename, _collections, _commands);
                    break;
                case "plaintext":
                    try
                    {
                        StreamReader reader = new StreamReader(filename);
                        readerFile.reader = reader;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var tokens = line.Split(' ');
                            if (tokens.Length == 0)
                            {
                                Console.WriteLine("Empty command.");
                                return;
                            }

                            var commandName = tokens[0];

                            if (_commands.TryGetValue(commandName, out var cmd))
                            {
                                cmd.Execute(tokens, this, _collections);
                            }
                            else
                            {
                                Console.WriteLine($"Unknown command: {commandName}");
                            }
                        }
                        readerFile.reader = new StreamReader(Console.OpenStandardInput());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading commands from file: {ex.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid export format. Supported formats: xml, plaintext");
                    break;
            }
        }
        public void ImportFromXml(string filename, Dictionary<string, IMyCollections<object>> _collections, Dictionary<string, ICommand> _commands)
        {
            try
            {
                using (XmlTextReader reader = new XmlTextReader(filename))
                {
                    while (reader.Read())
                    {
                        
                        if (reader.NodeType == XmlNodeType.Element && _commands.TryGetValue(reader.Name, out var _) == true)
                        {
                            string description = null;

                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Description")
                                {
                                    reader.Read();
                                    description = reader.Value;
                                    break;
                                }
                            }

                            if (!string.IsNullOrEmpty(description))
                            {
                                var tokens = description.Split(' ','\n');
                                if (tokens.Length == 0)
                                {
                                    Console.WriteLine("Empty command.");
                                    return;
                                }

                                var commandName = tokens[0];

                                if (_commands.TryGetValue(commandName, out var cmd))
                                {
                                    cmd.Execute(tokens, this, _collections);
                                }
                                else
                                {
                                    Console.WriteLine($"Unknown command: {commandName}");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Command queue imported from '{filename}' successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing command queue from XML: {ex.Message}");
            }
        }
        public void Clear()
        {
            _queue.Clear();
        }
        public void Print()
        {
            foreach (var command in _queue)
            {
                Console.WriteLine(command.ToString());
            }
        }

        public void Export(string filename, string format)
        {
            switch (format.ToLower())
            {
                case "xml":
                    ExportToXml(filename);
                    break;
                case "plaintext":
                    ExportToPlainText(filename);
                    break;
                default:
                    Console.WriteLine("Invalid export format. Supported formats: xml, plaintext");
                    break;
            }
        }

        public void ExportToXml(string filename)
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(filename, null))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("CommandQueue");

                    foreach (var command in _queue)
                    {
                        var tmp = command.ToString().Split(' ')[0];
                        writer.WriteStartElement(tmp);
                        writer.WriteElementString("Description", command.ToString());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                Console.WriteLine($"Command queue exported to '{filename}' successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting command queue to XML: {ex.Message}");
            }
        }

        public void ExportToPlainText(string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var command in _queue)
                {
                    writer.WriteLine(command.ToString());
                }
                Console.WriteLine("Export successful.");
            }
        }

        public void Commit(Dictionary<string, IMyCollections<object>> collections)
        {
            while (_queue.Count > 0)
            {
                var command = _queue.Dequeue();
                command.Execute(collections);
            }
            Console.WriteLine("Commit successful.");
        }
    }
    public interface ICommandQueueItem : ISerializable
    {
        void Execute(Dictionary<string, IMyCollections<object>> collections);
    }
    public class FindCommandQueueItem : ICommandQueueItem
    {
        private readonly string _className;
        private readonly List<string> _requirements;
        public FindCommandQueueItem() { }
        public FindCommandQueueItem(string className, List<string> requirements)
        {
            _className = className;
            _requirements = requirements;
        }
        public void Execute(Dictionary<string, IMyCollections<object>> collections)
        {
            if (!collections.ContainsKey(_className))
            {
                Console.WriteLine($"Class {_className} not found.");
                return;
            }

            var iterator = collections[_className].GetIterator();
            List<object> results = new List<object>();
            while (iterator.HasNext())
            {
                var obj = iterator.Current;
                results.Add(obj);
                iterator.MoveNext();
            }
            if (_requirements.Count == 0)
            {
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            for (int i = 0; i < _requirements.Count; i++)
            {
                string[] requirement = _requirements[i].Split('=');
                string propertyName = requirement[0];
                string propertyValue = requirement[1];

                foreach (var result in results)
                {
                    var property = result.GetType().GetProperty(propertyName);
                    if (property == null)
                    {
                        Console.WriteLine($"Property {propertyName} not found.");
                        continue;
                    }

                    var value = property.GetValue(result);
                    if (!value.ToString().Equals(propertyValue))
                    {
                        continue;
                    }

                    Console.WriteLine($"{result}");
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_className), _className);
            info.AddValue(nameof(_requirements), _requirements);
        }

        public override string ToString()
        {
            var k = "";
            foreach (var reqs in _requirements)
            {
                k += reqs.ToString();
            }
            return $"find {_className} {k}";
        }
    }
    public class DeleteCommandQueueItem : ICommandQueueItem
    {
        private readonly string _className;
        private readonly List<string> _requirements;
        private readonly bool _flag;

        public DeleteCommandQueueItem() { }
        public DeleteCommandQueueItem(string className, List<string> requirements, bool flag)
        {
            _className = className;
            _requirements = requirements;
            _flag = flag;
        }

        public void Execute(Dictionary<string, IMyCollections<object>> collections)
        {
            if (_flag == false) return;
            if (!collections.TryGetValue(_className, out var collection))
            {
                Console.WriteLine($"Class {_className} not found in collections.");
                return;
            }

            var iterator = collection.GetIterator();
            var deletedCount = 0;

            while (iterator.HasNext())
            {
                var obj = iterator.Current;

                if (CheckRequirements(obj))
                {
                    collection.Remove(obj);
                    deletedCount++;
                }

                iterator.MoveNext();
            }

            Console.WriteLine($"{deletedCount} record(s) deleted.");
        }

        private bool CheckRequirements(object obj)
        {
            foreach (var requirement in _requirements)
            {
                var tokens = requirement.Split('=');
                if (tokens.Length != 2)
                {
                    Console.WriteLine($"Invalid requirement: {requirement}");
                    return false;
                }

                var propertyName = tokens[0];
                var propertyValue = tokens[1];

                var property = obj.GetType().GetProperty(propertyName);
                if (property == null)
                {
                    Console.WriteLine($"Property {propertyName} not found.");
                    return false;
                }

                var value = property.GetValue(obj);
                if (value.ToString() != propertyValue)
                {
                    return false;
                }
            }

            return true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_className), _className);
            info.AddValue(nameof(_requirements), _requirements);
        }

        public override string ToString()
        {
            var k = "";
            foreach (var reqs in _requirements)
            {
                k += reqs.ToString();
            }
            return $"delete {_className} {k}";
        }
    }

    public class AddCommandQueueItem : ICommandQueueItem
    {
        private readonly string _className;
        private readonly object _newObject;
        private readonly bool _flag;
        private readonly bool _base;
        private readonly List<string> _input;
        public AddCommandQueueItem()
        {

        }
        public AddCommandQueueItem(string className, object newObject, bool flag, List<string> input, bool isBase)
        {
            _className = className;
            _newObject = newObject;
            _flag = flag;
            _input = input;
            _base = isBase;
        }

        public void Execute(Dictionary<string, IMyCollections<object>> collections)
        {
            if (_flag == false) return;
            if (collections.TryGetValue(_className, out var collection))
            {
                collection.Add(_newObject);
                Console.WriteLine("[Added]");
            }
            else
            {
                Console.WriteLine($"Class {_className} not found in collections.");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_className, true);
            info.AddValue(_newObject.ToString(), true);
        }

        public override string ToString()
        {
            var k = "";
            for(int i = 0; i < _input.Count - 1; i++)
            {
                k += _input[i] + "\n";
            }
            if(_flag && _base)
                return $"add {_className} base\n{k}Done";
            if (_flag && !_base)
                return $"add {_className} secondary\n{k}Done";
            if (!_flag && _base)
                return $"add {_className} base\n{k}Exit";
            return $"add {_className}\n secondary{k}Exit";
        }
    }
    [Serializable]
    public class ListCommandQueueItem : ICommandQueueItem
    {
        private readonly string _className;
        public ListCommandQueueItem() { }
        public ListCommandQueueItem(string className)
        {
            _className = className;
        }

        public void Execute(Dictionary<string, IMyCollections<object>> collections)
        {
            if (!collections.ContainsKey(_className))
            {
                Console.WriteLine($"Class {_className} not found.");
                return;
            }

            var iterator = collections[_className].GetIterator();
            while (iterator.HasNext())
            {
                var obj = iterator.Current;
                Console.WriteLine(obj.ToString());
                iterator.MoveNext();
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_className, true);
        }

        public override string ToString()
        {
            return $"list {_className}";
        }
    }

    public class EditCommandQueueItem : ICommandQueueItem
    {
        private readonly string _className;
        private readonly List<string> _requirements;
        private readonly List<string> _inputTokens;
        private readonly bool _flag;
        public EditCommandQueueItem() { }
        public EditCommandQueueItem(string className, List<string> requirements, List<string> input, bool flag)
        {
            _className = className;
            _requirements = requirements;
            _inputTokens = input;
            _flag = flag;
        }

        public void Execute(Dictionary<string, IMyCollections<object>> collections)
        {
            if (_flag == false) return;
            if (collections.TryGetValue(_className, out var collection))
            {
                var iterator = collection.GetIterator();
                while (iterator.HasNext())
                {
                    var obj = iterator.Current;
                    bool match = true;
                    foreach (var requirement in _requirements)
                    {
                        var tokens = requirement.Split('=');
                        var propertyName = tokens[0];
                        var propertyValue = tokens[1];

                        var property = obj.GetType().GetProperty(propertyName);
                        if (property == null)
                        {
                            Console.WriteLine($"Property {propertyName} not found.");
                            match = false;
                            break;
                        }

                        var value = property.GetValue(obj);
                        if (!value.ToString().Equals(propertyValue))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        for (int i = 0; i < _inputTokens.Count; i += 2)
                        {
                            string fieldName = _inputTokens[i];
                            string newValue = _inputTokens[i + 1];
                            SetProperty(obj, fieldName, newValue);

                            Console.WriteLine($"Field {fieldName} changed to {newValue}.");
                        }

                        Console.WriteLine($"Object {_className} edited.");
                    }

                    iterator.MoveNext();
                }
            }
            else
            {
                Console.WriteLine($"Class {_className} not found in collections.");
            }
        }

        public override string ToString()
        {
            var k = "";
            foreach(var reqs in _requirements)
            {
                k += reqs.ToString() + " ";
            }
            var kk = "";
            for(int i = 0;i < _inputTokens.Count; i+=2)
            {
                kk += _inputTokens[i] + "=" + _inputTokens[i + 1] + "\n";
            }
            if(_flag) return $"edit {_className} {k}\n{kk}Done";
            return $"edit {_className} {k}\n {kk}Exit";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var k = "";
            foreach (var reqs in _requirements)
            {
                k += reqs.ToString();
            } 
            info.AddValue(_className, true);
            info.AddValue(k, true);
            k = "";
            foreach (var reqs in _inputTokens)
            {
                k += reqs.ToString();
            }
            info.AddValue(k, true);
        }



        public static void SetProperty(object obj, string propertyName, object newValue)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, Convert.ChangeType(newValue, propertyInfo.PropertyType));
            }
            else
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }
    }

}
