/*using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

public class MultiplayServer : MonoBehaviour
{
    private const ushort MaxPlayer = 4;
    private const string DefaultServerName = "RebirtOfCepheidServer";
    private const string k_DefaultGameType = "OpenWorld";
	private const string k_DefaultBuildId = "";
	private const string k_DefaultMap = "MyMap";

    public ushort  currentPlayers;
    private IServerQueryHandler m_ServerQueryHandler;
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }catch (Exception e)
        {
            Debug.LogError(e);
        }
        m_ServerQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(MaxPlayer,DefaultServerName,k_DefaultGameType,k_DefaultBuildId,k_DefaultMap);
    }
    void Update()
    {
        m_ServerQueryHandler.UpdateServerCheck();
    }
    public void ChangeQueryResponseValues(ushort maxPlayers, string serverName, string gameType, string buildId)
	{
		m_ServerQueryHandler.MaxPlayers = maxPlayers;
		m_ServerQueryHandler.ServerName = serverName;
		m_ServerQueryHandler.GameType = gameType;
		m_ServerQueryHandler.BuildId = buildId;
	}
    public void PlayerCountChanged(ushort newPlayerCount)
	{
		m_ServerQueryHandler.CurrentPlayers = newPlayerCount;
	}
}*/
