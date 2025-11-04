using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Player player;
    private PlayerController controller;
    private Animator animator;

    [SerializeField] private Transform leg_L;
    [SerializeField] private Transform leg_R;
    [SerializeField] private Transform arm_L;
    [SerializeField] private Transform arm_R;
    [SerializeField] private Transform torso;
    [SerializeField] private Transform head;

    private const string IS_WALKING = "IsWalking";

    private float walkSpeed = 10f;
    private Vector3 baseTorsoPos = Vector3.zero;
    private Vector3 baseHeadPos = new(0, 1.6f, 0);
    private bool isHoldingSomething = false;
    Quaternion targetLeft = Quaternion.Euler(-90f, 0f, 30f);
    Quaternion targetRight = Quaternion.Euler(-90f, 0f, -30f);


    private void Awake()
    {
        player = GetComponentInParent<Player>();
        controller = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        controller.OnPlayerHoldingSomething += Controller_OnPlayerHoldingSomething;
        controller.OnPlayerNotHoldingSomething += Controller_OnPlayerNotHoldingSomething;
    }

    private void Update()
    {
        float blendSpeed = 6.5f; // tweak for faster/slower transition

        if (isHoldingSomething)
        {
            arm_L.localRotation = Quaternion.Slerp(arm_L.localRotation, targetLeft, Time.deltaTime * blendSpeed);
            arm_R.localRotation = Quaternion.Slerp(arm_R.localRotation, targetRight, Time.deltaTime * blendSpeed);
        }
        else
        {
            arm_L.localRotation = Quaternion.Slerp(arm_L.localRotation, Quaternion.identity, Time.deltaTime * blendSpeed);
            arm_R.localRotation = Quaternion.Slerp(arm_R.localRotation, Quaternion.identity, Time.deltaTime * blendSpeed);
        }

        if (player.IsWalking())
        {
            animator.SetBool(IS_WALKING, true);
            WalkingLegAnimation();
            WalkingTorsoAndHeadAnimation();
            if (!isHoldingSomething) WalkingHandAnimation();
        }
        else
        {
            animator.SetBool(IS_WALKING, false);
        }
    }

    private void Controller_OnPlayerHoldingSomething(object sender, System.EventArgs e)
    {
        isHoldingSomething = true;
    }

    private void Controller_OnPlayerNotHoldingSomething(object sender, System.EventArgs e)
    {
        isHoldingSomething = false;
    }

    private void WalkingLegAnimation()
    {
        float t = Time.time * walkSpeed;
        float legSwing = Mathf.Sin(t) * Mathf.Abs(Mathf.Sin(t)) * 25f;

        leg_L.localRotation = Quaternion.Euler(legSwing, 0, 0);
        leg_R.localRotation = Quaternion.Euler(-legSwing, 0, 0);
    }

    private void WalkingHandAnimation()
    {
        float armSwing = Mathf.Sin(Time.time * walkSpeed + Mathf.PI / 2f) * 15f;

        arm_L.localRotation = Quaternion.Euler(-armSwing, 0, 0);
        arm_R.localRotation = Quaternion.Euler(armSwing, 0, 0);
    }

    private void WalkingTorsoAndHeadAnimation() 
    {
        float torsoBob = Mathf.Sin(Time.time * walkSpeed * 2f) * 0.03f;
        torso.localPosition = baseTorsoPos + new Vector3(0, torsoBob, 0);

        head.localPosition = baseHeadPos + new Vector3(0, -torsoBob, 0);

        float hipSway = Mathf.Sin(Time.time * walkSpeed) * 0.02f;
        torso.localPosition = baseTorsoPos + new Vector3(hipSway, torsoBob, 0);
    }
}
