using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialProgress : MonoBehaviour
{
    [Header("Referencias")]
    public Image circuloImagen;

    [Header("Configuración")]
    public float duracionCiclo = 4f;
    public bool iniciarAutomaticamente = false;
    public bool repetirInfinito = false;

    [Header("Colores")]
    public Color colorCirculo = Color.white;

    void Start()
    {
        if (circuloImagen != null)
        {
            circuloImagen.type = Image.Type.Filled;
            circuloImagen.fillMethod = Image.FillMethod.Radial360;
            circuloImagen.fillOrigin = (int)Image.Origin360.Top;
            circuloImagen.fillClockwise = true;
            circuloImagen.fillAmount = 1f;
            circuloImagen.color = colorCirculo;
        }

        if (iniciarAutomaticamente)
        {
            IniciarProgress();
        }
    }

    public void IniciarProgress()
    {
        StopAllCoroutines();
        StartCoroutine(AnimarProgress());
    }

    public void ReiniciarProgress()
    {
        if (circuloImagen != null)
        {
            circuloImagen.fillAmount = 1f;
        }
        IniciarProgress();
    }

    IEnumerator AnimarProgress()
    {
        do
        {
            float tiempoTranscurrido = 0f;

            while (tiempoTranscurrido < duracionCiclo)
            {
                tiempoTranscurrido += Time.deltaTime;
                float progreso = tiempoTranscurrido / duracionCiclo;

                if (circuloImagen != null)
                {
                    circuloImagen.fillAmount = 1f - progreso;
                }

                yield return null;
            }

            if (circuloImagen != null)
            {
                circuloImagen.fillAmount = 0f;
            }

            if (repetirInfinito && circuloImagen != null)
            {
                circuloImagen.fillAmount = 1f;
            }

        } while (repetirInfinito);
    }

    public void SetDuracion(float nuevaDuracion)
    {
        duracionCiclo = nuevaDuracion;
    }

    public void DetenerProgress()
    {
        StopAllCoroutines();
    }

    public void SetFillAmount(float valor)
    {
        if (circuloImagen != null)
        {
            circuloImagen.fillAmount = Mathf.Clamp01(valor);
        }
    }
}