using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterController : BaseCounter
{
    [SerializeField] private KitchenObjectScriptableObject kitchenObjectSO;

    public override void Interact(PlayerInteractionController interactionController)
    {
        if (!interactionController.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, interactionController);
        }
    }
}
