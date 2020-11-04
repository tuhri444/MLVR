using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStartingPosition : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float heightModifier;

    void Start()
    {
        Transform trans = Camera.main.transform;
        Vector3 forward = trans.forward;
        forward.y = 0;
        forward.Normalize();

        transform.position = trans.position + forward * distanceFromPlayer +
                             Vector3.up * heightModifier;
    }

}
