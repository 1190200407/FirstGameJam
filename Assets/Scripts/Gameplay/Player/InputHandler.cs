using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public Vector2 movementInput;
    public float scroll;

    PlayerControls inputActions;
    PlayerMovement playerMovement;

    public bool jumpFlag;
    public bool switchFlag;
    public bool itemFlag;
    public bool interactionFlag;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.Movement.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Movement.Jump.performed += OnJumpInput;
            inputActions.Movement.JumpUp.performed += OnJumpUpInput;
            inputActions.Movement.SwitchState.performed += inputActions => switchFlag = true;
            inputActions.Item.UseItem.performed += inputActions => itemFlag = true;
            inputActions.Item.SelectItem.performed += inputActions => scroll = inputActions.ReadValue<Vector2>().y;
            inputActions.Interaction.Interact.performed += inputActions => interactionFlag = true;
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnJumpInput(InputAction.CallbackContext inputActions)
    {
        jumpFlag = true;
        playerMovement.OnJumpInput();
    }

    private void OnJumpUpInput(InputAction.CallbackContext inputActions)
    {
        jumpFlag = false;
        playerMovement.OnJumpUpInput();
    }
}
