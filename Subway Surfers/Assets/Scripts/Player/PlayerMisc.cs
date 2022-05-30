using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMisc : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string obstacleTag;
    [SerializeField] private string grabableTag;
    [SerializeField] private string despawnTriggerTag;

    [Header("Scripts")]
    [SerializeField] private Movement movement;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager GameManager;

    private void OnTriggerEnter(Collider col)
    {
        string tag = col.tag;

        if (tag == obstacleTag)
            Die();
        else if (tag == grabableTag)
            col.GetComponent<IGrabable>().Grab();
        else if (tag == despawnTriggerTag)
            col.GetComponent<TileSpawner>().InstantiateNewTileAndDestroyTheCurrentOne(col.transform);
        }

    private void Die()
    {
        uiManager.ShowOrHideMenu(uiManager.DeadMenu, true);
        GameManager.Pause();
        movement.enabled = false;
    }
}
