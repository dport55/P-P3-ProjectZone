using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StaticEnemy : MonoBehaviour
{ 
    enum damageType { moving, stationary }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] float damageAmount;
    [SerializeField] float O2Damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    bool isDamaging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damageType.moving)
        {
            //rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.TakeDamage(0f, 0f, O2Damage);

        }

        if (type == damageType.moving)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == damageType.stationary)
        {
            if (!isDamaging)
                StartCoroutine(damageOther(dmg));
        }


    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.TakeDamage(0f, 0f, O2Damage);
        yield return new WaitForSeconds(2);
        isDamaging = false;

    }

}