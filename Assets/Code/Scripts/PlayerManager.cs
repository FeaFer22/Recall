using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Animator animator;
    PlayerInputManager inputManager;
    PlayerMovementController movementController;
    // CameraManager cameraManager;

    // public bool isInteracting;

    private void Awake()
    {
        // animator = GetComponent<Animator>();
        inputManager = GetComponent<PlayerInputManager>();
        // cameraManager = FindObjectOfType<CameraManager>();
        movementController = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
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
