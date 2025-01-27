using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

[System.Serializable]
public struct LevelData
{
    public LevelDifficulties Difficulty; // Dificultad: J, B, I, A, E
    public int Index;         // Número al lado de la letra
    public string Level;      // El resto del texto

    public LevelData(LevelDifficulties difficulty, int index, string level)
    {
        Difficulty = difficulty;
        Index = index;
        Level = level;
    }
}
public enum LevelDifficulties
{
    J = 0, B = 1, I = 2, A = 3, E = 4, _NA = -1
}
/// <summary>
/// Este script busca convertir los niveles en un .json de manera rápida y efectiva, para esto se requiere poner los niveles en un .txt
/// con el siguiente formato:
/// P J1
/// 
/// .......
/// .rrrf..
/// hh..fq.!
/// o.xx.q.!
/// o.xx.q.
/// ob..ee.
/// .bppp..
/// 
/// Donde J es de Junior, B Begginers, I intermidiate, A advanced y E expert
/// Dudas o consultas a squiroz21@alumnos.utalca.cl
/// </summary>
public class LevelConvertor : MonoBehaviour
{
    public string inputFilePath; // Ruta del archivo de entrada
    public string outputFileName = "output.json"; // Nombre del archivo JSON de salida

    public void ConvertTextToJson()
    {
        if (string.IsNullOrEmpty(inputFilePath))
        {
            Debug.LogError("Input file path is not specified.");
            return;
        }

        if (!File.Exists(inputFilePath))
        {
            Debug.LogError($"File not found at path: {inputFilePath}");
            return;
        }

        try
        {
            var lines = File.ReadAllLines(inputFilePath);
            var levelDataList = new List<LevelData>();

            string currentKey = null;
            StringBuilder currentLevel = new StringBuilder();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("P ") || line.StartsWith("B ") || line.StartsWith("I ") || line.StartsWith("A ") || line.StartsWith("E "))
                {
                    if (currentKey != null && currentLevel.Length > 0)
                    {
                        var parsedData = ParseLevelKey(currentKey, currentLevel.ToString());
                        levelDataList.Add(parsedData);
                        currentLevel.Clear();
                    }

                    currentKey = line;
                }
                else
                {
                    currentLevel.AppendLine(line);
                }
            }

            if (currentKey != null && currentLevel.Length > 0)
            {
                var parsedData = ParseLevelKey(currentKey, currentLevel.ToString());
                levelDataList.Add(parsedData);
            }

            string jsonOutput = JsonConvert.SerializeObject(levelDataList, Formatting.Indented);

            string outputPath = Path.Combine(Application.dataPath, outputFileName);
            File.WriteAllText(outputPath, jsonOutput);

            Debug.Log($"JSON file created at: {outputPath}");
            Application.OpenURL($"file://{outputPath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error occurred while processing the file: {ex.Message}");
        }
    }

    private LevelData ParseLevelKey(string key, string levelContent)
    {
        string difficulty = key.Substring(2, 1); // Extrae la letra de dificultad
        if (int.TryParse(key.Substring(3), out int index))
        {
            return new LevelData(GetDifiiculty(difficulty), index, levelContent.Trim());
        }

        Debug.LogWarning($"Failed to parse key: {key}");
        return new LevelData(GetDifiiculty(difficulty), 0, levelContent.Trim());
    }

    public static List<LevelData> ParseJsonToLevelList(string json)
    {
        Dictionary<string, string> levelDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        List<LevelData> levelList = new List<LevelData>();

        foreach (var entry in levelDictionary)
        {
            string key = entry.Key;
            string value = entry.Value;

            if (key.Length >= 3 && key.StartsWith("P "))
            {
                string difficulty = key.Substring(2, 1);
                if (int.TryParse(key.Substring(3), out int index))
                {
                    LevelData levelData = new LevelData
                    {
                        Difficulty = GetDifiiculty(difficulty),
                        Index = index,
                        Level = value
                    };
                    levelList.Add(levelData);
                }
            }
        }

        return levelList;
    }
    private static LevelDifficulties GetDifiiculty(string diff)
    {
        switch (diff)
        {
            case "J": return LevelDifficulties.J; 
            case "B": return LevelDifficulties.B;
            case "I": return LevelDifficulties.I;
            case "A": return LevelDifficulties.A;
            case "E": return LevelDifficulties.E;
                default: return LevelDifficulties._NA;
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(LevelConvertor))]
public class LevelConvertorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelConvertor script = (LevelConvertor)target;
        if (GUILayout.Button("Convert Text to JSON"))
        {
            script.ConvertTextToJson();
        }
    }
}
#endif
