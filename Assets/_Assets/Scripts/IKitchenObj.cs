using UnityEngine;

public interface IKitchenObj
{
    public void SetKitchenObj(KitchenObj kitchenObj);
    public KitchenObj GetKitchenObj();
    public void ClearKitchenObj();
    public bool HasKitchenObj();
    public Transform GetKitchenObjPlacingPoint();
}
