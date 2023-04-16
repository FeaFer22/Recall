using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerMovementController movementController;


    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    public bool sprint_input;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            playerInput.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerInput.PlayerActions.Sprint.performed += i => sprint_input = true;
            playerInput.PlayerActions.Sprint.canceled += i => sprint_input = false;
        }

        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
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
