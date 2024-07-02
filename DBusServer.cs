using Tmds.DBus;

[DBusInterface("com.pukie.runner")]
public interface IDBusServer : IDBusObject
{
    public Task<string> HelloWorldAsync(string teststring);
}

public class DBusServer : IDBusServer
{
    ObjectPath IDBusObject.ObjectPath => new ObjectPath("/com/pukie/runner");

    public Task<string> HelloWorldAsync(string teststring)
    {
        Console.WriteLine("Func called");
        return Task.FromResult($"Hello World, {teststring}");
    }
}