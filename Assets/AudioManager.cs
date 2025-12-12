using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Referencias")]
    public Slider volumeSlider;
    public AudioMixerGroup masterMixer; // Opcional, si usas AudioMixer
    
    private const string VOLUME_KEY = "MasterVolume";
    
    void Awake()
    {
        // Singleton para persistir entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Cargar volumen guardado
        LoadVolume();
    }
    
    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }
    
    public void ChangeVolume(float value)
    {
        // Cambiar el volumen general
        AudioListener.volume = value;
        
        // O si usas AudioMixer:
        // masterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        
        // Guardar el valor
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }
    
    void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
        
        AudioListener.volume = savedVolume;
    }
    
    // Llamar esto cuando cambies de escena y necesites reconectar el slider
    public void RegisterSlider(Slider slider)
    {
        volumeSlider = slider;
        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(ChangeVolume);
    }
}