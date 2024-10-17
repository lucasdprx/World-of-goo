using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private Toggle _toggleFullScreen;
    public AudioMixer _audioMixer;

    [SerializeField] private Slider _sliderSFX;
    [SerializeField] private Slider _sliderMusic;
    void OnEnable()
    {
        _toggleFullScreen.isOn = Screen.fullScreen;

        if (PlayerPrefs.HasKey("SFX"))
            _sliderSFX.value = PlayerPrefs.GetFloat("SFX");

        if (PlayerPrefs.HasKey("Music"))
            _sliderMusic.value = PlayerPrefs.GetFloat("Music");

    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Music", _sliderMusic.value);
        PlayerPrefs.SetFloat("SFX", _sliderSFX.value);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetVolumeSFX(float volume)
    {
        _audioMixer.SetFloat("SFX", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        _audioMixer.SetFloat("Music", volume);
    }
}
