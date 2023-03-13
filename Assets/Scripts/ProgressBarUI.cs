using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress _hasProgress;

    private void Start()
    {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        _hasProgress.OnProgressChanged += UpdateBarImage;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void UpdateBarImage(float progressNormalized)
    {
        Show();
        barImage.fillAmount = progressNormalized;
        if (progressNormalized is 1 or 0)
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
