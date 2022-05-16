using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

    public Dictionary<string, Queue<GameObject>> PoolDict; // Dictionary, ke kterému se accessuju přes tag (string), má v sobě Queue GameObjectů

    private void Start()
    {
        PoolDict = new Dictionary<string, Queue<GameObject>>(); // Nastaví se Dictionary na nový Dictionary :)

        foreach (Pool item in Pools) // Pojíždí všechny classy Pool v listu Pools
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); // Vytvoří se Queue, Queue funguje doslova jako řada, která si bere vždycky věci popořadě

            for (int i = 0; i < item.GameObjects.Count; i++) // Slouží k vytvoření objektů
            {
                GameObject newObject = Instantiate(item.GameObjects[i]); // Vytvoří objekt, uloží si to jako GameObject
                newObject.SetActive(false); // Vypne je, aby nebyli vidět
                objectPool.Enqueue(newObject); // Dá je do queue - na konec
            }

            PoolDict.Add(item.Tag, objectPool); // Dá queue objectPool do Dictionary PoolDict a přidá tomu tag, abych se k tomu mohl accessovat přes tag
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position) // Stejně jako instantiate vrací GameObject, abych s ním mohl dále pracovat, tak to vracím i tady
    {
        GameObject spawnObject = PoolDict[tag].Dequeue(); // Vezme první GameObject z Queue, uloží si to jako spawnObject
        spawnObject.transform.position = position; // Nastaví požadovanou pozici
        spawnObject.SetActive(true); // Zapne
        
        PoolDict[tag].Enqueue(spawnObject); // Dá to zpátky do Queue, aby se mohl znovu využívat

        return spawnObject; // vrátí GameObject stejně jako ho vrací instantiate
    }
}
