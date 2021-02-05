using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTracker : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;

    private void FixedUpdate()
    {
        Vector3 targetPosition = _ball.transform.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.fixedDeltaTime);
        transform.LookAt(_ball.transform.position);
    }
}
