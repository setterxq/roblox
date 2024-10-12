using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;

public class CameraMov : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraSensitivity;

    int leftFingerID, rightFingerID;
    float halfScreenWidth;

    Vector2 lookInput;
    float cameraPitch;
    bool _isDesktop;
    public SC_FPSController Controller;

    void Start()
    {
        leftFingerID = -1;
        rightFingerID = -1;

        halfScreenWidth = Screen.width / 2;

        _isDesktop = YandexGame.EnvironmentData.isDesktop;
    }

    void Update()
    {
        if (_isDesktop) return;
        GetTouchInput();

        if (rightFingerID != -1)
        {
            LookAround();
        }
    }

    bool IsPointerOverUIObject(Touch touch)
    {

        if (Input.touchCount > 0)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
        return false;
    }

    void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (IsPointerOverUIObject(t))  return; 

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
                        lookInput = t.deltaPosition * Controller.lookSpeed * Time.deltaTime;
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