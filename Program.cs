﻿using System;
using System.IO;
using System.Net.Sockets;
using Tmds.DBus;

class Program
{
    static string[] GetDesktopDirectories(){
        string? xdg_data_dirs = Environment.GetEnvironmentVariable("XDG_DATA_DIRS");
        if (xdg_data_dirs != null)
            return xdg_data_dirs.Split(':').Select(d => d + (d.EndsWith('/') ? "" : "/") + "applications/").ToArray();
        else
            return ["/usr/share/applications/", "/usr/local/share/applications/"];
    }

    static async Task Main(){
        // Setup desktop file locations
        string[] desktop_locations = GetDesktopDirectories();

        // Loop over each desktop location
        HashSet<DesktopEntry> desktop_entries = [];

        // Collect desktop files
        Queue<string> dirs_to_search = [];
        foreach(string l in desktop_locations){
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
                            desktop_entries.Add(new DesktopEntry(f, l));
                        }
                    }
                }
            }
        }

        // Print desktop files
        // foreach(DesktopEntry e in desktop_entries)
        //     Console.WriteLine(e);

        desktop_entries.First().Print();

        // Setup DBusServer

        Connection c = new Connection(Address.Session);
        await c.ConnectAsync();
        var dbs = new DBusServer();
        await c.RegisterObjectAsync(dbs);
        await c.RegisterServiceAsync("com.pukie.runner");

        // await c.RegisterObjectAsync(s);
        // try{
        //     await c.RegisterServiceAsync("com.pukie.runner", ServiceRegistrationOptions.None);
        // }
        // catch {
        //     Console.WriteLine("Error Occured");
        // }
        // Console.WriteLine("Fine here");
        await Task.Delay(-1);
    }
}
