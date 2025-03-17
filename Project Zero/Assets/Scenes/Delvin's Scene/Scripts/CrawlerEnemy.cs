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

    [SerializeField] float attackRate;

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
        if (GameManager.instance.playerScript == null) return;

        attackTimer += Time.deltaTime;
        roamTimer += Time.deltaTime;
        growlTimer += Time.deltaTime; // Track growl cooldown

        if (GameManager.instance.playerScript.isHiding)
        {
            CheckRoam();
            return;
        }

        if (playerInRange && CanSeePlayer())
        {
            EngagePlayer();
            FaceTarget(); // Ensure rotation happens when engaging
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            FaceTarget(); // Rotate even when stationary
        }
        else if (!isEngaged)
        {
            CheckRoam();
        }
    }

    void CheckRoam()
    {
        if (roamTimer > roamPauseTime && agent.remainingDistance < .01f || GameManager.instance.playerScript.HP <= 0f)
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
        playerDir = GameManager.instance.playerScript.transform.position - headPos.position;
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
        if (GameManager.instance.playerScript == null) return;

        isEngaged = true; // Set engaged flag
        
        agent.SetDestination(GameManager.instance.playerScript.transform.position);
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

        FaceTarget();

        agent.SetDestination(target.position);

        anim.Play("crawl_fast");
     
    }

    public void TakeDamage(float damage, float freeze, float O2)
    {
    
        HP -= damage;   
      
        StartCoroutine(FlashRed());
        
       
        //DisableCollider();

        if (GameManager.instance.playerScript != null)
        {
            agent.SetDestination(GameManager.instance.playerScript.transform.position);
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
        if (GameManager.instance.playerScript == null) return;

        Vector3 direction = (GameManager.instance.playerScript.transform.position - headPos.position).normalized;
        direction.y = 0; // Keep rotation only on the Y-axis

        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 90-degree rotation offset
        Quaternion correctedRotation = lookRotation * Quaternion.Euler(0, 95, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, correctedRotation, Time.deltaTime * faceTargetSpeed);

        //Debug.Log("Rotating towards player with offset: " + transform.rotation.eulerAngles);
    }

}
