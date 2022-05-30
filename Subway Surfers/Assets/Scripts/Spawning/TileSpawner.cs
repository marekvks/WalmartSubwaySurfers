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
    [Header("Trigger Options")]
    [SerializeField] private string playerTag;

    [Header("Tile Info")]
    [SerializeField] private float tileSpawnZOffset = 400f;
    private GameObject _curTile;

    public void InstantiateNewTileAndDestroyTheCurrentOne(Transform trigger)
    {
        Transform prevTile = trigger.parent;
        Vector3 spawnPos = new Vector3(0f, 0f, prevTile.position.z + tileSpawnZOffset);
        ObjectPooler.Instance.SpawnFromPool("Tiles", spawnPos);
        prevTile.gameObject.SetActive(false);
    }
}
