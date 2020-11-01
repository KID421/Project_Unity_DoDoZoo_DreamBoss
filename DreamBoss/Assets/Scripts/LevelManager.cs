using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 載入關卡管理器與音效管理
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("按鈕音效")]
    public AudioClip soundButton;

    /// <summary>
    /// 要載入的場景名稱
    /// </summary>
    private string nameScene;
    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 延遲載入場景並播放音效
    /// </summary>
    /// <param name="delay"></param>
    public void DelayLoadScene(string nameScene)
    {
        aud.PlayOneShot(soundButton);
        this.nameScene = nameScene;
        Invoke("LoadScene", 1f);
    }

    /// <summary>
    /// 延遲離開遊戲並播放音效
    /// </summary>
    public void DelayQuit()
    {
        aud.PlayOneShot(soundButton);
        Invoke("Quit", 1f);
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    private void LoadScene()
    {
        SceneManager.LoadScene(nameScene);
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void Quit()
    {
        Application.Quit();
    }
}
