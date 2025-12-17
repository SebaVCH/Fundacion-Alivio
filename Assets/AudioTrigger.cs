using UnityEngine;

public class AudioTriggerPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Eventos Audios")]
    public AudioTimedUnityEvent[] events;

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

    void Update()
    {
        if (!audioSource.isPlaying)
            return;

        foreach (var e in events)
        {
            if (!e.triggered && audioSource.time >= e.time)
            {
                Debug.Log("SE INVOKO UN EVENTO");
                e.triggered = true;
                e.onTrigger.Invoke();
            }
        }
    }
}
