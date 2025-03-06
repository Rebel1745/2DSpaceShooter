using UnityEngine;

public class ReturnParticlesToPool : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
