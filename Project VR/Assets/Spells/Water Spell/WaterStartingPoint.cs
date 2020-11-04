using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStartingPoint : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float heightModifier;

    void Start()
    {
        Transform trans = Camera.main.transform;
        Transform charTransform = FindObjectOfType<CharacterController>().transform;
        Vector3 forward = trans.forward;
        forward.y = 0;
        forward.Normalize();

        transform.position = charTransform.position + forward * distanceFromPlayer +
                             Vector3.up * heightModifier;
    }

}