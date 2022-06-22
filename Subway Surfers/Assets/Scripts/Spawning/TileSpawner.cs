using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TileSpawner : MonoBehaviour
{
    [Header("Trigger Options")]
    [SerializeField] private string playerTag;

    [Header("Tile Info")]
    [SerializeField] private float tileSpawnZOffset = 400f;
    private GameObject _curTile;

    public void InstantiateTile(Transform trigger)
    {
        Transform prevTile = trigger.parent;
        Vector3 spawnPos = new Vector3(0f, 0f, prevTile.position.z + tileSpawnZOffset);
        ObjectPooler.Instance.SpawnFromPool("Tiles", spawnPos);
        prevTile.gameObject.SetActive(false);
    }
}
