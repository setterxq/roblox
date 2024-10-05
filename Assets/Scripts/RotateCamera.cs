using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    private Vector2 lastTouchPosition;

    void Update()
    {
        // Если один палец коснулся экрана 
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // При начале касания запоминаем позицию 
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            // При движении пальца вращаем камеру 
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 deltaPosition = touch.position - lastTouchPosition;

                // Вращение вокруг вертикальной оси (по горизонтали) 
                transform.Rotate(Vector3.up, deltaPosition.x * rotationSpeed);

                // Вращение вокруг горизонтальной оси (по вертикали) 
                transform.Rotate(Vector3.left, deltaPosition.y * rotationSpeed);

                lastTouchPosition = touch.position;
            }
        }
    }

}
