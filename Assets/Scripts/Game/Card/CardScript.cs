using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardScript: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private float scaleFactor;
    private CanvasGroup _canvasGroup;
    public Transform hand;
    private int siblingIndex;
    
    
    public string cardName;
    public int cardPower;
    public CardColor cardColor;
    public Image image;
    public Sprite imageSprite;
    public string imagePath;
    public CardAbility cardAbility;
    public int cardAbilityPower;
   
    public enum CardColor
    {
        Red,
        Green,
        Blue,
    }


    public enum CardAbility
    {
        Nothing,
        ThrowAnyCard,
        TrowCardOwnColor,
        DrawCard,
        TrowAgainstAnyCardByLosingStar,
        ReturnCardFromDiscard,
        DealDamageToHero,
        RestoreHealthToHero,
        WinsDraw,
        CantBeatHero,
        BothDiscard,
        BlockNextCardAbility
    }


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        //image.sprite = Resources.Load<Sprite>(imagePath);
        _rectTransform = gameObject.GetComponent<RectTransform>();
        scaleFactor = gameObject.GetComponentInParent<Canvas>().scaleFactor;
        _canvasGroup = transform.GetComponent<CanvasGroup>();
        hand = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(hand.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!transform.parent.CompareTag("Battlefield"))
        {
            transform.SetParent(hand);
            transform.SetSiblingIndex(siblingIndex);
            transform.parent.GetComponent<HorizontalLayoutGroup>().SetLayoutHorizontal();
            transform.parent.GetComponent<HorizontalLayoutGroup>().SetLayoutVertical();
            _canvasGroup.blocksRaycasts = true;
        }
    }

    public CardScript(string _cardName, int _cardPower, CardScript.CardColor _cardColor, string _imagePath,
        CardScript.CardAbility _ability, int _cardAbilityPower)
    {
        cardPower = _cardPower;
        cardName = _cardName;
        cardColor = _cardColor;
        imagePath = _imagePath;
        
        imageSprite = Resources.Load<Sprite>(imagePath);
        cardAbility = _ability;
        cardAbilityPower = _cardAbilityPower;
    }
}