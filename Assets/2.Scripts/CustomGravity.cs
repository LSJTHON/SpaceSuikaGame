using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    //[SerializeField] private float _gravityScale = 1f;
    //[SerializeField, Range(0, 0.05f)] private float _centerCoefficient = 0.03f;
    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_rb.simulated == true)
            CalcGravity(Vector2.zero);
    }

    private void CalcGravity(Vector2 center)
    {
        Vector2 gravityDir = (center - _rb.position).normalized;
        Vector2 gravity = 9.8f * gravityDir;

        Vector2 nowVelocity = _rb.velocity;

        Vector2 newVelocity = nowVelocity + gravity * Time.fixedDeltaTime;

        float fixedSpeed = 5f;
        if (newVelocity.magnitude > fixedSpeed)
        {
            newVelocity = newVelocity.normalized * fixedSpeed;
        }


        _rb.velocity = newVelocity;
    }
}
