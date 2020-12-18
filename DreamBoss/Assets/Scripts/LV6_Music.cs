using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LV6_Music : LevelBase
{
    [Header("所有音效")]
    public AudioClip[] sounds;
    [Header("音符特效")]
    public ParticleSystem psNote;

    private int indexCorrect;
    private int indexClick;

    private void Start()
    {
        StartCoroutine(Question(2));
    }

    protected override IEnumerator Question(float delayStart)
    {
        yield return new WaitForSeconds(1.5f);

        indexCorrect = Random.Range(0, sounds.Length);
        aud.PlayOneShot(sounds[indexCorrect]);
        psNote.Play();                                  // 播放音符特效

        yield return base.Question(sounds[indexCorrect].length);
    }

    protected override void StartGame()
    {
        base.StartGame();
    }

    public void ClickMusicalInstrument(int index)
    {
        indexClick = index;

        if (index == indexCorrect) StartCoroutine(Correct(index));
        else StartCoroutine(Wrong());
    }

    public void ClickMusicInstrument(Button btn)
    {
        if (indexClick != indexCorrect) btn.interactable = false;
    }
}
