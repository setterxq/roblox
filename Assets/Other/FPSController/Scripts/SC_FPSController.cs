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
    public UIBehaviour UIControl;
    public bool IsDesktop = false;
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
            return;
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

    public void RotateCameraDesktop()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }


    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    public void Silence(bool silence)
    {
        AudioListener.pause = silence;
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