using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;        

    private void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += _rotationSpeed * Time.deltaTime;
        transform.eulerAngles = rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ball _ball))
        {
            _ball.CollectCoin();
            Destroy(gameObject);
        }
    }
}
