


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Numerics;

var snippet = System.IO.File.ReadAllText("./day7.txt");

Command[] Parse(string snippet) {
    Command ParseCommand(ref int i, ref Span<string> tokens) {
        var token = tokens[i];
        if("cd".Equals(token)) {
            return new Command {
                Name = "cd",
                Argument = tokens[++i],
                Result = new List<(string, string)>()
            };
        } else if("ls".Equals(token)) {
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
    Span<string> tokens = snippet.Split(new char[] {' ', '\n'}, StringSplitOptions.None).Select(token => token.Trim()).ToArray();
    List<Command> commands = new();
    for (int i = 0; i < tokens.Length; i++)
    {
        i++;
        commands.Add(ParseCommand(ref i, ref tokens));
    }
    return commands.ToArray();
}

var commands = Parse(snippet);
var Configuration = Folder.Root.Apply(new List<string>(), commands); 
var part1 = Configuration
                    .Flatten().Select(folder => folder.Size())
                    .Where(size => size < 100_000)
                    .Sum();
long targetSize = 30_000_000 - (70_000_000 - Folder.Root.Size());
Console.WriteLine(part1);
Console.WriteLine(targetSize);
var part2 = Configuration
                    .Flatten().Select(folder => folder.Size())
                    .Where(size => size >= targetSize)
                    .Order().First();
Console.WriteLine(part2);

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
            } else if ("..".Equals(subdir)) {
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
                if("dir".Equals(data)) {
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
}