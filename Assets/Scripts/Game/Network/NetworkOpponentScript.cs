using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class NetworkOpponentScript : NetworkBehaviour
{
    private NetworkOpponentSetterScript networkOpponentSetter;
    public GameManagerScript gameManager;
    private Transform opponentField;
    private OpponentHandScript opponentHand;

    private PlayerFieldScript playerField;


    private PlayerScript player;
    public string opponentNick;
    private Text hpPanel;
    private int health;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        NetworkManager.Singleton.OnClientConnectedCallback += DisableWaiting;
    }

    void StartOnGameScene()
    {
        networkOpponentSetter = GameObject.Find("NetworkOpponentSetter").GetComponent<NetworkOpponentSetterScript>();
        networkOpponentSetter.opponent = this;
        gameManager = networkOpponentSetter.gameManager;
        gameManager.networkOpponent = this;
        player = networkOpponentSetter.player;
        playerField = networkOpponentSetter.playerField;
        playerField.networkOpponent = this;
        opponentField = networkOpponentSetter.opponentField;
        //passButton = networkOpponentSetter.passButton;
        opponentHand = networkOpponentSetter.opponentHand;
        //deck = networkOpponentSetter.deck;
        //deck.networkOpponent = this;
        hpPanel = networkOpponentSetter.hpPanel;
        gameManager.waitingScreen.gameObject.SetActive(false);
        networkOpponentSetter.cardAbilityManager.opponent = this;
        networkOpponentSetter.deck.opponent = this;

    
        if (IsLocalPlayer)
        {
            player.isPlayerLayCard = IsHost;
            player.host = IsHost;
            // player.playerName = playerNick.ToString();
            //networkOpponentSetter.playerNick.text = playerNick.ToString();
            gameManager.isPlayerAttack = IsHost;
            SetOpponentNick();
        }

        health = 5;
        hpPanel.text = health.ToString();
    }

    public void LayOpponentCard(CardScript card)
    {
        string cardPath = card.cardColor.ToString() + '/' + card.cardName;
        if (player.host)
            LayOpponentCardClientRpc(!player.host, cardPath);
        else
            LayOpponentCardServerRpc(!player.host, cardPath);
    }

    [ServerRpc]
    private void LayOpponentCardServerRpc(bool id, string cardPath)
    {
        if (player.host == id)
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("Cards/" + cardPath), opponentField, true);
            card.transform.localScale = new Vector3(0.18f, 0.18f, 1);
            card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, 0);
            card.SetActive(true);
            opponentHand.UpdateOpponentHand(-1);
            if (card.GetComponent<CardScript>().cardAbility == CardScript.CardAbility.DealDamageToHero)
            {
                player.TakeDamage(card.GetComponent<CardScript>().cardAbilityPower);
            }
            else if (card.GetComponent<CardScript>().cardAbility == CardScript.CardAbility.RestoreHealthToHero)
            {
                TakeDamage(-card.GetComponent<CardScript>().cardAbilityPower);
            }
            /*else if (card.GetComponent<CardScript>().cardAbility == CardScript.CardAbility.DrawCard)
            {
                opponentHand.UpdateOpponentHand(1);
            }*/

            gameManager.NextTurn();
        }
    }

    [ClientRpc]
    private void LayOpponentCardClientRpc(bool id, string cardPath)
    {
        if (player.host == id)
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("Cards/" + cardPath), opponentField, true);
            card.transform.localScale = new Vector3(0.18f, 0.18f, 1);
            card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, 0);
            card.SetActive(true);
            opponentHand.UpdateOpponentHand(-1);
            if (card.GetComponent<CardScript>().cardAbility == CardScript.CardAbility.DealDamageToHero)
            {
                player.TakeDamage(card.GetComponent<CardScript>().cardAbilityPower);
            }
            else if (card.GetComponent<CardScript>().cardAbility == CardScript.CardAbility.RestoreHealthToHero)
            {
                TakeDamage(-card.GetComponent<CardScript>().cardAbilityPower);
            }
            /* else if (card.GetComponent<CardScript>().cardAbility == CardScript.CardAbility.DrawCard)
            {
                opponentHand.UpdateOpponentHand(1);
            }*/

            gameManager.NextTurn();
        }
    }


    public void Pass()
    {
        if (player.host)
        {
            PassClientRpc(!player.host);
        }
        else
        {
            PassServerRpc(!player.host);
        }
    }

    [ServerRpc]
    private void PassServerRpc(bool id)
    {
        if (player.host == id)
        {
            gameManager.EndRound();
        }
    }

    [ClientRpc]
    private void PassClientRpc(bool id)
    {
        if (player.host == id)
        {
            gameManager.EndRound();
        }
    }

    public void DrawCardsByOpponent(int count)
    {
        if (player.host)
            DrawCardsByOpponentClientRpc(!player.host, count);
        else
            DrawCardsByOpponentServerRpc(!player.host, count);
    }

    [ServerRpc]
    private void DrawCardsByOpponentServerRpc(bool id, int count)
    {
        if (player.host == id)
            opponentHand.UpdateOpponentHand(count);
    }

    [ClientRpc]
    private void DrawCardsByOpponentClientRpc(bool id, int count)
    {
        if (player.host == id)
            opponentHand.UpdateOpponentHand(count);
    }

    private void SetOpponentNick()
    {
        if (player.host)
            SetOpponentNickClientRpc(!player.host, player.playerNick);
        else
            SetOpponentNickServerRpc(!player.host, player.playerNick);
    }

    [ServerRpc]
    private void SetOpponentNickServerRpc(bool id, string _opponentNick)
    {
        if (player.host == id)
        {
            opponentNick = _opponentNick;
            networkOpponentSetter.opponentNick.text = opponentNick;
        }
    }

    [ClientRpc]
    private void SetOpponentNickClientRpc(bool id, string _opponentNick)
    {
        if (player.host == id)
        {
            opponentNick = _opponentNick;
            networkOpponentSetter.opponentNick.text = opponentNick;
        }
    }

    private void DisableWaiting(ulong id)
    {
        if (id == 1)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                StartOnGameScene();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = (health > 5 ? 5 : health);
        hpPanel.text = health.ToString();
        if (health <= 0)
        {
            gameManager.EndGame(player.playerNick);
        }
    }
}