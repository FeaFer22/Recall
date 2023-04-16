using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerCollisionController collisionController;
    private PlayerInputManager inputManager;
    public ClearCounterController selectedClearCounter;

    [SerializeField] private LayerMask counterLayerMask;

    public static PlayerInteractionController Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounterController selectedCounter;
    }


    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        collisionController = GetComponent<PlayerCollisionController>();
        inputManager = GetComponent<PlayerInputManager>();

        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }
        Instance = this;
    }
    private void Start()
    {
        inputManager.OnInteractAction += inputManager_OnInteractionAction;
    }

    private void inputManager_OnInteractionAction(object sender, System.EventArgs e)
    {
        if (selectedClearCounter != null)
        {
            selectedClearCounter.Interact();
        }
    }

    public void HandleInteraction()
    {
        if (collisionController.HandleInteractionCollision(movementController.moveDirection, counterLayerMask, movementController.lastInteractionDirection))
        {
            if (collisionController.raycastHit.transform.TryGetComponent(out ClearCounterController clearCounter))
            {
                if (clearCounter != selectedClearCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(ClearCounterController selectedCounter)
    {
        this.selectedClearCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedClearCounter });
    }
}