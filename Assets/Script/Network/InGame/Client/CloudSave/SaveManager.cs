using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static Action SavePosition = delegate{};
    public static async Task<Vector3> LoadPlayerPosition()
    {
        Vector3 position = new Vector3(0,0,0);
        var PlayerPosition = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"player_position"});
        if(PlayerPosition.TryGetValue("player_position",out var playerPos))
        {
            position = playerPos.Value.GetAs<Vector3>();
            Debug.Log("Player pos found"+position.ToString());
            return position;
        }
        Debug.Log("No save found");  
        var playerData = new Dictionary<string,object>{{"player_position",position}};
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        return position;
    }


    public static async void SavePlayerPosition(Vector3 position)
    {
        #pragma warning disable
        await CloudSaveService.Instance.Data.Player.DeleteAsync("player_position");
        #pragma warning restore
        var playerData = new Dictionary<string,object>{{"player_position",position}};
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }
    public static IEnumerator Coutdown (int seconds)
    {
        int counter = seconds;
        Debug.Log("Initiated");
        while(counter > 0)
        {
            counter--;
            yield return new WaitForSecondsRealtime(1f);
        }
        SavePosition?.Invoke();
    }
}
