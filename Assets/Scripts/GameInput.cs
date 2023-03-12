using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event Action<bool> OnInteractAction;
    
    private PlayerInputActions _playerInputActions;

    private const bool IS_ALTERNATE = true;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
    }

    public Vector2 GetInputVectorNormalized()
    {
        var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(!IS_ALTERNATE);
    }

    private void InteractAlternatePerformed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(IS_ALTERNATE);
    }
}
