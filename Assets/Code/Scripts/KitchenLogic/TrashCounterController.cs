using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.KitchenLogic
{
    public class TrashCounterController : BaseCounter
    {
        public override void Interact(PlayerInteractionController playerInteraction)
        {
            if (playerInteraction.HasKitchenObject())
            {
                playerInteraction.GetKitchenObject().DestroySelf();
            }
        }
    }
}
