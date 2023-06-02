using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkOpponentSetterScript : MonoBehaviour
{
    public NetworkOpponentScript opponent;
    public GameManagerScript gameManager;
    public Transform opponentField;
    public PlayerFieldScript playerField;
    public PlayerScript player;
    public OpponentHandScript opponentHand;
    public Text hpPanel;
    public TextMeshProUGUI opponentNick;
    public CardAbilityScript cardAbilityManager;
    public PlayerDeckScript deck;
}
