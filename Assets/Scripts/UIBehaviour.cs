using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Slider _sensivitySlider;
    [SerializeField] private SC_FPSController _controller;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenClosePause();
        }
    }

    public void OpenClosePause()
    {
        if (_pausePanel.activeInHierarchy)
        {
            _pausePanel.SetActive(false);

            if (YandexGame.EnvironmentData.isDesktop)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            _pausePanel.SetActive(true);
            if (YandexGame.EnvironmentData.isDesktop)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OnChangedSensivity()
    {
        _controller.lookSpeed = (_sensivitySlider.value + 0.3f)*2.5f;
    }
}
