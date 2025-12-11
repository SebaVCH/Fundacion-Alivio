using UnityEngine;

public class AudioTriggerPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private bool alreadyPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si el que entra es el XR Origin (el jugador)
        if (!alreadyPlayed && other.CompareTag("Player"))
        {
            audioSource.Play();
            alreadyPlayed = true;
        }
    }
}
