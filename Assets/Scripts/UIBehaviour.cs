using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Slider _sensivitySlider;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private SC_FPSController _controller;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Text _timerText;
    private ControlProgress _controlProgress;
    private bool _isDesktop;

    private bool _lose = false;
    private float timer = 0;

    private void Start()
    {
        YandexGame.SwitchLanguage(YandexGame.savesData.language);
        _controller.lookSpeed = YandexGame.savesData.Sensivity;
        _sensivitySlider.value = (YandexGame.savesData.Sensivity / 4);
        _volumeSlider.value = YandexGame.savesData.Volume;
        _controller.Source.volume = _volumeSlider.value;
        SwitchQuality();
        _controlProgress = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControlProgress>();
        _controlProgress.uIBehaviour = this;
        ChangeProgressbar(_controlProgress.LastSave / 73f);
        _isDesktop = _controller.IsDesktop;
    }

    public void Lose()
    {
        _lose = true;
        _loseScreen.SetActive(true);
        _controller.characterController.enabled = false;

        if (YandexGame.EnvironmentData.isDesktop)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ChangeProgressbar(float value)
    {
        Debug.Log(value);
        _progressBar.fillAmount = value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_lose)
        {
            OpenClosePause();
        }

        if(timer > 70)
        {
            timer = 0;
            StartCoroutine(WaitTime());
        }

        timer += Time.deltaTime;
    }

    public void RespawnButton()
    {
        _controlProgress.Spawn(true);
        Destroy(_controller.gameObject);
        timer = 0;
        YandexGame.FullscreenShow();
        if (_isDesktop)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SwitchQuality()
    {
        if (_isDesktop)
        {
            QualitySettings.globalTextureMipmapLimit = 0;
        }
        else
        {
            QualitySettings.globalTextureMipmapLimit = 2;
        }
    }

    public void ChangeLanguage(string lang)
    {
        YandexGame.SwitchLanguage(lang);
        YandexGame.savesData.language = lang;
        YandexGame.SaveProgress();
    }

    public void ChangeVolume()
    {
        _controller.Source.volume = _volumeSlider.value;
        YandexGame.savesData.Volume = _volumeSlider.value;
        YandexGame.SaveProgress();
    } 

    public void OpenClosePause()
    {
        if (_pausePanel.activeInHierarchy)
        {
            _pausePanel.SetActive(false);
            _controller.Pause = false;

            if (_isDesktop)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            _pausePanel.SetActive(true);
            _controller.Pause = true;
            if (_isDesktop)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OnChangedSensivity()
    {
        if (_sensivitySlider.value == 0) _sensivitySlider.value = 0.01f;
        _controller.lookSpeed = (_sensivitySlider.value + (1/4))*4;
        YandexGame.savesData.Sensivity = _controller.lookSpeed;
        YandexGame.SaveProgress();
    }

    public IEnumerator WaitTime()
    {
        _timerText.gameObject.SetActive(true);
        if (YandexGame.lang == "ru")  
        {
            _timerText.text = "Реклама через 2";
        }
        else
        {
            _timerText.text = "advertising in 2";
        }
        yield return new WaitForSeconds(1);
        if (YandexGame.lang == "ru")
        {
            _timerText.text = "Реклама через 1";
        }
        else
        {
            _timerText.text = "advertising in 1";
        }
        yield return new WaitForSeconds(1);
        _timerText.gameObject.SetActive(false);
        YandexGame.FullscreenShow();
    }
}
