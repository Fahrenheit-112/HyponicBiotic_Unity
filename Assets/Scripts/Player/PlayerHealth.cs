using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/* Author:  Fouche', Els
 * Updated: 05/04/2024
 * Notes:   This basic script handles player health. 
 *          Health and MaxHealth are public for access via inspector
 *          but should be modified via the mutators provided to allow for
 *          maxHealth & PlayerDeath checking & display updating. 
 */


public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 20;
    [Range(0,20)]
    public int health;
    [Tooltip("Health Display")]
    public TMP_Text healthText;
    public string healthSymbol;
    private string currHealth;

    /// <summary>
    /// Initialize display of player health. 
    /// </summary>
    private void Start()
    {
        UpdateHealth();
    }

    /// <summary>
    /// Current end-of-game state. If player dies, reload current
    /// scene to allow player to try again from beginning.
    /// This needs to be removed once we're out of the Stealth 
    /// unit testing. 
    /// </summary>
    private void PlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Designated mutator for player being damaged. 
    /// Updates the player's health, determines if they've
    /// died or updates the health display otherwise. 
    /// </summary>
    /// <param name="damage"></param>
    public void DamagePlayer(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            PlayerDeath();
        } else
        {
            UpdateHealth();
        }
    }

    /// <summary>
    /// Designated mutator for the player being healed.
    /// Updates the player's health, sets it to max
    /// allowable value if over that amount, and
    /// updates the display. 
    /// </summary>
    /// <param name="heal"></param>
    public void HealPlayer(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealth();
    }

    /// <summary>
    /// Health display controller. Ideally would
    /// be wrapped into a UIManager script instead. 
    /// Requires a canvas and TMPro Text asset
    /// assigned in the inspector. 
    /// </summary>
    private void UpdateHealth()
    {
        currHealth = "";
        for (int i = 0; i < health; i++)
        {
            currHealth += healthSymbol;
        }
        healthText.text = currHealth;
    }
}
