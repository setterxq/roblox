using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public BlockType blockType = BlockType.Normal;

    public Transform TargetForTeleport;

    private void Start()
    {
        if(blockType == BlockType.Damage || blockType == BlockType.Damage)
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
        if (blockType == BlockType.Fake)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(blockType == BlockType.Damage && other.gameObject.tag == "Player")
        {
            other.GetComponent<SC_FPSController>().UIControl.Lose();
        }

        if (blockType == BlockType.Stairs && other.gameObject.tag == "Player")
        {
            other.transform.gameObject.GetComponent<SC_FPSController>().InStair = true;
        }

        if (blockType == BlockType.Teleport && other.gameObject.tag == "Player")
        {
            CharacterController controller = null;
            controller = other.GetComponent<CharacterController>();
            controller.enabled = false;
            other.gameObject.transform.position = TargetForTeleport.position;
            controller.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (blockType == BlockType.Stairs && other.gameObject.tag == "Player")
        {
            other.transform.gameObject.GetComponent<SC_FPSController>().InStair = false;
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
