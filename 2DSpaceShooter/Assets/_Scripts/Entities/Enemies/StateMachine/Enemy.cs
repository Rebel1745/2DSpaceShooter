using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IEnemy, IDamageable, IDestroyable
{
    protected EnemyStateMachine _stateMachine;

    public EnemySO EnemyData { get; private set; }
    public SplineAnimate SA { get; private set; }
    protected bool _hasComeOnscreenYet = false;
    protected bool _splineAnimationFinished = false;
    protected Vector2 _enemyGridPosition;
    protected bool _hasPerformedFirstAttack;
    protected float _timeOfNextAttack;
    public bool CanAttack { get; private set; }
    public Transform AttackSpawnPoint { get; private set; }

    protected virtual void Awake()
    {
        _stateMachine = new EnemyStateMachine();
        SA = GetComponent<SplineAnimate>();
    }

    protected virtual void Start()
    {
        if (EnemyData.CanAttack) SetNextAttackTime();
    }

    protected virtual void Update()
    {
        if (!_hasComeOnscreenYet)
        {
            CheckIfObjectHasComeOnScreen();
            return; // Should this return here? Or should it just continue to the logic update? If there are any problems executing logic, this will no doubt be the cause
        }

        if (!CanAttack && EnemyData.CanAttack) CheckIfCanAttack();

        _stateMachine.CurrentState.LogicUpate();
    }

    protected virtual void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }

    protected virtual void OnEnable()
    {
        _hasComeOnscreenYet = false;
        _hasPerformedFirstAttack = false;
        _splineAnimationFinished = false;
    }

    #region Check Functions
    void OnTriggerEnter2D(Collider2D collision)
    {
        // check if we run into the player
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(1);
            TakeDamage(1);
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

    protected void CheckIfCanAttack()
    {
        if (Time.time > _timeOfNextAttack)
            CanAttack = true;
    }

    public void CheckSplineAnimationFinished()
    {
        _splineAnimationFinished = !SA.IsPlaying;
    }
    #endregion

    #region Set Functions
    public void SetNextAttackTime()
    {
        CanAttack = false;
        float randomness = Random.Range(-EnemyData.RandomnessTimeForAttacks, EnemyData.RandomnessTimeForAttacks);
        _timeOfNextAttack = Time.time + EnemyData.TimeBetweenAttacks + randomness;
    }

    public void SetGridPosition(Vector2 pos)
    {
        _enemyGridPosition = pos;
    }

    public void SetEnemyData(EnemySO enemyData)
    {
        EnemyData = enemyData;
        if (EnemyData.CanAttack)
        {
            // Dont know if this is a good way to go about it but oh well
            AttackSpawnPoint = transform.GetChild(1);
        }

        // after setting the data, we can now initialise the states for the enemy
        InitialiseStates();
    }

    public virtual void InitialiseStates() { }

    public void SetSplineAnimateProperties(SplineContainer spline, float speed)
    {
        SA.Container = spline;
        SA.MaxSpeed = speed;
    }

    public virtual void TakeDamage(int amount)
    {
    }

    public virtual void DestroyObject()
    {
        WaveManager.Instance.EnemyDestroyed();
        if (EnemyData.DestructionParticles)
            ObjectPoolManager.SpawnObject(EnemyData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        SA.ElapsedTime = 0;
        ObjectPoolManager.ReturnObjectToPool(SA.Container.gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public virtual void QuietDestroy()
    {
        SA.ElapsedTime = 0;
        WaveManager.Instance.EnemyDestroyed();
        ObjectPoolManager.ReturnObjectToPool(SA.Container.gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    #endregion
}
