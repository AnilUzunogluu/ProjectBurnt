using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ContainerCounter containerCounter;

    private void Start()
    {
        containerCounter.OnInteract += PlayInteractAnimation;
    }

    private void PlayInteractAnimation()
    {
        animator.SetTrigger("OpenClose");
    }
}