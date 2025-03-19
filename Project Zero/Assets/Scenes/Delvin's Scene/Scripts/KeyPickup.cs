using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public string keyID;

    public Sprite keySprite; // Assign a sprite for the key in Unity Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddKey(keyID, keySprite);
                Destroy(gameObject); // Remove the key from the scene
            }
        }
    }
}