using System.Diagnostics;

var commands = Command.Parse(System.IO.File.ReadAllText("./day7.txt"));
var Configuration = Folder.Root.Apply(new List<string>(), commands); 
var part1 = from folder in Configuration.Flatten()
              let size = folder.Size() 
              where size < 100_000
              select size;
var result1 = part1.Sum();
var part2 = from folder in Configuration.Flatten()
              let size = folder.Size() 
              let target = 30_000_000 - (70_000_000 - Folder.Root.Size())
              where size >= target
              orderby size ascending
              select size;
var result2 = part2.First();

record Node(String name) {
    public long Size() =>
        this switch {
            File file => file.size,
            Folder folder => 
                folder.children.Aggregate(0, (long acc, Node item) => acc + item.Size())
        };
}
record File(String name, long size) : Node(name);
record Folder(String name, Folder parent, List<Node> children) : Node(name) {
    public static Folder Root = new Folder(
        name : "/", children : new List<Node>(), parent: null
    );

    public IEnumerable<Node> Flatten() {
        yield return this;
        foreach (var item in children)
        {
            if(item is Folder folder) {
                foreach (var child in folder.Flatten())
                {
                    yield return child;
                }
            }
        }
    }

    public Folder Apply(List<string> path, Span<Command> commands) {
        Folder Navigate(Folder @this, string subdir) {
            if("/" == subdir) {
                path.Clear();
                return Folder.Root;
            } else if (".." == subdir) {
                path.RemoveAt(path.Count - 1);
                return @this.parent;
            } else {
                path.Add(subdir);
                return @this.children.Where(child => child.name == subdir).First() as Folder;
            }
        }

        void Show(Folder @this, List<(string, string)> results) {
            foreach (var (data, name) in results)
            {
                if("dir" == data) {
                    @this.children.Add(new Folder(
                        name, @this, new List<Node>()
                    ));
                } else {
                    @this.children.Add(new File(
                        name, Int64.Parse(data)
                    )); 
                }
            }
        }

        Folder current = this;
        foreach (var command in commands)
        {
            switch (command.Name)
            {
                case "cd":
                    current = Navigate(current, command.Argument);
                    break;
                case "ls":
                    Show(current, command.Result);
                    break;
                default : throw new UnreachableException();
            }
        }        
        return this;
    }
}

record Command {
    public string Name;
    public string Argument;
    public List<(string, string)> Result;
    public static Command[] Parse(string snippet) {
        Command ParseCommand(scoped ref int i, ref Span<string> tokens) {
            var token = tokens[i];
            if("cd" == token) {
                return new Command {
                    Name = "cd",
                    Argument = tokens[++i],
                    Result = new List<(string, string)>()
                };
            } else if("ls" == token) {
                var results = new List<(string, string)>();
                do {
                    results.Add((tokens[++i], tokens[++i]));
                } while (i + 1 < tokens.Length && tokens[i + 1] != "$");           
                return new Command {
                    Name = "ls",
                    Argument = string.Empty,
                    Result = results
                };
            } 
            else throw new UnreachableException($"token unkonwn");
        }
        Span<string> tokens = snippet.Split(
            new char[]{' ', '\n'}, 
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
        List<Command> commands = new();
        for (int i = 0; i < tokens.Length; i++)
        {
            i++;
            commands.Add(ParseCommand(ref i, ref tokens));
        }
        return commands.ToArray();
    }
}
