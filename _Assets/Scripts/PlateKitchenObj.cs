using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObj : KitchenObj
{
    [SerializeField] private List<KitchenObjSO> validKichenObjSOList;

    private List<KitchenObjSO> KitchenObjSO_OnPlateList;

    public event EventHandler<OnAddingIngredientEventArgs> OnAddingIngredient;
    public class OnAddingIngredientEventArgs : EventArgs
    {
        public KitchenObjSO kitchenObjSO;
    }

    private void Awake()
    {
        KitchenObjSO_OnPlateList = new List<KitchenObjSO>();
    }

    public bool TryAddIngredient(KitchenObjSO kitchenObjSO)
    {
        if (!validKichenObjSOList.Contains(kitchenObjSO))
        {
            return false;
        }

        if (KitchenObjSO_OnPlateList.Contains(kitchenObjSO))
        {
            return false;
        }
        else
        {
            OnAddingIngredient?.Invoke(this, new OnAddingIngredientEventArgs
            {
                kitchenObjSO = kitchenObjSO
            });

            KitchenObjSO_OnPlateList.Add (kitchenObjSO);
            return true;
        }
    }
}
