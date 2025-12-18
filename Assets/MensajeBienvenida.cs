using UnityEngine;
using TMPro;
using System.Collections;

public class MensajeBienvenida : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject panelMensaje;
    [SerializeField] private TextMeshProUGUI textoMensaje;
    
    [Header("Configuracion")]
    [SerializeField] private string mensaje = "Bienvenido a Calendula";
    [SerializeField] private float duracionMensaje = 5f;
    
    [Header("Seguir Camara VR")]
    [SerializeField] private Transform camaraVR;
    [SerializeField] private float distancia = 2f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, 0f);
    [SerializeField] private float suavizado = 10f;
    
    private bool mostrandoMensaje = false;
    
    void Start()
    {
        // Buscar la MainCamera si no está asignada
        if (camaraVR == null)
        {
            GameObject mainCameraObj = GameObject.Find("MainCamera");
            if (mainCameraObj != null)
            {
                camaraVR = mainCameraObj.transform;
            }
            else
            {
                camaraVR = Camera.main.transform;
            }
        }
        
        MostrarMensaje();
    }
    
    void LateUpdate()
    {
        if (!mostrandoMensaje || camaraVR == null) return;
        
        // Posicionar el Canvas frente a la cámara
        Vector3 posicionObjetivo = camaraVR.position 
            + camaraVR.forward * distancia 
            + camaraVR.right * offset.x 
            + camaraVR.up * offset.y;
        
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * suavizado);
        
        // Hacer que siempre mire a la cámara
        Vector3 direccion = camaraVR.position - transform.position;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(-direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * suavizado);
    }
    
    void MostrarMensaje()
    {
        mostrandoMensaje = true;
        
        if (panelMensaje != null)
        {
            panelMensaje.SetActive(true);
        }
        
        if (textoMensaje != null)
        {
            textoMensaje.text = mensaje;
        }
        
        // Posicionar inmediatamente frente a la cámara
        if (camaraVR != null)
        {
            Vector3 posicionInicial = camaraVR.position 
                + camaraVR.forward * distancia 
                + camaraVR.right * offset.x 
                + camaraVR.up * offset.y;
            
            transform.position = posicionInicial;
            transform.LookAt(camaraVR);
            transform.Rotate(0, 180, 0);
        }
        
        StartCoroutine(OcultarMensajeDespuesDeTiempo());
    }
    
    IEnumerator OcultarMensajeDespuesDeTiempo()
    {
        yield return new WaitForSeconds(duracionMensaje);
        
        mostrandoMensaje = false;
        
        if (panelMensaje != null)
        {
            panelMensaje.SetActive(false);
        }
        
        Debug.Log("Mensaje de bienvenida ocultado");
    }
}