using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Mode mode;
    
    private enum Mode{LookAt, InvertedLookAt}
    
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.InvertedLookAt:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
