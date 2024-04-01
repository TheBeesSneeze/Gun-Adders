using UnityEngine;

namespace ModularOptions
{
	[AddComponentMenu("Modular Options/Display/Render Distance Slider")]
	public sealed class RenderDistanceSlider : SliderOption, ISliderDisplayFormatter
	{
		protected override void ApplySetting(float _value)
		{
			Camera.main.farClipPlane = _value;
		}

		public string OverrideFormatting(float _value)
		{
			return (Mathf.RoundToInt(_value / 100f)).ToString();
		}
	}
}
