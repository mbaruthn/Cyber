using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int CardID { get; private set; }
    public bool isFlipped = false;
    public bool IsMatched { get; private set; } = false;

    public System.Action<Card> OnCardSelected;

    private float flipDuration = 0.5f; // Duration for the flip animation

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

        // Start the flip animation
        StartCoroutine(FlipAnimation(isFlipped));
    }

    private System.Collections.IEnumerator FlipAnimation(bool flipToFace)
    {
        float startRotationY = transform.eulerAngles.y;
        float endRotationY = flipToFace ? 180f : 0f; // Rotate to 180 degrees if flipped, otherwise back to 0
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            elapsedTime += Time.deltaTime;
            float newYRotation = Mathf.Lerp(startRotationY, endRotationY, elapsedTime / flipDuration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);
            yield return null;
        }

        // Ensure final rotation is set
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotationY, transform.eulerAngles.z);

        Debug.Log($"Card {(flipToFace ? "flipped to face" : "flipped to back")}: {CardID}");
    }

    public void SetMatched()
    {
        IsMatched = true;
        Debug.Log("Card matched: " + CardID);
    }
}
