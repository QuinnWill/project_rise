using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerPreferenceEditor
{
    public static UnityEvent<float> VolumeCallback;

    public static void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        VolumeCallback?.Invoke(value);
    }
    public static float GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    public static UnityEvent<float> SensitivityCallback;

    public static void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        SensitivityCallback?.Invoke(value);
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", 1);
    }

    public static UnityEvent<string> PlayerNameCallback;

    public static void SetPlayerName(string value)
    {
        PlayerPrefs.SetString("PlayerName", value);
        PlayerNameCallback?.Invoke(value);
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName", "");
    }
}
