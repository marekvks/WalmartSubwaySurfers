using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour // Uklidit si tu :P
{
    [SerializeField] private string uri;
    [SerializeField] private string token;

    [SerializeField] private ScoreManager scoreManager;
    
    [System.Serializable]
    public class LeaderboardClass
    {
        public string username;
        public int score;
    }

    public List<LeaderboardClass> LeaderboardList;

    private string loadedJson;

    class DataToSend
    {
        public string token;
        public LeaderboardClass leaderboard;
    }

    [SerializeField] private TMP_InputField usernameInput;
    
    private void Start()
    {
        Application.stackTraceLogType = StackTraceLogType.Full;
        if (SceneManager.GetActiveScene().buildIndex == 0) // jen v Menu
            StartCoroutine(GetRequest(uri));
    }

    public void Load()
    {
        if (loadedJson != null)
        {
            LeaderboardList = JsonConvert.DeserializeObject<List<LeaderboardClass>>(loadedJson).ToList();

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
            score = Convert.ToInt32(scoreManager.score)
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
            loadedJson = webRequest.downloadHandler.text;
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
    }


    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform leaderboardRoot;

    private const string positionTag = "Position";
    private const string nameTag = "Name";
    private const string scoreTag = "Score";

    private void SetUI()
    {
        int position = 1; // Slouží k očíslování
        foreach (LeaderboardClass player in LeaderboardList)
        {
            GameObject prefab = Instantiate(this.prefab, leaderboardRoot.transform);

            List<TMP_Text> texts = prefab.GetComponentsInChildren<TMP_Text>().ToList();

            for (int i = 0; i < texts.Count; i++)
            {
                TMP_Text txt = texts[i]; // Abych nemusel furt hard codovaqt texts[i]

                string desiredText = "NaN";

                switch (texts[i].name)
                {
                    case positionTag:
                        desiredText = position.ToString();
                        break;
                    case nameTag:
                        desiredText = player.username;
                        break;
                    case scoreTag:
                        desiredText = player.score.ToString();
                        break;
                }

                txt.text = desiredText;
            }
            position++;
        }
    }

}
