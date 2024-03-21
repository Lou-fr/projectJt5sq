using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using UnityEngine;

public class FriendsManager : MonoBehaviour
{
    public static Action ClearList = delegate{};
    public static Action<List<Member>> refreshRequest = delegate {};
    public static Action<List<Member>> refreshFriends = delegate {};
    public static Action<List<Member>,List<Member>> RefreshPendingRequest = delegate {};
    public static Action<IReadOnlyList<Relationship>> refreshBlocked = delegate{};
    public static List<FriendsEntryData> friends = new List<FriendsEntryData>();
    public static List<PlayerProfile> request = new List<PlayerProfile>();
    public static List<PlayerProfile> pending = new List<PlayerProfile>();
    public static Action<PlayerProfile> joinRequestA = delegate{};
    public static Action<string> OnJoinRequestAccepted = delegate{};
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
        BlockledPrefab.UnBlock += UnBlockFriend;
        FriendsUI.RefreshFriend += RefreshFriends;
        FriendsUI.BlockedFriend += RefreshBlock;
        FriendsUI.PendingIncomingFriend += RefreshPendingRequestRequest;
        InvitePrefabUI.AcceptJoinReq += SendAcceptJoinRequest;
        await SetPresence();
    }
     void OnDestroy()
    {
        FriendPrefab.RemoveFriend -= RemoveFriends;
        FriendPrefab.BlockFriend -= BlockFriend;
        PendingPrefab.CancelOutgoingRequest -= CancelOutgoingRequest;
        FriendsUI.OnAddFriend -= HandleFriendsRequest;
        RequestPrefab.AcceptFriend -= AcceptFriend;
        RequestPrefab.DenyFriend -= Denyfriend;
        BlockledPrefab.UnBlock -= UnBlockFriend;
        FriendsUI.RefreshFriend -= RefreshFriends;
        FriendsUI.BlockedFriend -= RefreshBlock;
        FriendsUI.PendingIncomingFriend -= RefreshPendingRequestRequest;
        InvitePrefabUI.AcceptJoinReq -= SendAcceptJoinRequest;
    }

    private async void UnBlockFriend(string obj)
    {
        try
        {
            await FriendsService.Instance.DeleteBlockAsync(obj);
            Debug.Log("Unblock "+ obj);
            RefreshBlock();
        }catch(FriendsServiceException e){Debug.LogError(e);}
    }

    private async void Denyfriend(string obj)
    {
        try
        {
            await FriendsService.Instance.DeleteIncomingFriendRequestAsync(obj);
            Debug.Log("Friend request deny");
            RefreshPendingRequestRequest();
        }catch(FriendsServiceException e){Debug.LogError(e);}
    }

    private async void AcceptFriend(string obj)
    {
        try
        {
            await SendFriendRequest(obj);
            Debug.Log("Friend request accepted");
            RefreshPendingRequestRequest();
        }catch(FriendsServiceException e){Debug.LogError(e);}
    }

    private async void CancelOutgoingRequest(string obj)
    {
        try{
            await FriendsService.Instance.DeleteOutgoingFriendRequestAsync(obj);
            Debug.Log("Friend request cancel");
            RefreshPendingRequestRequest();
        }catch(FriendsServiceException e){Debug.LogError("Friend request cancel error "+e,this);}
    }

    private async void BlockFriend(string obj)
    {
        try
        {
            await FriendsService.Instance.AddBlockAsync(obj);
            Debug.Log("Player id "+obj+" is now blocked on this account");
            RefreshFriends();
        }catch(FriendsServiceException e){Debug.LogError("Failed to block "+obj + " reason "+e,this);}
    }

    private async void RemoveFriends(string id)
    {
        try
            {
                await FriendsService.Instance.DeleteFriendAsync(id);
                Debug.Log($"{id} was removed from the friends list.");
                RefreshFriends();
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
        RefreshBlock();
        RefreshPendingRequestRequest();
    }

    private void RefreshBlock()
    {
        Debug.Log("Refreshing Blocked ...");
        var request = FriendsService.Instance.Blocks;
        refreshBlocked?.Invoke(request);
        Debug.Log("Found "+request.Count+" blocked");
    }

    private void RefreshFriends()
    {
        friends.Clear();
        Debug.Log("Refreshing friends...");
        var Friends = GetNonBlockedMembers(FriendsService.Instance.Friends);
        refreshFriends?.Invoke(Friends);
        Debug.Log("Found "+Friends.Count+" friends");
    }
    void RefreshPendingRequestRequest()
    {
        pending.Clear();
        Debug.Log("Refreshin pending request...");
        var pendings = GetNonBlockedMembers(FriendsService.Instance.OutgoingFriendRequests);
        Debug.Log("Found "+pendings.Count+" outgoing request");
        request.Clear();
        Debug.Log("Refreshing incoming request...");
        List<Member> Request = GetNonBlockedMembers(FriendsService.Instance.IncomingFriendRequests);
        RefreshPendingRequest?.Invoke(Request,pendings);
        Debug.Log("Found "+Request.Count+" incoming request");
    }

    private void susbscribeToFriendsCallback()
    {
        try
        {
            FriendsService.Instance.RelationshipAdded += e =>
            {
                RefreshFriends();
                RefreshPendingRequestRequest();
                Debug.Log("Create relation ship "+e.Relationship);
            };
            FriendsService.Instance.PresenceUpdated += e =>
            {
                RefreshFriends();
                Debug.Log("Presence updated");
            };
            FriendsService.Instance.RelationshipDeleted +=e =>
            {
                RefreshFriends();
                Debug.Log("Relation ship deleted "+e.Relationship);
            };
            FriendsService.Instance.MessageReceived += e =>
            {
                StandaloneFriendsRefresh();
                HandleMessage(e);
                Debug.Log("Message received");
            };
        }catch(FriendsServiceException e){Debug.LogError(e,this);}
    }

    void HandleMessage(IMessageReceivedEvent message)
    {
       var t =message.GetAs<object>();
       string _t = t.ToString();
       try
       {
            Debug.Log("Try to read it as JoinRequest");
            JoinRequest request = JsonConvert.DeserializeObject<JoinRequest>(_t);
            if (request.RequestToJoin == true)
            {
                foreach(FriendsEntryData entryData in friends)
                {
                    if(entryData.Id == message.UserId){var sending = new PlayerProfile(entryData.Name,entryData.Id);joinRequestA?.Invoke(sending);return;}
                }
            }
       }catch(JsonException e){Debug.LogWarning(e);}
       try
       {
            Debug.Log("Try to read it as JoinResponse");
            JoinResponse response = JsonConvert.DeserializeObject<JoinResponse>(_t);
            if(response.yesOrno is false){return;}
            if(response.yesOrno is true){OnJoinRequestAccepted?.Invoke(response.lobbyCode);return;}
            
       }catch(JsonException e){Debug.LogWarning(e);}
       
    }
    public static void SendjoinRequest(string TargetId)
    {
        var e = new JoinRequest {RequestToJoin = true};
        JsonConvert.SerializeObject(e);
        SendMessageAsync(TargetId,e);
    }
    public static void SendAcceptJoinRequest(string TargetId)
    {
        string lobbyCode = LobbyManager.GetLobbyCode();
        bool isAuthorized = true;
        if(lobbyCode == null) isAuthorized = false;
        var toJson = new JoinResponse{yesOrno = isAuthorized,lobbyCode=lobbyCode};
        JsonConvert.SerializeObject(toJson);
        SendMessageAsync(TargetId,toJson);
    }
    static async void SendMessageAsync(String TargetUserId, object content) 
    {
        await FriendsService.Instance.MessageAsync(TargetUserId,content);
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
    class JoinRequest
    {
        public bool RequestToJoin {get;set;}
    }
    class JoinResponse
    {
        public bool yesOrno {get;set;}
        public string lobbyCode {get;set;}
    }
    private void StandaloneFriendsRefresh()
    {
        var Friends = GetNonBlockedMembers(FriendsService.Instance.Friends);
        foreach(var friend in Friends)
        {
            string activityText;
            if (friend.Presence.Availability == Availability.Offline ||friend.Presence.Availability == Availability.Invisible)
            {
                activityText = ""+friend.Presence.LastSeen.ToLocalTime();
            }else
            {
                activityText = friend.Presence.Availability.ToString();
            }
            var info = new FriendsEntryData
            {
                Name = friend.Profile.Name,
                Id = friend.Id,
                Availability = friend.Presence.Availability,
                Activity = activityText
            };
            friends.Add(info);
        }
    }
}
