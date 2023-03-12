using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += SetSelectedVisual;
    }

    private void SetSelectedVisual(BaseCounter selectedCounter)
    {
        if (selectedCounter == baseCounter)
        {
            Selected();
        }
        else
        {
            NotSelected();
        }
    }

    private void Selected()
    {
        foreach (var visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void NotSelected()
    {
        foreach (var visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }

}
