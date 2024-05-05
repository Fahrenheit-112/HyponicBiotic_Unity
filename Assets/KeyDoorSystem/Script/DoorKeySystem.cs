using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeySystem : MonoBehaviour
{
    // 1. Make sure player has rigidbody as trigger

    // Ref to door which will be opened and key which will be collected
    public GameObject connectedKeyDoorObject;
    public GameObject keyObject;

    // Int variables for amount of keys collected and amount of keys needed to open the connected door
    public int keyCount;
    public int keyRequirement;

    private void Start()
    {
        keyCount = 0;
        keyRequirement = 1;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KeyTriggerRegion") // "If the player touches the Key object, increase their key count
        {
            keyObject.SetActive(false);
            keyCount++;
        }
        else if (other.tag == "KeyDoorTriggerRegion" && keyCount < keyRequirement)
        {
            Debug.Log("Not enough keys for door");
            // Here is where you would connect a UI text object or sound effect to give the player visual or audible feedback
            return;
        }
        else if (other.tag == "KeyDoorTriggerRegion" && keyCount >= keyRequirement)  // "If the player touches the KeyDoor with enough keys, open door
        {
            Debug.Log("Door opened with key(s)");
            // Here is where you would connect a UI text object or sound effect to give the player visual or audible feedback
            connectedKeyDoorObject.SetActive(false);    // Deactivate door object. In final game, this is where the door opening animation would play.
        }
    }
    
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Key") // "If the player touches the Key object, increase their key count
        {
            collision.gameObject.SetActive(false);
            keyCount++;
            Debug.Log("Key added");
        }
        else if (collision.gameObject.tag == "KeyDoor" && keyCount < keyRequirement)
        {
            Debug.Log("Not enough keys for door");
            // Here is where you would connect a UI text object or sound effect to give the player visual or audible feedback
            return;
        }
        else if (collision.gameObject.tag == "KeyDoor" && keyCount >= keyRequirement)  // "If the player touches the KeyDoor with enough keys, open door
        {
            Debug.Log("Door opened with key(s)");
            // Here is where you would connect a UI text object or sound effect to give the player visual or audible feedback
            connectedKeyDoorObject.SetActive(false);    // Deactivate door object. In final game, this is where the door opening animation would play.
        }
    }
    */
}
