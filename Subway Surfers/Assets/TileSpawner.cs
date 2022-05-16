using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private string playerTag;

    public List<GameObject> TilePrefabs = new List<GameObject>();

    private GameObject _curTile;


    private ObjectPooler _objectPool;

    private void Awake()
    {
        _objectPool = ObjectPooler.Instance; // Nastaví _objectPool na Instanci ObjectPooleru - Singleton Pattern
    }

    public void InstantiateNewTileAndDestroyTheCurrentOne(Transform collider)
    {
        Transform prevTile = collider.parent;
        Debug.Log(prevTile);
        _objectPool.SpawnFromPool("Tiles", new Vector3(0f, 0f, prevTile.position.z + 400f));
        prevTile.gameObject.SetActive(false);
    }

    /*private GameObject RandomTile()
    {
        int randomNumber = Random.Range(0, TilePrefabs.Count);
        GameObject randomTile = TilePrefabs[randomNumber];
        return randomTile;
    }*/

}
