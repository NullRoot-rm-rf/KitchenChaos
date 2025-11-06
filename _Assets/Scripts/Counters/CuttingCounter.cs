using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    [SerializeField] private CuttingRecipeSO[] CuttingRecipeSOArray;


    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;


    private int cuttingProgress;
    public override void Interact()
    {
        if (!HasKitchenObj())
        {
            if (Player.Instance.HasKitchenObj())
            {
                //there is no kitchenObj here, so if the player has one, give it to the counter
                if (HasRecipeWithInput(Player.Instance.GetKitchenObj().GetKitchenObjSO()))
                {
                    Player.Instance.GetKitchenObj().SetKitchenObjParent(this);
                    //reset cutting progress
                    cuttingProgress = 0;
                    //invoke progress changed event to update progress bar
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / GetCuttingRecipeSOWithInput(GetKitchenObj().GetKitchenObjSO()).cuttingProgressMax
                    });
                }
            }
        }
        else
        {
            if (Player.Instance.HasKitchenObj())
            {
                //checking if player has a plate for picking stuff on it
                if (Player.Instance.GetKitchenObj().TryGetPlate(out PlateKitchenObj plateKitchenObj))
                {
                    if (plateKitchenObj.TryAddIngredient(GetKitchenObj().GetKitchenObjSO()))
                    {
                        GetKitchenObj().DestroySelf();
                    }
                }
            }
            //there is a kitchenaObj here, so if the player does not have one, give it to the player
            else
            {
                base.GetKitchenObj().SetKitchenObjParent(Player.Instance);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });

                cuttingProgress = 0;
            }
        }
    }

    public override void InteractAlternate()
    {
        if (HasKitchenObj() && GetKitchenRecipeOutput(GetKitchenObj().GetKitchenObjSO()))
        {
            KitchenObjSO output = GetKitchenRecipeOutput(GetKitchenObj().GetKitchenObjSO());//Defing outhere to be used for spawning new object after cutting is done
            //there is a kitchenObj here that can be cut
            int cuttingProgressMax = GetCuttingRecipeSOWithInput(GetKitchenObj().GetKitchenObjSO()).cuttingProgressMax;
            //increase cutting progress
            cuttingProgress++;
            if (cuttingProgress >= cuttingProgressMax)
            {
                GetKitchenObj().DestroySelf();
                KitchenObj.SpawnKitchenObj(output, this);//using output defined above as the kichen obj is now null after destroying its
            }


            OnCut?.Invoke(this, EventArgs.Empty);
            //invoke progress changed event to update progress bar
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingProgressMax
            });
        }
    }

    private KitchenObjSO GetKitchenRecipeOutput(KitchenObjSO inputKitchenObjSO)
    {
        CuttingRecipeSO CuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjSO);
        if (CuttingRecipeSO != null)
        {
            return CuttingRecipeSO.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjSO inputKitchenObjSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObjSO) != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjSO inputKitchenObjSO)
    {
        foreach (CuttingRecipeSO kitchenRecipe in CuttingRecipeSOArray)
        {
            if (kitchenRecipe.input == inputKitchenObjSO)
            {
                return kitchenRecipe;
            }
        }
        return null;
    }
}
