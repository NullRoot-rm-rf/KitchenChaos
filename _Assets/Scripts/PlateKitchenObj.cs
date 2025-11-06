using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObj : KitchenObj
{
    [SerializeField] private List<KitchenObjSO> validKitchenObjSOList;
    [SerializeField] private KitchenObjSO meatPattyCooked;
    [SerializeField] private KitchenObjSO meatPattyBurned;

    private List<KitchenObjSO> KitchenObjSO_OnPlateList;
    private HashSet<KitchenObjSO> MeatPrefabs;


    public event EventHandler<OnAddingIngredientEventArgs> OnAddingIngredient;
    public class OnAddingIngredientEventArgs : EventArgs
    {
        public KitchenObjSO kitchenObjSO;
    }

    private void Awake()
    {
        MeatPrefabs = new HashSet<KitchenObjSO> { meatPattyCooked, meatPattyBurned };
        KitchenObjSO_OnPlateList = new List<KitchenObjSO>();
    }

    public bool TryAddIngredient(KitchenObjSO kitchenObjSO)
    {
        if (!validKitchenObjSOList.Contains(kitchenObjSO))
        {
            return false;
        }

        if (KitchenObjSO_OnPlateList.Contains(kitchenObjSO))
        {
            return false;
        }

        bool incomingIsMeat = MeatPrefabs.Contains(kitchenObjSO);
        bool plateHasMeat = KitchenObjSO_OnPlateList.Contains(meatPattyBurned) || KitchenObjSO_OnPlateList.Contains(meatPattyCooked);

        if (incomingIsMeat && plateHasMeat) 
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
