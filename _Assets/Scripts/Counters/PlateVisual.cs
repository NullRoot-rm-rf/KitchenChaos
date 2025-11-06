using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateVisual : MonoBehaviour
{
    [SerializeField] PlateKitchenObj plateKitchenObj;

    [Serializable]
    public struct PlateVisualList
    {
        public KitchenObjSO KitchenObjSO;
        public GameObject ganeObject;
    }

    [SerializeField] List<PlateVisualList> plateVisualList;

    private void Start()
    {
        plateKitchenObj.OnAddingIngredient += PlateKitchenObj_OnAddingIngredient;
        foreach (PlateVisualList plateVisualList in plateVisualList)
        {
            plateVisualList.ganeObject.SetActive(false);
        }
    }

    private void PlateKitchenObj_OnAddingIngredient(object sender, PlateKitchenObj.OnAddingIngredientEventArgs e)
    {
        foreach (PlateVisualList plateVisualList in plateVisualList)
        {
            if (plateVisualList.KitchenObjSO  == e.kitchenObjSO)
            {
                plateVisualList.ganeObject.SetActive(true);
            }
        }
    }
}
