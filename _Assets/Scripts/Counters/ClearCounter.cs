using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact()
    {
        if (!HasKitchenObj())
        {
            //there is no kitchenObj here, so if the player has one, give it to the counter
            if (Player.Instance.HasKitchenObj())
            {
                //if player has kitchenObj and counter does not have kitchenObj
                Player.Instance.GetKitchenObj().SetKitchenObjParent(this);
            }
        }
        else
        {
            //there is a kitchenaObj here, so if the player does not have one, give it to the player
            if (Player.Instance.HasKitchenObj())
            {
                //Checking if player is caring a plate to put items on it to serve
                if (Player.Instance.GetKitchenObj().TryGetPlate(out PlateKitchenObj plateKitchenObj)) 
                {
                    if (plateKitchenObj.TryAddIngredient(GetKitchenObj().GetKitchenObjSO()))
                    {
                        GetKitchenObj().DestroySelf();
                    }
                }
            }
            else
            {
                //if player does not have kitchenObj and counter has kitchenObj
                base.GetKitchenObj().SetKitchenObjParent(Player.Instance);
            }
        }
    }
}
