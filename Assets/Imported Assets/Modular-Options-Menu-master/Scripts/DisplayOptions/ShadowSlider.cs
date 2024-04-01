using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ModularOptions
{
	[AddComponentMenu("Modular Options/Display/Shadow Slider")]
	public sealed class ShadowSlider : SliderOption, ISliderDisplayFormatter
	{
		protected override void ApplySetting(float _value)
		{
			UniversalRenderPipelineAsset urpAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
			urpAsset.shadowDistance = _value;
			if (_value == 0)
				QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;		
			else
				QualitySettings.shadows = UnityEngine.ShadowQuality.All;
		}

		public string OverrideFormatting(float _value)
		{
        	return (Mathf.RoundToInt(_value / 100f)).ToString();
        }
    }
}
