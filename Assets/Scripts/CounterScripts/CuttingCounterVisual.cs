using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;

    [SerializeField] private Animator animator;

    private void Start()
    {
        cuttingCounter.OnCut += PlayCuttingAnimation;
    }

    private void PlayCuttingAnimation()
    {
        animator.SetTrigger("Cut");
    }
}
