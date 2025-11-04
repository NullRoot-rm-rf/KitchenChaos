using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    public KitchenObjSO KitchenObjSO;
    public event EventHandler OnPlayerGrabbingItem;

    public override void Interact()
    {
        if (!Player.Instance.HasKitchenObj())
        {
            OnPlayerGrabbingItem?.Invoke(this, EventArgs.Empty);
            KitchenObj.SpawnKitchenObj(KitchenObjSO, Player.Instance);
        }
    }
}