using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private Animator animator;
    private const string CUT_TRIGGER = "Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CuttingCounter cuttingCounter = GetComponentInParent<CuttingCounter>();
        cuttingCounter.OnCut += CuttingCounter_OnCut; ;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT_TRIGGER);
    }
}
