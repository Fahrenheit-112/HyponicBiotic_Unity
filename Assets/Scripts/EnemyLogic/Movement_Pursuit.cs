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
    private GameObject target;
    private Vector3 targetPos;
    private bool playerSpotted = false;
    private bool isPursuing = false;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();    
    }
 
    private void Update() 
    {
        targetPos = target.transform.position;
        if (!navMeshAgent.pathPending) 
        {
            navMeshAgent.SetDestination(targetPos);
        }
    }

    public void PlayerFound() { playerSpotted = true; isPursuing = true; }
    public void PlayerLost() { playerSpotted = false; isPursuing = false; }
    public bool IsPursuing() { return isPursuing; }
    
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        Debug.Log(target);
        // navMeshAgent.SetDestination(target.transform.position);
    }

    public void StartPursuit() { this.enabled = true; }
    public void EndPursuit() { this.enabled = false; }
}
