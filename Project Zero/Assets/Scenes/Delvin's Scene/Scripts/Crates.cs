using UnityEngine;

public class Crates : MonoBehaviour, IDamage
{ 
    [Header("Health Settings")]
    public float health;

    [Header("Spawn Settings")]
    public GameObject spawnMaterial;
    //public int spawnAmount;
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
        if (spawnMaterial == null)
        {
            Debug.LogWarning("No spawn materials assigned.");
            return;
        }

        Vector3 itemSpawnPosition = transform.position + new Vector3(0, 0.25f, 0);
        GameObject spawnedObject = Instantiate(spawnMaterial, itemSpawnPosition, Quaternion.identity);

          
            if (spawnedObject.GetComponent<Collider>() == null)
            {
                spawnedObject.AddComponent<BoxCollider>();
            }
        
    }
}

