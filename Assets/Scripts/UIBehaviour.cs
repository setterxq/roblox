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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenClosePause();
        }
    }

    public void OpenClosePause()
    {
        if (_pausePanel.activeInHierarchy)
        {
            _pausePanel.SetActive(false);
            _controller.Pause = false;

            if (YandexGame.EnvironmentData.isDesktop)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            _pausePanel.SetActive(true);
            _controller.Pause = true;
            if (YandexGame.EnvironmentData.isDesktop)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OnChangedSensivity()
    {
        _controller.lookSpeed = (_sensivitySlider.value + (1/4))*4;
        YandexGame.savesData.Sensivity = _controller.lookSpeed;
        YandexGame.SaveProgress();
    }
}
