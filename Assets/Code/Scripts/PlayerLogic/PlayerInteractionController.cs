using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionController : NetworkBehaviour, IKitchenObjectParent
{
    public static PlayerInteractionController LocalInstance { get; private set; }

    public static event EventHandler OnAnyPlayerSpawned;
    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
    }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    private PlayerMovementController movementController;
    private PlayerCollisionController collisionController;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        collisionController = GetComponent<PlayerCollisionController>();
        //Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }


        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectedCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectedCallback(ulong clientId)
    {
        if(clientId == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
        }
    }

    private void Start()
    {
        PlayerInputManager.Instance.OnInteractAction += InputManager_OnInteractionAction;
        PlayerInputManager.Instance.OnInteractAltAction += InputManager_OnInteractionAltAction;
    }

    private void InputManager_OnInteractionAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void InputManager_OnInteractionAltAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
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

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
