using UnityEngine;

public class LookAtCemera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.forward = -Camera.main.transform.forward;
    }
}
