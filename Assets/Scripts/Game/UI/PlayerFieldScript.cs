using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFieldScript : MonoBehaviour, IDropHandler
{
    private BattlefieldScript BattleField;
    public GameManagerScript gameManager;
    public CardAbilityScript CardAbilityManager;
    public NetworkOpponentScript networkOpponent;
    [SerializeField] private PlayerScript player;

    private void Start()
    {
        BattleField = transform.parent.GetComponent<BattlefieldScript>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardScript card = eventData.pointerDrag.GetComponent<CardScript>();
        if (BattleField.CheckPlacedCard(card))
        {
            card.transform.SetParent(BattleField.playerCards);
            networkOpponent.LayOpponentCard(card);
            if (player.isNextCardAbilityBlocked)
            {
                player.isNextCardAbilityBlocked = false;
            }
            else
            {
                CardAbilityManager.UseCardAbility(card);
            }
            
            if (card.cardAbility != CardScript.CardAbility.ReturnCardFromDiscard)
                gameManager.NextTurn();
        }
    }
}