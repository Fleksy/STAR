using UnityEngine;
using UnityEngine.UI;

public class CollectionPanelScript : MonoBehaviour
{
    [SerializeField] private DeckBuilderScript deckBuilder;

    private int page;
    private int pointer;

    private int selectedPower;
    private CardScript.CardColor selectedColor;
    private CardScript[] selectedArray;
    public CardScript[] redCards;
    private CardScript[] blueCards;
    private CardScript[] greenCards;

    public  CardScript[] shownCards;

    private void Start()
    {
        redCards = Resources.LoadAll<CardScript>("Cards/Red");
        blueCards = Resources.LoadAll<CardScript>("Cards/Blue");
        greenCards = Resources.LoadAll<CardScript>("Cards/Green");
        selectedPower = 0;
        selectedColor = CardScript.CardColor.Red;
        ShowCards();
    }
    
    private void ShowCards()
    {
        switch (selectedColor)
        {
            case (CardScript.CardColor.Red):
                selectedArray = redCards;
                break;

            case (CardScript.CardColor.Blue):
                selectedArray = blueCards;
                break;

            case (CardScript.CardColor.Green):
                selectedArray = greenCards;
                break;
        }

        pointer = page * 8;


        for (int i = 0; (i < 8);)
        {
            if ((pointer >= selectedArray.Length))
            {
                transform.GetChild(i).gameObject.SetActive(false);
                i++;
            }
            else if ((selectedArray[pointer].cardPower == selectedPower) || (selectedPower == 0))
            {
                shownCards[i] = selectedArray[pointer];
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<Image>().sprite = selectedArray[pointer].image.sprite;
                selectedArray[pointer].imageSprite = selectedArray[pointer].image.sprite;
                i++;
            }

            pointer++;
        }
    }

    public void SetSelectedColor(string color)
    {
        switch (color)
        {
            case "Green":
                selectedColor = CardScript.CardColor.Green;
                break;
            case "Red":
                selectedColor = CardScript.CardColor.Red;
                break;
            case "Blue":
                selectedColor = CardScript.CardColor.Blue;
                break;
        }

        ShowCards();
    }

    public void SetSelectedPower(int power)
    {
        selectedPower = power;
        ShowCards();
    }

    public void NextPage()
    {
        if ((page + 1) * 8 < selectedArray.Length)
            page++;
        ShowCards();
    }

    public void PreviousPage()
    {
        if (page > 0)
            page--;
        ShowCards();
    }
    
}