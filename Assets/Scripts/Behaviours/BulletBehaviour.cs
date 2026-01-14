using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletBehaviour : MonoBehaviour
{
    private bool dealtWith;
    private Vector3 target;
    private float bulletSpeed;
    private Rigidbody rigidbodyComponent;
    void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        rigidbodyComponent.AddForce(Vector3.Normalize(target - transform.position) * bulletSpeed, ForceMode.VelocityChange);
    }

    void OnDisable()
    {
        rigidbodyComponent.velocity = Vector3.zero;
    }

    public void SetTarget(Vector3 t, float bs)
    {
        target = t;
        bulletSpeed = bs;
    }
    void OnCollisionStay(Collision other)
    {
        Destroy(gameObject);
    }
    void OnCollisionExit(Collision other) 
    {
        Destroy(gameObject);
    }
}