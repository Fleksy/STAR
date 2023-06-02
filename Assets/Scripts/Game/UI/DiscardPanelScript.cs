using UnityEngine;
using UnityEngine.UI;

public class DiscardPanelScript : MonoBehaviour
{
    [SerializeField] private Transform discardCardsList;
    [SerializeField] private Transform playerDiscard;
    [SerializeField] private PlayerDeckScript playerDeck;
    [SerializeField] private GameManagerScript gameManager;

    public void DiscardCard(Transform card)
    {
        for (int i = 0; i < discardCardsList.childCount; i++)
        {
            if (!discardCardsList.GetChild(i).gameObject.activeSelf)
            {
                discardCardsList.GetChild(i).gameObject.SetActive(true);
                discardCardsList.GetChild(i).GetComponent<Image>().sprite = card.GetComponent<Image>().sprite;
                discardCardsList.GetChild(i).GetComponent<DiscardListButton>().card = card;
                break;
            }
        }

        card.transform.SetParent(playerDiscard, false);
    }

    public void ReturnCardToDeck(Transform card)
    {
       
        playerDeck.ReturnCardToDeck(card);
        gameObject.SetActive(false);
        gameManager.NextTurn();
    }
}