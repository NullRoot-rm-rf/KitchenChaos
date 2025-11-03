using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObj
{
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public static Player Instance { get; private set; } 

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float playerRotationSpeed = 5f;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform playerHoldingPoint;

    private KitchenObj kitchenObj;
    private Vector3 lastMoveDir;
    private bool isWalking;
    private BaseCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        playerController.OnInteractAlternateAction += PlayerController_OnInteractAlternateAction; ;//subscribing to the interact action event
        playerController.OnInteractAction += GameInput_OnInteractAction;//subscribing to the interact action event
    }

    //this method is called when the player interacts with an object
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)//listener method for interact action event
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void PlayerController_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate();
        }
    }

    private void Update()
    {
       HandleMovement();
       HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = playerController.GetMovementInputNormalised();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }
        //this checks if there is an interactable object in front of the player within the set interaction range
        float interactionRange = 1f;//defining interaction range
        //this is to detect and select the counter in front of the player
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, interactionRange))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (selectedCounter != baseCounter)//this checks if the selected counter is different from the current one
                {
                    SetSelectedCounter(baseCounter);
                }
            } else SetSelectedCounter(null);
        } else SetSelectedCounter(null);
    }

    public void SetSelectedCounter(BaseCounter thisSelectedCounter)
    {
        selectedCounter = thisSelectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    private void HandleMovement()
    {
        float playerHeight = 1.3f;
        float playerRadius = 0.5f;
        float moveDistance = moveSpeed * Time.deltaTime;

        Vector2 inputVector = playerController.GetMovementInputNormalised();//importing input from PlayerController
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);//converting 2D input to 3D movement

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerRotationSpeed);
        }
        //rotating the player

        if (!canMove)
        {
            //If can not move check X direction only
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //If can not move in X direction check Z direction only
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }//if fails player can not move in any direction
            }
        }

        if (canMove)
        {
            //moving the player
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    //the following methods implement the IKitchenObj interface

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
        return playerHoldingPoint;
    }
}
