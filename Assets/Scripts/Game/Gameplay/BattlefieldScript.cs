using UnityEngine;

public class BattlefieldScript : MonoBehaviour
{
    public GameManagerScript GameManager;
    public PlayerScript player;

    public Transform playerCards;
    public Transform opponentCards;

    public PlayerDeckScript PlayerDeck;
    public DiscardPanelScript PlayerDiscard;
    public Transform OpponentDiscard;

    public bool CheckPlacedCard(CardScript card)
    {
        if ((GameManager.isPlayerAttack) && (player.isPlayerLayCard))
        {
            if (playerCards.childCount == 0 && opponentCards.childCount == 0)
            {
                return true;
            }


            if (player.isThrowAnyCard)
            {
                player.isThrowAnyCard = false;
                return true;
            }

            if (player.isTrowCardOwnColor)
            {
                player.isTrowCardOwnColor = false;
                if (Equals(card.cardColor, GetLastPlayerCard().cardColor))
                {
                    return true;
                }
            }

            bool checkcardPower = false;
            for (int i = 0; i < playerCards.childCount; i++)
            {
                if (card.cardPower == playerCards.GetChild(i).GetComponent<CardScript>().cardPower)
                {
                    checkcardPower = true;
                    break;
                }
            }

            if (!checkcardPower)
            {
                for (int i = 0; i < opponentCards.childCount; i++)
                {
                    if (card.cardPower == opponentCards.GetChild(i).GetComponent<CardScript>().cardPower)
                    {
                        checkcardPower = true;
                        break;
                    }
                }
            }

            return checkcardPower;
        }

        if ((!GameManager.isPlayerAttack) && (player.isPlayerLayCard))
        {
            if (Equals(card.cardColor, GetLastOpponentCard().cardColor))
                return true;
            if (card.cardAbility == CardScript.CardAbility.TrowAgainstAnyCardByLosingStar)
            {
                card.cardPower--;
                return true;
            }
        }

        return false;
    }

    public CardScript GetLastOpponentCard()
    {
        return opponentCards.GetChild(opponentCards.childCount - 1).GetComponent<CardScript>();
    }

    public CardScript GetLastPlayerCard()
    {
        return playerCards.GetChild(playerCards.childCount - 1).GetComponent<CardScript>();
    }


    public void DiscardCards()
    {
        for (int i = 0; ((i < playerCards.childCount) && (i < opponentCards.childCount));)
        {
            CardScript playerCard = playerCards.GetChild(i).GetComponent<CardScript>();
            CardScript opponentCard = opponentCards.GetChild(i).GetComponent<CardScript>();

            if ((playerCard.cardPower > opponentCard.cardPower &&
                 opponentCard.cardAbility != CardScript.CardAbility.BothDiscard) ||
                (playerCard.cardPower == opponentCard.cardPower &&
                 (playerCard.cardAbility == CardScript.CardAbility.WinsDraw &&
                  opponentCard.cardAbility != CardScript.CardAbility.WinsDraw)))
            {
                PlayerDeck.ReturnCardToDeck(playerCard.transform);
            }
            else
            {
                PlayerDiscard.DiscardCard(playerCard.transform);
            }

            opponentCard.transform.SetParent(OpponentDiscard, false);
        }

        if (playerCards.childCount > 0)
        {
            PlayerDeck.ReturnCardToDeck(playerCards.GetChild(0));
        }

        if (opponentCards.childCount > 0)
        {
            opponentCards.GetChild(0).transform.SetParent(OpponentDiscard, false);
        }
    }
}