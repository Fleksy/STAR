using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Transform lobbyListUI;
    [SerializeField] private Transform settingsUI;
    [SerializeField] private Button lobbyListButton;
    [SerializeField] private Button deckBuilderButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        bool isDeckCorrect = PlayerPrefs.GetInt("isDeckCorrect") == 1;
        if (!isDeckCorrect)
        {
            lobbyListButton.gameObject.SetActive(false);
            deckBuilderButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadDeckBuilderScene()
    {
        SceneManager.LoadScene("Deck Builder");
    }

    public void OpenLobby()
    {
        lobbyListUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}