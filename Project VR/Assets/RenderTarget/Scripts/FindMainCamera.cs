using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMainCamera : MonoBehaviour
{
    [SerializeField] private float m_Offset;
    void Start()
    {
        transform.parent = Camera.main.transform;
        transform.position = Camera.main.transform.position + (-Camera.main.transform.forward* m_Offset);
        transform.rotation = Camera.main.transform.rotation;
    }
}
