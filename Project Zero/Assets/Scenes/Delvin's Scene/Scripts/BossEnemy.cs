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
    [SerializeField] ParticleSystem Frozeen;
    public Transform Spot;

    private bool isStunned = false;
    private bool playerInRange;
    private bool isWaiting = false;
    private bool canBeFrozen = true; // Cooldown tracking
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] growl;
    [Range(0, 1)][SerializeField] float audgrowlVol;
    float growlTimer;
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
        growlTimer += Time.deltaTime;

        if (GameManager.instance.playerScript.isHiding)
        {
            if (!isWaiting) MoveToRandomSpawnPoint();
            return;
        }

        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && !isStunned)
        {
            StartCoroutine(WaitBeforeNextMove());
        }

        if (playerInRange && CanSeePlayer() && !isStunned)
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
        if (spawnPoints.Length == 0 || isWaiting && isStunned) return;

        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        agent.SetDestination(randomSpawn.position);
        anim.Play("walk3");
    }

    bool CanSeePlayer()
    {
        if (!isStunned)
        {
            FaceTarget();
            Vector3 playerDir = (GameManager.instance.playerScript.transform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, playerDir);

            if (angleToPlayer < 60f)
            {
                if (growlTimer >= 2f) // 2-second cooldown
                {
                    aud.PlayOneShot(growl[Random.Range(0, growl.Length)], audgrowlVol);
                    growlTimer = 0;
                }
                if (Physics.Raycast(transform.position, playerDir, out RaycastHit hit))
                {
                    return hit.collider.CompareTag("Player");
                }
            }
        }
        return false;
    }

    void EngagePlayer()
    {
        if (GameManager.instance.playerScript == null) return;
        if (!isStunned)
        {
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
    }

    public void Stun(float duration)
    {
        if (canBeFrozen) // Check if freeze is allowed
        {
            StartCoroutine(StunRoutine(duration));
            StartCoroutine(FreezeCooldown()); // Start cooldown after freezing
        }
        else
        {
            Debug.Log("Freeze gun is on cooldown!");
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
        agent.isStopped = true; // Stop movement
        isStunned = true;
        canBeFrozen = false; // Prevent re-freezing

        anim.Play("rage"); // Play the rage animation
        StartCoroutine(FlashBlue()); // Start flashing effect

        yield return new WaitForSeconds(duration); // Wait for stun duration

        agent.isStopped = false; // Resume movement
        isStunned = false;
    }

    IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(5f); // Wait for cooldown
        canBeFrozen = true; // Allow freezing again
        Debug.Log("Boss can be frozen again!");
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

    public void TakeDamage(float damage, float Freeze, float O2)
    {
        if (Freeze > 0)
        {
            Stun(Freeze); // Apply freeze effect if allowed
        }
        else if (damage > 1)
        {
            HP -= damage;
            StartCoroutine(FlashRed());
        }
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

    IEnumerator FlashBlue()
    {
        model.material.color = Color.blue;
        ParticleSystem effect = Instantiate(Frozeen, Spot);
        Destroy(effect.gameObject, GameManager.instance.playerScript.freezeTime);
        yield return new WaitForSeconds(5f);
        model.material.color = colorOrig;
        Frozeen.Stop();
    }
}