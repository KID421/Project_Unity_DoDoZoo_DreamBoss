using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("混音工具")]
    public AudioMixer mixer;
    [Header("滑桿")]
    public Slider sliderBGM;
    public Slider sliderSFX;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化滑桿
    /// </summary>
    private void Init()
    {
        float bgm;
        float sfx;
        mixer.GetFloat("音量_背景音樂", out bgm);
        mixer.GetFloat("音量_音效", out sfx);
        sliderBGM.value = (bgm + 10) / 20;
        sliderSFX.value = (sfx + 10) / 20;
    }

    /// <summary>
    /// 設定背景音樂音量
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetBGM(float volume)
    {
        mixer.SetFloat("音量_背景音樂", 20 * volume - 10);
    }

    /// <summary>
    /// 設定音效音量
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSFX(float volume)
    {
        mixer.SetFloat("音量_音效", 20 * volume - 10);
    }
}
