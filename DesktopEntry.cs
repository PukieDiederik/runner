using System;

enum DesktopType{
    Application,
    Link,
    Directory,
}



class DesktopEntry{
    public string id {get; private set;}

    // keys
    public DesktopType type                 {get; private set;}

    public string   name                    {get; private set;} = "";
    public string?  generic_name            {get; private set;}
    public bool     no_display              {get; private set;} = false;
    public string?  comment                 {get; private set;}
    public string?  icon                    {get; private set;}
    public bool     hidden                  {get; private set; } = false;
    public string[] actions                 {get; private set;} = [];
    public string[] mime_type               {get; private set;} = [];
    public string[] categories              {get; private set;} = [];
    public string[] implements              {get; private set;} = [];
    public string[] keywords                {get; private set;} = [];
    public bool     startup_notify          {get; private set;}
    public string?  startup_wm_class        {get; private set;}
    public bool     prefers_non_default_gpu {get; private set;} = false;
    public bool     single_main_window      {get; private set;} = false;

    public string[] only_show_in            {get; private set;} = [];
    public string[] not_show_in             {get; private set;} = [];

    public string?  exec                    {get; private set;}
    public string?  try_exec                {get; private set;}
    public bool     dbus_activactable       {get; private set;} = false;
    public string?  path                    {get; private set;}
    public bool     terminal                {get; private set;} = false;
    public string? url                      {get; private set;}

    // Helper functions:
    private static bool ParseBoolean(string s){
        if (s == "true")
            return true;
        else if (s == "false")
            return false;
        else
            throw new Exception("Could not parse boolean");
    }

    private static string[] ParseStrList(string s){
        return s.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }


    public DesktopEntry(string file_path, string path_base){

        if (!file_path.StartsWith(path_base))
            throw new Exception("Filepath does not start with `path_base`");

        string[] lines = File.ReadAllLines(file_path); // Will throw exception if any error occurs

        id = file_path.Remove(0,path_base.Length).Replace('/', '-');

        if (file_path.EndsWith(".directory"))
            type = DesktopType.Directory;
        else
            type = DesktopType.Application;

        // TODO: parse file
        for(int i = 0; i < lines.Length; ++i)
        {
            if (lines[i].StartsWith('#')) continue; // Skip if it's a comment
            if (lines[i].StartsWith('[')) // If it i s a group header
            {
                if(lines[i] == "[Desktop Entry]")
                {
                    ++i;

                    KeyValuePair<string, string> kv;
                    // Start parsing desktop entry
                    while(i < lines.Length && !lines[i].StartsWith('['))
                    {
                        if (lines[i].StartsWith('#') || lines[i].Trim().Length == 0) { ++i; continue;}
                        int sep = lines[i].IndexOf('=');
                        if (sep < 0)
                            throw new Exception("Desktop file parsing error");
                        kv = new KeyValuePair<string, string>(lines[i][..sep], lines[i][(sep+1)..]);



                        // Check between parsing and validation of different keys
                        switch(kv.Key){
                            case "Type":
                                switch(kv.Value){
                                    case "Application":
                                        type = DesktopType.Application;
                                        break;
                                    case "Link":
                                        type = DesktopType.Link;
                                        break;
                                    case "Directory":
                                        type = DesktopType.Directory;
                                        break;
                                }
                                break;

                            case "Name":
                                name = kv.Value;
                                break;

                            case "GenericName":
                                generic_name = kv.Value;
                                break;

                            case "NoDisplay":
                                no_display = ParseBoolean(kv.Value);
                                break;

                            case "Comment":
                                comment = kv.Value;
                                break;

                            case "Icon":
                                icon = kv.Value; 
                                break;

                            case "Hidden":
                                hidden = ParseBoolean(kv.Value);
                                break;

                            case "OnlyShowIn":
                                only_show_in = ParseStrList(kv.Value);
                                break;

                            case "NotShowIn":
                                not_show_in = ParseStrList(kv.Value);
                                break;

                            case "DBusActivateable":
                                dbus_activactable = ParseBoolean(kv.Value);
                                break;

                            case "TryExec":
                                try_exec = kv.Value;
                                break;

                            case "Exec":
                                exec = kv.Value;
                                break;

                            case "Path":
                                path = kv.Value;
                                break;
                            
                            case "Terminal":
                                terminal = ParseBoolean(kv.Value);
                                break;

                            case "Actions":
                                // TODO: implement this
                                break;
                            
                            case "MimeType":
                                mime_type = ParseStrList(kv.Value);
                                break;

                            case "Implements":
                                // TODO:
                                break;

                            case "Keywords":
                                keywords = ParseStrList(kv.Value);
                                break;

                            case "StartupNotify":
                                startup_notify = ParseBoolean(kv.Value);
                                break;
                            
                            case "StartupWMClass":
                                startup_wm_class = kv.Value;
                                break;

                            case "URL":
                                url = kv.Value;
                                break;

                            case "PrefersNonDefaultGPU":
                                prefers_non_default_gpu = ParseBoolean(kv.Value);
                                break;
                            
                            case "SingleMainWindow":
                                single_main_window = ParseBoolean(kv.Value);
                                break;

                            default:
                                break; // Unimplemented
                        }
                        ++i;
                    }
                }
                // Implement desktop actions
            }
        }

        if (name == "") // If name was not set
            throw new Exception("name not set in desktop file");
    }

    public override string ToString()
    {
        return id;
    }
}