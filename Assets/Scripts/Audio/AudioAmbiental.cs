using UnityEngine;
using System.Collections;

public class SonidoAmbiental : MonoBehaviour
{
    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    public float audioVolumenSource = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource != null)
        {
            audioVolumenSource = audioSource.volume;
            FadeIn();
        }
    }

    public void StopAudio(float fadeDuration = 1.5f)
    {
        if (audioSource == null || !audioSource.isPlaying)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.volume = audioVolumenSource;
    }

    public void FadeIn(float fadeDuration = 7f)
    {
        if (audioSource == null)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeInRoutine(fadeDuration, audioVolumenSource));
    }

    private IEnumerator FadeInRoutine(float duration, float targetVolume)
    {
        audioSource.volume = 0f;

        if (!audioSource.isPlaying)
            audioSource.Play();

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
