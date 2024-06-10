enum DesktopType{
    Application,
    Link,
    Directory,
}

class DesktopEntry{
    public string? name {get; private set;}
    public string? generic_name {get; private set;}

    public DesktopType type {get; private set;}

    public DesktopEntry(string? _name, DesktopType _type, string? _generic_name = "") {
        name = _name;
        generic_name = _generic_name;

        type = _type;
    }

    public DesktopEntry(string file_name){
        // TODO: parse file

        type = DesktopType.Application;
    }
}