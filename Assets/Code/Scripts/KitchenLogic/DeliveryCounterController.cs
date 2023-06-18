using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounterController : BaseCounter
{
    public static DeliveryCounterController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(PlayerInteractionController playerController)
    {
        if (playerController.HasKitchenObject())
        {
            if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                KitchenObject.DestroyKitchenObject(playerController.GetKitchenObject());
            }
        }
    }
}
