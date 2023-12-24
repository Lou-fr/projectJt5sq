using Assets.Script.Library.Request;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LoadAndSaveToken
{
    public static class LoadAndSaveToken 
    {
        public static void LogOut()
        {
            string path = Application.persistentDataPath + "/player.info";
            File.Delete(path);
        }
        public static AuthResponse LoadSaved()
        {
            string path = Application.persistentDataPath + "/player.info";
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                AuthResponse data = bf.Deserialize(stream) as AuthResponse;
                stream.Close();
                return data;
            }
            else
            {
                return null;
            }
        }
        public static void SaveToken(AuthResponse response)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.info";
            FileStream stream = new FileStream(path, FileMode.Create);
            Debug.Log(path);

            bf.Serialize(stream, response);
            stream.Close();
        }
    }
}

