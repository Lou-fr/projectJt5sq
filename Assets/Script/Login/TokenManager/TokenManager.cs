using Login.Library.Resonses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace TokenManager
{
    public static class TokenManager
    {
        public static void SaveToken(AuthResponse response)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.info";
            FileStream stream = new FileStream(path, FileMode.Create);
            bf.Serialize(stream, response);
            stream.Close();
        }
        public static AuthResponse LoadToken()
        {
            string path = Application.persistentDataPath + "/player.info";
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                AuthResponse Token = binaryFormatter.Deserialize(stream) as AuthResponse;
                stream.Close();
                return Token;
            }
            else
            {
                return null;
            }
        }
        public static void UnloadToken()
        {
            string path = Application.persistentDataPath + "/player.info";
            File.Delete(path);
        }
    }
}