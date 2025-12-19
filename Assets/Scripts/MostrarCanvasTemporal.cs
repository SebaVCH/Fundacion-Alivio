using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class MostrarCanvasTemporal : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject canvas;  // El Canvas completo
    public TextMeshProUGUI texto;  // El texto (opcional)
    
    [Header("Configuración")]
    public float tiempoVisible = 10f;  // Segundos que estará visible
    public string mensajePorDefecto = "Mensaje temporal";
    
    [Header("Animaciones")]
    public float duracionFade = 0.5f;  // Duración del fade in/out
    
    [Header("Seguir Cámara")]
    public Transform camaraVR;
    public float distancia = 1.5f;
    public Vector3 offset = new Vector3(0.5f, 0.4f, 0);
    public bool usarJerarquia = true;
    
    // EVENTOS que otros pueden escuchar
    public static event Action OnCanvasMostrado;
    public static event Action OnCanvasOcultado;
    
    private Coroutine coroutinaOcultar;
    private CanvasGroup canvasGroup;
    
    void Start()
    {
        // Configurar cámara
        if (camaraVR == null && Camera.main != null)
            camaraVR = Camera.main.transform;
        
        // Obtener o crear CanvasGroup
        if (canvas != null)
        {
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = canvas.AddComponent<CanvasGroup>();
            }
        }
        
        // Ocultar al inicio
        if (canvas != null)
        {
            canvas.SetActive(false);
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }
        
        // Hacer hijo de la cámara si está configurado
        if (usarJerarquia && camaraVR != null)
        {
            transform.SetParent(camaraVR);
            transform.localPosition = new Vector3(offset.x, offset.y, distancia);
            transform.localRotation = Quaternion.identity;
        }
        
        Debug.Log("[MostrarCanvasTemporal] Script listo. Llama a MostrarCanvas() para activar.");
    }
    
    void Update()
    {
        // Solo actualizar posición si NO es hijo de la cámara
        if (!usarJerarquia && camaraVR != null)
        {
            Vector3 posicionObjetivo =
                camaraVR.position +
                camaraVR.forward * distancia +
                camaraVR.right * offset.x +
                camaraVR.up * offset.y;
            
            transform.position = posicionObjetivo;
            transform.rotation = Quaternion.LookRotation(transform.position - camaraVR.position);
        }
    }
    
    // ═══════════════════════════════════════════════════
    // FUNCIÓN PRINCIPAL - LLAMA ESTA DESDE FUERA
    // ═══════════════════════════════════════════════════
    public void MostrarCanvas()
    {
        MostrarCanvasConMensaje(mensajePorDefecto);
    }
    
    // Versión con mensaje personalizado
    public void MostrarCanvasConMensaje(string mensaje)
    {
        Debug.Log($"[MostrarCanvasTemporal] Mostrando canvas por {tiempoVisible} segundos");
        
        // Detener coroutine anterior si existe
        if (coroutinaOcultar != null)
            StopCoroutine(coroutinaOcultar);
        
        // Actualizar texto si existe
        if (texto != null)
            texto.text = mensaje;
        
        // Iniciar animación completa
        coroutinaOcultar = StartCoroutine(AnimacionCompleta());
    }
    
    IEnumerator AnimacionCompleta()
    {
        // Activar canvas
        if (canvas != null)
            canvas.SetActive(true);
        
        // FADE IN
        yield return StartCoroutine(FadeIn());
        
        // Disparar evento
        OnCanvasMostrado?.Invoke();
        
        // Esperar tiempo visible
        yield return new WaitForSeconds(tiempoVisible);
        
        // FADE OUT
        yield return StartCoroutine(FadeOut());
        
        // Ocultar canvas
        if (canvas != null)
            canvas.SetActive(false);
        
        // Disparar evento
        OnCanvasOcultado?.Invoke();
        
        Debug.Log("[MostrarCanvasTemporal] Ocultando canvas");
    }
    
    IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;
        
        float tiempo = 0f;
        
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    IEnumerator FadeOut()
    {
        if (canvasGroup == null) yield break;
        
        float tiempo = 0f;
        
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }
    
    // Método para ocultar manualmente
    public void OcultarCanvas()
    {
        if (coroutinaOcultar != null)
            StopCoroutine(coroutinaOcultar);
        
        StartCoroutine(OcultarConFade());
    }
    
    IEnumerator OcultarConFade()
    {
        yield return StartCoroutine(FadeOut());
        
        if (canvas != null)
            canvas.SetActive(false);
        
        OnCanvasOcultado?.Invoke();
        
        Debug.Log("[MostrarCanvasTemporal] Canvas ocultado manualmente");
    }
    
    // Cambiar el tiempo de visibilidad
    public void SetTiempoVisible(float nuevoTiempo)
    {
        tiempoVisible = nuevoTiempo;
        Debug.Log($"[MostrarCanvasTemporal] Tiempo visible actualizado a {nuevoTiempo}s");
    }
}