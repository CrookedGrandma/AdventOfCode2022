namespace AdventOfCode2022;
public class _7 : Base
{
    protected override void Action()
    {
        //UseExample();

        Folder root = new Folder("/");
        Folder cwd = root;
        List<Folder> folders = new();
        List<FFile> files = new();

        folders.Add(root);

        foreach (string line in InputLines)
        {
            string[] split = line.Split(' ');
            if (split[0] == "$")
            {
                // Command
                if (split[1] == "cd")
                {
                    // Change dir
                    string dir = split[2];
                    if (dir == "/")
                        cwd = root;
                    else if (dir == "..")
                        cwd = cwd.parent!;
                    else
                        cwd = cwd.GetChild(dir)!;
                }
                else if (split[1] == "ls")
                {
                    // Result is printed, nothing to do
                }
                else
                {
                    throw new Exception("wtf is this command uwu");
                }
            }
            else
            {
                // Result
                if (split[0] == "dir")
                {
                    // Folder
                    string dir = split[1];
                    if (!cwd.ContainsFolder(dir))
                    {
                        Folder newF = new(dir, cwd);
                        cwd.children.Add(newF);
                        folders.Add(newF);
                    }
                }
                else
                {
                    int size = int.Parse(split[0]);
                    string name = split[1];
                    FFile newF = new(name, cwd, size);
                    cwd.files.Add(newF);
                    files.Add(newF);
                }
            }
        }

        int rootSize = root.GetSize();

        int answer = folders.Where(f => f.GetSize() <= 100000).Select(f => f.GetSize()).Sum();

        WriteLine(answer);

        B();

        var sorted = folders.Where(f => f.GetSize() > 30000000 - (70000000 - rootSize)).OrderBy(f => f.GetSize()).ToList();

        WriteLine(sorted[0]);
    }
}

public class Folder
{
    public string name;
    public Folder? parent;
    public List<Folder> children;
    public List<FFile> files;

    private int size = -1;

    public Folder(string name, Folder? parent = null)
    {
        this.name = name;
        this.parent = parent;
        this.children = new();
        this.files = new();
    }

    public override string ToString() => $"{name} ({size})";

    public bool ContainsFolder(string name) => GetChild(name) != null;

    public Folder? GetChild(string name)
    {
        foreach (Folder child in children)
        {
            if (child.name == name)
                return child;
        }
        return null;
    }

    public bool ContainsFile(string name) => GetFile(name) != null;

    public FFile? GetFile(string name)
    {
        foreach (FFile file in files)
        {
            if (file.name == name)
                return file;
        }
        return null;
    }

    public int GetSize()
    {
        if (size >= 0)
            return size;
        if (children.Any(f => f.size < 0))
        {
            int sum = children.Select(f => f.GetSize()).Sum() + GetFileSize();
            size = sum;
            return sum;
        }
        else
        {
            int fileSum = GetFileSize();
            size = fileSum;
            return fileSum;
        }
    }

    private int GetFileSize()
    {
        if (size >= 0)
            return size;
        return files.Select(f => f.size).Sum();
    }
}

public class FFile
{
    public string name;
    public Folder location;
    public int size;

    public FFile(string name, Folder location, int size)
    {
        this.name = name;
        this.location = location;
        this.size = size;
    }

    public override string ToString() => $"{name} ({size})";
}
