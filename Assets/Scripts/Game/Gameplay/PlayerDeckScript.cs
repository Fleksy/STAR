using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerDeckScript : MonoBehaviour
{
    [SerializeField] private GameManagerScript GameManager;
    [SerializeField] private Transform playerHand;
    [SerializeField] private DeckContentScript deckPanel;

    [SerializeField] private TextMeshProUGUI cardCount;

    private List<CardScript> deck = new List<CardScript>();
    [SerializeField] private Transform cards;
    [SerializeField] private GameObject cardPref;
    public NetworkOpponentScript opponent;
    private const string deckKey = "Player_Deck";

    private async void Awake()
    {
        await DeckLoader.LoadDeck(deck);
        foreach (var cardData in deck)
        {
            GameObject newCard = Instantiate(cardPref, cards, true);
            CardScript newCardScript = newCard.GetComponent<CardScript>();
            newCardScript.cardName = cardData.cardName;
            newCardScript.cardPower = cardData.cardPower;
            newCardScript.cardColor = cardData.cardColor;
            newCardScript.imagePath = cardData.imagePath;
            newCardScript.cardAbility = cardData.cardAbility;
            newCardScript.cardAbilityPower = cardData.cardAbilityPower;

            newCard.GetComponent<Image>().sprite = cardData.imageSprite;
            newCard.transform.localScale = new Vector3(0.18f, 0.18f, 1);
            newCard.transform.position = new Vector3(0, 0, 0);
            newCard.gameObject.SetActive(false);
        }
        
        deckPanel.SetDeckPanel(cards);
        ShuffleDeck();
        GiveCardToHand(5);
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < cards.childCount; i++)
        {
            int k = Random.Range(0, cards.childCount);
            cards.GetChild(k).transform.SetSiblingIndex(i);
        }
    }

    public void GiveCardToHand(int count)
    {
        if (cards.childCount < count)
        {
            count = cards.childCount;
        }


        for (int i = 0; i < count; i++)
        {
            GameObject card = cards.GetChild(0).gameObject;
            card.transform.SetParent(playerHand.transform);
            card.transform.localScale = new Vector3(0.18f, 0.18f, 1);
            card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, 0);
            card.SetActive(true);
            card.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            deckPanel.UpdateDeckPanel(card.transform, false);
        }

        SetCardCount(cards.childCount);
        opponent.DrawCardsByOpponent(count);
        if ((cards.childCount == 0) && (playerHand.childCount == 0))
        {
            GameManager.EndGame(opponent.opponentNick);
        }
    }

    public void ReturnCardToDeck(Transform card)
    {
        card.transform.SetParent(cards, false);
        deckPanel.UpdateDeckPanel(card, true);
        card.gameObject.SetActive(false);
        SetCardCount(cards.childCount);
    }

    private void SetCardCount(int count)
    {
        cardCount.text = count.ToString();
    }

    private void OnMouseDown()
    {
        deckPanel.gameObject.SetActive(true);
    }

    private async void LoadDeck()
    {
        List<CardDataJSON> cardsDataJsons = new List<CardDataJSON>();
        Dictionary<string, string> uploadedData =
            await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { deckKey });
        cardsDataJsons = JsonConvert.DeserializeObject<List<CardDataJSON>>(uploadedData[deckKey]);
        foreach (var cardData in cardsDataJsons)
        {
            GameObject newCard = Instantiate(cardPref, cards, true);
            newCard.GetComponent<CardScript>();
        }
    }
}