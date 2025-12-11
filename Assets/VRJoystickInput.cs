using UnityEngine;
using UnityEngine.XR;

public class VRJoystickInput : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform vrCamera;
    [SerializeField] private Transform playerRoot;
    
    [Header("Configuracion Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float gravedad = -15f;
    [SerializeField] private bool permitirTecladoTambien = true;
    
    private InputDevice dispositivoIzquierdo;
    private Vector2 joystickInput;
    private float velocidadVertical;

    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (vrCamera == null)
        {
            vrCamera = Camera.main.transform;
        }

        InicializarDispositivoVR();
    }

    void Update()
    {
        LeerInputJoystick();
        AplicarMovimiento();
        AplicarGravedad();
    }

    void LeerInputJoystick()
    {
        joystickInput = Vector2.zero;

        if (!dispositivoIzquierdo.isValid)
        {
            InicializarDispositivoVR();
        }

        if (dispositivoIzquierdo.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 input))
        {
            if (input.magnitude > 0.1f)
            {
                joystickInput = input;
            }
        }

        if (permitirTecladoTambien && joystickInput.magnitude < 0.1f)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            joystickInput = new Vector2(horizontal, vertical);
        }
    }

    void AplicarMovimiento()
    {
        if (joystickInput.magnitude < 0.1f)
        {
            return;
        }

        Vector3 direccionAdelante = vrCamera.forward;
        Vector3 direccionDerecha = vrCamera.right;

        direccionAdelante.y = 0;
        direccionDerecha.y = 0;
        direccionAdelante.Normalize();
        direccionDerecha.Normalize();

        Vector3 direccionMovimiento = (direccionAdelante * joystickInput.y + direccionDerecha * joystickInput.x);
        Vector3 movimiento = direccionMovimiento * velocidadMovimiento * Time.deltaTime;

        characterController.Move(movimiento);
    }

    void AplicarGravedad()
    {
        if (characterController.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }
        else
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }

        Vector3 movimientoVertical = new Vector3(0, velocidadVertical, 0) * Time.deltaTime;
        characterController.Move(movimientoVertical);
    }

    void InicializarDispositivoVR()
    {
        var dispositivos = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, 
            dispositivos
        );

        if (dispositivos.Count > 0)
        {
            dispositivoIzquierdo = dispositivos[0];
            Debug.Log("Dispositivo VR conectado: " + dispositivoIzquierdo.name);
        }
    }

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        velocidadMovimiento = nuevaVelocidad;
    }
}