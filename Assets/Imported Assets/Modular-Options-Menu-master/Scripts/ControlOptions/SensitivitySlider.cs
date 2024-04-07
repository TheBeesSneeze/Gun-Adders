using UnityEngine;

namespace ModularOptions {
	/// <summary>
	/// Changes the sensitivity of a camera control script.
	/// A simple reference FPP (First Person Perspective) camera rotation script is included.
	/// Just replace it with your own if it doesn't fit your use-case.
	/// </summary>
	[AddComponentMenu("Modular Options/Controls/Sensitivity Slider")]
	public class SensitivitySlider : SliderOption, ISliderDisplayFormatter {

#if UNITY_EDITOR
		/// <summary>
		/// Auto-assign editor reference, if suitable component is found.
		/// </summary>
		protected override void Reset(){
			OptionInstance.sensitivity = 100;
			base.Reset();
		}
#endif

		protected override void ApplySetting(float _value){
			OptionInstance.sensitivity = _value;
		}

		public string OverrideFormatting(float _value){
			return Mathf.RoundToInt(_value/10f).ToString();
		}
    }
}