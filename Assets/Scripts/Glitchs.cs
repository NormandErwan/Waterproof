using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XPostProcessing;

public class Glitchs : MonoBehaviour
{
    public PostProcessProfile profile;

    [Header("Pixelize")]
    private PixelizeQuad pixelize;
    public AnimationCurve pixelizeCurve;
    private GlitchImageBlockV3 imageBlock;
    public AnimationCurve imageBlockCurve;

    [Header("Hue")]
    private ColorAdjustmentHue hue;

    private void Start()
    {
        pixelize = profile.GetSetting<PixelizeQuad>();
        hue = profile.GetSetting<ColorAdjustmentHue>();
        imageBlock = profile.GetSetting<GlitchImageBlockV3>();
    }

    public void Pixelize(float value)
    {
        pixelize.enabled.Override(value > 0);
        pixelize.pixelSize.Override(pixelizeCurve.Evaluate(value));

        imageBlock.enabled.Override(value > 0);
        imageBlock.BlockSize.Override(imageBlockCurve.Evaluate(value));
    }

    public void SetHue(float value)
    {
        //camera.backgroundColor = (1 - value) * Color.white;
        //floorTileMaterial.color = value * Color.white;
        hue.HueDegree.Override(value * 360 - 180);
    }
}
