using UnityEngine;

/// <summary>
/// Applies lightweight scene visuals without changing the render pipeline.
/// </summary>
public sealed class SceneVisualEnhancer : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Light keyLight;

    [Header("Camera")]
    [SerializeField] private Color backgroundColor = new Color(0.12f, 0.14f, 0.18f, 1f);
    [SerializeField, Range(0f, 1f)] private float cameraSaturationHint = 0.08f;

    [Header("Lighting")]
    [SerializeField] private Color ambientSkyColor = new Color(0.34f, 0.38f, 0.44f, 1f);
    [SerializeField] private Color ambientEquatorColor = new Color(0.22f, 0.24f, 0.27f, 1f);
    [SerializeField] private Color ambientGroundColor = new Color(0.12f, 0.11f, 0.10f, 1f);
    [SerializeField] private Color keyLightColor = new Color(1f, 0.92f, 0.78f, 1f);
    [SerializeField, Min(0f)] private float keyLightIntensity = 1.25f;

    [Header("Atmosphere")]
    [SerializeField] private bool useFog = true;
    [SerializeField] private Color fogColor = new Color(0.10f, 0.12f, 0.16f, 1f);
    [SerializeField, Min(0f)] private float fogDensity = 0.008f;

    private void Awake()
    {
        Apply();
    }

    /// <summary>
    /// Applies camera, lighting, fog, and quality settings to assigned scene objects.
    /// </summary>
    public void Apply()
    {
        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = Color.Lerp(backgroundColor, Color.white, cameraSaturationHint * 0.08f);
            targetCamera.allowHDR = true;
            targetCamera.allowMSAA = true;
            targetCamera.useOcclusionCulling = true;
        }

        if (keyLight != null)
        {
            keyLight.color = keyLightColor;
            keyLight.intensity = keyLightIntensity;
            keyLight.shadows = LightShadows.Soft;
            keyLight.shadowStrength = 0.72f;
            keyLight.shadowBias = 0.035f;
            keyLight.shadowNormalBias = 0.3f;
        }

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = ambientSkyColor;
        RenderSettings.ambientEquatorColor = ambientEquatorColor;
        RenderSettings.ambientGroundColor = ambientGroundColor;
        RenderSettings.fog = useFog;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;

        QualitySettings.antiAliasing = Mathf.Max(QualitySettings.antiAliasing, 4);
        QualitySettings.shadowDistance = Mathf.Max(QualitySettings.shadowDistance, 80f);
    }
}
