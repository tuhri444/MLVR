using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpell : MonoBehaviour
{
    public enum Direction
    {
        transform_up,
        transform_down,
        transform_left,
        transform_right,
        transform_forward,
        transform_backward,
        Vector3_up,
        Vector3_down,
        Vector3_left,
        Vector3_right,
        Vector3_forward,
        Vector3_backward,
        Vector3_default
    }

    [SerializeField] private Direction positionDirection;
    [SerializeField] private float positionModifier;

    [SerializeField] private Direction faceDirection;

    [SerializeField] private GameObject m_Prefab;

    private Vector3 direction;
    private Vector3 position;

    public void Cast()
    {
        Shoot();
    }

    private void Shoot()
    {
        direction = GetDirection();
        position = GetPosition();
        Instantiate(m_Prefab, position, Quaternion.identity).transform.forward =
            direction;
    }

    private Vector3 GetDirection()
    {
        switch (faceDirection)
        {
            case Direction.transform_up:
                return transform.up;
            case Direction.transform_down:
                return -transform.up;
            case Direction.transform_left:
                return -transform.right;
            case Direction.transform_right:
                return transform.right;
            case Direction.transform_forward:
                return transform.forward;
            case Direction.transform_backward:
                return -transform.forward;
            case Direction.Vector3_up:
                return Vector3.up;
            case Direction.Vector3_down:
                return Vector3.down;
            case Direction.Vector3_left:
                return Vector3.left;
            case Direction.Vector3_right:
                return Vector3.right;
            case Direction.Vector3_forward:
                return Vector3.forward;
            case Direction.Vector3_backward:
                return Vector3.back;
            case Direction.Vector3_default:
                return Vector3.zero;
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetPosition()
    {
        Vector3 output = transform.position;
        switch (positionDirection)
        {
            case Direction.transform_up:
                return output + transform.up * positionModifier;
            case Direction.transform_down:
                return output + -transform.up * positionModifier;
            case Direction.transform_left:
                return output + -transform.right * positionModifier;
            case Direction.transform_right:
                return output + transform.right * positionModifier;
            case Direction.transform_forward:
                return output + transform.up * positionModifier;
            case Direction.transform_backward:
                return output + -transform.forward * positionModifier;
            case Direction.Vector3_up:
                return output + Vector3.up * positionModifier;
            case Direction.Vector3_down:
                return output + Vector3.down * positionModifier;
            case Direction.Vector3_left:
                return output + Vector3.left * positionModifier;
            case Direction.Vector3_right:
                return output + Vector3.right * positionModifier;
            case Direction.Vector3_forward:
                return output + Vector3.forward * positionModifier;
            case Direction.Vector3_backward:
                return output + Vector3.back * positionModifier;
            case Direction.Vector3_default:
                return output;
            default:
                return output;
        }
    }

}
