using UnityEngine;

[CreateAssetMenu(menuName = "New Cutting Recipe", fileName = "Cutting Recipe")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
