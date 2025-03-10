using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float _cameraOrthographicSize;
    public float CameraOrthographicSize { get { return _cameraOrthographicSize; } }
    private float _widthHeighRatio;
    public float WidthHeighRatio { get { return _widthHeighRatio; } }
    private float _adjustedScreenWidth;
    public float AdjustedScreenWidth { get { return _adjustedScreenWidth; } }
    private float _previousWidth;
    private float _previousHeight;

    void Awake()
    {
        if (Instance == null) Instance = this;

        _cameraOrthographicSize = Camera.main.orthographicSize;
        _previousWidth = Screen.width;
        _previousHeight = Screen.height;
    }

    void Start()
    {
        _widthHeighRatio = (float)Screen.width / (float)Screen.height;
        _adjustedScreenWidth = _cameraOrthographicSize * _widthHeighRatio;
    }

    void Update()
    {
        CheckScreenSize();
    }

    private void CheckScreenSize()
    {
        if (_previousWidth != Screen.width || _previousHeight != Screen.height)
        {
            _previousWidth = Screen.width;
            _previousHeight = Screen.height;
            OnScreenSizeChanged();
        }
    }

    private void OnScreenSizeChanged()
    {
        _widthHeighRatio = (float)Screen.width / (float)Screen.height;
        _adjustedScreenWidth = _cameraOrthographicSize * _widthHeighRatio;
    }
}
