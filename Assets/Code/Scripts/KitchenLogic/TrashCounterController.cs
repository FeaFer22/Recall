using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;

namespace Assets.Code.Scripts.KitchenLogic
{
    public class TrashCounterController : BaseCounter
    {
        public event EventHandler OnTrashed;
        public override void Interact(PlayerInteractionController playerInteraction)
        {
            if (playerInteraction.HasKitchenObject())
            {
                KitchenObject.DestroyKitchenObject(playerInteraction.GetKitchenObject());
                TrashedServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void TrashedServerRpc()
        {
            TrashedClientRpc();
        }
        [ClientRpc]
        private void TrashedClientRpc()
        {
            OnTrashed?.Invoke(this, EventArgs.Empty);
        }

    }
}
