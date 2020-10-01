using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRG
{
    public class RasterizeImage : MonoBehaviour
    {
        /// <summary>
        /// Render texture used to read drawing.
        /// </summary>
        [SerializeField]
        [Tooltip("Render texture used to read drawing.")]
        RenderTexture m_RenderTarget;

        /// <summary>
        /// The Texture that will be used to read the pixels of the render texture onto.
        /// </summary>
        private Texture2D m_TargetTexture2D;
        /// <summary>
        /// Current Drawing object that will be deleted after rasterization. 
        /// </summary>
        private GameObject m_Drawing;
        /// <summary>
        /// The boolean used to activate the saving process of the image.
        /// </summary>
        private bool m_SaveImage;
        /// <summary>
        /// A reference to this object's own camera component.
        /// </summary>
        private Camera m_CameraSelf;
        void Awake()
        {
            HandleTargetRender();
            m_CameraSelf = GetComponent<Camera>();
            m_CameraSelf.targetTexture = m_RenderTarget;
        }
        void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        }
        void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        }
        /// <summary>
        /// Calls the OnPostRender because Unity forgot to run this specific one. :(
        /// </summary>
        /// <param name="context"></param>
        /// <param name="camera"></param>
        private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            OnPostRender();
        }
        /// <summary>
        /// Function that is run after rendering the camera in use.
        /// </summary>
        private void OnPostRender()
        {
            if (!m_SaveImage) return;
            m_SaveImage = false;
            SaveTargetImage();
        }
        /// <summary>
        /// Tries to look a texture2D asset to set as target. 
        /// If it can't find anything, it will create one and use that.
        /// </summary>
        private void HandleTargetRender()
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(Application.dataPath + "/RenderTarget/target.png");

                if (m_TargetTexture2D == null)
                    m_TargetTexture2D = new Texture2D(400, 400, TextureFormat.RGBA32, false);

                ImageConversion.LoadImage(m_TargetTexture2D, bytes);
                if (m_TargetTexture2D == null) throw new ArgumentException("Did not find a target image, will now create one");
                return;
            }
            catch (Exception e)
            {
                Texture2D temp = new Texture2D(400, 400, TextureFormat.RGBA32, false);

                if (!Directory.Exists(Application.dataPath + "/RenderTarget/"))
                    Directory.CreateDirectory(Application.dataPath + "/RenderTarget/");

                byte[] bytes = temp.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath + "/RenderTarget/target.png", bytes);
                m_TargetTexture2D = temp;
                UnityEditor.AssetDatabase.Refresh();
                Debug.Log("Succesfully made the target image file, have fun with it!");
            }
        }
        /// <summary>
        /// Will save all the pixels from the render target onto a texture2D (png file (inside of the asset/rendertarget folder))
        /// </summary>
        private void SaveTargetImage()
        {
            RenderTexture.active = m_RenderTarget;
            m_TargetTexture2D.ReadPixels(new Rect(0, 0, m_RenderTarget.width, m_RenderTarget.height), 0, 0, false);
            m_TargetTexture2D.Apply();
            RenderTexture.active = null;

            byte[] bytes = m_TargetTexture2D.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/RenderTarget/target.png", bytes);
            UnityEditor.AssetDatabase.Refresh();
            Destroy(m_Drawing);
        }
        /// <summary>
        /// Activation function for saving image.
        /// </summary>
        /// <param name="drawing">GameObject containing the drawing.</param>
        public void SaveImage(GameObject drawing)
        {
            m_Drawing = drawing;
            m_SaveImage = true;
        }
    }
}
