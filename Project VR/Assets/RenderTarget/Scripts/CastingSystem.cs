using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CastingSystem : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The XR controllers")]
    List<XRController> controllers;
    /// <summary>
    /// The XR controllers.
    /// </summary>
    public List<XRController> Controllers { get { return controllers; } set { controllers = value; } }

    private bool drawingMode;

    public delegate void OnDrawingMode();
    public delegate void OnDrawingStart();
    public delegate void OnDrawingStop();

    private void CheckDrawingMode()
    {
        foreach(XRController controller in controllers)
        {

        }
    }

}
