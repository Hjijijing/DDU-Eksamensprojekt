using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Storage
{

    static Storage(){
        Debug.Log(Application.persistentDataPath);
        }

    public static void SaveData<T>(T data, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = getPath(fileName);

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static T GetData<T>(string fileName, T fallback)
    {
        try {
            string path = getPath(fileName);
            if (File.Exists(path)) 
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                T result = (T)formatter.Deserialize(stream);
                stream.Close();
                return result;
            }
            else
            {
                return fallback;
            }
        }
        catch
        {
            return fallback;
        }
    }


    public static string getPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }



}
