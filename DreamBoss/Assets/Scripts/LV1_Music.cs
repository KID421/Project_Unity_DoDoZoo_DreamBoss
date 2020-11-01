using UnityEngine;

public class LV1_Music : LevelBase
{
    [Header("所有音效")]
    public AudioClip[] sounds;

    private int indexCorrect;

    private void Start()
    {
        Question(2);
    }

    protected override void Question(float delayStart)
    {
        int r = Random.Range(0, sounds.Length);

        indexCorrect = r;

        aud.PlayOneShot(sounds[r]);

        base.Question(sounds[r].length);
    }

    protected override void StartGame()
    {
        base.StartGame();
    }

    public void ClickMusicalInstrument(int index)
    {
        if (index == indexCorrect) StartCoroutine(Win());
        else StartCoroutine(Lose());
    }
}
