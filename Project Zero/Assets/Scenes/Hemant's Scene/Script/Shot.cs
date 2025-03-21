using UnityEngine;

public class Shot : MonoBehaviour
{
    public float speed = 50f;          // How fast the laser moves
    public float maxDistance = 100f;    // How far the laser travels
    public int damage = 10;              // How much damage it deals

    private Vector3 startPosition;
    private Vector3 moveForward;

    private void Start()
    {
        startPosition = transform.position;  // Store the starting point
        //transform.localRotation = Quaternion.Euler(90f, 0f, 0f) * transform.rotation;

    }

    private void Update()
    {
        // Move forward relative to the laser's local forward direction
        transform.localPosition += transform.up * speed * Time.deltaTime;


        // Destroy if it exceeds max distance
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
       
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, 0f, 0);
        }

     
        Destroy(gameObject);
    }
}
