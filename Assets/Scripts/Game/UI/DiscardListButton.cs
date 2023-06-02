using UnityEngine;

public class DiscardListButton : MonoBehaviour
{
    public Transform card;
    private DiscardPanelScript discardPanel;

    private void Start()
    {
        discardPanel = transform.parent.parent.GetComponent<DiscardPanelScript>();
    }

    public void OnClick()
    {
        discardPanel.ReturnCardToDeck(card);
        gameObject.SetActive(false);
    }
}
