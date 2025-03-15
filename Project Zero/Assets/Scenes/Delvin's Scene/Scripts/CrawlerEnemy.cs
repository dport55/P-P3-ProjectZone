using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CrawlerEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [SerializeField] Transform headPos;
    [SerializeField] float HP;
    [SerializeField] int animTransSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;

    [SerializeField] GameObject attack;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRate;
    PlayerController2 player;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] growl;
    [Range(0, 1)][SerializeField] float audgrowlVol;

    public Collider attackCol;

    Color colorOrig;
    float attackTimer;
    float roamTimer;
    float angleToPlayer;
    float growlTimer; // Added cooldown for growl sound
    float stoppingDisOrig;
    Vector3 playerDir;
    Vector3 startingPos;
    bool playerInRange;
    bool isEngaged = false; // Flag to track if the enemy is engaged with the player

    void Start()
    {
        colorOrig = model.material.color;
        startingPos = transform.position;
        stoppingDisOrig = agent.stoppingDistance;
    }


    void Update()
    {
        if (player == null) return;

        attackTimer += Time.deltaTime;
        roamTimer += Time.deltaTime;
        growlTimer += Time.deltaTime; // Track growl cooldown

        if (player.isHiding) // If the player is hiding, go back to roaming
        {
            /*if (!isEngaged)*/ CheckRoam(); // Only roam if not already engaged
            return; // Skip the rest of the logic when the player is hiding
        }

        if (playerInRange && CanSeePlayer())
        {
            EngagePlayer(); // Keep engaging if the player is in range
        }
        else
        {
            if (!isEngaged) // If not engaged, then roam
            {
                CheckRoam();
            }
        }
    }

    void CheckRoam()
    {
        if (roamTimer > roamPauseTime && agent.remainingDistance < 0.1f)
        {
            Roam();
        }
    }

    void Roam()
    {
        roamTimer = 0;
        Vector3 randomPos = Random.insideUnitSphere * roamDistance + startingPos;
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, roamDistance, 1))
        {
            agent.SetDestination(hit.position);
            anim.Play("crawl");
        }
    }

    bool CanSeePlayer()
    {
        playerDir = player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                // Prevent growl spam
                if (growlTimer >= 2f) // 2-second cooldown
                {
                    aud.PlayOneShot(growl[Random.Range(0, growl.Length)], audgrowlVol);
                    growlTimer = 0;
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FaceTarget();
                }
                agent.stoppingDistance = stoppingDisOrig;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void EngagePlayer()
    {
        if (player == null) return;

        isEngaged = true; // Set engaged flag
        
        agent.SetDestination(player.transform.position);
        float distanceToPlayer = agent.remainingDistance;
        FaceTarget();

        if (distanceToPlayer > 10)
        {
            anim.Play("crawl_fast");
        }
        else if (distanceToPlayer > 5 && distanceToPlayer <= 10)
        {
            anim.Play("pounce");
        }
        else if (distanceToPlayer <= 3)
        {
            anim.Play("attack");
            
        }
        else
        {
            anim.Play("crawl_fast");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController2>(); // Assign player when detected
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isEngaged = false; // Reset engaged flag when the player exits
        }
    }

    public void SetTarget(Transform target)
    {
        if (target == null) return; // Prevent null reference errors

        agent.SetDestination(target.position);

        anim.Play("crawl_fast");
        playerInRange = true;
        isEngaged = true; // Set engaged flag
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());

        //DisableCollider();

        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    public void EnableCollider()
    {
        attackCol.enabled = true;
    }

    public void DisableCollider()
    {
        attackCol.enabled = false;
    }

    void FaceTarget()
    {
        if (player == null) return;

        // Get direction from enemy to player
        playerDir = player.transform.position - transform.position;
        playerDir.y = 0; // Ensure no vertical rotation

        // If the player is at the same position, don't rotate
        if (playerDir.sqrMagnitude > 0.1f)
        {
            // Calculate the desired rotation towards the player
            Quaternion targetRotation = Quaternion.LookRotation(playerDir);

            // Smoothly rotate towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * faceTargetSpeed);
        }
    }
}
