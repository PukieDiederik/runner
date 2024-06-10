using System;
using System.IO;

class Program
{
    static int Main(){
        // Setup desktop file locations
        string? xdg_data_dirs = Environment.GetEnvironmentVariable("XDG_DATA_DIRS");
        string[] desktop_locations = [];
        if (xdg_data_dirs != null)
        {
            desktop_locations = xdg_data_dirs.Split(':').Select(d => d +
                                                                 (d.EndsWith('/') ? "" : "/") +
                                                                 "applications/").ToArray();
        }
        else {
            desktop_locations = ["/usr/share/applications/",
                                 "/usr/local/share/applications/"];
        }

        foreach(string entry in desktop_locations)
            Console.WriteLine(entry);

        // Loop over each desktop location
        List<DesktopEntry> desktop_entries = [];

        // Collect desktop files
        foreach(string l in desktop_locations){
            if (Directory.Exists(l))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(l);
                foreach(string f in files)
                    if (f.EndsWith(".desktop") || f.EndsWith(".directory"))
                    {
                        desktop_entries.Add(new DesktopEntry(f, l));
                    }
            }
        }

        // Print desktop files
        // foreach(string f in d_files)
        //     Console.WriteLine(f);

        return 0;
    }
}
