using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private float _cameraOrthographicSize;
    private bool _hasComeOnscreenYet = false;
    public bool HasComeOnscreenYet { get { return _hasComeOnscreenYet; } }
    private float _widthHeighRatio;
    private float _adjustedScreenWidth;

    void Awake()
    {
        _cameraOrthographicSize = Camera.main.orthographicSize;
    }

    void Start()
    {
        _widthHeighRatio = (float)Screen.width / (float)Screen.height;
        _adjustedScreenWidth = _cameraOrthographicSize * _widthHeighRatio;
    }
    protected virtual void Update()
    {
        if (!_hasComeOnscreenYet)
        {
            CheckIfObjectHasComeOnScreen();
        }
    }

    private void CheckIfObjectHasComeOnScreen()
    {
        if (transform.position.y < _cameraOrthographicSize &&
            transform.position.y > -_cameraOrthographicSize &&
            transform.position.x < _adjustedScreenWidth &&
            transform.position.x > -_adjustedScreenWidth)
        {
            _hasComeOnscreenYet = true;
        }
    }

    protected bool CheckIfOffscreenByAmount(float amount)
    {
        // limit movement to screen size
        if (transform.position.y > _cameraOrthographicSize + amount)
        {
            return true;
        }
        else if (transform.position.y < -_cameraOrthographicSize - amount)
        {
            return true;
        }
        if (transform.position.x > _adjustedScreenWidth + amount)
        {
            return true;
        }
        else if (transform.position.x < -_adjustedScreenWidth - amount)
        {
            return true;
        }

        return false;
    }
}
