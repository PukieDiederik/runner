using System.Collections.Generic;

public sealed class DesktopRepository
{
    private static DesktopRepository? instance = null;
    private static readonly object padlock = new object();

    // Variables used for loading Desktop entries
    public List<string> entry_locations {get; private set;} = new List<string>();

    public List<DesktopEntry> entries = new List<DesktopEntry>();

    // Constructor
    private DesktopRepository() {}

    public static DesktopRepository Instance { get {
        if (instance == null)
        {
            // Ensure thread-safety
            lock(padlock)
            {
                instance = new DesktopRepository();

                // Setup the instance

                // Where to find the desktop files:
                string? xdg_data_dirs = Environment.GetEnvironmentVariable("XDG_DATA_DIRS");
                if (xdg_data_dirs != null)
                    instance.entry_locations.AddRange(xdg_data_dirs.Split(':').Select(d => d + (d.EndsWith('/') ? "" : "/") + "applications/"));

                // Load the desktop files
                instance.entries = new List<DesktopEntry>();
                instance.Load();

            }
        }
        return instance;
    }}

    // Loads the desktop files vased on `entry_locations`
    public void Load(){
        // Collect desktop files
        Queue<string> dirs_to_search = [];
        foreach(string l in entry_locations){
            dirs_to_search.Enqueue(l);
            while(dirs_to_search.Count != 0)
            {
                string cur = dirs_to_search.Dequeue();
                if (Directory.Exists(cur))
                {
                    // Add all subdirectories to be searched
                    string[] dirs = Directory.GetDirectories(cur);
                    foreach(string d in dirs)
                        dirs_to_search.Enqueue(d);

                    // Find all desktop files
                    IEnumerable<string> files = Directory.EnumerateFiles(cur);
                    foreach(string f in files){
                        if (f.EndsWith(".desktop") || f.EndsWith(".directory"))
                        {
                            entries.Add(new DesktopEntry(f, l));
                        }
                    }
                }
            }
        }
    }
}