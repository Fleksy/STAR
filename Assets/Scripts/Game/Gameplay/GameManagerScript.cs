using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private BattlefieldScript BattleField;
    [SerializeField] private Transform deck;
    private BoxCollider2D deckCollider;

    [SerializeField] public PlayerScript player;
    [SerializeField] private Transform playerHand;
    [SerializeField] private OpponentHandScript opponentHand;


    [SerializeField] private GameObject RoundNotification;

    [SerializeField] private TextMeshProUGUI roundCounter;

    private int roundNum = 1;

    public Canvas canvas;
    private CanvasGroup canvasGroup;
    public Transform waitingScreen;

    public NetworkOpponentScript networkOpponent;


    public bool isPlayerAttack;
    private bool endGame = false;


    [SerializeField] private Transform endRoundNotification;
    [SerializeField] private Button endButton;

    private void Awake()
    {
        waitingScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        deckCollider = deck.GetComponent<BoxCollider2D>();
        StartCoroutine(NotifyAboutStartOfRound());
        endButton.GetComponent<Button>().onClick.AddListener(LoadLobbyScene);
    }

    private void StartRound()
    {
        roundNum++;
        isPlayerAttack = !isPlayerAttack;

        player.isPlayerLayCard = isPlayerAttack;
        playerHand.GetComponent<CanvasGroup>().blocksRaycasts = player.isPlayerLayCard;

        player.DrawСards();
        if ((!endGame) && (opponentHand.activeCardCount == 0))
        {
            EndGame(player.playerNick);
        }
        else
        {
            roundCounter.text = roundNum.ToString();
            StartCoroutine(NotifyAboutStartOfRound());
        }
    }

    public void NextTurn()
    {
        player.isPlayerLayCard = !player.isPlayerLayCard;
        playerHand.GetComponent<CanvasGroup>().blocksRaycasts = player.isPlayerLayCard;
    }

    public void Pass()
    {
        if (!(isPlayerAttack && BattleField.playerCards.childCount == 0) && player.isPlayerLayCard)
        {
            EndRound();
            networkOpponent.Pass();
        }
    }

    public void EndRound()
    {
        {
            if (BattleField.playerCards.childCount > BattleField.opponentCards.childCount)
            {
                if (BattleField.GetLastPlayerCard().cardAbility != CardScript.CardAbility.CantBeatHero)
                {
                    networkOpponent.TakeDamage(1);
                }
            }

            if (BattleField.playerCards.childCount < BattleField.opponentCards.childCount)
            {
                if (BattleField.GetLastOpponentCard().cardAbility != CardScript.CardAbility.CantBeatHero)
                {
                    player.TakeDamage(1);
                }
            }

            BattleField.DiscardCards();
            player.isNextCardAbilityBlocked = false;
            player.isThrowAnyCard = false;
            player.isTrowCardOwnColor = false;
            if (!endGame)
            {
                StartRound();
            }
        }
    }


    public void EndGame(string winner)
    {
        endGame = true;
        endRoundNotification.gameObject.SetActive(true);
        endRoundNotification.GetComponentInChildren<TextMeshProUGUI>().text =
            "Игрок " + winner + " победил!";
        endButton.gameObject.SetActive(true);
        deckCollider.enabled = false;
    }

    private void LoadLobbyScene()
    {
        LobbyManager.Instance.LeaveLobby();
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }


    IEnumerator NotifyAboutStartOfRound()
    {
        RoundNotification.GetComponentInChildren<TextMeshProUGUI>().text = "Раунд " + roundNum;
        RoundNotification.SetActive(true);
        canvasGroup.blocksRaycasts = false;
        deckCollider.enabled = false;
        yield return new WaitForSeconds(2);
        RoundNotification.SetActive(false);
        canvasGroup.blocksRaycasts = true;
        deckCollider.enabled = true;
    }
}