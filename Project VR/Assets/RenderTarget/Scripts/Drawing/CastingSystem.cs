using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRG
{
    /// <summary>
    /// List of available input methods for activation.
    /// </summary>
    public enum InputMethod
    {
        PrimaryButton,
        PrimaryTouch,
        SecondaryButton,
        SecondaryTouch,
        GripButton,
        TriggerButton,
        MenuButton,
        Primary2DAxisClick,
        Primary2DAxisTouch,
        Secondary2DAxisClick,
        Secondary2DAxisTouch
    }

    public class CastingSystem : MonoBehaviour
    {
        [Header("XR Controllers:")]
        /// <summary>
        /// The left controller.
        /// </summary>
        [SerializeField]
        [Tooltip("The left controllers")]
        XRController m_LeftController;

        /// <summary>
        /// The right controller.
        /// </summary>
        [SerializeField]
        [Tooltip("The right controllers")]
        XRController m_RightController;

        [Header("Input Methods:")]
        /// <summary>
        /// Input method used to draw.
        /// </summary>
        [SerializeField]
        [Tooltip("Input method used to draw.")]
        [Space(10)]
        InputMethod m_InputMethodDrawing;

        /// <summary>
        /// Input method used to enter drawing mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Input method used to enter drawing mode.")]
        InputMethod m_InputMethodDrawingMode;

        /// <summary>
        /// The spacing in between the activation of OnDraw event.
        /// </summary>
        [SerializeField]
        [Tooltip("The spacing in between the activation of OnDraw event.")]
        [Space(10)]
        float m_Spacing;

        /// <summary>
        /// Gives you the controller assigned as Left.
        /// </summary>
        public XRController LeftController => m_LeftController;

        /// <summary>
        /// Gives you the controller assigned as Right.
        /// </summary>
        public XRController RightController => m_RightController;

        public delegate void ToggleDrawing();
        /// <summary>
        /// Will be invoked every frame if drawing mode is on.
        /// </summary>
        public static event ToggleDrawing OnDrawingMode;
        /// <summary>
        /// Will be invoked only if last frame drawing mode was not on.
        /// </summary>
        public static event ToggleDrawing OnDrawingModeStart;
        /// <summary>
        /// Will be invoked only when previous frame drawing mode was on and not this frame.
        /// </summary>
        public static event ToggleDrawing OnDrawingModeStop;

        public delegate void Draw(Transform controllerTransform);
        /// <summary>
        /// Will be invoked every frame if drawing mode is on and user is drawing.
        /// </summary>
        public static event Draw OnDraw;
        /// <summary>
        /// Will be invoked only when the user was not drawing the frame before but drawing now.
        /// </summary>
        public static event Draw OnDrawStart;
        /// <summary>
        /// Will be invoked only when the user was drawing before and is not drawing now.
        /// </summary>
        public static event Draw OnDrawStop;

        private bool _drawingMode;
        private bool _drawing;
        private bool _canDraw = true;
        void Update()
        {
            CheckForInput();
        }

        /// <summary>
        /// Chooses the right checking function depending on the controller.
        /// </summary>
        private void CheckForInput()
        {
            if (m_LeftController != null && m_LeftController.enableInputActions)
                CheckToDraw(m_LeftController.inputDevice, m_LeftController.transform);

            if (m_RightController != null && m_RightController.enableInputActions)
                CheckDrawingMode(m_RightController.inputDevice);
        }

        /// <summary>
        /// Checks the criteria for drawing mode.
        /// </summary>
        /// <param name="device">The device being checked for input.</param>
        private void CheckDrawingMode(InputDevice device)
        {
            InputFeatureUsage<bool> input = TranslateInputMethod(m_InputMethodDrawingMode);
            if (!device.TryGetFeatureValue(input, out bool value)) return;

            if (_drawingMode && !value)
                OnDrawingModeStop?.Invoke();
            else if (!_drawingMode && value)
                OnDrawingModeStart?.Invoke();
            if (value) OnDrawingMode?.Invoke();

            _drawingMode = value;
        }

        /// <summary>
        /// Checks the criteria for drawing.
        /// </summary>
        /// <param name="device">The device being checked for input.</param>
        /// <param name="controllerTransform">The transform component of the controller.</param>
        private void CheckToDraw(InputDevice device, Transform controllerTransform)
        {
            InputFeatureUsage<bool> input = TranslateInputMethod(m_InputMethodDrawing);
            if (!device.TryGetFeatureValue(input, out bool value) || !_drawingMode) return;

            if (_drawing && !value)
                OnDrawStop?.Invoke(controllerTransform);
            else if (!_drawing && value)
                OnDrawStart?.Invoke(controllerTransform);
            if (value && _canDraw)
            {
                _canDraw = false;
                StartCoroutine(DrawResetTimer(m_Spacing));
                OnDraw?.Invoke(controllerTransform);
            }

            _drawing = value;
        }

        /// <summary>
        /// Enumator timer for waiting in between draw calls.
        /// </summary>
        /// <param name="timeToWait">Time to wait in seconds.</param>
        /// <returns></returns>
        private IEnumerator DrawResetTimer(float timeToWait)
        {
            float timePassed = 0.0f;

            while (timePassed < timeToWait)
            {
                timePassed += Time.deltaTime;
                yield return null;
            }
            _canDraw = true;
        }

        /// <summary>
        /// Gives you the appropriate InputFeatureUsage depending on what input method you fill in.
        /// </summary>
        /// <param name="inputMethod">Enum value of the input methods</param>
        /// <returns></returns>
        private static InputFeatureUsage<bool> TranslateInputMethod(InputMethod inputMethod)
        {
            switch (inputMethod)
            {
                case InputMethod.PrimaryButton:
                    return CommonUsages.primaryButton;
                case InputMethod.PrimaryTouch:
                    return CommonUsages.primaryTouch;
                case InputMethod.SecondaryButton:
                    return CommonUsages.secondaryButton;
                case InputMethod.SecondaryTouch:
                    return CommonUsages.secondaryTouch;
                case InputMethod.GripButton:
                    return CommonUsages.gripButton;
                case InputMethod.TriggerButton:
                    return CommonUsages.triggerButton;
                case InputMethod.MenuButton:
                    return CommonUsages.menuButton;
                case InputMethod.Primary2DAxisClick:
                    return CommonUsages.primary2DAxisClick;
                case InputMethod.Primary2DAxisTouch:
                    return CommonUsages.primary2DAxisTouch;
                case InputMethod.Secondary2DAxisClick:
                    return CommonUsages.secondary2DAxisClick;
                case InputMethod.Secondary2DAxisTouch:
                    return CommonUsages.secondary2DAxisTouch;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputMethod), inputMethod, null);
            }
        }
    }
}




