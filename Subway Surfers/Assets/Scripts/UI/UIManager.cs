using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("")]
    
    [Header("TMP")]
    public TMP_Text ScoreTMP;
    public TMP_Text CoinsTMP;
    
    [Header("Menu")]
    [SerializeField] private GameObject menu;
    public GameObject DeadMenu;

    private bool _escapeMenuOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_escapeMenuOpen)
        {
            TimeScale(0f);
            Menu(true);
            _escapeMenuOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _escapeMenuOpen)
        {
            TimeScale(1f);
            Menu(false);
            _escapeMenuOpen = false;
        }
    }

    public void TimeScale(float timescale)
    {
        Time.timeScale = timescale;
    }

    public void Menu(bool trueorfalse)
    {
        menu.SetActive(trueorfalse);
    }

    public void ShowOrHideMenu(GameObject menu, bool show)
    {
        menu.SetActive(show);
    }

    public void ChangeText(TMP_Text tmp, string text)
    {
        tmp.text = text;
    }
}
