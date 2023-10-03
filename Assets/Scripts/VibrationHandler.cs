using UnityEngine;
using CandyCoded.HapticFeedback;

public static class VibrationHandler
{
    private static bool isVibrationEnabled = true;

    public static void SetVibrationEnabled(bool isEnabled)
    {
        isVibrationEnabled = isEnabled;
    }

    public static void DefaultVibration()
    {
        if (isVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }

    public static void LightVibration()
    {
        if (isVibrationEnabled)
        {
            HapticFeedback.LightFeedback();
        }  
    }

    public static void MediumVibration()
    {
        if (isVibrationEnabled)
        {
            HapticFeedback.MediumFeedback();
        }
    }

    public static void HeavyVibration()
    {
        if (isVibrationEnabled)
        {
            HapticFeedback.HeavyFeedback();
        }
    }
}
