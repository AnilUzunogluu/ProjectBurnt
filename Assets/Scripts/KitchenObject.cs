using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    private IKitchenObjectParent _currentKitchenObjectParent;
    
    public KitchenObjectSO KitchenObjectSO => kitchenObjectSo;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (_currentKitchenObjectParent != null)
        {
            _currentKitchenObjectParent.ClearKitchenObject();
        }

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("kitchenObjectParent already has kitchenObject.");
        }
        kitchenObjectParent.SetKitchenObject(this);
        
        transform.parent = kitchenObjectParent.KitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        
        _currentKitchenObjectParent = kitchenObjectParent;
    }

    public IKitchenObjectParent GetCurrentKitchenObjectParent()
    {
        return _currentKitchenObjectParent;
    }

    public void DestroySelf()
    {
        _currentKitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    
    
    
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent objectParent)
    {
        var kitchenObjectTransform = Instantiate(kitchenObjectSo.Prefab);
        var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(objectParent);

        return kitchenObject;
    }
}
