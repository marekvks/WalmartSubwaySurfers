using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour // Uklidit si tu :P
{
    [Header("Scripts")]
    [SerializeField] private ScoreManager scoreManager;

    [Header("Server Info")]
    [SerializeField] private string uri;
    [SerializeField] private string token;

    [Header("Json things ...")]
    private string _loadedJson;
    
    [Header("UI")]
    [SerializeField] private TMP_InputField usernameInput;

    public class LeaderboardClass
    {
        public string username;
        public int score;
    }

    public List<LeaderboardClass> LeaderboardList;

    class DataToSend
    {
        public string token;
        public LeaderboardClass leaderboard;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) // Loaduje se jen v main menu
            StartCoroutine(GetRequest(uri));
    }

    public void Load()
    {
        if (_loadedJson != null)
        {
            LeaderboardList = JsonConvert.DeserializeObject<List<LeaderboardClass>>(_loadedJson).ToList();

            LeaderboardList = LeaderboardList.OrderByDescending(c => c.score).ToList();
            
            SetUI();
        }
    }

    public void Save()
    {
        string uname = usernameInput.text;

        if (uname == null || uname == "")
            uname = "Noname";
        
        LeaderboardClass leaderboardClass = new LeaderboardClass
        {
            username = uname,
            score = Convert.ToInt32(scoreManager.Score)
        };
        DataToSend dataToSend = new DataToSend
        {
            token = token,
            leaderboard = leaderboardClass
        };

        string json = JsonConvert.SerializeObject(dataToSend, Formatting.Indented);

        StartCoroutine(PostRequest(uri, json));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest(); // Počká na stránku než zareaguje

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log($"Network error: {webRequest.error}");
        else
        {
            _loadedJson = webRequest.downloadHandler.text;
            Load();
        }
    }

    IEnumerator PostRequest(string uri, string jsonData)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(uri, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData); // Encoding.UTF8.GetBytes kóduje znaky do posloupnosti bytů
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw); // UploadHandler stojí za managováním "těla" dat, které jsou uploadnutý na server
        webRequest.SetRequestHeader("Content-Type", "application/json"); // Nastaví HTTP request header content na json, "Content-Type" je name headeru - Case sensitive - musí být správně velká a malá písmena

        yield return webRequest.SendWebRequest(); // Počká na stránku než zareaguje

        if (webRequest.result == UnityWebRequest.Result.Success) Debug.Log("Success!🎉");
        else Debug.Log("Error!");

        webRequest.uploadHandler.Dispose();
    }

    
    
    // SetUI
    [Header("UI Options")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform leaderboardRoot;

    private const string positionTag = "Position";
    private const string nameTag = "Name";
    private const string scoreTag = "Score";

    private void SetUI()
    {
        for (int pos = 1; pos <= LeaderboardList.Count; pos++)
        {
            if (pos > 10) break;

            LeaderboardClass player = LeaderboardList[pos - 1];

            GameObject spawnedPrefab = Instantiate(prefab, leaderboardRoot.transform);

            List<TMP_Text> textsInPrefab = spawnedPrefab.GetComponentsInChildren<TMP_Text>().ToList();

            foreach (TMP_Text text in textsInPrefab)
            {
                string desiredText = "NaN";

                switch (text.name)
                {
                    case positionTag:
                        desiredText = pos.ToString();
                        break;
                    case nameTag:
                        desiredText = player.username;
                        break;
                    case scoreTag:
                        desiredText = player.score.ToString();
                        break;
                }

                text.text = desiredText;
            }
        }
    }
}
