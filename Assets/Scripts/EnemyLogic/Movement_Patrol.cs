using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Author:  Fouche', Els
 * Updated: 04/28/2024
 * Notes:   Basic enemy movement logic.
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

    /// <summary>
    /// Determines if we're close to our patrol point before heading to the
    /// next patrol position. navMeshAgent stoppingDistance is added to the
    /// value we're checking to see if we're close enough so that we can 
    /// offset how far we stop from the player without messing up our patrol.
    /// </summary>
    private void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < (0.1f + navMeshAgent.stoppingDistance))
        {
            NextPatrolPos();
        }    
    }

    /// <summary>
    /// Update where we're heading, then increment our patrol index. 
    /// </summary>
    private void NextPatrolPos()
    {
        navMeshAgent.SetDestination(patrolPoints[patrolIndex].position);

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    /// <summary>
    /// Called from EnemyController to stop position updating
    /// in Update. 
    /// </summary>
    public void BreakPatrol() { this.enabled = false; }

    /// <summary>
    /// Called from EnemyController so that we resume our patrol
    /// after we're done pursuing. Currently a minor bug exists where, 
    /// due to how we index through our patrol, we resume the patrol 
    /// at the point after the one we had previously been heading to.
    /// </summary>
    public void ResumePatrol() 
    {
        this.enabled = true;
        navMeshAgent.SetDestination(patrolPoints[patrolIndex].position);
    }
}
