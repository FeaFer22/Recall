using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionController : MonoBehaviour, IKitchenObjectParent
{
    private PlayerMovementController movementController;
    private PlayerCollisionController collisionController;
    private PlayerInputManager inputManager;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private Transform kitchenObjectHoldPoint;

  

    public static PlayerInteractionController Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
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
        inputManager.OnInteractAltAction += inputManager_OnInteractionAltAction;
    }

    private void inputManager_OnInteractionAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void inputManager_OnInteractionAltAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }


    public void HandleInteraction()
    {
        if (collisionController.HandleInteractionCollision(movementController.moveDirection, counterLayerMask, movementController.lastInteractionDirection))
        {
            if (collisionController.raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
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

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
