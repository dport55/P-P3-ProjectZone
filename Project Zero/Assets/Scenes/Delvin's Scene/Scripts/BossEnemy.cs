using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossEnemy : MonoBehaviour,IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [SerializeField] float roamRadius = 10f; // The radius within which the boss will roam
    [SerializeField] float HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float stunDuration = 2f;
    [SerializeField] int roamPauseTime; // Time in seconds to wait at each roam point before moving again
    [SerializeField] float roamTimer = 0f; // Timer to trigger the roaming

    public PlayerController2 player;
    private bool isStunned = false;
    private bool playerInRange;
    private bool isWaiting = false;

    public Collider attackCol1;
    public Collider attackCol2;

    Color colorOrig;

    void Start()
    {
        colorOrig = model.material.color;
        MoveToRandomRoamPoint();
    }

      void Update()
    {
        roamTimer += Time.deltaTime; // Increment the roam timer by the time elapsed since last frame

        if (player.isHiding) // If the player is hiding, go back to roaming
        {
            if (!isWaiting) MoveToRandomRoamPoint(); // Roam if not waiting
            return; // Skip the rest of the logic when the player is hiding
        }

        if (roamTimer >= roamPauseTime && !isWaiting)
        {
            StartCoroutine(WaitBeforeNextMove());
        }

        if (playerInRange && CanSeePlayer()) // Engage if player is in range
        {
            EngagePlayer();
        }
    }

    IEnumerator WaitBeforeNextMove()
    {
        isWaiting = true;
        anim.Play("idle3"); // Play idle animation while waiting
        yield return new WaitForSeconds(roamPauseTime); // Wait for the specified roamPauseTime
        isWaiting = false;
        roamTimer = 0f; // Reset the timer after roaming
        MoveToRandomRoamPoint(); // Move to the next random point after waiting
    }

    void MoveToRandomRoamPoint()
    {
        // Generate a random position within the roam radius
        Vector3 randomPos = Random.insideUnitSphere * roamRadius;
        randomPos += transform.position; // Offset by the current position to ensure it roams within the radius

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, roamRadius, NavMesh.AllAreas)) // Find a valid NavMesh position
        {
            agent.SetDestination(hit.position); // Set the random position as the destination
            anim.Play("walk3");
        }
    }

    bool CanSeePlayer()
    {
        FaceTarget();
        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, playerDir);

        if (angleToPlayer < 60f) // Only detect players within a 60-degree vision cone
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
        if (player == null) return;

        agent.SetDestination(player.transform.position);
        float distance = agent.remainingDistance;

        if (distance > agent.stoppingDistance) // Keep walking towards player
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk3"))
            {
                anim.Play("walk3");
            }
        }
        else // Attack if within range
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
        isStunned = true;
        agent.isStopped = true;
        anim.Play("Rage");
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
        isStunned = false;
        MoveToRandomRoamPoint();
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
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keep rotation on the horizontal plane

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * faceTargetSpeed);
    }

    public void SetTarget(Transform target)
    {
        if (target == null) return; // Prevent null reference errors

        agent.SetDestination(target.position);

        anim.Play("run2");
        playerInRange = true;
        //isEngaged = true; // Set engaged flag
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