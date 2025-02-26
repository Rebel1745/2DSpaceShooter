using System;
using UnityEngine;

public class DestroyWhenOffscreen : MonoBehaviour
{
    [SerializeField] private float _distanceOffscreenBeforeDestroy = 2f;
    float _cameraOrthographicSize;
    bool _hasComeOnscreenYet = false;
    float _widthHeighRatio;
    float _adjustedScreenWidth;

    void Awake()
    {
        _cameraOrthographicSize = Camera.main.orthographicSize;
    }

    void Start()
    {
        _widthHeighRatio = (float)Screen.width / (float)Screen.height;
        _adjustedScreenWidth = _cameraOrthographicSize * _widthHeighRatio;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasComeOnscreenYet)
        {
            CheckIfObjectHasComeOnScreen();
            return;
        }

        if (_hasComeOnscreenYet && CheckIfOffscreenByAmount())
            GetComponent<IDestroyable>().DestroyObject();
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

    private bool CheckIfOffscreenByAmount()
    {
        // limit movement to screen size
        if (transform.position.y > _cameraOrthographicSize + _distanceOffscreenBeforeDestroy)
        {
            return true;
        }
        else if (transform.position.y < -_cameraOrthographicSize - _distanceOffscreenBeforeDestroy)
        {
            return true;
        }
        if (transform.position.x > _adjustedScreenWidth + _distanceOffscreenBeforeDestroy)
        {
            return true;
        }
        else if (transform.position.x < -_adjustedScreenWidth - _distanceOffscreenBeforeDestroy)
        {
            return true;
        }

        return false;
    }
}
