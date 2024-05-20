using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace Unity.Services.Friends.Models
{

    [Preserve]
    [DataContract]
    public class Activity
    {
        /// Status of the player.
        [Preserve]
        [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
        public string Status { get; set; }
    }
}
