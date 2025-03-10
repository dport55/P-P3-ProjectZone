using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [SerializeField] Transform[] roamPoints;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float stunDuration = 2f;

    public PlayerController player;
    private int currentRoamIndex = 0;
    private bool isStunned = false;
    private bool playerInRange;

    void Start()
    {
        RoamToNextPoint();
    }

    void Update()
    {
        if (!isStunned)
        {
            if (playerInRange && CanSeePlayer())
            {
                EngagePlayer();
            }
            else if (agent.remainingDistance < 0.5f)
            {
                RoamToNextPoint();
            }
        }
    }

    void RoamToNextPoint()
    {
        if (roamPoints.Length == 0) return;
        agent.SetDestination(roamPoints[currentRoamIndex].position);
        anim.Play("Walk3");
        currentRoamIndex = (currentRoamIndex + 1) % roamPoints.Length;
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
        agent.SetDestination(player.transform.position);
        float distance = agent.remainingDistance;

        if (distance <= 1)
        {
            anim.Play(Random.Range(0, 2) == 0 ? "attack2RLSpike" : "attack3");
        }
        else
        {
            anim.Play("Walk3");
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
        RoamToNextPoint();
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

