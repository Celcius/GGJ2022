using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFadeIn : MonoBehaviour
{
    [SerializeField]
    private float secondsToFade = 5.0f;

    [SerializeField]
    private float finalVolume = 0.5f;
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsed = 0;
        float delta = finalVolume/secondsToFade;
        source.Play();
        source.volume = 0.0f;

        while(elapsed < secondsToFade)
        {
            yield return new WaitForEndOfFrame();
            source.volume += Mathf.Clamp(Time.deltaTime * delta, 0, 1);
            elapsed += Time.deltaTime;
        }

        source.volume = finalVolume;
    }
}
