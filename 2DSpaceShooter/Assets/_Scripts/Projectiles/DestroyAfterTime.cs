using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    float _timeUntileDestroy;

    public void SetDestroyAfterTime(float timeUntilDestroy)
    {
        _timeUntileDestroy = timeUntilDestroy;
    }

    void Update()
    {
        _timeUntileDestroy -= Time.deltaTime;
        if (_timeUntileDestroy < 0 && TryGetComponent<IDestroyable>(out IDestroyable obj))
            obj.DestroyObject();
    }
}
