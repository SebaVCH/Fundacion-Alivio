using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MetronomoVRSimple : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text textoContador;
    public RawImage circuloIndicador;  // Cambiar a RawImage
    
    [Header("Configuracion")]
    public int numeroTiempos = 4;
    public float duracionPorTiempo = 1f;
    
    [Header("Seguir Camara")]
    public Transform camaraVR;
    public float distancia = 1.5f;
    public Vector3 offset = new Vector3(0.5f, 0.4f, 0);
    public float suavizado = 5f;
    
    [Header("Colores")]
    public Color colorActivo = Color.green;
    public Color colorInactivo = Color.gray;
    
    [Header("Texturas")]
    public Texture2D texturaActiva;  // Nueva variable para textura activa
    public Texture2D texturaInactiva; // Nueva variable para textura inactiva
    
    private int tiempoActual = 1;
    
    void Start()
    {
        if (camaraVR == null)
        {
            camaraVR = Camera.main.transform;
        }
        
        StartCoroutine(MetronomoInfinito());
    }
    
    void LateUpdate()
    {
        if (camaraVR == null) return;
        
        Vector3 posicionObjetivo = camaraVR.position 
            + camaraVR.forward * distancia 
            + camaraVR.right * offset.x 
            + camaraVR.up * offset.y;
        
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * suavizado);
        
        Vector3 direccion = camaraVR.position - transform.position;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(-direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * suavizado);
    }
    
    IEnumerator MetronomoInfinito()
    {
        while (true)
        {
            tiempoActual = 1;
            
            while (tiempoActual <= numeroTiempos)
            {
                if (textoContador != null)
                {
                    textoContador.text = tiempoActual.ToString();
                }
                
                if (circuloIndicador != null)
                {
                    circuloIndicador.texture = texturaActiva;  // Cambiar la textura a la activa
                }
                
                yield return new WaitForSeconds(duracionPorTiempo * 0.15f);
                
                if (circuloIndicador != null)
                {
                    circuloIndicador.texture = texturaInactiva;  // Cambiar la textura a la inactiva
                }
                
                yield return new WaitForSeconds(duracionPorTiempo * 0.85f);
                
                tiempoActual++;
            }
        }
    }
}
