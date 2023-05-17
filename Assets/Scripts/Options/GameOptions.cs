using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle vibrationToggle;

    private float soundVolume;
    private float musicVolume;
    private bool vibrationEnabled;

    private void Start()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;

        soundSlider.value = soundVolume;
        musicSlider.value = musicVolume;
        vibrationToggle.isOn = vibrationEnabled;

    }

    private void Update()
    {
        if (soundVolume != soundSlider.value)
        {
            soundVolume = soundSlider.value;
            PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        }

        if (musicVolume != musicSlider.value)
        {
            musicVolume = musicSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }

        if (vibrationEnabled != vibrationToggle.isOn)
        {
            vibrationEnabled = vibrationToggle.isOn;
            PlayerPrefs.SetInt("VibrationEnabled", vibrationEnabled ? 1 : 0);
        }
    }
}
