using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject shoot_effect;
	public GameObject hit_effect;
	public GameObject firing_ship;

	// Use this for initialization
	void Start()
	{
		GameObject obj = (GameObject)Instantiate(shoot_effect, transform.position - new Vector3(0, 0, 5), Quaternion.identity); //Spawn muzzle flash
		obj.transform.parent = firing_ship.transform;
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		//Don't want to collide with the ship that's shooting this thing, nor another projectile.
		if (col.gameObject != firing_ship && col.gameObject.tag != "Projectile")
		{
			//Instantiate(hit_effect, transform.position, Quaternion.identity);
			ObjectPoolManager.SpawnObject(hit_effect, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.Projectile);
			//Destroy(gameObject);
			ObjectPoolManager.ReturnObjectToPool(hit_effect);
		}
	}

}
