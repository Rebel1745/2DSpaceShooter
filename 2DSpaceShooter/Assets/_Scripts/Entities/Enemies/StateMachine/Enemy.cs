using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IEnemy
{
    protected EnemyStateMachine _stateMachine;

    protected EnemySO _enemyData;
    public SplineAnimate _sa;
    protected bool _hasComeOnscreenYet = false;
    protected bool _splineAnimationFinished = false;
    protected Vector2 _enemyGridPosition;

    protected virtual void Awake()
    {
        _stateMachine = new EnemyStateMachine();
        _sa = GetComponent<SplineAnimate>();
    }

    protected virtual void Update()
    {
        if (!_hasComeOnscreenYet)
        {
            CheckIfObjectHasComeOnScreen();
        }

        _stateMachine.CurrentState.LogicUpate();
    }

    protected virtual void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }

    #region Check Functions
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

    public void CheckSplineAnimationFinished()
    {
        _splineAnimationFinished = !_sa.IsPlaying;
    }
    #endregion

    #region Set Functions
    public void SetGridPosition(Vector2 pos)
    {
        _enemyGridPosition = pos;
    }

    public void SetEnemyData(EnemySO enemyData)
    {
        _enemyData = enemyData;
    }

    public void SetSplineAnimateProperties(SplineContainer spline, float speed)
    {
        _sa.Container = spline;
        _sa.MaxSpeed = speed;
    }
    #endregion
}
