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

    private void Start()
    {
        YandexGame.SwitchLanguage(YandexGame.savesData.language);
        _controller.lookSpeed = YandexGame.savesData.Sensivity;
        _sensivitySlider.value = (YandexGame.savesData.Sensivity / 4);
        SwitchQuality();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenClosePause();
        }
    }

    public void SwitchQuality()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            QualitySettings.globalTextureMipmapLimit = 0;
        }
        else
        {
            QualitySettings.globalTextureMipmapLimit = 3;
        }
    }

    public void ChangeLanguage(string lang)
    {
        YandexGame.SwitchLanguage(lang);
        YandexGame.savesData.language = lang;
        YandexGame.SaveProgress();
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
