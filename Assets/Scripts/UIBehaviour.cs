using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Slider _sensivitySlider;
    public SC_FPSController _controller;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Text _timerText;
    [SerializeField] private GameObject _closePauseUI;
    [SerializeField] private GameObject SettingsImageForPC;

    public float times;


    private ControlProgress _controlProgress;
    private bool _isDesktop;

    private bool _lose = false;
    private float timer = 0;

    private void Start()
    {
        YandexGame.SwitchLanguage(YandexGame.savesData.language);
        _controller.lookSpeed = YandexGame.savesData.Sensivity;
        _sensivitySlider.value = (YandexGame.savesData.Sensivity / 4);
        SwitchQuality();
        _controlProgress = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControlProgress>();
        _controlProgress.uIBehaviour = this;
        ChangeProgressbar(_controlProgress.LastSave / 73f);
        _isDesktop = _controller.IsDesktop;

        if (_isDesktop)
        {
            SettingsImageForPC.SetActive(true);
        }
        else
        {
            SettingsImageForPC.SetActive(false);
        }
    }


    public void Lose()
    {
        if (_lose) return;

        if (!_isDesktop)
        {
            for (int i = 0; i < _closePauseUI.transform.childCount; i++)
            {
                _closePauseUI.transform.GetChild(i).gameObject.SetActive(false);
            }


        }
        else
        {
            SettingsImageForPC.SetActive(false);
        }

        _controller.characterController.enabled = false;
        _controller.rb.isKinematic = true;
        _progressBar.gameObject.SetActive(false);
        _lose = true;
        _loseScreen.SetActive(true);
        _controller.Pause = true;
        _controller.rb.isKinematic = true;

        if (YandexGame.EnvironmentData.isDesktop)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ChangeProgressbar(float value)
    {
        _progressBar.value = value;
    }

    private void Update()
    {
        times = YandexGame.TimeToAd;

        if (Input.GetKeyDown(KeyCode.Tab) && !_lose)
        {
            OpenClosePause();
        }

        if(timer > 70)
        {
            timer = 0;
            StartCoroutine(WaitTime());
        }

        if (!_controller.Pause)
        {
            timer += Time.deltaTime;
        }
    }

    public void RespawnButton()
    {
        _controlProgress.Spawn(true);
        Destroy(_controller.gameObject);
        timer = 0;
        if (YandexGame.timerShowAd > 60)
        {
            AudioListener.volume = 0;
            YandexGame.FullscreenShow();
        }
        _controller.Pause = false;
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
            _controller.characterController.enabled = true;
            _controller.rb.isKinematic = false;

            if (_isDesktop)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _closePauseUI.transform.GetChild(0).gameObject.SetActive(true);
                SettingsImageForPC.SetActive(true);
            }
            else
            {
                for (int i = 0; i < _closePauseUI.transform.childCount; i++)
                {
                    _closePauseUI.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {

            _pausePanel.SetActive(true);
            _controller.Pause = true;
            _controller.characterController.enabled = false;
            _controller.rb.isKinematic = true;

            if (_isDesktop)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SettingsImageForPC.SetActive(false);
                _closePauseUI.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < _closePauseUI.transform.childCount; i++)
                {
                    _closePauseUI.transform.GetChild(i).gameObject.SetActive(false);
                }
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
        if (times >= 0) 
        {
            StopCoroutine(WaitTime());        
        }
        _timerText.transform.parent.gameObject.SetActive(true);
        _controller.Pause = true;
        _timerText.gameObject.SetActive(true);
        _controller.rb.isKinematic = true;
        _controller.characterController.enabled = false;

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
        _controller.characterController.enabled = true;
        _controller.rb.isKinematic = false;
        _timerText.gameObject.SetActive(false);
        _timerText.transform.parent.gameObject.SetActive(false);
        _controller.Pause = false;
        YandexGame.FullscreenShow();
    }
}
