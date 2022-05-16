using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMisc : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Camera cam;
    
    [Header("Tags")]
    [SerializeField] private string obstacleTag;
    [SerializeField] private string grabableTag;
    [SerializeField] private string despawnTriggerTag;

    [Header("Scripts")]
    [SerializeField] private Movement movement;
    [SerializeField] private UIManager uiManager;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == obstacleTag)
        {
            Die();
        }
        else if (col.tag == grabableTag)
        {
            col.GetComponent<IGrabable>().Grab();
        } else if (col.tag == despawnTriggerTag)
        {
            col.GetComponent<TileSpawner>().InstantiateNewTileAndDestroyTheCurrentOne(col.transform);
        }
    }

    private void Die()
    {
        uiManager.ShowOrHideMenu(uiManager.DeadMenu, true);
        uiManager.TimeScale(0f);
        movement.enabled = false;
    }
}
