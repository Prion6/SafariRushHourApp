using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;
using UnityEngine.UI;

public class UIPuzzleManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private List<TMP_Dropdown> dropdowns;
    [SerializeField] private Button submitInitialDataButton;
    //[SerializeField] private Button submitLevelDataButton;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button CloseAppBtn;


    [Header("Panels")]
    [SerializeField] private GameObject initialDataPanel;
    [SerializeField] private GameObject postGamePanel;
    //[SerializeField] private GameObject winPanel;

    [Header("Panels")]
    [SerializeField] private TMP_InputField age;
    [SerializeField] private TMP_Dropdown hoursPlayedDropdown;
    [SerializeField] private TMP_Dropdown puzzleSkillsDropdown;

    private PlayerCloudData _playerData;
    private List<LevelCompleteData> _levelsCompleteData;
    private void Start()
    {
        _playerData = new PlayerCloudData();
        _levelsCompleteData = new List<LevelCompleteData>();
        initialDataPanel.SetActive(true);
        postGamePanel.SetActive(false);
        //submitLevelDataButton.onClick.AddListener(UploadData);
        submitInitialDataButton.onClick.AddListener(CompleteInitialQuestionarie);
        nextLevelBtn.onClick.AddListener(GotoNextLevel);
        CloseAppBtn.onClick.AddListener(CloseApp);
        PuzzleManager.OnLevelComplete += OnLevelComplete;
    }
    private void OnDestroy()
    {
        //submitLevelDataButton.onClick.RemoveListener(UploadData);
        submitInitialDataButton.onClick.RemoveListener(CompleteInitialQuestionarie);
        nextLevelBtn.onClick.RemoveListener(GotoNextLevel);
        CloseAppBtn.onClick.RemoveListener(CloseApp);
        PuzzleManager.OnLevelComplete -= OnLevelComplete;
    }
    private async void SaveDataPlayer()
    {
        string fileName = "player_data.json";
        string jsonData = JsonUtility.ToJson(_playerData);
        Debug.Log($"json: {jsonData}");
        byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);


        List<FileItem> files = await CloudSaveService.Instance.Files.Player.ListAllAsync();

        foreach (FileItem file in files)
        {
            if (file.Key == fileName)
            {
                await CloudSaveService.Instance.Files.Player.DeleteAsync(fileName);
                Debug.Log($"Deleted existing file: {fileName}");
                break;
            }
        }

        await CloudSaveService.Instance.Files.Player.SaveAsync(fileName, fileBytes);
        Debug.Log($"File saved: {fileName}");
    }
    private async void SavelDataLevel()
    {
        string fileName = "levels_data.json";

        int movements = Puzzle.instance.movements;
        int restarts = Puzzle.instance.restarts;
        int timeToComplete = (int)Puzzle.instance.timeToComplete;
        bool wasCompleted= Puzzle.instance.wasCompleted;
        LevelCompleteData data = new LevelCompleteData(GameManager.puzzle.ID, movements, timeToComplete, restarts, wasCompleted);
        _levelsCompleteData.Add(data);
        Debug.Log($"level data added: {GameManager.puzzle.ID}");

        string jsonData = JsonUtility.ToJson(new SerializationWrapper<LevelCompleteData>(_levelsCompleteData), true);

        Debug.Log($"json: {jsonData}");
        byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);


        List<FileItem> files = await CloudSaveService.Instance.Files.Player.ListAllAsync();

        foreach (FileItem file in files)
        {
            if (file.Key == fileName)
            {
                await CloudSaveService.Instance.Files.Player.DeleteAsync(fileName);
                Debug.Log($"Deleted existing file: {fileName}");
                return;
            }
        }

        await CloudSaveService.Instance.Files.Player.SaveAsync(fileName, fileBytes);
        Debug.Log($"Level completed data saved: {fileName}");
    }
    private void GotoNextLevel()
    {
        Puzzle.instance.wasCompleted = true;
        SavelDataLevel();
        //UploadData(true);
        List<int> info = new List<int>();

        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            info.Add(dropdown.value);
        }
        //GameManager.LoadPuzzle(UnityEngine.Random.Range(0, (int)LevelDifficulties.E));
        GameManager.LoadARandomLevel();

        initialDataPanel.SetActive(false);
        postGamePanel.SetActive(false);
        Puzzle.instance.ResetValues();
        //winPanel.SetActive(false);
    }
    private void CompleteInitialQuestionarie()
    {
        _playerData.age = int.Parse(age.text);
        _playerData.hourPlayed = hoursPlayedDropdown.value;
        _playerData.puzzleHabilities = puzzleSkillsDropdown.value;

        initialDataPanel.SetActive(false);
        postGamePanel.SetActive(false);
        //UploadData(false);
        SaveDataPlayer();
        //winPanel.SetActive(false);
    }
    public void CloseApp()
    {
        UnityWebBridge.QuitAndClose();
        SavelDataLevel();
    }
    private void OnApplicationQuit()
    {
        Debug.Log("On apllication quit");
        SavelDataLevel();
    }
    private void OnLevelComplete()
    {
        initialDataPanel.SetActive(false);
        postGamePanel.SetActive(true);
    }
}
public struct PlayerCloudData
{
    public int age;
    public int hourPlayed;
    public int puzzleHabilities;
}
[Serializable]
public struct LevelCompleteData
{
    public int id;
    public int movements;
    public int time;
    public int resets;
    public bool wasCompleted;
    public LevelCompleteData(int id, int movements, int time, int resets, bool wasCompleted)
    {
        this.id = id;
        this.movements= movements;
        this.time = time;
        this.resets = resets;
        this.wasCompleted = wasCompleted;
    }
}

[Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;

    public SerializationWrapper(List<T> list)
    {
        items = list;
    }
}