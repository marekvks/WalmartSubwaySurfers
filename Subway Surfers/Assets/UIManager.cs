using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menu.activeInHierarchy)
        {
            TimeScale(0f);
            Menu(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.activeInHierarchy)
        {
            TimeScale(1f);
            Menu(false);
        }
    }

    private void TimeScale(float timescale)
    {
        Time.timeScale = timescale;
    }

    public void Menu(bool trueorfalse)
    {
        menu.SetActive(trueorfalse);
    }
}
