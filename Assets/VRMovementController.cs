using UnityEngine;
using UnityEngine.XR;

public class VRMovementController : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    public float velocidadMovimiento = 3f;
    public bool movimientoSuave = true;
    public float suavizado = 10f;

    [Header("Referencias")]
    public Transform cameraRig;
    public Transform cameraTransform;

    private InputDevice dispositivoIzquierdo;
    private Vector2 inputJoystick;
    private Vector3 velocidadActual;

    void Start()
    {
        if (cameraRig == null)
        {
            cameraRig = transform;
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        InicializarDispositivo();
    }

    void Update()
    {
        if (!dispositivoIzquierdo.isValid)
        {
            InicializarDispositivo();
        }

        if (dispositivoIzquierdo.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputJoystick))
        {
            MoverJugador(inputJoystick);
        }
    }

    void InicializarDispositivo()
    {
        var dispositivos = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, dispositivos);

        if (dispositivos.Count > 0)
        {
            dispositivoIzquierdo = dispositivos[0];
        }
    }

    void MoverJugador(Vector2 input)
    {
        if (input.magnitude < 0.1f)
        {
            if (movimientoSuave)
            {
                velocidadActual = Vector3.Lerp(velocidadActual, Vector3.zero, Time.deltaTime * suavizado);
            }
            return;
        }

        Vector3 direccionAdelante = cameraTransform.forward;
        Vector3 direccionDerecha = cameraTransform.right;

        direccionAdelante.y = 0;
        direccionDerecha.y = 0;
        direccionAdelante.Normalize();
        direccionDerecha.Normalize();

        Vector3 direccionMovimiento = (direccionAdelante * input.y + direccionDerecha * input.x);
        Vector3 movimiento = direccionMovimiento * velocidadMovimiento * Time.deltaTime;

        if (movimientoSuave)
        {
            velocidadActual = Vector3.Lerp(velocidadActual, movimiento, Time.deltaTime * suavizado);
            cameraRig.position += velocidadActual;
        }
        else
        {
            cameraRig.position += movimiento;
        }
    }

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        velocidadMovimiento = nuevaVelocidad;
    }
}