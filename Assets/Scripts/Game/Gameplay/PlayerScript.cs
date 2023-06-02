using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript: MonoBehaviour
{
    public GameManagerScript GameManager;
    public PlayerDeckScript Deck;
    public Transform Hand;

    public bool isPlayerLayCard;

    public Text hpPanel;
    public int health;

    public bool isThrowAnyCard;
    public bool isTrowCardOwnColor;
    public bool isNextCardAbilityBlocked;
    
    public bool host;

    public string playerNick;
    [SerializeField] private TextMeshProUGUI playerNickText;
    void Awake()
    {
        playerNick = AuthenticationService.Instance.Profile;
        playerNickText.text = playerNick;
        
        health = 5;
    }
    
    public void DrawСards()
    {
        if (Hand.childCount < 5)
        {
            Deck.GiveCardToHand(5 - Hand.childCount);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = (health > 5 ? 5 : health);
        hpPanel.text = health.ToString();
        if (health<= 0)
        {
            GameManager.EndGame(GameManager.networkOpponent.opponentNick);
        }
    }
}