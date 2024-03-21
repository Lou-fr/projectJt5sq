using System;

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescritption;
    private string _commandFormat;
    public string commandId {get {return _commandId;}}
    public string commandDescritption {get {return _commandDescritption;}}
    public string commandFormat {get {return _commandFormat;}}

    public DebugCommandBase(string id,string descritprion, string format)
    {
        _commandId = id;
        _commandDescritption = descritprion;
        _commandFormat = format;
    }
}
public class DebugCommand : DebugCommandBase
{
    public Action command;
    public DebugCommand(string id, string descritprion, string format, Action command):base (id,descritprion,format)
    {
        this.command = command;
    }
    public void Invoke()
    {
        command.Invoke();
    }
}
public class DebugCommand<T1> : DebugCommandBase
{
    public Action<T1> command;
    public DebugCommand(string id, string descritprion, string format, Action<T1> command):base (id,descritprion,format)
    {
        this.command = command;
    }
    public void Invoke(T1 v)
    {
        command.Invoke(v);
    }
}
public class DebugCommand<T1,T2> : DebugCommandBase
{
    public Action<T1,T2> command;
    public DebugCommand(string id, string descritprion, string format, Action<T1,T2> command):base (id,descritprion,format)
    {
        this.command = command;
    }
    public void Invoke(T1 v,T2 v2)
    {
        command.Invoke(v,v2);
    }
}
