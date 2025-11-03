using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    private KitchenObj playerKitchenObj;
    private bool isDestroying = false;

    public override void Interact()
    {
        if (Player.Instance.HasKitchenObj() && !isDestroying)
        {
            playerKitchenObj = Player.Instance.GetKitchenObj();
            Player.Instance.ClearKitchenObj();
            StartCoroutine(MoveAndDestroy(playerKitchenObj));
        }
    }
    private IEnumerator MoveAndDestroy(KitchenObj obj)
    {
        Transform target = GetKitchenObjPlacingPoint();
        isDestroying = true;

        float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startPos = obj.transform.position;
        Vector3 endPos = target.position;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // This pauses until the next frame
        }

        obj.transform.position = endPos;

        obj.DestroySelf();

        isDestroying = false;
    }
}

