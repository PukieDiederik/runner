enum DesktopType{
    Application,
    Link,
    Directory,
}

class DesktopEntry{
    public string id {get; private set;}
    public string name {get; private set;}
    public string? generic_name {get; private set;}

    public DesktopType type {get; private set;}

    public DesktopEntry(string file_path, string path_base){

        if (!file_path.StartsWith(path_base))
            throw new ArgumentException();
        id = file_path.Remove(0,path_base.Length).Replace('/', '-');
        Console.WriteLine(id);
        // TODO: parse file



        type = DesktopType.Application;
        name = "";
    }

    public string toString()
    {
        return id;
    }
}