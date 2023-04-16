using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ClearCounterVisualHandler : MonoBehaviour
{
    [SerializeField] private ClearCounterController clearCounter;
    [SerializeField] private GameObject visualGameObject;
    private void Start()
    {
        PlayerInteractionController.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, PlayerInteractionController.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }

}
