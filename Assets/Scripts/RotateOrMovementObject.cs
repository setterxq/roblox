using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOrMovementObject : MonoBehaviour
{
    public TypeMove type;

    public float speed;

    [Header("ForMovement")]
    public Transform target1;
    public Transform target2;

    private bool left = true;

    private void FixedUpdate()
    {
        if(type == TypeMove.Rotate)
        {
            transform.Rotate(transform.up, Time.deltaTime * speed);
        }
        else
        {
            if(left)
            {
                transform.position = Vector3.MoveTowards(transform.position, target1.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target1.transform.position) < 1)
                {
                    left = false;
                }
            }
            if (!left)
            {
                transform.position = Vector3.MoveTowards(transform.position, target2.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target2.transform.position) < 1)
                {
                    left = true;
                }
            }
        }


    }
}

public enum TypeMove
{
    Rotate,
    Movement
}
