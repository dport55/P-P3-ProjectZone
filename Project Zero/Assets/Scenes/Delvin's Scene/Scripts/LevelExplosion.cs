using UnityEngine;

public class LevelExplosion : MonoBehaviour
{
 
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float force = 700f;
    [SerializeField] public AudioSource explosionSound;
    [SerializeField] public AudioClip[] explosionClip;
    [SerializeField] public float vol;


    private float countdown;
    private bool hasExploded = false;

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Spawn explosion effect
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Get all colliders in explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            // Check if the object is tagged as "Level"
            if (nearbyObject.CompareTag("Level"))
            {
                // Ensure object has a Rigidbody
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = nearbyObject.gameObject.AddComponent<Rigidbody>(); // Add Rigidbody if missing
                    rb.mass = 1f; // Set a reasonable mass
                }

                // Ensure MeshCollider is convex if present
                MeshCollider meshCollider = nearbyObject.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.convex = true;
                }

                // Apply explosion force
                rb.AddExplosionForce(force, transform.position, radius);
                explosionSound.PlayOneShot(explosionClip[Random.Range(0, explosionClip.Length)], vol);
                Debug.Log(vol);
            }
        }

        Debug.Log("Explosion!");

        // Destroy grenade object
        Destroy(gameObject);
    }

}
