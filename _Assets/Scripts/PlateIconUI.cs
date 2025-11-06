using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObj plateKitchenObj;
    [SerializeField] private IconTemplate iconTemplate;

    private void Start()
    {
        plateKitchenObj.OnAddingIngredient += PlateKitchenObj_OnAddingIngredient;
    }

    private void PlateKitchenObj_OnAddingIngredient(object sender, PlateKitchenObj.OnAddingIngredientEventArgs e)
    {
        iconTemplate.SetIconSprite(e.kitchenObjSO);
        Instantiate(iconTemplate, transform);
    }
}
