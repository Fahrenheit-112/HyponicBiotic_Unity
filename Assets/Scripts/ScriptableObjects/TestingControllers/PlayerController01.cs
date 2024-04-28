using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController01 : MonoBehaviour
{

    private Rigidbody rb;

    private PlayerInputs playerInputs;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerInputs = new PlayerInputs();
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 myVector = context.ReadValue<Vector2>();
        rb.AddForce(new Vector3(myVector.x, 0, myVector.y) * 5f, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveVector = playerInputs.Movement.Move.ReadValue<Vector2>();
        rb.AddForce(new Vector3(moveVector.x, 0, moveVector.y) * 2f, ForceMode.Force);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

}
