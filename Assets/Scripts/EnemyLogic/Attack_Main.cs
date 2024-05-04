using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

/* Author:  Fouche', Els
 * Updated: 05/04/2024
 * Notes:   This script handles the primary attack logic
 *          for enemies. Additional scripts can be generated
 *          for more specialized attack situations such as
 *          AoE's, grapples, etc. This script should serve
 *          both as a basic attack and as the jumping off
 *          point for specialized additions.
 */

[RequireComponent(typeof(EnemyController))]
public class Attack_Main : MonoBehaviour
{
    [Header("Attack Stats")]
    public int dmgOfAtk = 1;
    public float rangeOfAtk = 2.0f;
    public float atkDelay = 1.5f;
    // public float knockBack = 5.0f;

    private GameObject target;
    private TagManager targetTags;
    private EnemyController enemyController;
    private float atkDelayTimer = 0.0f;

    /// <summary>
    /// Initializes script by attempting to grab the enemy controller. Attempts
    /// to find the script by moving up the hierarchy if not found at the same
    /// level as this script. We square the attack range due to using .sqrMagnitude
    /// to avoid the costly square root function when determining distance to player.
    /// </summary>
    private void Start()
    {
        if (!(enemyController = GetComponent<EnemyController>()))
        {
            Transform parent;
            while (parent = transform.parent)
            {
                try
                {
                    enemyController = parent.gameObject.GetComponent<EnemyController>(); 
                } catch { Debug.Log("EnemyController not found. Moving up hierarchy."); }
            }

            if (!enemyController)
            {
                Debug.Log("Unable to find EnemyController: disabling this script.");
                this.enabled = false;
            }
        }

        target = GameObject.Find("Player");

        rangeOfAtk *= rangeOfAtk;
        rangeOfAtk += GetComponent<NavMeshAgent>().stoppingDistance;
    }

    /// <summary>
    /// Initiates an attack based on atkDelay cooldown (frame dependent). 
    /// Only initiates the attack if the enemy is currently pursuing the
    /// target and if the distance to the target is less than the range
    /// of the attack. Resets the attack delay after attacking.
    /// </summary>
    private void Update()
    {
        atkDelayTimer = Mathf.Clamp(atkDelayTimer + Time.deltaTime, 0.0f, atkDelay);
        if (enemyController.IsPursuing() && 
            enemyController.DistanceToPlayer() <= rangeOfAtk)
        {
            if (atkDelayTimer > atkDelay - 0.01f)
            {
                atkDelayTimer = 0.0f;
                Attack();
            }
        }
    }

    /// <summary>
    /// Initiates an attack by sending damage to the PlayerHealth script.
    /// This will need to be modified for greater flexibility later. 
    /// First, checks if our target is valid. Then checks if the
    /// target has a tag manager. Then checks to see if the the target
    /// is the player. 
    /// </summary>
    public void Attack()
    {
        if ((targetTags = target.GetComponent<TagManager>()) &&
            (targetTags.baseType == TagManager.BaseType.Player))
        {
            target.GetComponent<PlayerHealth>().DamagePlayer(dmgOfAtk);
        }
    }
}