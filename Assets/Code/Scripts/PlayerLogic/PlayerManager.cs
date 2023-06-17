using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
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
        inputManager.HandleAllInputs();
        interactionController.HandleInteraction();
        playerCollisionController.enabled = true;
    }

    private void FixedUpdate()
    {
        movementController.HandleAllMovement();
    }
}
