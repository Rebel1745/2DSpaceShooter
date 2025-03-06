using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;
    private static GameObject _particleSytemsEmpty;
    private static GameObject _enemiesEmpty;
    private static GameObject _projectilesEmpty;

    public enum POOL_TYPE
    {
        ParticleSystem,
        Enemy,
        Projectile,
        None
    }
    public static POOL_TYPE PoolingType;

    void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSytemsEmpty = new GameObject("Particle Systems");
        _particleSytemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _enemiesEmpty = new GameObject("Enemies");
        _enemiesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _projectilesEmpty = new GameObject("Projectiles");
        _projectilesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, POOL_TYPE poolType = POOL_TYPE.None)
    {
        /*        
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo poolInfo in ObjectPools)
        {
            if (poolInfo.LookupString == objectToSpawn.name)
            {
                pool = poolInfo;
                break;
            }
        }
        */
        // The code above does the same as this one line, but this way is much cleaner
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        // If we dont find a pool, we should create one
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        // check for an inactive object in the pool
        /*GameObject spawnableObj = null;
        foreach (GameObject obj in pool.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }*/
        // The above code is also inelegant, using System.Linq function

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        // Did we find anything from our list of inactive game objects?
        if (spawnableObj == null)
        {
            // find the parent of the empty object
            GameObject parentObject = SetParentObject(poolType);

            // we didnt find anything, instantiate it
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            // There is an inactive object, reactivate it an move it to the desired position
            spawnableObj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    // This version of the function parents the spawned object to another object, for example if we had a muzzle flash for firing from the player
    // the flash should always be at the end of where the projectile is fired from (not currently in use, but might in the future)
    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        // If we dont find a pool, we should create one
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        // Did we find anything from our list of inactive game objects?
        if (spawnableObj == null)
        {

            // we didnt find anything, instantiate it
            spawnableObj = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            // There is an inactive object, reactivate it an move it to the desired position
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        // when an object is instantiated, it has the string (Clone) added to the end, we need to remove that to find it in our list
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + goName);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(POOL_TYPE poolType)
    {
        return poolType switch
        {
            POOL_TYPE.ParticleSystem => _particleSytemsEmpty,
            POOL_TYPE.Enemy => _enemiesEmpty,
            POOL_TYPE.Projectile => _projectilesEmpty,
            _ => null,
        };
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
