using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO :ScriptableObject
{
    public KitchenObjSO input;
    public KitchenObjSO output;
    public int fryingProgressMax;
}
