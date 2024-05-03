using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInputs controls;

    private float moveSpeed = 5f;
    private float gravity = -9.81f;
    private float jumpHeight = 2.4f;

    //private CharacterController controller;

    private Vector3 velocity;
    private Vector3 _moveDirection;

    private Vector2 move;

    private bool isGrounded;

    public Rigidbody rb;
    
    public Transform ground;

    public float distanceToGround = 0.4f;

    public LayerMask groundMask;

    public InputActionReference _move;


    private void Awake()
    {
        controls = new PlayerInputs();
        controls = GetComponent<PlayerInputs>();
    }

    private void Update()
    {

        _moveDirection = _move.action.ReadValue<Vector3>();
        Grav();
        PlayerMovement();
        Jump();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(x: _moveDirection.x * moveSpeed, y: _moveDirection.y * moveSpeed, z: _moveDirection.z * moveSpeed);
    }


    private void Grav()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        //controls.Move(velocity * Time.deltaTime);

    }

    private void PlayerMovement()
    {
        

        //rb.velocity = new Vector3(x: _moveDirection.x * moveSpeed, y: _moveDirection.y * moveSpeed, z: _moveDirection.z * moveSpeed);
    }

    private void Jump()
    {

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


}
