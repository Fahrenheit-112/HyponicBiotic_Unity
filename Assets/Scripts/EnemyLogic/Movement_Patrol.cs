using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Author:  Fouche', Els
 * Updated: 04/28/2024
 * Notes:   Basic enemy movement logic. 
 *            TODO: Add variation, pausing, random searching.
 *            TODO: Allow player pursuit logic to take over. 
 */
public class Movement_Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int patrolIndex = 0;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        NextPatrolPos();
    }

    private void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            NextPatrolPos();
        }    
    }

    private void NextPatrolPos()
    {
        navMeshAgent.SetDestination(patrolPoints[patrolIndex].position);

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    public void BreakPatrol() { this.enabled = false; }
    public void ResumePatrol() 
    {
        this.enabled = true;
        navMeshAgent.SetDestination(patrolPoints[patrolIndex].position);
    }
}
