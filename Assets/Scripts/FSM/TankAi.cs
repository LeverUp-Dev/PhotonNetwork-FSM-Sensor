using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAi : MonoBehaviour
{
    GameObject player;
    Animator animator;
    Vector3 checkDirection;
    NavMeshAgent navMeshAgent;

    [SerializeField] Transform points;

    int currentTarget;
    float distanceFromTarget;
    Transform[] wayPoints;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        wayPoints = points.GetComponentsInChildren<Transform>();

        currentTarget = 1;
        navMeshAgent.SetDestination(wayPoints[currentTarget].position);
    }

    void FixedUpdate()
    {
        float currentDistance = Vector3.Distance(player.transform.position, transform.position);
        animator.SetFloat("distanceFromPlayer", currentDistance);

        checkDirection = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, checkDirection * 6, Color.red);

        if (Vector3.Distance(player.transform.position, transform.position) < 6)
            animator.SetBool("isPlayerVisible", true);
        else
            animator.SetBool("isPlayerVisible", false);

        distanceFromTarget = Vector3.Distance(wayPoints[currentTarget].position, transform.position);
        animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
    }

    public void SetNextPoint()
    {
        switch (currentTarget)
        {
            case 1:
                currentTarget = 2; 
                break;
            case 2:
                currentTarget = 1;
                break;
        }
        navMeshAgent.SetDestination(wayPoints[currentTarget].position);
    }
}
