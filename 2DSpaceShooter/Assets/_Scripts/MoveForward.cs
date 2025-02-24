using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] float _speed = 0f;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void Update()
    {
        transform.Translate(0f, _speed * Time.deltaTime, 0f);
    }
}
