using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    [SerializeField] private List<KitchenObjectSO> validKitchenObjects;

    public event Action<KitchenObjectSO> OnIngredientAdded;
    
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo)
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSo) || !validKitchenObjects.Contains(kitchenObjectSo))
        {
            return false;
        }
        else
        {        
            kitchenObjectSOList.Add(kitchenObjectSo);
            OnIngredientAdded?.Invoke(kitchenObjectSo);
            return true;
        }
    }
}
