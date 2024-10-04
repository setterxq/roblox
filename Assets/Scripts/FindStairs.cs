using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStairs : MonoBehaviour
{
    public SC_FPSController controller;

    private void OnTriggerEnter(Collider other)
    {
        BlockBehaviour blockBehaviour = other.GetComponent<BlockBehaviour>();
        if(blockBehaviour != null && blockBehaviour.blockType == BlockType.Stairs)
        {
            //controller.InStair = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BlockBehaviour blockBehaviour = other.GetComponent<BlockBehaviour>();
        if (blockBehaviour != null && blockBehaviour.blockType == BlockType.Stairs)
        {
            //controller.InStair = false;
        }
    }
}
