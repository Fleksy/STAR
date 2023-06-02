using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPanel : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button ClientBtn;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().isPlayerAttack = true;
            GameObject.Find("Player").GetComponent<PlayerScript>().isPlayerLayCard = true; 
            gameObject.SetActive(false);
        });
        ClientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }
}
