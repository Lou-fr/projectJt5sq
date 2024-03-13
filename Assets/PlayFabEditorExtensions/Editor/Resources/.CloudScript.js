handlers.SendFriendRequest = function(args, context){ 
      server.AddFriend({
            PlayFabId : currentPlayerId,
            FriendPlayFabId : args.FriendPlayFabId
      });
      server.SetFriendTags({
            PlayFabId : currentPlayerId,
            FriendPlayFabId: args.FriendPlayFabId, 
            Tags: ["requestee"] 
      });
      server.AddFriend({ 
            PlayFabId: args.FriendPlayFabId, 
            FriendPlayFabId: currentPlayerId 
      }); 
      server.SetFriendTags({ 
            PlayFabId: args.FriendPlayFabId, 
            FriendPlayFabId: currentPlayerId, 
            Tags: ["requester"] 
      });
  }
  handlers.AcceptFriendRequest= function(args, context){ 
        server.SetFriendTags({ 
            PlayFabId: currentPlayerId, 
            FriendPlayFabId: args.FriendPlayFabId, 
            Tags: ["confirmed"] 
        }); 
        server.SetFriendTags({ 
            PlayFabId: args.FriendPlayFabId, 
            FriendPlayFabId: currentPlayerId, 
            Tags: ["confirmed"] 
        });}
handlers.DenyFriendRequest = function(args,context){ 
      server.RemoveFriend({
            PlayFabId:currentPlayerId, 
            FriendPlayFabId: args.FriendPlayFabId, 
      }); 
      server.RemoveFriend({ 
            PlayFabId: args.FriendPlayFabId, 
            FriendPlayFabId: currentPlayerId, 
      });}