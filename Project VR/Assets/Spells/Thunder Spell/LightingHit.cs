using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingHit : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Instantiate(explosionPrefab, pos, Quaternion.identity);
            if (rb)
            {
                rb.AddExplosionForce(1000.0f, pos, 3.0f);
            }
            i++;
        }
    }
}
