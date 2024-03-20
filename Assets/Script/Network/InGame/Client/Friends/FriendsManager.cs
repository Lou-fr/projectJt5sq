using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using UnityEngine;

public class FriendsManager : MonoBehaviour
{
    public static Action ClearList = delegate{};
    public static Action<List<Member>> refreshRequest = delegate {};
    public static Action<List<Member>> refreshFriends = delegate {};
    public static Action<List<Member>> refreshPending = delegate {};
    public static List<FriendsEntryData> friends = new List<FriendsEntryData>();
    public static List<PlayerProfile> request = new List<PlayerProfile>();
    public static List<PlayerProfile> pending = new List<PlayerProfile>();
    async void Awake()
    {
        await FriendsService.Instance.InitializeAsync();
        susbscribeToFriendsCallback();
        FriendPrefab.RemoveFriend += RemoveFriends;
        FriendPrefab.BlockFriend += BlockFriend;
        PendingPrefab.CancelOutgoingRequest += CancelOutgoingRequest;
        FriendsUI.OnAddFriend += HandleFriendsRequest;
        RequestPrefab.AcceptFriend += AcceptFriend;
        RequestPrefab.DenyFriend += Denyfriend;
        FriendsUI.refresh += RefreshAll;
        await SetPresence();
    }
     void OnDestroy()
    {
        FriendPrefab.RemoveFriend -= RemoveFriends;
        FriendPrefab.BlockFriend -= BlockFriend;
        PendingPrefab.CancelOutgoingRequest -= CancelOutgoingRequest;
        FriendsUI.OnAddFriend -= HandleFriendsRequest;
        RequestPrefab.AcceptFriend -= AcceptFriend;
        RequestPrefab.DenyFriend += Denyfriend;
        FriendsUI.refresh -= RefreshAll;
    }

    private async void Denyfriend(string obj)
    {
        try
        {
            await FriendsService.Instance.DeleteIncomingFriendRequestAsync(obj);
            Debug.Log("Friend request deny");
            RefreshAll();
        }catch(FriendsServiceException e){Debug.LogError(e);}
    }

    private async void AcceptFriend(string obj)
    {
        try
        {
            await SendFriendRequest(obj);
            Debug.Log("Friend request accepted");
            RefreshAll();
        }catch(FriendsServiceException e){Debug.LogError(e);}
    }

    private async void CancelOutgoingRequest(string obj)
    {
        try{
            await FriendsService.Instance.DeleteOutgoingFriendRequestAsync(obj);
            Debug.Log("Friend request cancel");
            RefreshAll();
        }catch(FriendsServiceException e){Debug.LogError("Friend request cancel error "+e,this);}
    }

    private async void BlockFriend(string obj)
    {
        try
        {
            await FriendsService.Instance.AddBlockAsync(obj);
            Debug.Log("Player id "+obj+" is now blocked on this account");
            RefreshAll();
        }catch(FriendsServiceException e){Debug.LogError("Failed to block "+obj + " reason "+e,this);}
    }

    private async void RemoveFriends(string id)
    {
        try
            {
                await FriendsService.Instance.DeleteFriendAsync(id);
                Debug.Log($"{id} was removed from the friends list.");
                RefreshAll();
            }
            catch (FriendsServiceException e){Debug.Log($"Failed to remove {id}. - {e}");}
    }

    private async void HandleFriendsRequest(string name)
    {
        var succes = await SendFriendRequest(name);
        if (succes)
        {
            if(request.Find(entry => entry.Name == name) != null){}
            RefreshAll();
        }else
        {
            
        }
        
    }
    public void RefreshAll()
    {
        Debug.Log("Refreshing...");
        ClearList?.Invoke();
        RefreshFriends();
        RefreshRequest();
        RefreshPending();
    }

    private void RefreshRequest()
    {
        request.Clear();
        Debug.Log("Refreshing incoming request...");
        List<Member> Request = GetNonBlockedMembers(FriendsService.Instance.IncomingFriendRequests);
        refreshRequest?.Invoke(Request);
        Debug.Log("Found "+Request.Count+" incoming request");
    }

    private void RefreshFriends()
    {
        friends.Clear();
        Debug.Log("Refreshing friends...");
        var Friends = GetNonBlockedMembers(FriendsService.Instance.Friends);
        refreshFriends?.Invoke(Friends);
        Debug.Log("Found "+Friends.Count+" friends");
    }
    void RefreshPending()
    {
        pending.Clear();
        Debug.Log("Refreshin pending request...");
        var pendings = GetNonBlockedMembers(FriendsService.Instance.OutgoingFriendRequests);
        Debug.Log("Found "+pendings.Count+" outgoing request");
        refreshPending?.Invoke(pendings);
    }

    private void susbscribeToFriendsCallback()
    {
        try
        {
            FriendsService.Instance.RelationshipAdded += e =>

            {
                Debug.Log("Create relation ship "+e.Relationship);
            };
            FriendsService.Instance.PresenceUpdated += e =>
            {
                Debug.Log("Presence updated");
            };
            FriendsService.Instance.RelationshipDeleted +=e =>
            {
                Debug.Log("Relation ship deleted "+e.Relationship);
            };
        }catch(FriendsServiceException e){Debug.LogError(e,this);}
    }
    async Task<bool> SendFriendRequest(string playerName)
    {
        try
        {
            var relationship = await FriendsService.Instance.AddFriendByNameAsync(playerName);
            Debug.Log("Friend request sent to "+playerName);
            return relationship.Type is RelationshipType.FriendRequest or RelationshipType.Friend;
        }catch (FriendsServiceException e)
        {
            Debug.LogError("Failed to request "+ playerName +" reason" + e);
            return false;
        }
    }
    async Task SetPresence()
    {
        var activity = new Activity {Status = ""};
        try
        {
            await FriendsService.Instance.SetPresenceAsync(Availability.Online,activity);
            Debug.Log("Availability changed to Online");
        }catch(FriendsServiceException e){Debug.LogError(e);}
    }
    private List<Member> GetNonBlockedMembers(IReadOnlyList<Relationship> relationships)
    {
            var blocks = FriendsService.Instance.Blocks;
            return relationships
                   .Where(relationship =>!blocks.Any(blockedRelationship => blockedRelationship.Member.Id == relationship.Member.Id))
                   .Select(relationship => relationship.Member)
                   .ToList();
    }
}
