using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;


    private void Start()
    {
        stoveCounter.OnStoveActive += SetStoveVisuals;
    }

    private void SetStoveVisuals(bool isStoveActive)
    {
        stoveOnGameObject.SetActive(isStoveActive);
        particlesGameObject.SetActive(isStoveActive);
    }
}
