using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    //[HideInInspector] 
    public bool InStair = false;
    public bool Pause;

    public CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    public Joystick joystick;
    public GameObject JumpButton;
    public GameObject PauseButton;

    private float _saveGravity;
    public Rigidbody rb;
    public AudioSource Source;
    public UIBehaviour UIControl;
    public bool IsDesktop;
    private Touch touch;
    private bool isTouch;
    private bool _firstChangeTouch;

    private Vector2 lastTouchPosition;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        IsDesktop = YandexGame.EnvironmentData.isDesktop;

        if (IsDesktop)
        {
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            joystick.gameObject.SetActive(false);
            JumpButton.SetActive(false);
            PauseButton.SetActive(false);
        }

        _saveGravity = gravity;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (IsDesktop)
        {
            if (Pause) return;
            RotateCameraDesktop();
        }
        else
        {
            if (Input.touches.Length == 0 || Pause) return;
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (!IsPointerOverUIObject(Input.GetTouch(i)))
                {
                    RotateCameraMobile(Input.GetTouch(i));
                    break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isTouch = false;
        }

        // Player and Camera rotation
        if (canMove && !Pause)
        {
            Movement(); 
        }

        if(transform.position.y < -30)
        {
            UIControl.Lose();
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

    public void RotateCameraDesktop()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    public void RotateCameraMobile(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            lastTouchPosition = touch.position;
            _firstChangeTouch = true;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            
            Vector2 deltaPosition = touch.position - lastTouchPosition;

            if(deltaPosition.magnitude > 50)
            {
                lastTouchPosition = touch.position;
                deltaPosition = Vector3.zero;
                _firstChangeTouch = false;
            }

            
            transform.Rotate(Vector3.up, deltaPosition.x * lookSpeed / 6);

            playerCamera.gameObject.transform.Rotate(Vector3.left, deltaPosition.y * lookSpeed / 5);



            if (playerCamera.transform.eulerAngles.x > 80 && playerCamera.transform.eulerAngles.x < 150)
            {
                playerCamera.transform.eulerAngles = new Vector3(80, playerCamera.transform.eulerAngles.y, 0);
            }

            if (playerCamera.transform.eulerAngles.x < 280 && playerCamera.transform.eulerAngles.x > 200)
            {
                playerCamera.transform.eulerAngles = new Vector3(280, playerCamera.transform.eulerAngles.y, 0);
            }

            lastTouchPosition = touch.position;
        }
    }

    public void Movement()
    {
        if (Pause) return;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = false;
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

        if (joystick.gameObject.activeInHierarchy)
        {
            curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * joystick.Vertical : 0;
            curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * joystick.Horizontal : 0;
        }

        float movementDirectionY = moveDirection.y;

        if (!InStair)
        {
            gravity = _saveGravity;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }
        else
        {
            gravity = 0;
            rb.velocity = new Vector2(rb.velocity.x, (10));
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            Jump();
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
    }
}