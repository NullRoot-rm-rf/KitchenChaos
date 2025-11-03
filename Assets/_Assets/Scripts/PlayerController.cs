using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction playerInputAction;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPlayerHoldingSomething;
    public event EventHandler OnPlayerNotHoldingSomething;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performed;//subscribe to the interact action event
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed; ;//subscribe to the interact action event for cutting counter
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);//fire OnInteractCuttingCounterAction event the subscribers is in the player scripts
    }

    private void Interact_performed(InputAction.CallbackContext obj)//subscriber method for interact action
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);//fire OnInteractAction event the subscribers is in the player scripts
        if (Player.Instance.HasKitchenObj())
        {
            OnPlayerHoldingSomething?.Invoke(this, EventArgs.Empty);
        }
        else 
        { 
            OnPlayerNotHoldingSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementInputNormalised()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
