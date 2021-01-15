using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager
{
    static private string _DirectoryPath = "DirectoryFolder";
    static string DirectoryPath { get
        {
            if(!Directory.Exists(_DirectoryPath))
            {
                Directory.CreateDirectory(_DirectoryPath);
            }
            return _DirectoryPath;
        }
    }
    /// <summary>
    /// Crea un uevo archivo en la ruta entrega y lo retorna, si el archivo ya exite intentara cargarlo.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T NewData<T>(string name = "")
    {
        string path = DirectoryPath + "/" + name + "Data.dat";
        T newData;

        if (File.Exists(path))
        {
            Debug.LogWarning("[A file with this path already exists]: " + path);
            return LoadData<T>(name);
        }
        else
        {
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
    public static T SaveData<T>(T data, string name = "")
    {
        string path = DirectoryPath + "/" + name + "Data.dat";
        
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
            return data;
    }

    /// <summary>
    /// Carga un archivo guardado, si este no existe o esta corrupto entrega el valor por defecto o nulo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T LoadData<T>(string name = "")
    {
        T instance;

        string path = DirectoryPath + "/" + name + "Data.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            instance = (T)formatter.Deserialize(stream);
            if (instance == null)
            {
                instance = Activator.CreateInstance<T>();
                Debug.LogWarning("[Corrupted file]: " + path);
                return default(T);
            }
            stream.Close();
            return instance;
        }
        else
        {
            Debug.LogWarning("[The file does not exists]: " + path);
            return default(T);
        }
    }
}


