using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    private PlayerInputManager inputManager;
    private PlayerMovementController movementController;
    private PlayerInteractionController interactionController;
    private PlayerCollisionController playerCollisionController;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        movementController = GetComponent<PlayerMovementController>();
        interactionController = GetComponent<PlayerInteractionController>();
        playerCollisionController = GetComponent<PlayerCollisionController>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        movementController.HandleAllMovement();
        inputManager.HandleAllInputs();
        interactionController.HandleInteraction();
        playerCollisionController.enabled = true;

    }
}
