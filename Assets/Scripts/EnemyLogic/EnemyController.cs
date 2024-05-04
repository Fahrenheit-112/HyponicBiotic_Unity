using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/28/2024
 * Notes:   This script handles enemy sight based on
 *          the distance to the player, a specified view cone,
 *          and raycasts that are only used when the previous 
 *          two conditions are met to determine if the player
 *          is visible. If the player is spotted, the enemy
 *          switches from patrol to pursuit. 
 */

public class EnemyController : MonoBehaviour
{
    [Header("Detect Player")]
    public float viewDistance = 20.0f;
    public float coneAngle = 45.0f;

    private GameObject player;
    private Vector3 distanceToPlayer;
    private Movement_Patrol patrol;
    private Movement_Pursuit pursuit;
    private bool pursuingPlayer = false;
    private Vector3 headRaycast = new Vector3(0,2,0);
    private Vector3 bodyRaycast = new Vector3(0,1,0);
    private Vector3 raycastOriginOffset = new Vector3(0,1.5f,0);
    private Vector3 raycastOrigin;

    private void Start()
    {
        viewDistance *= viewDistance;   // Square the inspector-set view distance to avoid sqrt function later
        player = GameObject.Find("Player"); // Fragile implementation, fix later
        Debug.Log("Player found: " + player);
        patrol = GetComponent<Movement_Patrol>();
        pursuit = GetComponent<Movement_Pursuit>();
    }

    /// <summary>
    /// Calls the CheckForPlayer() function to determine if
    /// the player is in sight. Starts the pursuit protocol if
    /// they are. 
    /// </summary>
    private void Update()
    {
        if (!pursuingPlayer && CheckForPlayer())
        {
            StartPursuit(player);
        }
    }

    public void StartPursuit(GameObject target) 
    {
        pursuingPlayer = true;
        patrol.BreakPatrol(); 
        pursuit.StartPursuit(target);
    }
    public void EndPursuit() 
    {
        pursuingPlayer = false;
        pursuit.EndPursuit(); 
        patrol.ResumePatrol(); 
    }

    /// <summary>
    /// Determines if the player is in sight in stages for efficiency.
    /// First, determines the distance to the player and only proceeds if
    /// the player is close enough. Second, it checks if the player is 
    /// within a view cone specified by coneAngle. Finally, three ray casts
    /// are used to determine if the head, body, or feet of the player are 
    /// visible. Returns true if the player is spotted by one of the ray 
    /// casts, returns false otherwise. 
    /// </summary>
    /// <returns></returns>
    public bool CheckForPlayer()
    {
        distanceToPlayer = (player.transform.position - transform.position);
        if (distanceToPlayer.sqrMagnitude < viewDistance)
        {
            if (Vector3.Angle(transform.forward, distanceToPlayer) < (coneAngle / 2.0f))
            {
                RaycastHit hit;
                raycastOrigin = transform.position + raycastOriginOffset;
                if (Physics.Raycast(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, out hit))
                {
                    Debug.DrawRay(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, Color.green, 0.1f);
                    return true;
                }
                else if (Physics.Raycast(raycastOrigin, player.transform.position + bodyRaycast - raycastOrigin, out hit))
                {
                    Debug.DrawRay(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, Color.green, 0.1f);
                    return true;
                }
                else if (Physics.Raycast(raycastOrigin, player.transform.position - raycastOrigin, out hit))
                {
                    Debug.DrawRay(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, Color.green, 0.1f);
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public bool IsPursuing() { return pursuingPlayer; }
    public float DistanceToPlayer() { return (player.transform.position - transform.position).sqrMagnitude; }
    public GameObject CurrTarget() { return player; }
}
