using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class LevelBase : MonoBehaviour
{
    [Header("互動物件")]
    public Transform interactable;
    [Header("結束畫面")]
    public Image final;
    [Header("正確與錯誤音效")]
    public AudioClip soundCorrect;
    public AudioClip soundWrong;
    [Header("正確特效")]
    public RectTransform correctRect;
    public ParticleSystem correctParticle;

    protected Transform canvas;
    protected AudioSource aud;
    
    public  static int winCount;

    private void Awake()
    {
        canvas = GameObject.Find("畫布").transform;
        aud = GetComponent<AudioSource>();

        InteractableSwitch(false);
    }

    /// <summary>
    /// 轉換互動物件
    /// </summary>
    /// <param name="interactableSwitch">能否互動</param>
    private void InteractableSwitch(bool interactableSwitch)
    {
        for (int i = 0; i < interactable.childCount; i++)
            interactable.GetChild(i).GetComponent<Button>().interactable = interactableSwitch;
    }

    /// <summary>
    /// 問題
    /// </summary>
    /// <param name="delayStart">延遲開始</param>
    protected virtual void Question(float delayStart)
    {
        Invoke("StartGame", delayStart);
    }

    /// <summary>
    /// 開始遊戲：啟動互動
    /// </summary>
    protected virtual void StartGame()
    {
        InteractableSwitch(true);
    }

    /// <summary>
    /// 勝利
    /// </summary>
    protected virtual IEnumerator Win(int index)
    {
        winCount++;
        aud.PlayOneShot(soundCorrect);
        correctParticle.Play();
        final.raycastTarget = true;
        StartCoroutine(ShowCorrectObject(index));

        while (final.color.a < 0.5f)
        {
            final.color += new Color(0, 0, 0, 0.1f) * Time.deltaTime * 5;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        if (winCount < 5) Replay();
        else WinPanel();
    }

    private IEnumerator ShowCorrectObject(int index)
    {
        RectTransform rect = interactable.GetChild(index).GetComponent<RectTransform>();
        rect.SetParent(canvas);
        rect.GetComponent<Image>().raycastTarget = false;
        correctRect.SetAsLastSibling();

        while (rect.anchoredPosition != new Vector2(0, 120))
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, 120), 10 * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 失敗
    /// </summary>
    protected virtual IEnumerator Lose()
    {
        aud.PlayOneShot(soundWrong, 2);
        final.raycastTarget = true;

        while (final.color.a < 0.5f)
        {
            final.color += new Color(0, 0, 0, 0.1f) * Time.deltaTime * 5;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        Replay();
    }

    private void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void WinPanel()
    {
        SceneManager.LoadScene("選取關卡");
    }
}
