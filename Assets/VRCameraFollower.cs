using UnityEngine;

public class VRCameraFollower : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform objetoASeguir;
    
    [Header("Configuracion")]
    [SerializeField] private Vector3 offsetPosicion = new Vector3(0, 1.6f, 0);
    
    private Vector3 posicionLocalVR;

    void Start()
    {
        if (objetoASeguir == null)
        {
            Debug.LogError("Falta asignar 'Objeto A Seguir' en VRCameraFollower");
        }
    }

    void LateUpdate()
    {
        if (objetoASeguir == null) return;

        // Guardar la posición local del tracking VR
        posicionLocalVR = transform.localPosition;
        
        // Mover la cámara completa a la posición del PlayerCapsule
        transform.position = objetoASeguir.position + offsetPosicion + posicionLocalVR;
    }
}