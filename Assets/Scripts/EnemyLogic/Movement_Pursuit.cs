using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Author:  Fouche', Els
 * Updated: 04/28/2024
 * Notes:   Enemy logic for pursuing the player when spotting them.
 */

public class Movement_Pursuit : MonoBehaviour
{
    [Header("Pursue Player")]
    public int checkForPlayer = 5;
    public float timeBetweenChecks = 1.0f;

    private GameObject target;
    private NavMeshAgent navMeshAgent;
    private EnemyController enemyController;
    private int numChecks = 0;

    /// <summary>
    /// Initialize components and disable this script.
    /// </summary>
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<EnemyController>();
        this.enabled = false;
    }
 
    /// <summary>
    /// If the enemy has a valid target, update the destination
    /// every frame so long as we've finished computing the previous
    /// destination. 
    /// </summary>
    private void Update() 
    {
        if (target && !navMeshAgent.pathPending)
        {
            navMeshAgent.SetDestination(target.transform.position);
        }
    }
    
    /// <summary>
    /// Called from EnemyController. Enables the script, sets the
    /// target, sets the navMeshAgent's destination to the target,
    /// and begins the HuntForPlayer() function set. 
    /// </summary>
    /// <param name="newTarget"></param>
    public void StartPursuit(GameObject newTarget) 
    { 
        this.enabled = true;
        target = newTarget;
        navMeshAgent.SetDestination(target.transform.position);
        HuntForPlayer();
    }

    /// <summary>
    /// Called from EnemyController when we've finished pursuing
    /// the player. We call this from EnemyController so that 
    /// EnemyController can reinitialize the enemy patrol 
    /// protocol simultaneously. 
    /// </summary>
    public void EndPursuit() 
    { 
        target = null; 
        this.enabled = false; 
    }

    /// <summary>
    /// This script is designed to allow the enemy to pursue the player
    /// for several seconds (default 5) with periodic checks to determine
    /// if the player is visible to the enemy or not. 
    /// </summary>
    private void HuntForPlayer()
    {
        if (enemyController.CheckForPlayer())
        {
            numChecks = 0;
            StartCoroutine("PursuitDecay");
        } else
        {
            numChecks++;
            if (numChecks >= checkForPlayer)
            {
                numChecks = 0;
                enemyController.EndPursuit();
            } else
            {
                StartCoroutine("PursuitDecay");
            }
        }
    }

    /// <summary>
    /// This function simply waits for timeBetweenChecks
    /// seconds before calling looping back into HuntForPlayer().
    /// </summary>
    /// <returns></returns>
    private IEnumerator PursuitDecay()
    {
        yield return new WaitForSeconds(timeBetweenChecks);
        HuntForPlayer();
    }
}
