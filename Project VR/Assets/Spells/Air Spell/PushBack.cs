using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBack : MonoBehaviour
{
    [SerializeField] private float push_force;
    [SerializeField] private float lifting_force;
    [SerializeField] private float moving_speed;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moving_speed;
    }
    void OnTriggerEnter(Collider collider)
    {
        Rigidbody other_rb = collider.gameObject.GetComponent<Rigidbody>();
        if (other_rb != null && !collider.gameObject.CompareTag("Player") && !collider.gameObject.CompareTag("Spell"))
        {
            other_rb.AddForce(transform.up * lifting_force);
            other_rb.AddForce(transform.forward* push_force);
        }
    }
}
