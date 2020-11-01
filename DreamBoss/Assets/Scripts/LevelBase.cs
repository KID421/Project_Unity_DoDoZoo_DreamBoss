using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    protected AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();

        InteractableSwitch(false);
    }

    private void InteractableSwitch(bool interactableSwitch)
    {
        for (int i = 0; i < interactable.childCount; i++)
            interactable.GetChild(i).GetComponent<Button>().interactable = interactableSwitch;
    }

    protected virtual void Question(float delayStart)
    {
        Invoke("StartGame", delayStart);
    }

    protected virtual void StartGame()
    {
        InteractableSwitch(true);
    }

    protected virtual IEnumerator Win()
    {
        aud.PlayOneShot(soundCorrect);
        final.raycastTarget = true;

        while (final.color.a < 0.5f)
        {
            final.color += new Color(0.2f, 0.2f, 0.2f, 0.1f) * Time.deltaTime * 5;
            yield return null;
        }
    }

    protected virtual IEnumerator Lose()
    {
        aud.PlayOneShot(soundWrong, 2);
        final.raycastTarget = true;

        while (final.color.a < 0.5f)
        {
            final.color += new Color(0, 0, 0, 0.1f) * Time.deltaTime * 5;
            yield return null;
        }
    }
}
