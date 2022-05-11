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

    private float score = 0;
    private int coins = 0;
    
    void Update()
    {
        score = Mathf.Round(Time.time * scoreMultiplyer);
        uiManager.ChangeText(uiManager.ScoreTMP, score.ToString() + "m");
    }

    public void AddCoin(int ammount)
    {
        score += ammount;
        uiManager.ChangeText(uiManager.CoinsTMP, ammount.ToString());
    }
}
