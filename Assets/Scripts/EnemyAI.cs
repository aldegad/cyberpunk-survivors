using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 100f;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;

    // Patroling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange = 10f;

    // Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange = 1000f, attackRange = 1f;
    private bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        
        if (rb.velocity.magnitude < 0.02f && !agent.enabled) 
        {
            Debug.Log(rb.velocity.magnitude);
            agent.enabled = true;
        }
        if (agent.isActiveAndEnabled)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                ProjectileMoveScript projectile = collision.gameObject.GetComponent<ProjectileMoveScript>();
                TakeDamage(projectile.damage);
                TakeKnockback(collision, projectile.knockback);
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        { 
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        // make sure enemy doesn't move
        agent.SetDestination(transform.position);

        // transform.LookAt(player);

        if (!alreadyAttacked)
        { 
            // Attack code here
            
            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        { 
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void TakeKnockback(Collision collision, int knockback)
    {
        agent.enabled = false;
        Vector3 direction = transform.position - collision.transform.position;
        direction.y = 0;
        direction.Normalize();
        rb.AddForceAtPosition(direction * knockback, collision.transform.position, ForceMode.Impulse);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
