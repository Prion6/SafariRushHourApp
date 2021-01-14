using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager
{
    static string DirectoryPath = "DataFolder";

    /// <summary>
    /// Crea un uevo archivo en la ruta entrega y lo retorna, si el archivo ya exite intentara cargarlo.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T NewData<T>(string name = "")
    {
        if(!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        string path = DirectoryPath + "/" + name + "Data.dat";
        T newData;

        if (File.Exists(path))
        {
            Debug.LogWarning("[A file with this path already exists]: " + path);
            return LoadData<T>(name);
        }
        else
        {
            //File.Create(path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            newData = Activator.CreateInstance<T>();
            formatter.Serialize(stream, newData);
            stream.Close();
            return newData;
        }
    }

    /// <summary>
    /// Guarda un archivo, si existe lo sobrescribe, si no, lo crea.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public static void SaveData<T>(T data, string name = "")
    {
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
        string path = DirectoryPath + "/" + name + "Data.dat";

        Debug.Log(data);

        if (!File.Exists(path))
        {
            Debug.LogWarning("[there is no saved information]: " + path);
            File.Create(path);            
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.CreateNew);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    /// <summary>
    /// Carga un archivo guardado, si este no existe o esta corrupto entrega el valor por defecto o nulo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T LoadData<T>(string name = "")
    {
        T instance = default;

        string path = DirectoryPath + "/" + name + "Data.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            try
            {
                instance = (T)formatter.Deserialize(stream);
                if (instance == null)
                {
                    instance = Activator.CreateInstance<T>();
                    Debug.LogWarning("[Corrupted file]: " + path);
                    stream.Close();
                    return default;
                }
                stream.Close();
            }
            catch
            {
                //Debug.Log("Empty");
            }
            
            return instance;
        }
        else
        {
            Debug.LogWarning("[The file does not exists]: " + path);
            return default;
        }
    }


}
