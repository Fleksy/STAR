using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private Button authenticateButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private SettingsScript settings;
    private void Awake()
    {
        settings.SetSettings(); 
        authenticateButton.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
        });
        if (AuthenticationService.Instance.IsAuthorized)
        { 
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
        }
       
    }

    private void Hide()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}