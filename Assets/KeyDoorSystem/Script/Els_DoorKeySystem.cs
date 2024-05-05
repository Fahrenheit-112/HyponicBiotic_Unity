using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TagManager;

public class Els_DoorKeySystem : MonoBehaviour
{
    [Header("Door Container")]
    [Tooltip("Add doors being used. Use the Doorway_[color] gameobject.")]
    public List<GameObject> doors = new List<GameObject>();
    [Header("Keys Obtained - For Testing Purposes")]
    [Tooltip("This is public for easy display purposes only.")]
    public List<TagManager.KeyType> keys = new List<TagManager.KeyType>();

    private TagManager doorTag;
    private TagManager keyTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>() && 
            other.gameObject.GetComponent<TagManager>().doorType != TagManager.DoorType.None)
        {
            doorTag = other.gameObject.GetComponent<TagManager>();
            if (OpenDoorWithKey(doorTag.doorType))
            {
                other.gameObject.SetActive(false);
            }
        } else if (other.gameObject.GetComponent<TagManager>() &&
                   other.gameObject.GetComponent<TagManager>().keyType != TagManager.KeyType.None)
        {
            keyTag = other.gameObject.GetComponent<TagManager>();
            keys.Add(keyTag.keyType);
            other.gameObject.SetActive(false);
        }
    }

    private bool OpenDoorWithKey(TagManager.DoorType doorType)
    {
        if (keys != null)
        {
            foreach (TagManager.KeyType key in keys)
            {
                if ((int)key == (int)doorType)
                {
                    return true;
                }
            } 
        }
        
        return false;
    }
}
