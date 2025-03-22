using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    private Dictionary<string, Sprite> collectedKeys = new Dictionary<string, Sprite>();

    [Header("UI References")]
    public GameObject inventoryPanel; // Assign the Inventory UI Panel in the Inspector
    public GameObject keyContainer;   // Assign a UI Container (e.g., an empty GameObject with Grid Layout)
    public GameObject keyImagePrefab; // Prefab for displaying collected keys
    public int collectedParts = 0;
    private bool isInventoryOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            
            ToggleInventory();
        }

    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
          
            UpdateInventoryUI();
        }
      
    }

    void UpdateInventoryUI()
    {
        // Clear previous keys from UI (destroy only instantiated instances)
        foreach (Transform child in keyContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Add collected keys to UI
        foreach (var key in collectedKeys)
        {
            GameObject keyImage = Instantiate(keyImagePrefab, keyContainer.transform); // Correctly instantiating in scene hierarchy
            keyImage.GetComponent<Image>().sprite = key.Value; // Assign the key sprite
        }
    }
    public void AddKey(string keyID, Sprite keySprite)
    {
        if (!collectedKeys.ContainsKey(keyID))
        {
            collectedKeys[keyID] = keySprite;
            Debug.Log("Collected Key: " + keyID);
            UpdateInventoryUI(); // Update UI instantly when a key is collected
        }
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.ContainsKey(keyID);
    }

    public void getParts(GameObject part)
    {
        collectedParts++; // Increase part count
        GameManager.instance.updateGameGoal(collectedParts);
        Debug.Log("Parts Collected: " + collectedParts);
    }
}