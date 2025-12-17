using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MetronomoVRSimple : MonoBehaviour
{
    [Header("Componente Hijo")]
    public GameObject child;

    [Header("Referencias UI")]
    public Text textoContador;
    public TextMeshProUGUI textAux;
    public RawImage circuloIndicador;

    [Header("Configuracion")]
    public float numeroTiempos = 4;
    public float duracionPorTiempo = 1.4f;

    [Header("Seguir Camara")]
    public Transform camaraVR;
    public float distancia = 1.5f;
    public Vector3 offset = new Vector3(0.5f, 0.4f, 0);
    public bool usarJerarquia = true;

    [Header("Colores")]
    public Color colorActivo = Color.green;
    public Color colorInactivo = Color.gray;

    [Header("Texturas")]
    public Texture2D texturaActiva;
    public Texture2D texturaInactiva;

    [Header("Progreso Radial")]
    public RadialProgress radialProgress;

    private bool activo = false;        
    private bool activoBucle = false;   
    private int tiempoActual = 1;
    private Coroutine bucleRespiracion;


    private void OnEnable()
    {
        SoundTrigger.OnActivateTimer += StartTimer;
        SoundTrigger.OnDeactivateTimer += StopTimer;
    }

    private void OnDisable()
    {
        SoundTrigger.OnActivateTimer -= StartTimer;
        SoundTrigger.OnDeactivateTimer -= StopTimer;
    }

    public void LlamarBucle(float intervalo)
    {
        activoBucle = true;
        activo = false;

        if (bucleRespiracion != null)
            StopCoroutine(bucleRespiracion);

        if (child != null)
            child.SetActive(true);

        bucleRespiracion = StartCoroutine(
            BucleRespiracion(intervalo, intervalo, intervalo)
        );
    }

    public void DetenerBucle()
    {
        activoBucle = false;
        activo = false;

        if (bucleRespiracion != null)
            StopCoroutine(bucleRespiracion);

        if (radialProgress != null)
            radialProgress.DetenerProgress();

        if (child != null)
            child.SetActive(false);
    }

    IEnumerator BucleRespiracion(float inhalar, float mantener, float exhalar)
    {
        while (activoBucle)
        {
            yield return Fase("INHALAR", inhalar);
            if (!activoBucle) yield break;

            yield return Fase("MANTENER", mantener);
            if (!activoBucle) yield break;

            yield return Fase("EXHALAR", exhalar);
            if (!activoBucle) yield break;

            yield return Fase("MANTENER", mantener);
        }
    }

    IEnumerator Fase(string texto, float duracion)
    {
        if (textAux != null)
            textAux.text = texto;

        float tiempoRestante = duracion;

        if (radialProgress != null)
        {
            radialProgress.SetDuracion(duracion);
            radialProgress.ReiniciarProgress();
        }

        while (tiempoRestante > 0f && activoBucle)
        {
            tiempoRestante -= Time.deltaTime;

            if (textoContador != null)
                textoContador.text = Mathf.CeilToInt(tiempoRestante).ToString();

            yield return null;
        }
    }



    public void StartTimer(float segundos)
    {
        if (activoBucle) return;

        if (child != null)
            child.SetActive(true);

        numeroTiempos = Mathf.RoundToInt(segundos);
        tiempoActual = 1;
        activo = true;

        if (radialProgress != null)
        {
            radialProgress.SetDuracion(segundos);
            radialProgress.IniciarProgress();
        }
    }

    public void StopTimer()
    {
        if (activoBucle) return;

        activo = false;

        if (child != null)
            child.SetActive(false);

        if (radialProgress != null)
            radialProgress.DetenerProgress();
    }


    void Start()
    {
        if (camaraVR == null && Camera.main != null)
            camaraVR = Camera.main.transform;

        if (child != null)
            child.SetActive(false);

        if (usarJerarquia && camaraVR != null)
        {
            transform.SetParent(camaraVR);
            transform.localPosition = new Vector3(offset.x, offset.y, distancia);
            transform.localRotation = Quaternion.identity;
        }

        StartCoroutine(MetronomoInfinito());
    }

    void Update()
    {
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

    IEnumerator MetronomoInfinito()
    {
        while (true)
        {
            yield return new WaitUntil(() => activo && !activoBucle);

            float tiempoRestante = numeroTiempos;

            if (radialProgress != null)
            {
                radialProgress.SetDuracion(numeroTiempos);
                radialProgress.ReiniciarProgress();
            }

            while (activo && !activoBucle && tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;

                if (textoContador != null)
                    textoContador.text = Mathf.CeilToInt(tiempoRestante).ToString();

                yield return null;
            }

            if (!activoBucle)
                StopTimer();
        }
    }


    public void Inhalar(float segundos)
    {
        textAux.text = "INHALAR";
        StartTimer(segundos);
    }

    public void Mantener(float segundos)
    {
        textAux.text = "MANTENER";
        StartTimer(segundos);
    }

    public void Exhalar(float segundos)
    {
        textAux.text = "EXHALAR";
        StartTimer(segundos);
    }
}
