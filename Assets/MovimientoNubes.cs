using UnityEngine;

public class MovimientoNubes : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    [SerializeField] private Vector3 direccionMovimiento = new Vector3(1f, 0f, 0f); // Dirección X por defecto
    [SerializeField] private float velocidad = 0.5f;
    [SerializeField] private float duracionMovimiento = 400f; // 5 minutos = 300 segundos
    
    [Header("Opciones")]
    [SerializeField] private bool moverAlIniciar = true;
    
    private float tiempoTranscurrido = 0f;
    private bool estaMoviendo = false;
    private Vector3 posicionInicial;
    
    void Start()
    {
        posicionInicial = transform.position;
        
        if (moverAlIniciar)
        {
            IniciarMovimiento();
        }
    }
    
    void Update()
    {
        if (!estaMoviendo) return;
        
        // Mover las nubes
        transform.position += direccionMovimiento.normalized * velocidad * Time.deltaTime;
        
        // Contar tiempo
        tiempoTranscurrido += Time.deltaTime;
        
        // Detener después de 5 minutos
        if (tiempoTranscurrido >= duracionMovimiento)
        {
            DetenerMovimiento();
        }
    }
    
    public void IniciarMovimiento()
    {
        estaMoviendo = true;
        tiempoTranscurrido = 0f;
        Debug.Log("Nubes comenzaron a moverse durante " + duracionMovimiento + " segundos");
    }
    
    public void DetenerMovimiento()
    {
        estaMoviendo = false;
        Debug.Log("Movimiento de nubes detenido después de " + tiempoTranscurrido.ToString("F1") + " segundos");
    }
    
    public void ReiniciarPosicion()
    {
        transform.position = posicionInicial;
        tiempoTranscurrido = 0f;
    }
}