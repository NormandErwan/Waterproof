using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XPostProcessing;

public class Glitchs : MonoBehaviour
{
    public PostProcessProfile profile;
    public AnimationCurve pixelizeCurve;

    private PixelizeQuad pixelize;
    private ColorAdjustmentHue hue;

    private void Start()
    {
        pixelize = profile.GetSetting<PixelizeQuad>();
        hue = profile.GetSetting<ColorAdjustmentHue>();
    }

    public void Pixelize(float value)
    {
        pixelize.enabled.Override(value > 0);
        pixelize.pixelSize.Override(pixelizeCurve.Evaluate(value));
    }

    public void SetHue(float value)
    {
        //camera.backgroundColor = (1 - value) * Color.white;
        //floorTileMaterial.color = value * Color.white;
        hue.HueDegree.Override(value * 360 - 180);
    }
}
