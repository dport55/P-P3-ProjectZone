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
    PlayerController player;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] growl;
    [Range(0, 1)][SerializeField] float audgrowlVol;

    [SerializeField] Collider attackCol;

    Color colorOrig;
    float attackTimer;
    float roamTimer;
    float angleToPlayer;
    float growlTimer; // Added cooldown for growl sound
    float stoppingDisOrig;
    Vector3 playerDir;
    Vector3 startingPos;
    bool playerInRange;

    void Start()
    {
        colorOrig = model.material.color;
        startingPos = transform.position;
        stoppingDisOrig = agent.stoppingDistance;
        //agent.stoppingDistance = 1;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        roamTimer += Time.deltaTime;
        growlTimer += Time.deltaTime; // Track growl cooldown

        if (playerInRange && player != null && CanSeePlayer())
        {
            EngagePlayer();
        }
        else
        {
            CheckRoam();
        }

        if (agent.isOnNavMesh)
        {
            Debug.Log("Agent is on NavMesh. Current position: " + transform.position + ", Destination: " + agent.destination);
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

        agent.SetDestination(player.transform.position);
        float distanceToPlayer = agent.remainingDistance;

        if (distanceToPlayer > 10)
        {
            anim.Play("crawl_fast");
        }
        else if (distanceToPlayer > 5 && distanceToPlayer <= 10)
        {
            anim.Play("pounce");
        }
        else if (distanceToPlayer <= 1)
        {
            anim.Play("attack");
            Attack();
        }
        else
        {
            anim.Play("crawl_fast");
        }
    }

    void Attack()
    {
        if (attackTimer >= attackRate)
        {
            attackTimer = 0;
            Instantiate(attack, attackPos.position, transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>(); // Assign player when detected
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void SetTarget(Transform target)
    {
        if (target == null) return; // Prevent null reference errors

        agent.SetDestination(target.position);
        
        anim.Play("crawl_fast");
        playerInRange = true;

        Debug.Log("Crawler: Target set to " + target.name);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(FlashRed());

        attackOff();

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

    public void attackOn()
    {
        attackCol.enabled = true;
    }

    public void attackOff()
    {
        attackCol.enabled = false;
    }

    void FaceTarget()
    {
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keep rotation on the horizontal plane

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * faceTargetSpeed);
    }
}
