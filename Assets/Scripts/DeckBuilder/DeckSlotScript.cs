using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckSlotScript : MonoBehaviour
{
    [SerializeField] private DeckBuilderScript deckBuilder;
    [SerializeField] private TextMeshProUGUI cardPower;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardColor;
    [SerializeField] private Image cardImage;


    public void InstallCardInSlot(CardScript card)
    {
        cardPower.text = card.cardPower.ToString();
        cardName.text = card.cardName;
        switch (card.cardColor)
        {
            case CardScript.CardColor.Red:
                cardColor.color = Color.red;
                break;
            case CardScript.CardColor.Blue:
                cardColor.color = Color.blue;
                break;
            case CardScript.CardColor.Green:
                cardColor.color = Color.green;
                break;
        }

      
            cardImage.sprite = card.imageSprite;
    }

    public void OnClick()
    {
        deckBuilder.RemoveСardFromDeck(transform.GetSiblingIndex());
    }
}