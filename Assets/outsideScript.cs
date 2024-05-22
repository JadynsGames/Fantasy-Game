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
    private Image fadeScreen;

    // for whether we want a fade out or in
    private float desiredAlpha;
    private float currentAlpha;

    // Start is called before the first frame update
    void Start()
    {
        // fade
        desiredAlpha = 0f;
        // sound
        soundEffect.clip = (AudioClip)Resources.Load("voiceline4");
        soundEffect.Play();
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
