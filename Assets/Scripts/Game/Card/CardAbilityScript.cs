using UnityEngine;

public class CardAbilityScript : MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private PlayerScript player;
    public NetworkOpponentScript opponent;
    [SerializeField] private PlayerDeckScript playerDeck;
    [SerializeField] private DiscardPanelScript discardPanel;
    [SerializeField] private Transform playerDiscard;

    public void UseCardAbility(CardScript card)
    {
        switch (card.cardAbility)
        {
            case CardScript.CardAbility.ThrowAnyCard:
                if (gameManager.isPlayerAttack)
                {
                    ThrowAnyCard();
                }

                break;
            case CardScript.CardAbility.TrowCardOwnColor:
                if (gameManager.isPlayerAttack)
                {
                    TrowCardOwnColor();
                }

                break;
            case CardScript.CardAbility.DrawCard:
                DrawCard();
                break;
            case CardScript.CardAbility.DealDamageToHero:
                DealDamageToHero(card.cardAbilityPower);
                break;
            case CardScript.CardAbility.RestoreHealthToHero:
                RestoreHealthToHero(card.cardAbilityPower);
                break;
            case CardScript.CardAbility.BlockNextCardAbility:
                BlockNextCardAbility();
                break;
            case CardScript.CardAbility.ReturnCardFromDiscard:
                ReturnCardFromDiscard();
                break;
        }
    }

    private void ThrowAnyCard()
    {
        player.isThrowAnyCard = true;
    }

    private void TrowCardOwnColor()
    {
        player.isTrowCardOwnColor = true;
    }

    private void DealDamageToHero(int damage)
    {
        opponent.TakeDamage(damage);
    }

    private void RestoreHealthToHero(int heal)
    {
        player.TakeDamage(-heal);
    }

    private void DrawCard()
    {
        playerDeck.GiveCardToHand(1);
    }

    private void BlockNextCardAbility()
    {
        player.isNextCardAbilityBlocked = true;
    }

    private void ReturnCardFromDiscard()
    {
        if (playerDiscard.transform.childCount > 0)
            discardPanel.gameObject.SetActive(true);
    }
}