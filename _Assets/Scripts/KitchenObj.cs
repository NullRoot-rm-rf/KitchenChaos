using UnityEngine;

public class KitchenObj : MonoBehaviour
{
    private IKitchenObj kitchenObj;
    public KitchenObjSO kitchenObjSO;
    public KitchenObjSO GetKitchenObjSO()
    {
        return kitchenObjSO;
    }
    public void SetKitchenObjParent(IKitchenObj kitchenObj)
    {
        if(this.kitchenObj != null)
        {
            this.kitchenObj.ClearKitchenObj();
        }

        this.kitchenObj = kitchenObj;

        if(this.kitchenObj.HasKitchenObj())
        {
            Debug.LogError("IKitchenObj already has a kitchenObj!");
        }               

        this.kitchenObj.SetKitchenObj(this);

        transform.parent = this.kitchenObj.GetKitchenObjPlacingPoint();
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf()
    {
        kitchenObj.ClearKitchenObj();
        Destroy(gameObject);
    }


    public static KitchenObj SpawnKitchenObj(KitchenObjSO kitchenObjSO, IKitchenObj kitchenObj)
    {
        Transform kitchenObjTransform = Instantiate(kitchenObjSO.prefab);
        KitchenObj kitchenObjComponent = kitchenObjTransform.GetComponent<KitchenObj>();
        kitchenObjComponent.SetKitchenObjParent(kitchenObj);
        return kitchenObjComponent;
    }
}
