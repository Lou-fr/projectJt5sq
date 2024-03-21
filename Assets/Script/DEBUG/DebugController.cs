using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;
    string input;

    public static DebugCommand<string> SEND_JOINREQ;
    public static DebugCommand HELP;
    public List<object> commandList;
    Vector2 scroll;

    void Awake()
    {
        SEND_JOINREQ =new DebugCommand<string>("send_joinreq","Send a join request to a target user","send_joinreq <TargetId> ",(x) =>
        {
            FriendsManager.SendjoinRequest(x);
        });
        HELP = new DebugCommand("help","show this help message","help",()=>
        {
            showHelp= true;
        });
        commandList = new List<object>
        {
            SEND_JOINREQ,
            HELP
        };
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
    }
    public void OnReturn(InputValue v)
    {
        if(showConsole)
        {
            HandleInput();
            input="";
        }
    }
    void OnGUI()
    {
        if (!showConsole) return;
        float y = 0;
        if (showHelp)
        {
            GUI.Box(new Rect(0,y,Screen.width,100),"");
            Rect viewport = new Rect(0,0,Screen.width -30, 20*commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0,y+5f,Screen.width,90),scroll,viewport);
            for(int i=0;i<commandList.Count;i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescritption}";
                Rect labelRect = new Rect(5,20*i,viewport.width -100,20);
                GUI.Label(labelRect,label);
            }
            GUI.EndScrollView();
            y+=100;
        }
        GUI.Box(new Rect(0, y,Screen.width,30),"");
        GUI.backgroundColor = new Color(0,0,0,0);
        input = GUI.TextField(new Rect(10f, y+5f,Screen.width-20f,20f),input);
    }
    private void HandleInput()
    {
        string[] proprieties = input.Split(" ");
        for (int i=0;i<commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if(input.Contains(commandBase.commandId))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }else if (commandList[i] as DebugCommand<string> !=null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(string.Format(proprieties[1]));
                }
            }
        }
    }
}
