using UnityEngine;

public class Missile : MonoBehaviour
{
    float _missileSpeed = 0;

    public void SetMissileSpeed(float speed)
    {
        _missileSpeed = speed;
    }

    private void Update()
    {
        transform.Translate(0f, _missileSpeed * Time.deltaTime, 0f);
    }
}
