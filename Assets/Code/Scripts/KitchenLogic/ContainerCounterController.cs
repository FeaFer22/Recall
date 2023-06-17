using System;
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
            OnPlayerGetObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
