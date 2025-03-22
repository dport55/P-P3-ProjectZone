using UnityEngine;

public class Shot : MonoBehaviour
{
    
    public float speed;         
    public float maxDistance;  
    public float damage;              
    public float freezetime;
    public float O2Level = 0;
    public ParticleSystem hitEffect;
    public Collider bulletCollider;



    private Vector3 startPosition;
    private Vector3 moveForward;

     void Start()
    {
        startPosition = transform.position;  // Store the starting point
        //transform.localRotation = Quaternion.Euler(90f, 0f, 0f) * transform.rotation;

    }

     void Update()
    {
        // Move forward relative to the laser's local forward direction
        transform.localPosition += transform.up * speed * Time.deltaTime;


        // Destroy if it exceeds max distance
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


     void OnTriggerEnter(Collider other)
     {
        // Check if the object hit takes damage
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, freezetime, O2Level);
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hitEffect.gameObject, 0.05f);
        }

        // Destroy the laser and hiteffct on impact
        Destroy(gameObject);
       
    }
}
