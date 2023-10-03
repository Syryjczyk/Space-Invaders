using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle vibrationToggle;

    private float _soundVolume;
    private float _musicVolume;
    private bool _vibrationEnabled;

    private void Start()
    {
        _soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        _vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;

        soundSlider.value = _soundVolume;
        musicSlider.value = _musicVolume;
        vibrationToggle.isOn = _vibrationEnabled;

    }

    private void Update()
    {
        if (_soundVolume != soundSlider.value)
        {
            _soundVolume = soundSlider.value;
            PlayerPrefs.SetFloat("SoundVolume", _soundVolume);
        }

        if (_musicVolume != musicSlider.value)
        {
            _musicVolume = musicSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        }

        if (_vibrationEnabled != vibrationToggle.isOn)
        {
            _vibrationEnabled = vibrationToggle.isOn;
            PlayerPrefs.SetInt("VibrationEnabled", _vibrationEnabled ? 1 : 0);
        }
    }
}
