using UnityEngine;

namespace ModularOptions {
	[AddComponentMenu("Modular Options/Display/Field Of View Slider")]
	public sealed class FieldOfViewSlider : SliderOption
	{
		protected override void ApplySetting(float _value){
			Camera.main.fieldOfView = _value;
        }
    }
}
