using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMainCamera : MonoBehaviour
{
    void Start()
    {
        transform.parent = Camera.main.transform;
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;
    }
}
