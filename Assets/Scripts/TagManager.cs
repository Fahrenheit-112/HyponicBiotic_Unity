using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/28/2024
 * Notes:   Tag management system. Most gameobjects are required
 *          to have this if they interact. 
 */

public class TagManager : MonoBehaviour
{
    public enum BaseType
    {
        None,
        Player, 
        Enemy,
        Hazard
    }

    public enum EnemyType
    {
        None,
        Normal,
        Strong
    }

    public BaseType baseType;
    public EnemyType enemyType;
}
