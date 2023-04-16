using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput playerInput;


    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    public event EventHandler OnInteractAction;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            playerInput.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerInput.PlayerInteraction.Interact.performed += Interact_performed;
        }

        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }


    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
}
