using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    
    [Header("Player variables")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 7f;
    
    [Header("Restriction Values")]
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float interactDistance = 2f;
    
    [Header("References")]
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private GameInput gameInput;

    public event Action<BaseCounter> OnSelectedCounterChanged; 

    private Vector2 _inputVector;
    private Vector3 _moveDirection;
    private Vector3 _lastInteractDirection;
    private bool _canMove;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
           Debug.LogError("There is more than one player");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += InteractAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    
    private void HandleMovement()
    {
        SetInputVector();
        SetMoveDirection();

        if (!CanMove(_moveDirection))
        {
            //Attempt only X movement
            var moveDirectionX = new Vector3(_moveDirection.x, 0, 0).normalized;

            if (CanMove(moveDirectionX) && _moveDirection.x != 0)
            {
                _moveDirection = moveDirectionX;
            }
            else
            {
                //Attempt only z movement
                var moveDirectionZ = new Vector3(0, 0, _moveDirection.z).normalized;

                if (CanMove(moveDirectionZ) && _moveDirection.z != 0)
                {
                    _moveDirection = moveDirectionZ;
                }
            }
        }

        if (CanMove(_moveDirection))
        {
            var adjustedMovement = _moveDirection * (Time.deltaTime * moveSpeed);
            transform.position += adjustedMovement;
        }
        
        RotateTowardsMoveDirection();
    }

    private void SetInputVector()
    {
        _inputVector = gameInput.GetInputVectorNormalized();
    }
    
    private void SetMoveDirection()
    {
        _moveDirection.x = _inputVector.x;
        _moveDirection.y = 0;
        _moveDirection.z = _inputVector.y;
    }

    private void RotateTowardsMoveDirection()
    {
        transform.forward = Vector3.Slerp(transform.forward, _moveDirection, Time.deltaTime * rotationSpeed);
    }
    

    private void HandleInteractions()
    {
        SetInteractDirection();
        
        if (Physics.Raycast(transform.position, _lastInteractDirection, out var raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (_selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetInteractDirection()
    {
        SetInputVector();
        var interactDirection = new Vector3(_inputVector.x, 0f, _inputVector.y);
        

        if (interactDirection != Vector3.zero)
        {
            _lastInteractDirection = interactDirection;
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(_selectedCounter);
    }

    private void InteractAction(bool isAlternate)
    {
        if (_selectedCounter == null) return;

        if (!isAlternate)
        {
            _selectedCounter.Interact(this);
        }
        else
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    public bool IsWalking()
    {
        return _moveDirection != Vector3.zero;
    }

    private bool CanMove(Vector3 moveDirection)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveSpeed * Time.deltaTime);
    }
    
    public Transform KitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
