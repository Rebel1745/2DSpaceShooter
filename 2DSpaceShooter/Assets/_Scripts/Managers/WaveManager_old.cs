using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManager_old : MonoBehaviour
{
    /*public static WaveManager Instance;

    [SerializeField] WaveSO[] _waves;
    private WaveSO _currentWave;
    private int _currentWaveIndex = 0;
    private WAVE_STATE _currentWaveState;
    [SerializeField] private float _waveSpawnPointYOffset = 2f;
    private float _waveStartTime;
    private float _waveEndTime;
    private EnemySO _enemy;
    private int _enemiesSpawned;
    private int _enemiesDestroyed;
    private GameObject _tmpEnemy;
    private GameObject _tmpEnemyPath;
    private float _spawnPointX;
    private float _lastEnemySpawnedTime;

    // Grid of enemies info
    [SerializeField] private float _enemyGridXBounds;
    [SerializeField] private float _enemyGridYBounds;
    private Vector2 _enemyGridBoundsMin;
    private Vector2 _enemyGridBoundsMax;
    private float _enemyGridStartPosX;
    private float _enemyGridStartPosY;
    private float _enemiesPerRow;
    private float _distancePerEnemyX;
    private float _distancePerEnemyY;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (_waves.Length == 0)
        {
            Debug.LogError("NO WAVES HAVE BEEN ADDED TO THE LEVEL");
            return;
        }

        SetupWave(_waves[_currentWaveIndex]);
    }

    private void SetupWave(WaveSO currentWave)
    {
        _currentWave = currentWave;
        _currentWaveState = WAVE_STATE.WaitingToBegin;
        _waveStartTime = Time.time;
        _enemiesSpawned = 0;
        _enemiesDestroyed = 0;

        // figure out the size of the grid of enemies for this wave. This will make the grid go across the screen but no lower than half the height
        _enemyGridBoundsMin = new(-((GameManager.Instance.AdjustedScreenWidth / 2) + _enemyGridXBounds), _enemyGridYBounds);
        _enemyGridBoundsMax = new(GameManager.Instance.AdjustedScreenWidth - _enemyGridXBounds, GameManager.Instance.CameraOrthographicSize - _enemyGridYBounds);
        _enemyGridStartPosX = _enemyGridBoundsMin.x;
        _enemyGridStartPosY = _enemyGridBoundsMax.y;
        _enemiesPerRow = Mathf.Ceil((float)currentWave.EnemyCount / (float)currentWave.EnemyRows);
        _distancePerEnemyX = ((GameManager.Instance.AdjustedScreenWidth * 2) - (_enemyGridXBounds * 2)) / _enemiesPerRow;
        _distancePerEnemyY = (GameManager.Instance.CameraOrthographicSize - (_enemyGridYBounds * 2)) / _currentWave.EnemyRows;
    }

    // Update is called once per frame
    void Update()
    {
        DoWave();
    }

    void DoWave()
    {
        // if we are waiting for all enemies to be destroyed, do nothing
        if (_currentWaveState == WAVE_STATE.WaitingForDestruction)
            return;

        if (_currentWaveState == WAVE_STATE.WaitingToBegin)
        {
            if (Time.time > _waveStartTime + _currentWave.TimeBeforeWaveBegins)
            {
                //Debug.Log("Wave starting");
                _currentWaveState = WAVE_STATE.Spawning;
                return;
            }
        }
        if (_currentWaveState == WAVE_STATE.Spawning)
        {
            if (_enemiesSpawned >= _currentWave.EnemyCount)
            {
                if (_currentWave.WaitForEnemiesToBeDestroyedBeforeNextWave)
                    _currentWaveState = WAVE_STATE.WaitingForDestruction;
                else
                {
                    _currentWaveState = WAVE_STATE.WaitingToEnd;
                    _waveEndTime = Time.time;
                }

                return;
            }

            // actually spawn the current enemy in the wave
            if (Time.time > _lastEnemySpawnedTime + _currentWave.TimeBetweenEnemy)
                SpawnEnemy();
        }
        if (_currentWaveState == WAVE_STATE.WaitingToEnd)
        {
            if (Time.time > _waveEndTime + _currentWave.TimeBeforeWaveEnds)
            {
                _currentWaveIndex++;
                if (_currentWaveIndex < _waves.Length)
                {
                    SetupWave(_waves[_currentWaveIndex]);
                }
                else
                {
                    //Debug.Log("End of wave(s)");
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        if (_currentWave.RandomEnemy)
            _enemy = _currentWave.Enemies[UnityEngine.Random.Range(0, _currentWave.Enemies.Length)];
        else _enemy = _currentWave.Enemies[0];

        // Determine the position to place the spline that the enemy will follow starts at
        if (_currentWave.SpawnLocationType == SPAWN_LOCATION_TYPE.Range)
        {
            if (_currentWave.SpawnPointXRange == Vector2.zero)
                _spawnPointX = UnityEngine.Random.Range((float)-GameManager.Instance.AdjustedScreenWidth, (float)GameManager.Instance.AdjustedScreenWidth);
            else
                _spawnPointX = UnityEngine.Random.Range(_currentWave.SpawnPointXRange.x, _currentWave.SpawnPointXRange.y);
        }
        else _spawnPointX = _currentWave.SpawnPointX;

        _tmpEnemyPath = null;
        _tmpEnemyPath = ObjectPoolManager.SpawnObject(_enemy.SplinePathToFollow, new Vector3(_spawnPointX, 7f, 0f), Quaternion.identity, ObjectPoolManager.POOL_TYPE.SplinePath);
        SplineContainer enemySpline = _tmpEnemyPath.GetComponent<SplineContainer>();

        _tmpEnemy = null;

        _tmpEnemy = ObjectPoolManager.SpawnObject(_enemy.EnemyPrefab, new Vector3(_spawnPointX, GameManager.Instance.CameraOrthographicSize + _waveSpawnPointYOffset, 0f), Quaternion.identity, ObjectPoolManager.POOL_TYPE.Enemy);
        IEnemy iEnemy = _tmpEnemy.GetComponent<IEnemy>();

        if (_enemy.RotateSpriteRandomly)
        {
            SpriteRenderer sr = _tmpEnemy.GetComponentInChildren<SpriteRenderer>();
            sr.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
        }

        if (_enemy.AlignEnemyInGridAfterSplinePathFollowed)
        {
            // add the final position of the enemy to the end of the spline
            Vector2 finalPos = CalculateEnemyGridPosition();
            float3 finalPosKnot = new(finalPos.x - enemySpline.transform.position.x, finalPos.y - enemySpline.transform.position.y, 0.0f);
            enemySpline.Spline.Add(finalPosKnot);
        }

        iEnemy.SetSplineAnimateProperties(enemySpline, _enemy.EnemySpeed);
        iEnemy.SetEnemyData(_enemy);

        _enemiesSpawned++;
        _lastEnemySpawnedTime = Time.time;
    }

    private Vector2 CalculateEnemyGridPosition()
    {
        //Vector2 placeInGrid = new(_enemiesSpawned % _enemiesPerRow, Mathf.FloorToInt((float)_enemiesSpawned / (float)_enemiesPerRow));
        Vector2 placeInGrid = new(Mathf.FloorToInt((float)_enemiesSpawned / (float)_currentWave.EnemyRows), _enemiesSpawned % _currentWave.EnemyRows);
        float xPos = _enemyGridStartPosX + (placeInGrid.x * _distancePerEnemyX);
        float yPos = _enemyGridStartPosY - (placeInGrid.y * _distancePerEnemyY);

        return new Vector2(xPos, yPos);
    }

    public void EnemyDestroyed()
    {
        _enemiesDestroyed++;

        if (_enemiesDestroyed == _enemiesSpawned)
            _currentWaveState = WAVE_STATE.WaitingToEnd;
    }
}

public enum WAVE_STATE
{
    WaitingToBegin,
    Spawning,
    WaitingForDestruction,
    WaitingToEnd*/
}
