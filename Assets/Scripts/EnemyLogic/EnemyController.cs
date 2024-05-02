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
        player = GameObject.Find("Player"); // Fragile implementation
        Debug.Log("Player found: " + player);
        patrol = GetComponent<Movement_Patrol>();
        pursuit = GetComponent<Movement_Pursuit>();
    }

    /// <summary>
    /// Keep track of the distance between player and enemy. sqrMagnitude leaves off sqrt function
    /// (it's slow) so our viewDistance is squared at the start to create a valid comparison. 
    /// 
    /// Check to see if the player is within a view cone based on coneAngle
    /// 
    /// First of 3 raycasts, checks if main body of player visible
    /// These raycasts need to originate from the enemy's head area
    /// They need to target the player's head, left & right pectoral region, and center of mass
    /// </summary>
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position);
        if (distanceToPlayer.sqrMagnitude < viewDistance && !pursuingPlayer)
        {
            if (Vector3.Angle(transform.forward, distanceToPlayer) < (coneAngle / 2.0f))
            {
                RaycastHit hit;
                raycastOrigin = transform.position + raycastOriginOffset;
                if (Physics.Raycast(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, out hit))
                {
                    StartPursuit(player);
                    Debug.DrawRay(raycastOrigin, player.transform.position + headRaycast - raycastOrigin, Color.green, 0.1f);
                } else if (Physics.Raycast(raycastOrigin, player.transform.position + bodyRaycast - raycastOrigin, out hit))
                {
                    StartPursuit(player);
                    Debug.DrawRay(raycastOrigin, player.transform.position + bodyRaycast - raycastOrigin, Color.green, 0.1f);
                } else if (Physics.Raycast(raycastOrigin, player.transform.position - raycastOrigin, out hit))
                {
                    StartPursuit(player);
                    Debug.DrawRay(raycastOrigin, player.transform.position - raycastOrigin, Color.green, 0.1f);
                }
            }
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

    public bool CheckForPlayer()
    {
        distanceToPlayer = (player.transform.position - transform.position);
        if (distanceToPlayer.sqrMagnitude < viewDistance && !pursuingPlayer)
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
}
