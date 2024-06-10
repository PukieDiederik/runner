using System;
using System.IO;

class Program
{
    private static readonly string[] desktop_locations = ["/usr/share/applications/",
                                                   "/usr/local/share/applications/",
                                                   "~/.local/share/applications/"];

    private List<DesktopEntry> desktop_entries;

    static int Main(){
        // Loop over each desktop location
        List<string> d_files = [];

        // Collect desktop files
        foreach(string l in desktop_locations){
            if (Directory.Exists(l))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(l);
                foreach(string f in files)
                    if (f.EndsWith(".desktop") || f.EndsWith(".directory"))
                        d_files.AddRange(files);
            }
        }

        // Print desktop files
        foreach(string f in d_files)
            Console.WriteLine(f);

        return 0;
    }
}
