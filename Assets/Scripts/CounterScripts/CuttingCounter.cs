using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int _cuttingProgress;
    private CuttingRecipeSO _currentCuttingRecipeSO;

    public event Action<float> OnProgressChanged;
    public event Action OnCut;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                _currentCuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                if (HasRecipeWithInput(GetKitchenObject().KitchenObjectSO))
                {
                    _cuttingProgress = 0;
                    OnProgressChanged?.Invoke(CuttingProgressNormalized());
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                if (HasRecipeWithInput(GetKitchenObject().KitchenObjectSO))
                {
                    _cuttingProgress = 0;
                    OnProgressChanged?.Invoke(CuttingProgressNormalized());
                }
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().KitchenObjectSO))
        {
            var outputKitchenObjectSO = GetOutputFromRecipe();
            _cuttingProgress++;
            OnCut?.Invoke();
            OnProgressChanged?.Invoke(CuttingProgressNormalized());
            
            if (_currentCuttingRecipeSO.cuttingProgressMax <= _cuttingProgress)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var cuttingRecipeSo in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSo.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSo;
            }
        }

        return null;
    }
    private KitchenObjectSO GetOutputFromRecipe()
    {
        var cuttingRecipeSo = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
        if (cuttingRecipeSo!=null)
        {
            return cuttingRecipeSo.output;
        }
        
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        var cuttingRecipeSo = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSo != null;
    }

    private float CuttingProgressNormalized()
    {
        return (float) _cuttingProgress / _currentCuttingRecipeSO.cuttingProgressMax;
    }
}
