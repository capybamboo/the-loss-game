using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static int status = 0;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    Resolution[] resolutions;
    private float _mouseSpeed = 1f;
    private float _volumeLevel = 0.5f;
    private uint _targetFrameRateValue = 300;

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<UIObject> _playerUIObjects = new();

    [SerializeField] private Dropdown _resolutionDropdown;
    [SerializeField] private Dropdown _qualityDropdown;
    [SerializeField] private InputField _targetFrameRate;
    [SerializeField] private Scrollbar _volume;
    [SerializeField] private Scrollbar _sensitivity;
    [SerializeField] private UnityEngine.UI.Toggle _vSync;
    [SerializeField] private UnityEngine.UI.Toggle _fullscreen;

    private void Start()
    {
        Resume();

        _resolutionDropdown.ClearOptions();
        List<string> options = new();
        resolutions = Screen.resolutions;

        int currentResolutionIndex = 0;
        int i = 0;
        string option;

        _targetFrameRate.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return ValidateChar(text, addedChar);
        };

        foreach (Resolution resolution in resolutions)
        {
            option = resolution.width + "x" + resolution.height + " " + Convert.ToString(Math.Round(resolution.refreshRateRatio.value)) + "Hz";
            options.Add(option);
            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
            i++;
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();

        LoadSettings(currentResolutionIndex);

        _resolutionDropdown.value = currentResolutionIndex;

        _qualityDropdown.value = 3;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_inventory.activeSelf)
            {
                if (_inventory.GetComponent<Inventory>().background.activeSelf && _player.GetComponent<PlayerMovement>().blockMovement && _inventory.GetComponent<Inventory>().currentID == -1)
                {
                    _player.GetComponent<PlayerMovement>().blockMovement = false;
                    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                    UnityEngine.Cursor.visible = false;

                    _inventory.GetComponent<Inventory>().background.SetActive(false);
                    return;
                }
            }
            if (status == 0)
            {
                Pause();
            }
            else if (status == 1)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetInt("vSyncPreference", QualitySettings.vSyncCount);
        PlayerPrefs.SetFloat("MouseSpeed", _mouseSpeed);
        PlayerPrefs.SetFloat("VolumeLevel", _volumeLevel);
        PlayerPrefs.SetInt("TargetFrameRate", Convert.ToInt16(_targetFrameRateValue));
    }

    #region Main Menu
    public void Resume()
    {
        _player.GetComponent<PlayerMovement>().blockMovement = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        pauseMenuUI.SetActive(false);

        foreach (UIObject obj in _playerUIObjects) 
        {
            if (obj.startVisibility)
            {
                obj.gameObject.SetActive(true);
            }
        }
        Time.timeScale = 1f;
        status = 0;
    }

    private void Pause()
    {
        _player.GetComponent<PlayerMovement>().blockMovement = true;

        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;

        pauseMenuUI.SetActive(true);

        foreach (UIObject obj in _playerUIObjects)
        {
            obj.gameObject.SetActive(false);
        }
        settingsMenuUI.SetActive(false);
        Time.timeScale = 0f;
        status = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        int currentResolutionIndex = 0;
        int i = 0;

        foreach (Resolution resolution in resolutions)
        {
            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                break;
            }
            i++;
        }
        LoadSettings(currentResolutionIndex);
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        status = 2;
    }

    public void Save()
    {
        Debug.Log("Save");
    }
    #endregion

    #region Settings Menu
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetvSync(bool isvSync)
    {
        QualitySettings.vSyncCount = isvSync ? 1 : 0;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ExitSettings()
    {
        Debug.Log("going to pause menu");
        Pause();
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        #region Volume Level
        if (PlayerPrefs.HasKey("VolumeLevel"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("VolumeLevel");
            _volumeLevel = PlayerPrefs.GetFloat("VolumeLevel");

            _volume.value = PlayerPrefs.GetFloat("VolumeLevel");
        }
        else
        {
            AudioListener.volume = _volumeLevel;
        }
        #endregion

        #region Resolution Preference
        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            _resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
            currentResolutionIndex = PlayerPrefs.GetInt("ResolutionPreference");
        }
        else
        {
            _resolutionDropdown.value = currentResolutionIndex;
        }
        #endregion

        #region Fullscreen Preference
        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));

            _fullscreen.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }
        else
        {
            Screen.fullScreen = true;
        }
        #endregion

        #region vSync Preference
        if (PlayerPrefs.HasKey("vSyncPreference"))
        {
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("vSyncPreference");
            
            _vSync.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("vSyncPreference"));
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        #endregion

        #region Mouse Speed
        if (PlayerPrefs.HasKey("MouseSpeed"))
        {
            _mouseSpeed = PlayerPrefs.GetFloat("MouseSpeed");

            _sensitivity.value = PlayerPrefs.GetFloat("MouseSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("MouseSpeed", _mouseSpeed);
        }
        #endregion

        #region Target Frame Rate
        if (PlayerPrefs.HasKey("TargetFrameRate"))
        {
            _targetFrameRateValue = Convert.ToUInt16(PlayerPrefs.GetInt("TargetFrameRate"));

            Application.targetFrameRate = Convert.ToInt16(_targetFrameRateValue);

            _targetFrameRate.text = System.Convert.ToString(_targetFrameRateValue);
        }
        else
        {
            _targetFrameRateValue = 999;
            Application.targetFrameRate = 999;
        }
        #endregion
    }

    public void OnVolumeScrollbarChanged(float value)
    {
        AudioListener.volume = value;
        _volumeLevel = value;
    }

    public void OnSensitivityScrollbarChanged(float value)
    {
        _mouseSpeed = value;
    }

    public void OnTargetFrameRateChanged(string value)
    {
        uint integerValue = Convert.ToUInt16(value);

        if (integerValue < 10)
        {
            integerValue = 10;
            _targetFrameRate.text = "10";
        }
        _targetFrameRateValue = integerValue;

        Application.targetFrameRate = Convert.ToInt16(integerValue);
    }

    private char ValidateChar(string text, char addedChar)
    {
        if (text.Length > 2)
        {
            return '\0';
        }

        if (int.TryParse(Convert.ToString(addedChar), out _))
        {
            return Convert.ToChar(addedChar);
        }
        return '\0';
    }
    #endregion
}

[System.Serializable]
class UIObject
{
    public bool startVisibility = true;
    public GameObject gameObject;
}