using Beautify.Universal;
using UnityEngine;
using UnityEngine.Rendering;

namespace ModularOptions
{
	[AddComponentMenu("Modular Options/Display/Render Slider")]
	public sealed class RenderSlider : SliderOption
	{
		protected override void ApplySetting(float _value)
		{
			BeautifySettings.sharedSettings.downsamplingMultiplier.value = _value;
		}
	}
}
