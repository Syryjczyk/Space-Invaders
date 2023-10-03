using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI vibrationActive;
    [SerializeField] private AudioMixer audioMixer;

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetVibration(bool isActive)
    {
        if (isActive)
        {
            vibrationActive.text = "on";
            VibrationHandler.SetVibrationEnabled(true);
        }
        else
        {
            vibrationActive.text = "off";
            VibrationHandler.SetVibrationEnabled(false);
        }
    }
}
