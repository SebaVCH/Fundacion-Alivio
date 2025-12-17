using System;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public static event Action<float> OnActivateTimer;
    public static event Action OnDeactivateTimer;

    public void Activate(float segundos)
    {
        Debug.Log($"Activando cronómetro por {segundos} segundos");
        OnActivateTimer?.Invoke(segundos);
    }

    public void Deactivate()
    {
        Debug.Log("Desactivando cronómetro");
        OnDeactivateTimer?.Invoke();
    }
}
