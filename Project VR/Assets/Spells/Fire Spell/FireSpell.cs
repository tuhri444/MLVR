using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private GameObject m_Explosion;
    public Vector3 Forward;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(transform.forward*Time.deltaTime* m_Speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Spell") || !collision.transform.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<Rigidbody>())
                collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(50, transform.position, 1);
            Instantiate(m_Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
