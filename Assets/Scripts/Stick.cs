using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private Animator _animator;    
    [SerializeField] private Transform _fixationPoint;

    public Transform FixationPoint => _fixationPoint;    

    public void SetForce(float force)
    {        
        _animator.SetFloat("Blend", force);
    }
}
