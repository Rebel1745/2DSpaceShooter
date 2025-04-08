using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class SplinePathDetails : MonoBehaviour
{
    private SplineContainer _sc;
    private List<BezierKnot> _defaultSplineKnots;

    void Awake()
    {
        _sc = GetComponent<SplineContainer>();
        _defaultSplineKnots = _sc.Spline.Knots.ToList();
    }

    void OnEnable()
    {
        _sc.Spline.Knots = _defaultSplineKnots;
    }
}
