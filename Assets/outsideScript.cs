using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class outsideScript : MonoBehaviour
{

    [SerializeField]
    private AudioSource soundEffect;
    [SerializeField]
    private AudioSource voiceline;
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private Text subtitles;
    [SerializeField]
    private Text tooltip;

    [SerializeField]
    private Image fadeScreen;

    // for whether we want a fade out or in
    private float desiredAlpha;
    private float currentAlpha;

    IEnumerator voiceover2()
    {
        Debug.Log("into");
        // remove tooltip for the time being
        tooltip.gameObject.SetActive(false);

        if (info.tooltipsEnabled)
        {
            subtitles.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1);
        voiceline.clip = (AudioClip)Resources.Load("voiceline4");
        voiceline.Play();
        yield return new WaitForSeconds(0.4f);
        subtitles.text = "“NOOOOOOOOOO!”";
        yield return new WaitForSeconds(2);
        subtitles.text = "“I'll get you next time!!”";
        yield return new WaitForSeconds(3);
        subtitles.text = "";
        yield return new WaitForSeconds(1);

        if (info.tooltipsEnabled)
        {
            // fade in objective
            Color c = tooltip.color;
            c.a = 0f;
            tooltip.color = c;

            tooltip.gameObject.SetActive(true);

            Debug.Log("hi");
            for (float t = 0f; c.a < 1f; t += Time.deltaTime)
            {
                c.a = c.a + 0.005f;
                tooltip.color = c;
                yield return null;
                Debug.Log("hi2");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // fade
        desiredAlpha = 0f;
        // sound
        StartCoroutine(voiceover2());
        music.clip = (AudioClip)Resources.Load("2. Cogitation - Dungeon Dash OST");
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // fading stuff
        if (desiredAlpha != fadeScreen.color.a)
        {
            currentAlpha = Mathf.MoveTowards(fadeScreen.color.a, desiredAlpha, 0.5f * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, currentAlpha);
        }
    }
}
