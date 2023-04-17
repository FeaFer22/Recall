using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounterController cuttingCounterController;
    [SerializeField] private Image barImage;

    private void Start()
    {
        cuttingCounterController.OnProgressChanged += CuttingCounterController_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }

    private void CuttingCounterController_OnProgressChanged(object sender, CuttingCounterController.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if(e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
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
