using Unity.Services.Friends.Models;
using UnityEngine;

public class InviteUI : MonoBehaviour
{
    [SerializeField] InvitePrefabUI prefabUI;
    [SerializeField] Transform container;
    void Awake()
    {
        FriendsManager.joinRequestA += handleJoinRequest;
    }
    void OnDestroy()
    {
        FriendsManager.joinRequestA -= handleJoinRequest;
    }

    private void handleJoinRequest(PlayerProfile profile)
    {
        if(container.childCount >0)return;
        InvitePrefabUI invite = Instantiate(prefabUI,container);
        invite.Initialize(profile);
    }
}
