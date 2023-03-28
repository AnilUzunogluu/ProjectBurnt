using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSo;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _kitchenObjectSoGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += UpdateIngredients;
    }

    private void UpdateIngredients(KitchenObjectSO kitchenObjectSo)
    {
        foreach (var kitchenObjectSoGameObject in _kitchenObjectSoGameObjectList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectSo == kitchenObjectSo)
            {
                kitchenObjectSoGameObject.gameObject.SetActive(true);
            }
        }
    }
}
