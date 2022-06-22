using DefaultNamespace;
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
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider col)
    {
        string tag = col.tag;

        if (tag == null) return;

        if (tag == obstacleTag)
            Die();
        else if (tag == grabableTag)
            col.GetComponent<IGrabable>().Grab();
        else if (tag == despawnTriggerTag)
            col.GetComponent<TileSpawner>().InstantiateTile(col.transform);
    }

    private void Die()
    {
        uiManager.ShowOrHideMenu(uiManager.DeadMenu, true);
        gameManager.Pause();
        movement.enabled = false;
    }
}
