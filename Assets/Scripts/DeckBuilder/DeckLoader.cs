using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using UnityEngine;

[RequireComponent(typeof(DeckBuilderScript))]
public class DeckLoader : MonoBehaviour
{
   
    private const string deckKey = "Player_Deck";
    static List<CardDataJSON> cardsDataJsons = new List<CardDataJSON>();
    
    public static async Task LoadDeck(List<CardScript> deck)
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{deckKey});
        cardsDataJsons = JsonConvert.DeserializeObject<List<CardDataJSON>>(savedData[deckKey]);

        foreach (var cardJSON in cardsDataJsons)
        {
            deck.Add(new CardScript(cardJSON.cardName, cardJSON.cardPower, cardJSON.cardColor,
                cardJSON.imagePath,
                cardJSON.cardAbility, cardJSON.cardAbilityPower));
        }
    }

    public static async void SaveDeck(List<CardScript> deck)
    {
        cardsDataJsons = new List<CardDataJSON>();
        foreach (CardScript card in deck)
        {
            cardsDataJsons.Add(new CardDataJSON(card.cardName, card.cardPower, card.cardColor,
                card.imageSprite.name, card.cardAbility, card.cardAbilityPower));
        }

        Dictionary<string, object> data = new Dictionary<string, object>() { { deckKey, cardsDataJsons } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }
}

public class CardDataJSON
{
    public string cardName;
    public int cardPower;
    public CardScript.CardColor cardColor;
    public string imagePath;
    public CardScript.CardAbility cardAbility;
    public int cardAbilityPower;

    public CardDataJSON(string _cardName, int _cardPower, CardScript.CardColor _cardColor, string _imageSpriteName,
        CardScript.CardAbility _ability, int _cardAbilityPower)
    {
        cardPower = _cardPower;
        cardName = _cardName;
        cardColor = _cardColor;
        imagePath = "CardsSprites\\" + _cardColor.ToString() + "\\" + _imageSpriteName;
        cardAbility = _ability;
        cardAbilityPower = _cardAbilityPower;
    }
}