using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event Action<float> OnProgressChanged;
    public event Action<bool> OnStoveActive; 
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    
    private float _fryingTimer;
    private float _burningTimer;
    private FryingRecipeSO _currentFryingRecipeSO;
    private State _state;

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    OnStoveActive?.Invoke(true);
                    FryingState();
                    break;
                case State.Fried:
                    OnStoveActive?.Invoke(true);
                    FriedState();
                    break;
                case State.Burnt:
                    OnStoveActive?.Invoke(false);
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _currentFryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                    
                    _state = _currentFryingRecipeSO.input == fryingRecipeSOArray[0].input ? State.Frying : State.Fried;
                    
                    _fryingTimer = 0f;
                    _burningTimer = 0f;

                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                _state = State.Idle;
                OnStoveActive?.Invoke(false);
                OnProgressChanged?.Invoke(ProgressNormalized(_state));
            }
        }
    }
    
    
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }
    private KitchenObjectSO GetOutputFromRecipe()
    {
        var fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
        return fryingRecipeSO!=null ? fryingRecipeSO.output : null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        var fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private void FryingState()
    {
        _fryingTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(ProgressNormalized(_state));

        if (_fryingTimer >= _currentFryingRecipeSO.fryingTimerMax)
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(_currentFryingRecipeSO.output, this);
            _state = State.Fried;
            _currentFryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
        }
    }

    private void FriedState()
    {
        _burningTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(ProgressNormalized(_state));

        if (_burningTimer >= _currentFryingRecipeSO.fryingTimerMax)
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(_currentFryingRecipeSO.output, this);
            _state = State.Burnt;
            OnProgressChanged?.Invoke(ProgressNormalized(_state));
        }
    }

    private float ProgressNormalized(State state)
    {
        switch (state)
        {
            case State.Frying:
                return _fryingTimer / _currentFryingRecipeSO.fryingTimerMax;
            case State.Fried:
                return _burningTimer / _currentFryingRecipeSO.fryingTimerMax;
            case State.Idle:
                return 0f;
            case State.Burnt:
                return 0f;
            default:
                return 0f;
        }
    }
}
