using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class TimerCambioEscena : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tiempo en segundos antes de cambiar de escena")]
    public float tiempoEspera = 10f;
    
    [Tooltip("Nombre de la escena a la que regresar")]
    public string nombreEscenaDestino = "UIMenu";
    
    [Header("Opcional - UI")]
    [Tooltip("Texto para mostrar cuenta regresiva (opcional)")]
    public TMPro.TextMeshProUGUI textoCuentaRegresiva;
    
    // EVENTO que otros scripts pueden escuchar
    public static event Action<float> OnTimerActualizado;  // Se dispara cada frame con el tiempo restante
    public static event Action OnTimerCompletado;          // Se dispara cuando el timer termina
    
    private float tiempoRestante;
    private bool timerActivo = false;
    private Coroutine coroutinaTimer;
    
    void Start()
    {
        // Ya no inicia automáticamente
        Debug.Log("[TimerCambioEscena] Timer listo. Llama a IniciarTimer() para comenzar.");
    }
    
    // MÉTODO PRINCIPAL - Llama esto desde cualquier script
    public void IniciarTimer()
    {
        // Detener timer anterior si existe
        if (coroutinaTimer != null)
            StopCoroutine(coroutinaTimer);
        
        timerActivo = true;
        tiempoRestante = tiempoEspera;
        
        Debug.Log($"[TimerCambioEscena] Timer iniciado: {tiempoEspera} segundos hasta volver a {nombreEscenaDestino}");
        
        coroutinaTimer = StartCoroutine(ContadorRegresivo());
    }
    
    IEnumerator ContadorRegresivo()
    {
        while (timerActivo && tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            
            // Actualizar texto si existe
            if (textoCuentaRegresiva != null)
            {
                textoCuentaRegresiva.text = $"Regresando en: {Mathf.CeilToInt(tiempoRestante)}s";
            }
            
            // Disparar evento con tiempo restante
            OnTimerActualizado?.Invoke(tiempoRestante);
            
            yield return null;
        }
        
        // Timer completado
        if (tiempoRestante <= 0f)
        {
            timerActivo = false;
            OnTimerCompletado?.Invoke();
            CambiarEscena();
        }
    }
    
    void CambiarEscena()
    {
        Debug.Log($"[TimerCambioEscena] ¡Tiempo cumplido! Cambiando a escena: {nombreEscenaDestino}");
        SceneManager.LoadScene(nombreEscenaDestino);
    }
    
    // Método público para detener el timer
    public void DetenerTimer()
    {
        timerActivo = false;
        
        if (coroutinaTimer != null)
            StopCoroutine(coroutinaTimer);
        
        Debug.Log("[TimerCambioEscena] Timer detenido manualmente");
    }
    
    // Método público para reiniciar el timer
    public void ReiniciarTimer()
    {
        IniciarTimer();
    }
    
    // Método público para cambiar el tiempo y empezar
    public void IniciarTimerConTiempo(float nuevoTiempo)
    {
        tiempoEspera = nuevoTiempo;
        IniciarTimer();
    }
    
    // Obtener tiempo restante
    public float ObtenerTiempoRestante()
    {
        return tiempoRestante;
    }
    
    // Verificar si está activo
    public bool EstaActivo()
    {
        return timerActivo;
    }
}