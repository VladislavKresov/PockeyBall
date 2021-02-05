using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]

public class Ball : MonoBehaviour
{
    [SerializeField] private float _maximumForce;
    [SerializeField] private Stick _stickTemplate;
    [SerializeField] private float _additionPower;
    [SerializeField] private float _powerAdditingDelay;
    [SerializeField] private ParticleSystem _sparks;    

    private Rigidbody _rigidbody;
    private Stick _stick;        
    private float _power;
    private int _coins;
    private float _buttonPressedTimer;
    private float _stickExistTimer;

    public event UnityAction<int> CoinCollected;

    private void Start()
    {
        CoinCollected?.Invoke(_coins);

        Ray ray = new Ray(transform.position, Vector3.forward);
        Physics.Raycast(ray, out RaycastHit hitInfo);
        _stick = Instantiate(_stickTemplate, GetSpawnPsition(hitInfo.point), _stickTemplate.transform.rotation);
        transform.parent = _stick.FixationPoint;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {                        
        Ray ray = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (_stick == null)
                {
                    _stick = Instantiate(_stickTemplate, GetSpawnPsition(hitInfo.point), _stickTemplate.transform.rotation);                    
                    _rigidbody.velocity = Vector3.zero;
                    if (hitInfo.collider.TryGetComponent(out Block block))
                    {
                        transform.position = _stick.FixationPoint.position;
                        Instantiate(_sparks, hitInfo.point, _sparks.transform.rotation);
                        StartCoroutine(RemoveAfterTime(_stick.gameObject, 0.1f));
                    }
                    if (hitInfo.collider.TryGetComponent(out Segment segment))
                    {
                        _rigidbody.isKinematic = true;
                        transform.position = _stick.FixationPoint.position;
                        transform.parent = _stick.FixationPoint;                        
                    }
                    if (hitInfo.collider.TryGetComponent(out Finish finish))
                    {                        
                        Debug.Log("Finish!");
                    }
                }                
            }

            if (Input.GetMouseButton(0))
            {
                if (_stick != null)
                {
                    _power = (int)(_buttonPressedTimer / _powerAdditingDelay) * _additionPower;
                    _power = Mathf.Clamp(_power, 0, 1);

                    _stick.SetForce((_buttonPressedTimer / _powerAdditingDelay) * _additionPower);

                    _buttonPressedTimer += Time.deltaTime;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {                                
                Jump();
                _power = 0;
                _buttonPressedTimer = 0;
            }          
        }
    }

    private void Jump()
    {
        if (_power > 0)
        {
            transform.parent = null;
            if (_stick != null)
                Destroy(_stick.gameObject);
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.up * _power * _maximumForce, ForceMode.Impulse);
        }
    }

    private IEnumerator RemoveAfterTime(GameObject instance, float seconds)
    {
        if (_stickExistTimer < seconds)
        {
            _stickExistTimer = seconds;
            yield return new WaitForSeconds(seconds);
        }
        Destroy(instance);
        _stickExistTimer = 0;
        StopCoroutine(RemoveAfterTime(instance, seconds));        
    }

    public void CollectCoin()
    {
        _coins++;
        CoinCollected?.Invoke(_coins);
    }

    private Vector3 GetSpawnPsition(Vector3 hitPosition)
    {
        hitPosition.z -= 1;
        return hitPosition;
    }
}
