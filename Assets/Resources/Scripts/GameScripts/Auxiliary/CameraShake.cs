using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

//Потрушування камери при отримані урону

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        Instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float force)
    {
        if (impulseSource != null)
            impulseSource.GenerateImpulse(force);
    }
}
