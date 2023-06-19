using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;

        Show();
    }

    private void GameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        Hide();
        if (GameManager.Instance.IsLocalPlayerReady() || GameManager.Instance.IsGamePlaying())
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
