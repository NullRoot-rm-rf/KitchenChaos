using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum CookingStates
    {
        Idle,
        Cooking,
        Cooked,
        Burned
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChange;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs: EventArgs
    {
        public CookingStates state;
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private CookingStates state = CookingStates.Idle;
    private KitchenObjSO kitchenObjSO;
    private float fryingProgress;
    private float burningProgress;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Update()
    {
        switch (state)
        {
            case CookingStates.Idle:
                break;
            case CookingStates.Cooking:
                if (HasKitchenObj())
                {
                    if (fryingRecipeSO != null)
                    {
                        fryingProgress += Time.deltaTime;
                        if (fryingProgress >= fryingRecipeSO.fryingProgressMax)
                        {
                            GetKitchenObj().DestroySelf();
                            KitchenObj.SpawnKitchenObj(fryingRecipeSO.output, this);
                            kitchenObjSO = fryingRecipeSO.output;
                            state = CookingStates.Cooked;

                            OnStateChange?.Invoke(this , new OnStateChangedEventArgs(){state = state});

                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = fryingProgress / fryingRecipeSO.fryingProgressMax
                            });

                            fryingProgress = 0f;
                            break;
                        }
                    }
                }
                break;
            case CookingStates.Cooked:
                burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjSO);

                burningProgress += Time.deltaTime;
                if (burningProgress >= burningRecipeSO.burningProgressMax && HasKitchenObj())
                {
                    GetKitchenObj().DestroySelf();
                    KitchenObj.SpawnKitchenObj(burningRecipeSO.output, this);
                    kitchenObjSO = burningRecipeSO.output;
                    state = CookingStates.Burned;

                    OnStateChange?.Invoke(this, new OnStateChangedEventArgs() { state = state });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningProgress / burningRecipeSO.burningProgressMax
                    });

                    burningProgress = 0f;
                    break;
                }
                break;
            case CookingStates.Burned:
                break;
        }
    }

    public override void Interact()
    {
        if (!HasKitchenObj() && Player.Instance.HasKitchenObj())
        {
            KitchenObjSO inputSO = Player.Instance.GetKitchenObj().GetKitchenObjSO();
            if (HasRecipeWithInput(inputSO))
            {
                Player.Instance.GetKitchenObj().SetKitchenObjParent(this);
                state = CookingStates.Cooking;
                OnStateChange?.Invoke(this, new OnStateChangedEventArgs() { state = state });

                kitchenObjSO = GetKitchenObj().GetKitchenObjSO();
                fryingRecipeSO = GetCookingRecipeSOWithInput(kitchenObjSO);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingProgress/fryingRecipeSO.fryingProgressMax
                });

               fryingProgress = 0f;
            }
        }
        else if (!Player.Instance.HasKitchenObj() && HasKitchenObj())
        {
            GetKitchenObj().SetKitchenObjParent(Player.Instance);
            state = CookingStates.Idle;
            OnStateChange?.Invoke(this, new OnStateChangedEventArgs() { state = state });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });

            fryingProgress = 0f;
            burningProgress = 0f;
        }
    }

    private bool HasRecipeWithInput(KitchenObjSO inputKitchenObjSO)
    {
        return GetCookingRecipeSOWithInput(inputKitchenObjSO) != null;
    }

    private FryingRecipeSO GetCookingRecipeSOWithInput(KitchenObjSO inputKitchenObjSO)
    {
        foreach (FryingRecipeSO recipe in fryingRecipeSOArray)
        {
            if (recipe.input == inputKitchenObjSO)
            {
                return recipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjSO kitchenObjSO) 
    {
        foreach (BurningRecipeSO recipe in burningRecipeSOArray)
        {
            if (recipe.input == kitchenObjSO)
            {
                return recipe;
            }
        }
        return null;
    }
}
