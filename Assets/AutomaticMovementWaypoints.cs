using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutomaticMovementWaypoints : MonoBehaviour
{
    [Header("Configuracion de Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float velocidadMovimiento = 3f;
    [SerializeField] private float distanciaMinima = 2f;
    [SerializeField] private bool cicloInfinito = false;
    [SerializeField] private bool mostrarLineasEnEditor = true;
    
    [Header("Visualizacion en Editor")]
    [SerializeField] private float tamanoEsfera = 0.5f;
    [SerializeField] private bool mostrarNumeros = true;
    [SerializeField] private bool mostrarAltura = true;
    
    [Header("Pausas opcionales")]
    [SerializeField] private bool usarPausas = false;
    [SerializeField] private float tiempoPausaEnCadaPunto = 2f;
    
    [Header("Espera inicial")]
    [SerializeField] private float esperaInicial = 5f; // Espera antes de empezar
    
    private int waypointActualIndex = 0;
    private bool movimientoActivo = false;
    private bool enPausa = false;
    private float tiempoPausaActual = 0f;
    
    public void IniciarRecorrido()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No hay waypoints configurados para el recorrido automatico");
            return;
        }
        
        // Iniciar con espera de 5 segundos
        StartCoroutine(IniciarRecorridoConEspera());
    }
    
    IEnumerator IniciarRecorridoConEspera()
    {
        Debug.Log("Esperando " + esperaInicial + " segundos antes de iniciar recorrido...");
        yield return new WaitForSeconds(esperaInicial);
        
        waypointActualIndex = 0;
        movimientoActivo = true;
        enPausa = false;
        Debug.Log("¡Recorrido automatico iniciado con " + waypoints.Count + " puntos!");
    }
    
    public void DetenerRecorrido()
    {
        movimientoActivo = false;
        Debug.Log("Recorrido automatico detenido");
    }
    
    public Vector2 ObtenerDireccionMovimiento()
    {
        if (!movimientoActivo || waypoints.Count == 0)
        {
            return Vector2.zero;
        }
        
        if (enPausa)
        {
            tiempoPausaActual += Time.deltaTime;
            if (tiempoPausaActual >= tiempoPausaEnCadaPunto)
            {
                enPausa = false;
                tiempoPausaActual = 0f;
                Debug.Log("Pausa terminada, continuando al siguiente waypoint");
            }
            return Vector2.zero;
        }
        
        if (waypointActualIndex >= waypoints.Count)
        {
            if (cicloInfinito)
            {
                waypointActualIndex = 0;
                Debug.Log("Ciclo completado, reiniciando desde el primer waypoint");
            }
            else
            {
                Debug.Log("Recorrido completado, deteniendo movimiento");
                DetenerRecorrido();
                return Vector2.zero;
            }
        }
        
        Transform waypointObjetivo = waypoints[waypointActualIndex];
        
        if (waypointObjetivo == null)
        {
            Debug.LogWarning("Waypoint " + waypointActualIndex + " es null, saltando al siguiente");
            waypointActualIndex++;
            return Vector2.zero;
        }
        
        Vector3 posicionActual = transform.position;
        Vector3 posicionObjetivo = waypointObjetivo.position;
        
        posicionActual.y = posicionObjetivo.y;
        
        float distancia = Vector3.Distance(posicionActual, posicionObjetivo);
        
        Debug.DrawLine(posicionActual, posicionObjetivo, Color.cyan);
        
        if (distancia <= distanciaMinima)
        {
            Debug.Log("✓ Waypoint " + (waypointActualIndex + 1) + " alcanzado! (distancia: " + distancia.ToString("F2") + "m)");
            waypointActualIndex++;
            
            if (usarPausas && waypointActualIndex < waypoints.Count)
            {
                enPausa = true;
                tiempoPausaActual = 0f;
                Debug.Log("Iniciando pausa de " + tiempoPausaEnCadaPunto + " segundos");
                return Vector2.zero;
            }
            
            if (waypointActualIndex >= waypoints.Count)
            {
                if (!cicloInfinito)
                {
                    Debug.Log("Recorrido terminado");
                    DetenerRecorrido();
                    return Vector2.zero;
                }
            }
            
            return Vector2.zero;
        }
        
        Vector3 direccion = (posicionObjetivo - posicionActual).normalized;
        Vector2 direccion2D = new Vector2(direccion.x, direccion.z);
        
        return direccion2D;
    }
    
    public bool EstaEnMovimiento()
    {
        return movimientoActivo;
    }
    
    public int ObtenerWaypointActual()
    {
        return waypointActualIndex;
    }
    
    public int ObtenerTotalWaypoints()
    {
        return waypoints.Count;
    }
    
    void OnDrawGizmos()
    {
        if (waypoints.Count == 0)
            return;
        
        if (mostrarLineasEnEditor && waypoints.Count >= 2)
        {
            Gizmos.color = Color.yellow;
            
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                if (waypoints[i] != null && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
            
            if (cicloInfinito && waypoints[waypoints.Count - 1] != null && waypoints[0] != null)
            {
                Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }
        
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null)
                continue;
            
            Vector3 pos = waypoints[i].position;
            
            if (Application.isPlaying && i == waypointActualIndex && movimientoActivo)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            
            Gizmos.DrawSphere(pos, tamanoEsfera);
            
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawSphere(pos, distanciaMinima);
            
            Gizmos.color = Color.white;
            Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (waypoints.Count == 0)
            return;
        
        #if UNITY_EDITOR
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null)
                continue;
            
            Vector3 pos = waypoints[i].position;
            
            UnityEditor.Handles.color = Color.white;
            
            string label = "Waypoint " + (i + 1);
            if (mostrarAltura)
            {
                label += "\nY: " + pos.y.ToString("F2");
            }
            
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            
            Vector3 labelPos = pos + Vector3.up * 2.5f;
            UnityEditor.Handles.Label(labelPos, label, style);
            
            UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
            UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, distanciaMinima);
        }
        #endif
    }
}