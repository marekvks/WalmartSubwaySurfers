using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    #region Singleton Instance

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this; // Vytvoří instanci scriptu ObjectPooler, aby se mohl používat v jakémkoliv scriptu
    }

    #endregion
    
    
    [System.Serializable]
    public class Pool // Třída "Pool", která se má v sobě List GameObjectů - v tomto případě map tilů, tag
    {
        public List<GameObject> GameObjects;
        public string Tag;
    }

    public List<Pool> Pools; // List třídy nad

    public Dictionary<string, List<GameObject>> PoolDict; // Dictionary, ke kterému se accessuju přes tag (string), má v sobě List GameObjectů

    private void Start()
    {
        PoolDict = new Dictionary<string, List<GameObject>>(); // Nastaví se Dictionary na nový Dictionary :)

        foreach (Pool item in Pools) // Pojíždí všechny classy Pool v listu Pools
        {
            List<GameObject> objectPoolList = new List<GameObject>();

            for (int i = 0; i < item.GameObjects.Count; i++) // Slouží k vytvoření objektů
            {
                GameObject newObject = Instantiate(item.GameObjects[i]); // Vytvoří objekt, uloží si to jako GameObject
                newObject.SetActive(false); // Vypne je, aby nebyli vidět
                objectPoolList.Add(newObject);
            }

            PoolDict.Add(item.Tag, objectPoolList); // Dá List objectPool do Dictionary PoolDict a přidá tomu tag, abych se k tomu mohl accessovat přes tag
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position) // Stejně jako instantiate vrací GameObject, abych s ním mohl dále pracovat, tak to vracím i tady
    {
        GameObject spawnObject;
        
        do
        {
            int randomNum = Random.Range(0, PoolDict[tag].Count);
            spawnObject = PoolDict[tag][randomNum];
        } while (spawnObject.activeInHierarchy);
        spawnObject.transform.position = position; // Nastaví požadovanou pozici
        spawnObject.SetActive(true); // Zapne

        return spawnObject; // vrátí GameObject stejně jako ho vrací instantiate
    }
}
