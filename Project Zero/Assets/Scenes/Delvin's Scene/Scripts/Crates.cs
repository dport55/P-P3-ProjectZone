using UnityEngine;

public class Crates : MonoBehaviour, IDamage
{ 
    [Header("Health Settings")]
    public float health;

    [Header("Spawn Settings")]
    public GameObject[] spawnMaterials;
    public int spawnAmount;
    public float spawnHeightOffset = 1.5f;
    public float spawnRadius = 2f;
    public AudioSource aud;
    public AudioClip[] crateBreak;

    [Header("Destruction Settings")]
    public GameObject destroyedEffect;


    public void TakeDamage(float damage,float freeze,float O2 )
    {
        health -= damage;
    
        if( health <= 0)
        {
            aud.PlayOneShot(crateBreak[Random.Range(0, crateBreak.Length)]);
            DestroyObject();
        }
    }


    private void DestroyObject()
    {
        if (destroyedEffect)
        {
            Instantiate(destroyedEffect, transform.position, Quaternion.identity);
        }

        SpawnItemsInCrate();

        Destroy(gameObject);
    }

    private void SpawnItemsInCrate()
    {
        if (spawnMaterials == null || spawnMaterials.Length == 0)
        {
            Debug.LogWarning("No spawn materials assigned.");
            return;
        }

        for (int i = 0; i < spawnAmount; i++)
        {
            // Ensure valid object selection
            GameObject prefab = spawnMaterials[Random.Range(0, spawnMaterials.Length)];
            if (prefab == null) continue; // Skip null objects

            // Spawn the object exactly at the crate's position
            GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity);

            // Ensure it has a collider
            if (spawnedObject.GetComponent<Collider>() == null)
            {
                spawnedObject.AddComponent<BoxCollider>();
            }
        }
    }
}

