using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    private Vector2 lastTouchPosition;

    void Update()
    {
        // ���� ���� ����� �������� ������ 
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // ��� ������ ������� ���������� ������� 
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            // ��� �������� ������ ������� ������ 
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 deltaPosition = touch.position - lastTouchPosition;

                // �������� ������ ������������ ��� (�� �����������) 
                transform.Rotate(Vector3.up, deltaPosition.x * rotationSpeed);

                // �������� ������ �������������� ��� (�� ���������) 
                transform.Rotate(Vector3.left, deltaPosition.y * rotationSpeed);

                lastTouchPosition = touch.position;
            }
        }
    }

}
