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

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<EnemyController>();
        this.enabled = false;
    }
 
    private void Update() 
    {
        if (target && !navMeshAgent.pathPending)
        {
            navMeshAgent.SetDestination(target.transform.position);
        }
    }
    

    public void StartPursuit(GameObject newTarget) 
    { 
        this.enabled = true;
        target = newTarget;
        navMeshAgent.SetDestination(target.transform.position);
        HuntForPlayer();
    }
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
        Debug.Log("HuntForPlayer() entered.");
        if (enemyController.CheckForPlayer())
        {
            numChecks = 0;
            StartCoroutine("PursuitDecay");
        } else
        {
            numChecks++;
            Debug.Log("numChecks = " + numChecks);
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

    private IEnumerator PursuitDecay()
    {
        yield return new WaitForSeconds(timeBetweenChecks);
        HuntForPlayer();
    }
}
