using UnityEngine;

namespace ModularOptions
{
	[AddComponentMenu("Modular Options/Display/Shadow Toggle")]
	public sealed class ShadowToggle : ToggleOption
	{
		protected override void ApplySetting(bool _value)
		{
			if (_value == true)
				QualitySettings.shadows = ShadowQuality.All;
			else
				QualitySettings.shadows = ShadowQuality.Disable;
		}
	}
}