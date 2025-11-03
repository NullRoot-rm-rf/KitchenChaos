using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbingItem += ContainerCounter_OnPlayerGrabbingItem;
    }
    private void ContainerCounter_OnPlayerGrabbingItem(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
