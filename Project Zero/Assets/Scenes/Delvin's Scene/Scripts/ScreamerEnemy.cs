using UnityEngine;
using System.Collections;

public class ScreamerEnemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] screamSound;
    [SerializeField] float screamRadius = 15f;
    [SerializeField] LayerMask enemyLayer;

    bool hasScreamed = false;
    PlayerController player;

    void OnTriggerEnter(Collider other)
    {
        if (!hasScreamed && other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
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
        anim.Play("Roar_Action"); // Play scream animation

        if (screamSound.Length > 0)
        {
            aud.PlayOneShot(screamSound[Random.Range(0, screamSound.Length)], 1f);
        }
        else
        {
            Debug.LogWarning("ScreamerEnemy: No scream sounds assigned!");
        }

        AlertNearbyEnemies();
        yield return new WaitForSeconds(3f);
    }

    void AlertNearbyEnemies()
    {
        if (player == null) return; // Prevents null reference error

        Collider[] enemies = Physics.OverlapSphere(transform.position, screamRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            if (enemy.TryGetComponent(out CrawlerEnemy crawler))
            {
                crawler.SetTarget(player.transform);
            }
        }
    }
}