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
    /*public static class TempTokenLoader
    {
        public static void SaveTokenTemp(Tokens response)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.temp";
            FileStream stream = new FileStream(path, FileMode.Create);
            bf.Serialize(stream, response);
            stream.Close();
        }
        public static void UnloadTempToken()
        {
            string path = Application.persistentDataPath + "/player.temp";
            File.Delete(path);
        }
        public static Tokens LoadTempToken()
        {
            string path = Application.persistentDataPath + "/player.temp";
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
    }*/
}