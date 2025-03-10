using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool _hasComeOnscreenYet = false;
    public bool HasComeOnscreenYet { get { return _hasComeOnscreenYet; } }

    protected virtual void Update()
    {
        if (!_hasComeOnscreenYet)
        {
            CheckIfObjectHasComeOnScreen();
        }
    }

    private void CheckIfObjectHasComeOnScreen()
    {
        if (transform.position.y < GameManager.Instance.CameraOrthographicSize &&
            transform.position.y > -GameManager.Instance.CameraOrthographicSize &&
            transform.position.x < GameManager.Instance.AdjustedScreenWidth &&
            transform.position.x > -GameManager.Instance.AdjustedScreenWidth)
        {
            _hasComeOnscreenYet = true;
        }
    }

    protected bool CheckIfOffscreenByAmount(float amount)
    {
        // limit movement to screen size
        if (transform.position.y > GameManager.Instance.CameraOrthographicSize + amount)
        {
            return true;
        }
        else if (transform.position.y < -GameManager.Instance.CameraOrthographicSize - amount)
        {
            return true;
        }
        if (transform.position.x > GameManager.Instance.AdjustedScreenWidth + amount)
        {
            return true;
        }
        else if (transform.position.x < -GameManager.Instance.AdjustedScreenWidth - amount)
        {
            return true;
        }

        return false;
    }
}
