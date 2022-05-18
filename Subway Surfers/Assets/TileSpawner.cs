using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;
using Object = System.Object;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private string playerTag;

    private GameObject _curTile;


    private ObjectPooler objectPool;

    private void Awake()
    {
        objectPool = ObjectPooler.Instance; // Nastaví _objectPool na Instanci ObjectPooleru - Singleton Pattern
    }

    public void InstantiateNewTileAndDestroyTheCurrentOne(Transform trigger)
    {
        Transform prevTile = trigger.parent;
        Vector3 spawnPos = new Vector3(0f, 0f, prevTile.position.z + 400);
        ObjectPooler.Instance.SpawnFromPool("Tiles", spawnPos);
        prevTile.gameObject.SetActive(false);
    }
}
