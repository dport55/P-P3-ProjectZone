using UnityEngine;

public class PartsPickup : MonoBehaviour
{   //Delvin's Changes
    [SerializeField] private string partID; // Unique identifier for each part
    [SerializeField] private Sprite partSprite; // Sprite to display in inventory

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory pick = other.GetComponent<PlayerInventory>();

        if (pick != null)
        {
            pick.GetPart(partID, partSprite); // Pass part ID and sprite
            Destroy(gameObject); // Destroy part after collection
        }
    }
    //End of Delvin's Changes
}
