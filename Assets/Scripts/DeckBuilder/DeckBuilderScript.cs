using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DeckBuilderScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deckPowerText;
    [SerializeField] private TextMeshProUGUI cardCountText;
    public List<CardScript> deck = new List<CardScript>();
    public int deckPower;
    public int activeElementCount;
    private DeckSlotScript[] cardSlots = new DeckSlotScript[30];
    
    private async void Start()
    {
        Transform deckContent = transform.GetChild(0);
        activeElementCount = 0;
        for (int i = 0; i < deckContent.childCount; i++)
        {
            cardSlots[i] = deckContent.GetChild(i).GetComponent<DeckSlotScript>();
        }

        List<CardScript> uploadedСards = new List<CardScript>();
        await DeckLoader.LoadDeck(uploadedСards);
        foreach (var card in uploadedСards)
        {
            AddCartToDeck(card);
        }
    }

    public void AddCartToDeck(CardScript card)
    {
        if ((deckPower + card.cardPower <= 30) && (CheckForRepetition(card.cardName)))
        {
            int index = FindCardPosition(card);
            deck.Insert(index, card);
            cardCountText.text = deck.Count.ToString();
            activeElementCount++;
            deckPower += card.cardPower;
            deckPowerText.text = deckPower + "/30";
            cardSlots[activeElementCount].InstallCardInSlot(card);
            cardSlots[activeElementCount].gameObject.SetActive(true);
            cardSlots[activeElementCount].transform.SetSiblingIndex(index);
        }
    }

    public void RemoveСardFromDeck(int index)
    {
        deckPower -= deck[index].cardPower;
        deck.RemoveAt(index);
        cardCountText.text = deck.Count.ToString();
        deckPowerText.text = deckPower + "/30";
        transform.GetChild(0).GetChild(index).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(index).SetSiblingIndex(30);
    }

    int FindCardPosition(CardScript card)
    {
        int index;
        for (index = 0; (index < deck.Count); index++)
        {
            if ((card.cardPower <= deck[index].cardPower) && (card.cardColor <= deck[index].cardColor))
            {
                break;
            }
        }

        return index;
    }

    bool CheckForRepetition(string cardName)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].cardName == cardName)
                return false;
        }
        return true;
    }

    public void SaveDeck()
    {
        PlayerPrefs.SetInt("isDeckCorrect", ((deckPower <= 30) && (deck.Count >= 10)) ? 1 : 0);
        DeckLoader.SaveDeck(deck);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Lobby");
    }
}