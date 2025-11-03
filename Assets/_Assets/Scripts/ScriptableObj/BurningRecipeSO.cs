using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjSO input;
    public KitchenObjSO output;
    public int burningProgressMax;
}
