using PlayFab;
using PlayFab.MultiplayerModels;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using System;

#if !UNITY_SERVER
namespace BleizEntertainment.Multiplayer
{
    public class PlayerMultiplayerRequester : MonoBehaviour
    {
        public static string Ip = null;
        public static int Port;
        public static bool Done = false;
        bool done = false;
        string guid = null;
        public async void RequestMultiplayerServer()
        {
            GetGUIDOnline();
            while (done is false) await Waiter();
            if (guid is null) guid = SaveGUIDOnline();

            Done = false;
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
            Done = true;
        }
        void OnRequestMultiplayerServerError(PlayFabError error)
        {
            Debug.LogError(error.ToString());
            Done = true;
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
            done = true;
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
            PlayFabClientAPI.UpdateUserData(request, OndataSend, OnError);
            return temp_guid;
        }

        void OnError (PlayFabError error)
        {
            done = true;
            Debug.Log(error.ToString());
        }
        void OndataSend(UpdateUserDataResult result)
        {
            
        }

        async Task Waiter()
        {
            await Task.Delay(10);
        }
    }
}
#endif