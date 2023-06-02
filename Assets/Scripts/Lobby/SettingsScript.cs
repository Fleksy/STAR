using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Toggle toggle;
    public AudioMixer audioMixer;
    public Slider slider;
    private bool _isFullScreen = true;

    public void SetSettings()
    {
        slider.value = (PlayerPrefs.GetFloat("volume"));
        AudioVolume();
        toggle.SetIsOnWithoutNotify((PlayerPrefs.GetInt("screenMode") == 1));
        Screen.fullScreen = (PlayerPrefs.GetInt("screenMode") == 1);
        _isFullScreen =  (PlayerPrefs.GetInt("screenMode") == 1);
    }
    
    public void ScreenMode()
    {
        _isFullScreen = !_isFullScreen;
        Screen.fullScreen = _isFullScreen;
    }

    public void AudioVolume()
    {
        audioMixer.SetFloat("Music", slider.value);
    }

    public void Back()
    {
        float volume;
        audioMixer.GetFloat("Music", out volume);
        PlayerPrefs.SetFloat("volume",volume);
        PlayerPrefs.SetInt("screenMode",_isFullScreen?1:0);
    }
}
