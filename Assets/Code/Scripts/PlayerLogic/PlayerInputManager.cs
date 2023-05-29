using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    public static PlayerInputManager Instance { get; private set; }

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;
    public event EventHandler OnPauseAction;

    private void OnEnable()
    {
        Instance = this;
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            playerInput.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerInput.PlayerInteraction.Interact.performed += Interact_performed;
            playerInput.PlayerInteraction.InteractAlt.performed += InteractAlt_performed;
            playerInput.PlayerUI.Pause.performed += Pause_performed;
        }

        playerInput.Enable();
    }

    private void OnDestroy()
    {
        playerInput.PlayerInteraction.Interact.performed -= Interact_performed;
        playerInput.PlayerInteraction.InteractAlt.performed -= InteractAlt_performed;
        playerInput.PlayerUI.Pause.performed -= Pause_performed;

        playerInput.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlt_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAltAction?.Invoke(this, EventArgs.Empty);
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
