using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 direction = Vector3.right;
    public float duration = 3;
    public float speed;
    public float damage;
    
    private bool hit = false;


    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(direction * speed, ForceMode.VelocityChange);
        Destroy(gameObject, duration);
    }


    private void OnCollisionEnter(Collision collision)
    {
        hit = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        Debug.Log("hit");
    }

    private void FixedUpdate()
    {
        if (!hit && rb.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }
    }
}
