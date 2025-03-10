using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CrawlerEnemy : MonoBehaviour
{ 
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [SerializeField] Transform headPos;
    [SerializeField] int HP;
    [SerializeField] int animTransSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;


    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    public PlayerController player;

    Color colorOrig;

    float shootTimer;
    float roamTimer;
    float angleToPlayer;
    float stoppingDisOrig;

    Vector3 playerDir;
    Vector3 startingPos;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        //GameManager.instance.UpdateGameGoal(1);
        startingPos = transform.position;
        stoppingDisOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {

        float agentSpeed = agent.velocity.normalized.magnitude;
        float animCurSpeed = anim.GetFloat("speed");
        anim.SetFloat("speed", Mathf.MoveTowards(animCurSpeed, agentSpeed, Time.deltaTime * animTransSpeed));

        shootTimer += Time.deltaTime;

        if (agent.remainingDistance < .01f)
        {
            roamTimer += Time.deltaTime;
        }


        if (playerInRange && !CanSeePlayer())
        {
            CheckRoam();

        }

        else if (!playerInRange)
        {
            CheckRoam();
        }

    }

    void CheckRoam()
    {
        if (roamTimer > roamPauseTime && agent.remainingDistance < .01f || player.HP <= 0f)
        {
            Roam();
        }
    }

    void Roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDistance;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDistance, 1);
        agent.SetDestination(hit.position);

        Debug.Log(agent.remainingDistance);
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
                agent.SetDestination(player.transform.position);

                if (shootTimer >= shootRate)
                {
                    Shoot();
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
            agent.stoppingDistance = 0;
        }
    }
    void Movement()
    {

    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        StartCoroutine(FlashRed());
        agent.SetDestination(player.transform.position);

        if (HP <= 0)
        {
            //GameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void Shoot()
    {
        shootTimer = 0;

        Instantiate(bullet, shootPos.position, transform.rotation);


    }


}

