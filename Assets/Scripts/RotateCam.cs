using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCam : MonoBehaviour
{

    private float rotX = 0f;
    private float rotY = 0f;
    private Vector3 origRot;
    private float dir1 = -1;
    public Transform camTransform;
    private Touch initTouch;
    public Transform lookAt;
    private Vector3 dir;
    private Quaternion rotation;
    public float rotSpeed = 0.5f;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        camTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {

        rotation = Quaternion.Euler(0, rotY * speed, 0);
        dir = new Vector3(0, 0, -3);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
        foreach (Touch touch in Input.touches)
        {
            float deltaX = initTouch.position.x - touch.position.x;
            rotY += deltaX * Time.deltaTime * rotSpeed * dir1;
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject())
            {
                initTouch = touch;
            }

            else if (touch.phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject())
            {
                initTouch = new Touch();
            }
        }


    }

}
