using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController02 : MonoBehaviour
{

    PlayerInputs controls;
    PlayerInputs.MovementActions movement;

    

    
    Vector2 horizontalInput;

    private void Awake()
    {
        controls = new PlayerInputs();
        movement = controls.Movement;

        // movement.[action].performed += context => do something
        movement.Move.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

    }


    private void Update()
    {
        RecieveInput(horizontalInput);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void RecieveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }



}
