using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [SerializeField] Transform[] spawnPoints; // Array of spawn points
    [SerializeField] float HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float stunDuration = 2f;
    [SerializeField] int roamPauseTime;
    [SerializeField] float roamTimer = 0f;

    private bool isStunned = false;
    private bool playerInRange;
    private bool isWaiting = false;

    public Collider attackCol1;
    public Collider attackCol2;

    Color colorOrig;

    void Start()
    {
        colorOrig = model.material.color;
        MoveToRandomSpawnPoint();
    }

    void Update()
    {
        roamTimer += Time.deltaTime;

        if (GameManager.instance.playerScript.isHiding)
        {
            if (!isWaiting) MoveToRandomSpawnPoint();
            return;
        }

        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(WaitBeforeNextMove());
        }

        if (playerInRange && CanSeePlayer())
        {
            EngagePlayer();
        }
    }

    IEnumerator WaitBeforeNextMove()
    {
        isWaiting = true;
        anim.Play("idle3");
        yield return new WaitForSeconds(roamPauseTime);
        isWaiting = false;
        roamTimer = 0f;
        MoveToRandomSpawnPoint();
    }

    void MoveToRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0 || isWaiting) return;

        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        agent.SetDestination(randomSpawn.position);
        anim.Play("walk3");
    }

    bool CanSeePlayer()
    {
        FaceTarget();
        Vector3 playerDir = (GameManager.instance.playerScript.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, playerDir);

        if (angleToPlayer < 60f)
        {
            if (Physics.Raycast(transform.position, playerDir, out RaycastHit hit))
            {
                return hit.collider.CompareTag("Player");
            }
        }
        return false;
    }

    void EngagePlayer()
    {
        if (GameManager.instance.playerScript == null) return;

        agent.SetDestination(GameManager.instance.playerScript.transform.position);
        float distance = agent.remainingDistance;

        if (distance > agent.stoppingDistance)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk3"))
            {
                anim.Play("walk3");
            }
        }
        else
        {
            anim.Play("attack2RLSpike");
        }
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunRoutine(duration));
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
        }
    }

    IEnumerator StunRoutine(float duration)
    {
       
        agent.isStopped = true;
        anim.Play("Rage");
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
        isStunned = false;
        MoveToRandomSpawnPoint();
    }

    public void EnableCollider()
    {
        attackCol1.enabled = true;
        attackCol2.enabled = true;
    }

    public void DisableCollider()
    {
        attackCol1.enabled = false;
        attackCol2.enabled = false;
    }

    void FaceTarget()
    {
        if (GameManager.instance.playerScript == null) return;

        Vector3 direction = (GameManager.instance.playerScript.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * faceTargetSpeed);
    }

    public void SetTarget(Transform target)
    {
        if (target == null) return;

        agent.SetDestination(target.position);
        anim.Play("run2");
        playerInRange = true;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        StartCoroutine(FlashRed());
        agent.SetDestination(GameManager.instance.player.transform.position);

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
}