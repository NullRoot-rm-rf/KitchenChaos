using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObj : KitchenObj
{
    [SerializeField] private List<KitchenObjSO> validKichenObjSOList;

    private List<KitchenObjSO> KitchenObjSO_OnPlateList;
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
            KitchenObjSO_OnPlateList.Add (kitchenObjSO);
            return true;
        }
    }
}
