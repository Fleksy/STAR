using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    public static LobbyCreateUI Instance { get; private set; }


    [SerializeField] private Button createButton;
    [SerializeField] private Button publicPrivateButton;
    [SerializeField] private  TMP_InputField lobbyNameText;
    [SerializeField] private TextMeshProUGUI publicPrivateText;
    

    private string lobbyName;
    private bool isPrivate;
    private int maxPlayers = 2;

    private void Awake()
    {
        Instance = this;
        lobbyNameText.text = "Lobby" + Random.Range(1, 1000);
        createButton.onClick.AddListener(() => {
            LobbyManager.Instance.CreateLobby(
                lobbyName,
                maxPlayers,
                isPrivate
            );
            Hide();
        });
        
        publicPrivateButton.onClick.AddListener(() => {
            isPrivate = !isPrivate;
            UpdateText();
        });

        maxPlayers = 2;

        Hide();
    }

    private void UpdateText()
    {
        lobbyNameText.text = lobbyName;
        publicPrivateText.text = isPrivate ? "Private" : "Public";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        lobbyNameText.text = "Lobby" + Random.Range(1, 1000);
        lobbyName = lobbyNameText.text;
        isPrivate = false;
        maxPlayers = 2;

        UpdateText();
    }
}