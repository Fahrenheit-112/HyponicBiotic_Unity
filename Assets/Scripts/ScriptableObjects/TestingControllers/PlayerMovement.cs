using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public PlayerInputs playerInputs;

    public Rigidbody rb;

    public float speed = 10f;
    public float runSpeed = 15f;
    public float jumpForce = 200f;

    private bool _isGrounded;

   

    private void Awake()
    {
        playerInputs = new PlayerInputs();

        playerInputs.Movement.Jump.started += _ => Jump(); 
    }

    private void Update()
    {
        float forward = playerInputs.Movement.Move.ReadValue<float>();

        Vector3 move = transform.forward * forward + transform.forward * forward;

        //move *= playerInputs.Movement.Run.ReadValue<float>() = 0 ? speed : runSpeed;
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }


    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    void Jump()
    {
        if (_isGrounded) 
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

}
