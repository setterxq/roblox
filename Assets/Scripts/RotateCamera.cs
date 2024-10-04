using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraSensitivity;

    int leftFingerID, rightFingerID;
    float halfScreenWidth;

    Vector2 lookInput;
    float cameraPitch;

    void Start()
    {
        leftFingerID = -1;
        rightFingerID = -1;

        halfScreenWidth = Screen.width / 2;
    }

    void Update()
    {
        GetTouchInput();

        if (rightFingerID != -1)
        {
            LookAround();
        }
    }

    void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && leftFingerID == -1)
                    {
                        leftFingerID = t.fingerId;
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerID == -1)
                    {
                        rightFingerID = t.fingerId;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == leftFingerID)
                    {
                        leftFingerID = -1;
                    }
                    else if (t.fingerId == rightFingerID)
                    {
                        rightFingerID = -1;
                    }
                    break;

                case TouchPhase.Moved:

                    if (t.fingerId == rightFingerID)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    break;

                case TouchPhase.Stationary:

                    if (t.fingerId == rightFingerID)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }

    void LookAround()
    {
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        transform.Rotate(transform.up, lookInput.x);
    }

}
