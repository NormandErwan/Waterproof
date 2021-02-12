using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XPostProcessing;

public class Glitchs : MonoBehaviour
{
    [SerializeField]
    private PostProcessProfile profile;

    [Header("Pixelize")]
    private PixelizeQuad pixelize;

    [SerializeField]
    private AnimationCurve pixelizeCurve;
    private GlitchImageBlockV3 imageBlock;

    [SerializeField]
    private AnimationCurve imageBlockSpeedCurve;

    [SerializeField]
    private AnimationCurve imageBlockSizeCurve;

    private float oldPixelizeValue = 0;
    private bool pixelizeEnabled = false;

    [Header("Hue")]
    private ColorAdjustmentHue hue;

    [Header("Brightness")]
    private ColorAdjustmentBrightness brightness;

    [SerializeField]
    private Light light;

    [SerializeField]
    private AnimationCurve brightnessCurve;

    private void Start()
    {
        pixelize = profile.GetSetting<PixelizeQuad>();

        hue = profile.GetSetting<ColorAdjustmentHue>();
        imageBlock = profile.GetSetting<GlitchImageBlockV3>();
        brightness = profile.GetSetting<ColorAdjustmentBrightness>();
    }

    public void Pixelize(float value)
    {
        if (value != oldPixelizeValue)
        {
            if (value > 0)
            {
                pixelizeEnabled = !pixelizeEnabled;
                pixelize.enabled.Override(pixelizeEnabled);
                imageBlock.enabled.Override(pixelizeEnabled);
            }
            oldPixelizeValue = value;
        }
        /*pixelize.enabled.Override(value > 0);
        pixelize.pixelSize.Override(pixelizeCurve.Evaluate(value));

        imageBlock.BlockSize.Override(imageBlockSizeCurve.Evaluate(value));
        imageBlock.Speed.Override(imageBlockSpeedCurve.Evaluate(value));*/
    }

    public void SetHue(float value)
    {
        hue.HueDegree.Override(value * 360 - 180);
    }

    public void SetBrightness(float value)
    {
        //brightness.brightness.Override(brightnessCurve.Evaluate(value));
        light.intensity = brightnessCurve.Evaluate(value);
    }
}
