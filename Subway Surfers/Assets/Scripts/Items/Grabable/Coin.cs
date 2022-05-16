using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour, IGrabable
{
    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Values")]
    [SerializeField] private int scoreAmmount = 1;
    
    public void Grab()
    {
        scoreManager.AddCoin(scoreAmmount);
        audioManager.PlaySFXSound(audioManager.SFX_CoinPickup);
        gameObject.SetActive(false);
    }
}
