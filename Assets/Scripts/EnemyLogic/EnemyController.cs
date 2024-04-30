using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/28/2024
 * Notes:   This script handles detection logic and
 *          determines whether the enemy is in patrol mode
 *          or pursuit mode based on having seen the player.
 *          It initiates attacks when close to the player.
 */

public class EnemyController : MonoBehaviour
{
    [Header("Detect Player")]
    public float viewDistance = 20.0f;
    public float coneAngle = 45.0f;
    [Header("Pursue Player")]
    public int checkForPlayer = 5;
    public float timeBetweenChecks = 1.0f;

    private GameObject player;
    private Vector3 distanceToPlayer;
    private Movement_Patrol patrol;
    private Movement_Pursuit pursuit;
    private bool playerInSight = false;
    private int playerNotFoundCount = 0;
    private bool pursuingPlayer = false;
    private bool huntingPlayer = false;
    private Vector3 headRaycast = new Vector3(0,2,0);
    private Vector3 bodyRaycast = new Vector3(0,1,0);
    private Vector3 raycastOriginOffset = new Vector3(0,1.5f,0);
    private Vector3 raycastOrigin;

    private void Start()
    {
        viewDistance *= viewDistance;   // Square the inspector-set view distance to avoid sqrt function later
        player = GameObject.Find("Player"); // Fragile implementation
        Debug.Log("Player found: " + player);
        patrol = GetComponent<Movement_Patrol>();
        pursuit = GetComponent<Movement_Pursuit>();
    }

    private void Update()
    {
        // Keep track of the distance between player and enemy. sqrMagnitude leaves off sqrt function
        // (it's slow) so our viewDistance is squared at the start to create a valid comparison. 
        distanceToPlayer = (player.transform.position - transform.position);
        if (distanceToPlayer.sqrMagnitude < viewDistance)
        {
            // Check to see if the player is within a view cone based on coneAngle
            if (Vector3.Angle(transform.forward, distanceToPlayer) < (coneAngle / 2.0f))
            {
                RaycastHit hit;
                raycastOrigin = transform.position + raycastOriginOffset;
                // First of 3 raycasts, checks if main body of player visible
                // These raycasts need to originate from the enemy's head area
                // They need to target the player's head, left & right pectoral region, and center of mass
                if (Physics.Raycast(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, out hit))
                {
                    if (!pursuingPlayer) 
                    {
                        StartPursuit(player);
                    } 
                    Debug.DrawRay(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, Color.green, 0.1f);
                    // Debug.Log("Distance to player: " + distanceToPlayer);
                    // Player spotted, start pursuit
                } else if (Physics.Raycast(raycastOrigin, player.transform.position + bodyRaycast - raycastOrigin, out hit))
                {
                    Debug.DrawRay(raycastOrigin, player.transform.position + bodyRaycast - raycastOrigin, Color.green, 0.1f);
                    // Debug.Log("Distance to player: " + distanceToPlayer);
                } else if (Physics.Raycast(raycastOrigin, player.transform.position - raycastOrigin, out hit))
                {
                    Debug.DrawRay(raycastOrigin, player.transform.position - raycastOrigin, Color.green, 0.1f);
                    // Debug.Log("Distance to player: " + distanceToPlayer);
                }
            }
        }

    }

    /// <summary>
    /// This script is designed to allow the enemy to pursue the player
    /// for several seconds (default 5) with periodic checks to determine
    /// if the player is visible to the enemy or not. 
    /// </summary>
    private void HuntForPlayer()
    {
        if (!playerInSight)
        {
            playerNotFoundCount++;
            if (playerNotFoundCount < checkForPlayer)
            {
                // Wait, then start from the top 
                StartCoroutine("IsPlayerVisible");
            } else
            {
                EndPursuit();
            }
        } else
        {
            // Player was spotted during check
            // Reset number of checks and allow this function 
            // to be restarted the next time the player is lost. 
            playerNotFoundCount = 0;
            huntingPlayer = false;
        }
    }

    private IEnumerator IsPlayerVisible()
    {
        yield return new WaitForSeconds(timeBetweenChecks);
        HuntForPlayer();
    }

    private void StartPursuit(GameObject target) 
    {
        pursuingPlayer = true; 
        patrol.BreakPatrol(); 
        pursuit.StartPursuit();
        pursuit.SetTarget(target);
    }
    private void EndPursuit() 
    {
        huntingPlayer = false;
        pursuingPlayer = false;
        pursuit.EndPursuit(); 
        patrol.ResumePatrol(); 
    }
}
