using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class FadeBlack : MonoBehaviour
{
    [SerializeField]
    float velocity;

    [SerializeField]
    Texture texture;

    PostProcessVolume m_Volume;
    Vignette m_Vignette;

    Timer tim;
    void Start()
    {
        // Create an instance of a vignette
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.mode.Override(VignetteMode.Masked);
        m_Vignette.opacity.Override(1f);
        m_Vignette.mask.Override(texture);

        // Use the QuickVolume method to create a volume with a priority of 100, and assign the vignette to this volume
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);

        tim = Timers.Create(1);
    }
    void Update()
    {
        if(tim.Chck())
        {
            m_Vignette.opacity.value -= Time.deltaTime * velocity;
        }
        
        if (m_Vignette.opacity.value <= 0)
        {
            Timers.Destroy(tim);
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
            this.enabled = false;
        }
    }
}