using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Animator animator;
    private PlayerInputManager inputManager;
    private PlayerMovementController movementController;
    private PlayerInteractionController interactionController;

    // CameraManager cameraManager;

    // public bool isInteracting;

    private void Awake()
    {
        // animator = GetComponent<Animator>();
        inputManager = GetComponent<PlayerInputManager>();
        // cameraManager = FindObjectOfType<CameraManager>();
        movementController = GetComponent<PlayerMovementController>();
        interactionController = GetComponent<PlayerInteractionController>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        interactionController.HandleInteraction();
    }

    private void FixedUpdate()
    {
        movementController.HandleAllMovement();
    }

    private void LateUpdate()
    {
        // cameraManager.HandleAllCameraMovement();

        // isInteracting = animator.GetBool("isInteracting");
        // playerMovementHandler.isJumping = animator.GetBool("isJumping");
        // animator.SetBool("isGrounded", playerMovementHandler.isGrounded);
    }
}
