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
    public Material[] materials; // Array of materials to assign based on CardID

    private MeshRenderer cardRenderer; // Reference to the MeshRenderer component

    private void Awake()
    {
        // Find the MeshRenderer component in children
        cardRenderer = GetComponentInChildren<MeshRenderer>();
        if (cardRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found in child objects of the card!");
        }
    }

    public void SetCardID(int id)
    {
        CardID = id;
        UpdateCardMaterial(); // Update material when ID is set
    }

    private void UpdateCardMaterial()
    {
        if (materials != null && materials.Length > 0)
        {
            // Generate the material index based on CardID
            int materialIndex = CardID % materials.Length;
            if (materialIndex < materials.Length)
            {
                // Apply the material to the MeshRenderer
                Debug.Log(cardRenderer.transform.gameObject.name);
                Material[] currentMaterials = cardRenderer.materials; // Get all materials
                currentMaterials[2] = materials[materialIndex]; // Replace the first material (or desired index)
                cardRenderer.materials = currentMaterials; // Reassign materials to the renderer

                Debug.Log($"Material for CardID {CardID} set to {materials[materialIndex].name}");
            }
            else
            {
                Debug.LogError("Invalid material index calculated!");
            }
        }
        else
        {
            Debug.LogWarning("No materials assigned to the card!");
        }
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

        // Play card flip sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.cardFlipClip);
    }

    private System.Collections.IEnumerator FlipAnimation(bool flipToFace)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, flipToFace ? 180f : 0f, transform.eulerAngles.z);
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / flipDuration);
            yield return null;
        }

        // Ensure final rotation is set
        transform.rotation = endRotation;

        Debug.Log($"Card {(flipToFace ? "flipped to face" : "flipped to back")}: {CardID}");
    }

    public void SetMatched()
    {
        IsMatched = true;
        Debug.Log("Card matched: " + CardID);
    }
}
