using UnityEngine;
using System.Collections;

public class ScreamerEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] screamSound;
    [SerializeField] float screamRadius = 15f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float HP;
    [SerializeField] float roamRadius = 10f;
    [SerializeField] float roamSpeed = 2f;
    [SerializeField] float roamWaitTime = 2f;
    [SerializeField] float rotationSpeed = 5f; // Speed of turning
    public BossEnemy boss;

    bool hasScreamed = false;
    Vector3 roamTarget;
    bool isRoaming = true;

    void Start()
    {
        StartCoroutine(Roam());
    }

    void Update()
    {
        if (isRoaming)
        {
            MoveToRoamTarget();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasScreamed && other.CompareTag("Player"))
        {
            isRoaming = false;
            GameManager.instance.playerScript = other.GetComponent<PlayerController>();
            StartCoroutine(Scream());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasScreamed = false; // Reset so it can scream again if the player re-enters
        }
    }

    IEnumerator Roam()
    {
        while (isRoaming)
        {
            ChooseNewRoamTarget();
            anim.Play("Creep|Walk2_Action"); // Play walking animation
            yield return new WaitForSeconds(roamWaitTime);
        }
    }

    void ChooseNewRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Keep it on the same height
        roamTarget = randomDirection;
    }

    void MoveToRoamTarget()
    {
        if (Vector3.Distance(transform.position, roamTarget) > 0.5f)
        {
            // Smoothly rotate toward the movement direction
            Vector3 direction = (roamTarget - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Move forward
            transform.position = Vector3.MoveTowards(transform.position, roamTarget, roamSpeed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(Roam()); // Restart roaming when reaching the target
        }
    }

    IEnumerator Scream()
    {
        hasScreamed = true;
        anim.Play("Creep|Roar_Action"); // Play scream animation
        oneScream(); // Play scream sound
        yield return new WaitForSeconds(1.9f);
        AlertNearbyEnemies();
        anim.Play("Creep|JumpOut_Action"); // Play jump out animation
        Destroy(gameObject); // Destroy after screaming
    }

    void AlertNearbyEnemies()
    {
        if (GameManager.instance.playerScript == null) return; // Prevents null reference error

        Collider[] enemies = Physics.OverlapSphere(transform.position, screamRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            if (enemy.TryGetComponent(out CrawlerEnemy crawler))
            {
                crawler.SetTarget(transform); // Alert crawler
            }

            if (enemy.TryGetComponent(out BossEnemy boss))
            {
                boss.SetTarget(GameManager.instance.playerScript.transform); // Alert boss
            }
        }
    }

    public void oneScream()
    {
        aud.PlayOneShot(screamSound[Random.Range(0, screamSound.Length)], 2f);
    }

    public void TakeDamage(float damage, float Freeze, float O2)
    {
        HP -= damage;
        StartCoroutine(Scream());
        AlertNearbyEnemies();
    }
}