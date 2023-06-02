using UnityEngine;

public class AddingCardButtonScript : MonoBehaviour
{
    [SerializeField] DeckBuilderScript deckBuilder;
    [SerializeField] private CollectionPanelScript collectionPanel;

    public void OnClick()
    {
        deckBuilder.AddCartToDeck(collectionPanel.shownCards[transform.GetSiblingIndex()]);
    }

}
