using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int CardID { get; private set; }
    public bool isFlipped = false;
    public bool IsMatched { get; private set; } = false;

    public System.Action<Card> OnCardSelected;

    public void SetCardID(int id)
    {
        CardID = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");
        if (!isFlipped && !IsMatched && OnCardSelected != null)
        {
            OnCardSelected.Invoke(this);
        }
    }

    public void FlipCard()
    {
        isFlipped = !isFlipped;
        // Animation or visual change for flipping the card will be handled here
        Debug.Log("Card flipped: " + CardID);
        this.GetComponent<Animator>().SetBool("CardFlipped", isFlipped);
    }

    public void SetMatched()
    {
        IsMatched = true;
        // Animation or effect for card matching status will be handled here
        Debug.Log("Card matched: " + CardID);
    }
}
