using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static CuttingCounterController;

public class StoveCounterController : BaseCounter, IHasProgress
{

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    //public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    //public class OnStateChangedEventArgs : EventArgs
    //{
    //    public State state;
    //}

    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeScriptableObj[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeScriptableObj[] burningRecipeSOArray;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);
    private FryingRecipeScriptableObj fryingRecipeScriptableObj;
    private BurningRecipeScriptableObj burningRecipeScriptableObj;

    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = fryingRecipeScriptableObj != null ? fryingRecipeScriptableObj.fryingTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer.Value / fryingTimerMax
        });
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = burningRecipeScriptableObj != null ? burningRecipeScriptableObj.burningTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = burningTimer.Value / burningTimerMax
        });
    }

    private void State_OnValueChanged(State previousState, State newState)
    {
        //OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        //{
        //    state = state.Value
        //});

        if (state.Value == State.Burned || state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        if (HasKitchenObject())
        {
            switch (state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer.Value += Time.deltaTime;

                    if (fryingTimer.Value > fryingRecipeScriptableObj.fryingTimerMax)
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(fryingRecipeScriptableObj.output, this);

                        state.Value = State.Fried;
                        burningTimer.Value = 0f;

                        SetBurningRecipeSOClientRpc(GameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO()));
                    }
                    break;
                case State.Fried:
                    burningTimer.Value += Time.deltaTime;

                    if (burningTimer.Value > burningRecipeScriptableObj.burningTimerMax)
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(burningRecipeScriptableObj.output, this);

                        state.Value = State.Burned;

                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }
    public override void Interact(PlayerInteractionController playerInteraction)
    {
        if (!HasKitchenObject())
        {
            if (playerInteraction.HasKitchenObject())
            {
                if (HasRecipeWithInput(playerInteraction.GetKitchenObject().GetKitchenObjectSO()))
                {
                    KitchenObject kitchenObject = playerInteraction.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractionLogicPlaceObjOnCounterServerRpc(GameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO()));
                }
            }
            else
            {

            }
        }
        else
        {
            if (playerInteraction.HasKitchenObject())
            {
                if (playerInteraction.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state.Value = State.Idle;
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(playerInteraction);
                SetStateIdleServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        state.Value = State.Idle;
    }


    [ServerRpc(RequireOwnership = false)]
    private void InteractionLogicPlaceObjOnCounterServerRpc(int kitchenSOIndex)
    {
        fryingTimer.Value = 0f;
        SetFryingRecipeSOClientRpc(kitchenSOIndex);
    }
    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenSOIndex)
    {
        KitchenObjectScriptableObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenSOIndex);
        fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenSOIndex)
    {
        KitchenObjectScriptableObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenSOIndex);
        fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(kitchenObjectSO);
    }

    private bool HasRecipeWithInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        FryingRecipeScriptableObj fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(inputKitchenObjSO);
        return fryingRecipeScriptableObj != null;
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        FryingRecipeScriptableObj fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(inputKitchenObjSO);
        if (fryingRecipeScriptableObj != null)
        {
            return fryingRecipeScriptableObj.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeScriptableObj GetFryingRecipeScriptableObjWithInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        foreach (FryingRecipeScriptableObj recipeScriptableObj in fryingRecipeSOArray)
        {
            if (recipeScriptableObj.input == inputKitchenObjSO)
            {
                return recipeScriptableObj;
            }
        }
        return null;
    }

    private BurningRecipeScriptableObj GetBurningRecipeScriptableObjWithInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        foreach (BurningRecipeScriptableObj recipeScriptableObj in burningRecipeSOArray)
        {
            if (recipeScriptableObj.input == inputKitchenObjSO)
            {
                return recipeScriptableObj;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state.Value == State.Fried;
    }
}
