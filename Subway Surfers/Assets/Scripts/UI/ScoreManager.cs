using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private UIManager uiManager;

    [Header("Values")]
    [SerializeField] private float scoreMultiplyer = 4f;
    [SerializeField] private float desiredTimeMultiplyer = 1f;
    private float _desiredTime;

    [HideInInspector] public float Score = 0f;
    private int _coins = 0;

    private void Start()
    {
        _desiredTime = Time.time + desiredTimeMultiplyer;
    }

    void Update()
    {
        AddScore();
    }

    private void AddScore()
    {
        // Přidává se po čase
        if (Time.time > _desiredTime)
        {
            Score += scoreMultiplyer;
            _desiredTime = Time.time + desiredTimeMultiplyer;
            uiManager.ChangeText(uiManager.ScoreTMP, Score + "m");
        }
    }

    public void AddCoin(int ammount)
    {
        _coins += ammount;
        uiManager.ChangeText(uiManager.CoinsTMP, _coins.ToString());
    }
}
