using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void Awake()
    {
        FriendNetwork.OnDisplayFriends += HandleDissplayFriend;
    }

    private void OnDestroy()
    {
        FriendNetwork.OnDisplayFriends -= HandleDissplayFriend;

    }

    private void HandleDissplayFriend(List<FriendInfo> list)
    {
        foreach (Transform child in friendContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (FriendInfo friend in list)
        {
            PrefabFriendUi friendUI = Instantiate(prefabfriendUI, friendContainer);
            friendUI.Initialize(friend);
        }
    }

}
