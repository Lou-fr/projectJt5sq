using Login.Library.Resonses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace TokenManage
{
    public static class TokenLoader
    {
        public static void SaveToken(Tokens response)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.info";
            FileStream stream = new FileStream(path, FileMode.Create);
            bf.Serialize(stream, response);
            stream.Close();
        }
        public static Tokens LoadToken()
        {
            string path = Application.persistentDataPath + "/player.info";
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                Tokens Token = binaryFormatter.Deserialize(stream) as Tokens;
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