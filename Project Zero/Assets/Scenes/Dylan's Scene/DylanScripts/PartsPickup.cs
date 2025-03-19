using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    public int collectedParts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CollectPart()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.CollectPart();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger area
        if (other.CompareTag("Player"))
        {
            CollectPart();  // Collect the part
        }
    }
}
