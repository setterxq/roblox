using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public BlockType blockType = BlockType.Normal;

    private void Start()
    {
        if(blockType == BlockType.Damage)
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(blockType == BlockType.Damage && other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(blockType == BlockType.Stairs && collision.gameObject.tag == "Player")
        {
            
        }
    }
}

public enum BlockType
{
    Damage,
    Fake,
    Normal,
    Stairs,
    Teleport
}
