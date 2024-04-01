using UnityEngine;

namespace ModularOptions
{
	/// <summary>
	/// Refresh Rate Slider
	/// </summary>
	[AddComponentMenu("Modular Options/Display/Refresh Rate")]
	public sealed class RefreshRateSlider : SliderOption
	{	
		protected override void ApplySetting(float _value)
		{
			Application.targetFrameRate = Mathf.RoundToInt(_value);
		}
	}
}