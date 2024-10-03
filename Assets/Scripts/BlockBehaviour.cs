using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public BlockType blockType = BlockType.Normal;


}

public enum BlockType
{
    Damage,
    Fake,
    Normal,
    Stairs
}
