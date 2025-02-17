using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public void SetDestroyAfterTime(float timeUntilDestroy)
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
