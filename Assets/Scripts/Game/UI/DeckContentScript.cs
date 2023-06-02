using UnityEngine;
using UnityEngine.UI;

public class DeckContentScript : MonoBehaviour
{
    [SerializeField] private Transform cardsImages;
    
    public void SetDeckPanel(Transform cards)
    {
        for (int i = 0; i < cardsImages.childCount; i++)
        {
            if (i < cards.childCount)
                cardsImages.GetChild(i).GetComponent<Image>().sprite = cards.GetChild(i).GetComponent<Image>().sprite;
            else
                cardsImages.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdateDeckPanel(Transform card,bool state)
    {
        Sprite sprite = card.GetComponent<Image>().sprite;   
        for (int i = 0; i < cardsImages.childCount; i++)
        {
            if (Equals(cardsImages.GetChild(i).GetComponent<Image>().sprite,sprite))
            {
                cardsImages.GetChild(i).gameObject.SetActive(state);
                break;
            }
        }
    }
}