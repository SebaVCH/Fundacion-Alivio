using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MetronomoVRSimple : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text textoContador;
    public RawImage circuloIndicador;
    
    [Header("Configuracion")]
    public int numeroTiempos = 4;
    public float duracionPorTiempo = 1f;
    
    [Header("Seguir Camara")]
    public Transform camaraVR;
    public float distancia = 1.5f;
    public Vector3 offset = new Vector3(0.5f, 0.4f, 0);
    public bool usarJerarquia = true; // NUEVA OPCIÓN
    
    [Header("Colores")]
    public Color colorActivo = Color.green;
    public Color colorInactivo = Color.gray;
    
    [Header("Texturas")]
    public Texture2D texturaActiva;
    public Texture2D texturaInactiva;
    
    private int tiempoActual = 1;
    private bool inicializado = false;
    
    void Start()
    {
        if (camaraVR == null)
        {
            camaraVR = Camera.main.transform;
        }
        
        // Si usarJerarquia está activado, hacer este objeto hijo de la cámara
        if (usarJerarquia && camaraVR != null)
        {
            transform.SetParent(camaraVR);
            transform.localPosition = new Vector3(offset.x, offset.y, distancia);
            transform.localRotation = Quaternion.identity;
            inicializado = true;
        }
        
        StartCoroutine(MetronomoInfinito());
    }
    
    void Update()
    {
        // Solo actualizar posición si NO es hijo de la cámara
        if (!usarJerarquia && camaraVR != null)
        {
            Vector3 posicionObjetivo = camaraVR.position 
                + camaraVR.forward * distancia 
                + camaraVR.right * offset.x 
                + camaraVR.up * offset.y;
            
            transform.position = posicionObjetivo;
            
            Vector3 direccion = camaraVR.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-direccion);
        }
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
                    circuloIndicador.texture = texturaActiva;
                }
                
                yield return new WaitForSeconds(duracionPorTiempo * 0.15f);
                
                if (circuloIndicador != null)
                {
                    circuloIndicador.texture = texturaInactiva;
                }
                
                yield return new WaitForSeconds(duracionPorTiempo * 0.85f);
                
                tiempoActual++;
            }
        }
    }
}