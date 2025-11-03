using System;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    public enum CookingStates
    {
        Idle,
        Cooking,
        Cooked,
        Burned
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChange;
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

    private void Update()
    {
        switch (state)
        {
            case CookingStates.Idle:
                break;
            case CookingStates.Cooking:
                FryingRecipeSO recipe = GetCookingRecipeSOWithInput(kitchenObjSO);
                if (HasKitchenObj())
                {
                    if (recipe != null)
                    {
                        fryingProgress += Time.deltaTime;
                        if (fryingProgress >= recipe.fryingProgressMax)
                        {
                            GetKitchenObj().DestroySelf();
                            KitchenObj.SpawnKitchenObj(recipe.output, this);
                            kitchenObjSO = recipe.output;
                            state = CookingStates.Cooked;
                            OnStateChange?.Invoke(this , new OnStateChangedEventArgs(){state = state});

                            fryingProgress = 0f;
                            break;
                        }
                    }
                }
                break;
            case CookingStates.Cooked:
                BurningRecipeSO burningRecipe = GetBurningRecipeSOWithInput(kitchenObjSO);

                burningProgress += Time.deltaTime;
                if (burningProgress >= burningRecipe.burningProgressMax && HasKitchenObj())
                {
                    GetKitchenObj().DestroySelf();
                    KitchenObj.SpawnKitchenObj(burningRecipe.output, this);
                    kitchenObjSO = burningRecipe.output;
                    state = CookingStates.Burned;
                    OnStateChange?.Invoke(this, new OnStateChangedEventArgs() { state = state });
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
                fryingProgress = 0f;
            }
        }
        else if (!Player.Instance.HasKitchenObj() && HasKitchenObj())
        {
            GetKitchenObj().SetKitchenObjParent(Player.Instance);
            state = CookingStates.Idle;
            OnStateChange?.Invoke(this, new OnStateChangedEventArgs() { state = state });

            fryingProgress = 0f;
        }
    }

    private KitchenObjSO GetCookingRecipeOutput(KitchenObjSO inputKitchenObjSO)
    {
        FryingRecipeSO recipe = GetCookingRecipeSOWithInput(inputKitchenObjSO);
        return recipe != null ? recipe.output : null;
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
