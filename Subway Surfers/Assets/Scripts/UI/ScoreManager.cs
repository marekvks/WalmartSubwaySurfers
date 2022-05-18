using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private UIManager uiManager;

    [SerializeField] private float scoreMultiplyer = 4f;

    private float score = 0f;
    private int coins = 0;

    private float _desiredTime;
    [SerializeField] private float desiredTimeMultiplyer = 1f;

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
        if (Time.time > _desiredTime)
        {
            score += scoreMultiplyer;
            _desiredTime = Time.time + desiredTimeMultiplyer;
            uiManager.ChangeText(uiManager.ScoreTMP, score + "m");
        }
    }

    public void AddCoin(int ammount)
    {
        coins += ammount;
        uiManager.ChangeText(uiManager.CoinsTMP, coins.ToString());
    }

    public void ResetScore()
    {
        score = 0f;
    }

}
