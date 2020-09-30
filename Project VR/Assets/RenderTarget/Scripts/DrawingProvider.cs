using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawingProvider : MonoBehaviour
{
    /// <summary>
    /// "RasterizeImage script, found on the A-EYE camera object."
    /// </summary>
    [SerializeField]
    [Tooltip("RasterizeImage script, found on the A-EYE camera object.")]
    RasterizeImage m_Cam;

    /// <summary>
    /// "Drawing Prefab."
    /// </summary>
    [SerializeField]
    [Tooltip("Drawing Prefab.")]
    GameObject m_DrawingPrefab;

    /// <summary>
    /// Stroke Prefab.
    /// </summary>
    [SerializeField]
    [Tooltip("Stroke Prefab.")]
    GameObject m_StrokePrefab;

    /// <summary>
    /// Current drawing object being used.
    /// </summary>
    private GameObject m_DrawingObject;
    /// <summary>
    /// Current stroke object being drawn in.
    /// </summary>
    private GameObject m_StrokeObject;
    /// <summary>
    /// Integer holding the count of points in the active strokes line renderer.
    /// </summary>
    private int m_CountPointsInStroke;

    void Start()
    {
        CastingSystem.OnDrawingModeStart += StartDrawingMode;
        CastingSystem.OnDrawingModeStop += StopDrawingMode;
        CastingSystem.OnDrawingMode += DrawingMode;

        CastingSystem.OnDrawStart += DrawingStart;
        CastingSystem.OnDraw += Drawing;
    }

    /// <summary>
    /// Sends the camera a sign to save the current render texture.
    /// </summary>
    private void StopDrawingMode()
    {
        m_Cam.SaveImage(m_DrawingObject);
        Debug.Log("Stopping drawing mode");
    }
    /// <summary>
    /// Creates drawing object when drawing mode has started.
    /// </summary>
    private void StartDrawingMode()
    {
        m_DrawingObject = Instantiate(m_DrawingPrefab, transform.position, Quaternion.identity);
        m_DrawingObject.transform.parent = transform.parent;
        Debug.Log("Starting drawing mode");
    }
    /// <summary>
    /// Just for debugging.
    /// </summary>
    private void DrawingMode()
    {
        Debug.Log("Drawing Mode On");
    }
    /// <summary>
    /// Creates a stroke object when drawing starts. Also resets the stroke point count.
    /// </summary>
    /// <param name="controllerTransform">Transform of the controller.</param>
    private void DrawingStart(Transform controllerTransform)
    {
        m_StrokeObject = Instantiate(m_StrokePrefab, m_DrawingObject.transform);
        m_CountPointsInStroke = 0;
    }
    /// <summary>
    /// Adds point to the line renderer in the current stroke object.
    /// </summary>
    /// <param name="controllerTransform">Transform of the controller.</param>
    private void Drawing(Transform controllerTransform)
    {
        Debug.Log("Drawing");
        Vector3 tempPos = controllerTransform.position;
        LineRenderer line = m_StrokeObject.GetComponent<LineRenderer>();
        line.positionCount++;
        line.SetPosition(m_CountPointsInStroke, tempPos);
        m_CountPointsInStroke++;
    }
}
