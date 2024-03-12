using PlayFab;
using PlayFab.MultiplayerModels;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using System;

#if !UNITY_SERVER
namespace BleizEntertainment.Multiplayer
{
    public class PlayerMultiplayerRequester : MonoBehaviour
    {
        public static string Ip = null;
        public static int Port;
        public static Action<string> OnFinishrequest = delegate { };
        string guid = null;
        public void RequestMultiplayer()
        {
            GetGUIDOnline();
        }
        void RequestMultiplayerServer()
        {
            if (guid is null) guid = SaveGUIDOnline();
            RequestMultiplayerServerRequest request = new RequestMultiplayerServerRequest
            {
                BuildId = "c0b81dab-0df7-44d6-a995-d868564f96f6",
                SessionId = guid,
                PreferredRegions = new List<string> { "NorthEurope" }
            };
            PlayFabMultiplayerAPI.RequestMultiplayerServer(request, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
        }

        void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
        {
            if (response == null) return;

            Debug.Log("Ip : " + response.IPV4Address + " Port : " + (ushort)response.Ports[0].Num);

            Ip = response.IPV4Address;
            Port = (ushort)response.Ports[0].Num;
            OnFinishrequest?.Invoke("ok");
        }
        void OnRequestMultiplayerServerError(PlayFabError error)
        {
            Debug.LogError(error.ToString());
            OnFinishrequest?.Invoke("con_serv");
        }

        void GetGUIDOnline()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceive, OnError);   
        }

        void OnDataReceive(GetUserDataResult result)
        {
            if (result.Data != null && result.Data.ContainsKey("User_Guid"))
            {
                guid = result.Data["User_Guid"].Value;
            }
            RequestMultiplayerServer();

        }
        string SaveGUIDOnline()
        {
            string temp_guid = Guid.NewGuid().ToString();
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
            {
                {"User_Guid",temp_guid}
            }
            };
            PlayFabClientAPI.UpdateUserData(request, OndataSend, OnErrorSave);
            return temp_guid;
        }

        void OnError (PlayFabError error)
        {
            Debug.Log(error.ToString());
            RequestMultiplayerServer();
        }
        void OnErrorSave(PlayFabError error)
        {
            Debug.LogWarning(error.ToString());

        }
        void OndataSend(UpdateUserDataResult result)
        {
            Debug.Log(result);
        }
    }
}
#endif