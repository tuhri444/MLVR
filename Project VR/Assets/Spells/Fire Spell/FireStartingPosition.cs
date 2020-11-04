using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStartingPosition : MonoBehaviour
{
    void Start()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.75f;
        transform.forward = Camera.main.transform.forward;
    }
}
