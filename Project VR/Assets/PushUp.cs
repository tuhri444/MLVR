using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushUp : MonoBehaviour
{
    [SerializeField] private float force;

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * force*Time.deltaTime);
        }
    }
}
