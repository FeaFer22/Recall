using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static CuttingCounterController;

public class StoveCounterController : BaseCounter, IHasProgress
{

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeScriptableObj[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeScriptableObj[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeScriptableObj fryingRecipeScriptableObj;
    private BurningRecipeScriptableObj burningRecipeScriptableObj;


    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeScriptableObj.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeScriptableObj.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeScriptableObj.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeScriptableObj = GetBurningRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObject());
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeScriptableObj.burningTimerMax
                    });

                    if (burningTimer > burningRecipeScriptableObj.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeScriptableObj.output, this);

                        state = State.Burned;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
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
                if (HasRecipeWithInput(playerInteraction.GetKitchenObject().GetKitchenObject()))
                {
                    playerInteraction.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObject());

                    state = State.Frying;
                    fryingTimer = 0f;


                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeScriptableObj.fryingTimerMax
                    });
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

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(playerInteraction);
                state = State.Idle;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
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
}
