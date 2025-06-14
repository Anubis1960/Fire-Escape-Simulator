using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 2.5f;
    public Camera playerCamera;

    public bool IsSprinting { get; private set; }
    public bool IsJumping {  get; private set; }
    
    private float _currentSpeed;
    private float _xRotation;
    private float _yRotation;
    private bool _isGrounded;
    private PlayerDamage playerDamage;
    
    private Rigidbody _rigidbody;

    void Start()
    {
        playerDamage = GetComponent<PlayerDamage>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleRotation();
        HandleJump();
        HandleSprint();
    }

    void FixedUpdate() // Use FixedUpdate for physics-based movement
    {
        HandleMovement();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); 

        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 moveForce = transform.TransformDirection(direction) * _currentSpeed;
        
        // Apply movement (preserving Y velocity for gravity/jumping)
        _rigidbody.velocity = new Vector3(moveForce.x, _rigidbody.velocity.y, moveForce.z);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            _isGrounded = false;
            
            playerDamage.ReduceOxygen(5); // Oxygen loss on jumping
        }
    }

    void HandleSprint()
    {
        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        _currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) // Check if landing on ground
        {
            _isGrounded = true;
        }
    }
}