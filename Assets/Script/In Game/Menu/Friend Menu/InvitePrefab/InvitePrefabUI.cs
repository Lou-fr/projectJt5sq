using System;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

public class InvitePrefabUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] Button Accept, deny;
    public static Action<string> AcceptJoinReq = delegate{};
    public static Action<string> DenyJoinReq = delegate{};
    private PlayerProfile player;
    bool timerRunnig= false;
    float timeRemaining;
    string text;
    void OnDestroy()
    {
        Accept.onClick.RemoveAllListeners();
        deny.onClick.RemoveAllListeners(); 
    }

    private void OnRefuseJoinReq()
    {
        DenyJoinReq?.Invoke(player.Id);
        Destroy(this.gameObject);
    }

    private void OnAcceptJoinReq()
    {
        AcceptJoinReq?.Invoke(player.Id);
        Destroy(this.gameObject);
    }

    public void Initialize(PlayerProfile _player)
    {
        this.player =_player;
        Accept.onClick.AddListener(OnAcceptJoinReq);
        deny.onClick.AddListener(OnRefuseJoinReq);
        text = player.Name+" want to join your game";
        username.SetText(text);
        timeRemaining =10f;
        timerRunnig =true;
    }
        private void FixedUpdate()
    {
        if (timerRunnig)
        {
            
            if (timeRemaining > 0.1)
            {
                timeRemaining -= Time.deltaTime;
                username.text = text + "(" + Mathf.FloorToInt(timeRemaining % 60) +"s)";
            }
            else
            {
                Destroy(this.gameObject);
                timerRunnig = false;
            }
        }
    }

}
