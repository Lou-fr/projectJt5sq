using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendUI : MonoBehaviour
{
    [SerializeField] private GameObject Friend_panel;
    [SerializeField] private string displayname;
    BleizInputManager _input;
    public static Action<string> OnAddFriend = delegate { };

    public void SetAddFriendName(string name)
    {
        displayname = name;
    }
    public void AddFriend()
    {
        if (string.IsNullOrEmpty(displayname)) return;
        OnAddFriend?.Invoke(displayname);
    }

    public void friend()
    {
        if (_input is null) _input = FindAnyObjectByType<BleizInputManager>().GetComponent<BleizInputManager>();
        if (Friend_panel.activeSelf is true) { Friend_panel.SetActive(false); _input.enabled = true; }
        else { Friend_panel.SetActive(true); _input.enabled = false; };
    }

    [SerializeField] private Transform friendContainer;
    [SerializeField] private PrefabFriendUi prefabfriendUI;
    [SerializeField] private PrefabPendingFriendUi prefabpendingfriendUI;
    [SerializeField] private PrefabIncomeFriendReqUi prefabincomefriendUI;

    private void Awake()
    {
        FriendNetwork.OnDisplayFriends += HandleDissplayFriend;
        FriendNetwork.OnDisplayPendingFriends += HandleDislpayPendingFriend;
        FriendNetwork.OnDisplayIncomingFriendsReq += HandleDislpayIncomingFriendReq;
        FriendNetwork.DeleteAllChild += HandleRemoveAll;
    }

    private void OnDestroy()
    {
        FriendNetwork.OnDisplayFriends -= HandleDissplayFriend;
        FriendNetwork.OnDisplayPendingFriends -= HandleDislpayPendingFriend;
        FriendNetwork.OnDisplayIncomingFriendsReq -= HandleDislpayIncomingFriendReq;
        FriendNetwork.DeleteAllChild -= HandleRemoveAll;


    }

    private void HandleRemoveAll()
    {
        foreach (Transform child in friendContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void HandleDislpayIncomingFriendReq(FriendInfo friend)
    {
        PrefabIncomeFriendReqUi friendUI = Instantiate(prefabincomefriendUI, friendContainer);
        friendUI.Initialize(friend);
    }

    private void HandleDislpayPendingFriend(FriendInfo friend)
    {

        PrefabPendingFriendUi friendUI = Instantiate(prefabpendingfriendUI, friendContainer);
        friendUI.Initialize(friend);
    }

    private void HandleDissplayFriend(FriendInfo friend)
    {
        PrefabFriendUi friendUI = Instantiate(prefabfriendUI, friendContainer);
        friendUI.Initialize(friend);
    }

}
