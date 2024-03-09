using PlayFab;
using PlayFab.MultiplayerModels;
using PlayFab.Networking;
using System;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_SERVER
namespace BleizEntertainment.Multiplayer
{
    public class PlayerMultiplayerRequester : MonoBehaviour
    {
        public static string Ip = null;
        public static int Port;
        public static bool Done = false;
        public static void RequestMultiplayerServer()
        {
            Done = false;
            RequestMultiplayerServerRequest request = new RequestMultiplayerServerRequest
            {
                BuildId = "5edd2a80-3af0-43a0-96a0-0f4ad8ebb2bb",
                SessionId = GetGuid(),
                PreferredRegions = new List<string> { "NorthEurope" }
            };
            PlayFabMultiplayerAPI.RequestMultiplayerServer(request, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
        }

        static void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
        {
            if (response == null) return;

            Debug.Log("Ip : " + response.IPV4Address + " Port : " + (ushort)response.Ports[0].Num);

            Ip = response.IPV4Address;
            Port = (ushort)response.Ports[0].Num;
            Done = true;
        }
        static void OnRequestMultiplayerServerError(PlayFabError error)
        {
            Debug.Log(error.ToString());
            Done = true;
        }

        static string GetGuid()
        {
            if (PlayerPrefs.HasKey("GUID"))
            {
                return PlayerPrefs.GetString("GUID");
            }
            else
            {
                string guid = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("GUID", guid);
                return guid;
            }
        }

    }
}
#endif