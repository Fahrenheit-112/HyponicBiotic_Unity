using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement02 : MonoBehaviour
{

    public Rigidbody rb;

    public float moveSpeed;

    private Vector3 _moveDirection;

    public InputActionReference move;

    public InputActionReference fire;


    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector3>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(x: _moveDirection.x * moveSpeed, y: _moveDirection.y * moveSpeed, z: _moveDirection.z * moveSpeed);  
    }

    private void OnEnable()
    {
        fire.action.started += Fire;
    }private void OnDisable()
    {
        fire.action.started -= Fire;
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        Debug.Log(message: "Fired");
    }

}
