using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("TMP")]
    public TMP_Text ScoreTMP;
    public TMP_Text CoinsTMP;
    
    [Header("Menu")]
    [SerializeField] private GameObject _menu;
    public GameObject DeadMenu;

    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;

    private bool _escapeMenuOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_escapeMenuOpen)
        {
            gameManager.Pause();
            Menu(true);
            _escapeMenuOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _escapeMenuOpen)
        {
            gameManager.Resume();
            Menu(false);
            _escapeMenuOpen = false;
        }
    }

    public void Menu(bool trueOrFalse) => _menu.SetActive(trueOrFalse);

    public void ShowOrHideMenu(GameObject menu, bool show) => menu.SetActive(show);

    public void ChangeText(TMP_Text tmp, string text) => tmp.text = text;
}
