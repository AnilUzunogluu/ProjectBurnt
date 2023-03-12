using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;


    private void Start()
    {
        cuttingCounter.OnProgressChanged += UpdateBarImage;
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
