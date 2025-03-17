using UnityEngine;
using System.Collections;

public class ScreamerEnemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] screamSound;
    [SerializeField] float screamRadius = 15f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float HP;
    public BossEnemy boss;


    bool hasScreamed = false;
    PlayerController2 player;

    void OnTriggerEnter(Collider other)
    {
      
        if (!hasScreamed && other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController2>();
            StartCoroutine(Scream());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasScreamed = false; // Reset so it can scream again if player re-enters
        }
    }

    IEnumerator Scream()
    {
        hasScreamed = true;
        anim.Play("Creep|Roar_Action"); // Play scream animation
        oneScream(); // Play scream sound
        yield return new WaitForSeconds(1.9f);
        AlertNearbyEnemies();
        anim.Play("Creep|JumpOut_Action");
        Destroy(gameObject); // Destroy after screaming
    }

    void AlertNearbyEnemies()
    {
        if (player == null) return; // Prevents null reference error

        Collider[] enemies = Physics.OverlapSphere(transform.position, screamRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            bool alerted = false;

            if (enemy.TryGetComponent(out CrawlerEnemy crawler))
            {
                crawler.SetTarget(transform); // Alert crawler
                alerted = true;
            }

            if (enemy.TryGetComponent(out BossEnemy boss))
            {
                boss.SetTarget(player.transform); // Alert boss
                alerted = true;
            }

            if (alerted)
            {
                Debug.Log($"{enemy.name} has been alerted by {gameObject.name}!");
            }
        }
    }

    public void oneScream()
    {
        aud.PlayOneShot(screamSound[Random.Range(0, screamSound.Length)], 2f); 
    }

}