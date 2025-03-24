using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerInventory : MonoBehaviour
{
    private Dictionary<string, Sprite> collectedKeys = new Dictionary<string, Sprite>();
    private Dictionary<string, (Sprite sprite, int count)> parts = new Dictionary<string, (Sprite, int)>();

    [Header("UI References")]
    public GameObject inventoryPanel;
    public GameObject keyContainer;
    public GameObject keyImagePrefab;
    public int collectedParts = 0;
    private bool isInventoryOpen = false;
    public GameObject partsContainer;
    public GameObject partImagePrefab;
    [SerializeField] TMP_Text partsPicked;
    public AudioSource aud;
    public AudioClip pickup;

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
     
        foreach (Transform child in keyContainer.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (var key in collectedKeys)
        {
            GameObject keyImage = Instantiate(keyImagePrefab, keyContainer.transform);
            keyImage.GetComponent<Image>().sprite = key.Value;
        }

      
        foreach (Transform child in partsContainer.transform)
        {
            Destroy(child.gameObject);
        }

       
        foreach (var part in parts)
        {
            GameObject partImage = Instantiate(partImagePrefab, partsContainer.transform);
            partImage.GetComponent<Image>().sprite = part.Value.sprite;

            TMP_Text countText = partImage.GetComponentInChildren<TMP_Text>();
            if (countText != null)
            {
                countText.text = part.Value.count > 1 ? $"x{part.Value.count}" : "";
            }
         
        }
    }

    public void AddKey(string keyID, Sprite keySprite)
    {
        if (!collectedKeys.ContainsKey(keyID))
        {
            collectedKeys[keyID] = keySprite;
            Debug.Log("Collected Key: " + keyID);
            UpdateInventoryUI();
        }
    }
  
    public bool HasKey(string keyID)
    {
        return collectedKeys.ContainsKey(keyID);
    }

    public void GetPart(string partID, Sprite partSprite)
    {
        collectedParts++; // Always increase total collected parts, regardless of whether it's new or duplicate

        if (parts.ContainsKey(partID))
        {
            parts[partID] = (partSprite, parts[partID].count + 1); // Increase count of that specific part
        }
        else
        {
            parts[partID] = (partSprite, 1); // First time collecting this part
        }

        GameManager.instance.updateGameGoal(collectedParts);
        aud.PlayOneShot(pickup);
        partsPicked.text = ($"Collected Part: {partID}");

        UpdateInventoryUI(); 
    }

}