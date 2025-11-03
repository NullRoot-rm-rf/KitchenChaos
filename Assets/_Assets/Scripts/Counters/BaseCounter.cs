using UnityEngine;

public class BaseCounter :MonoBehaviour, IKitchenObj
{
    private KitchenObj kitchenObj;
    [SerializeField] private Transform counterTopPonit;

    public virtual void Interact()
    {
        Debug.LogError("BaseCounter Interact() method called. This should be overridden.");
    }

    public virtual void InteractAlternate() { }

    public void SetKitchenObj(KitchenObj kitchenObj)
    {
        this.kitchenObj = kitchenObj;
    }

    public KitchenObj GetKitchenObj()
    {
        return kitchenObj;
    }

    public void ClearKitchenObj()
    {
        kitchenObj = null;
    }

    public bool HasKitchenObj()
    {
        return kitchenObj != null;
    }

    public Transform GetKitchenObjPlacingPoint()
    {
        return counterTopPonit;
    }
}
