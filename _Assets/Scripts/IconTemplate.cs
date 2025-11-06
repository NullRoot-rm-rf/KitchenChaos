using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetIconSprite(KitchenObjSO kitchenObjSO)
    {
        image.sprite = kitchenObjSO.sprite;
    }
}
