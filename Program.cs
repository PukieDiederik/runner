using System;
using System.ComponentModel;
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

        // Make sure DesktopRepository is initialized on startup
        DesktopRepository dr = DesktopRepository.Instance;
        dr.entries.First().Print();

        // Setup DBusServer
        Connection c = new Connection(Address.Session);
        await c.ConnectAsync();
        var dbs = new DBusServer();
        await c.RegisterObjectAsync(dbs);
        await c.RegisterServiceAsync("com.pukie.runner");

        Console.WriteLine("Now listening for dbus connections");

        await Task.Delay(-1);
    }
}
