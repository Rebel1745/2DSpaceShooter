using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float _timeUntileDestroy;
    private Coroutine _returnToPoolTimerCoroutine;

    void OnEnable()
    {
        _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTime());
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _timeUntileDestroy)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_timeUntileDestroy < 0 && TryGetComponent<IDestroyable>(out IDestroyable obj))
            obj.DestroyObject();
    }
}
