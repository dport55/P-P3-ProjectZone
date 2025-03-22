using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    [SerializeField] GameObject part;
    //public int collectedParts;

    //Delvin's Changes
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory pick = other.GetComponent<PlayerInventory>(); // Get IPickup interface from Player

        if (pick != null)
        {
            pick.getParts(part); // Call getParts() on PlayerInventory
            Destroy(gameObject); // Destroy part after collection
        }
    }
    //End of Delvin's Changes
}
