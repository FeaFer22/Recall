using System;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounterController : BaseCounter
{
    public event EventHandler OnPlayerGetObject;
    [SerializeField] private KitchenObjectScriptableObject kitchenObjectSO;

    public override void Interact(PlayerInteractionController interactionController)
    {
        if (!interactionController.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, interactionController);
            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlayerGetObject?.Invoke(this, EventArgs.Empty);
    }

}
