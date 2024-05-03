using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement01 : MonoBehaviour
{

    private PlayerInputs playerControls;

    private Rigidbody rb;

    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
    }



    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Movement.Jump.performed += Jump;
        playerControls.Movement.Move.performed += Move;

    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Movement.Jump.performed -= Jump;

    }


    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Jump");
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    private void Move(InputAction.CallbackContext context)
    {

       // rb.velocity = context.Get<Vector2>() * moveSpeed;

    }

}
