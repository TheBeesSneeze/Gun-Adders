
Shader "Custom/Dissolve" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0
            // Edge color might not be used in this context, but kept for potential aesthetic adjustments
            _EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
    }

        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending
            ZWrite Off // Disable Z writing for transparency

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _DissolveThreshold;
                float4 _EdgeColor;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float noiseScale = 1.0;
                    fixed noise = frac(sin(dot(i.uv * noiseScale ,float2(12.9898,78.233))) * 43758.5453);
                    noise = noise * 0.5 + 0.5; // Normalize noise to 0..1

                    // Interpolate alpha from fully transparent to fully opaque as noise goes from 0 to threshold
                    float alpha = smoothstep(0.0, _DissolveThreshold, noise);

                    // If noise is below the threshold, make it fully transparent
                    if (noise < _DissolveThreshold) {
                        alpha = 0.0; // Ensure full transparency
                    }

                    // Adjusting for edge color and visibility right at the threshold, if needed
                    if (noise >= _DissolveThreshold && noise < _DissolveThreshold + 0.05) {
                        float edgeFactor = (noise - _DissolveThreshold) / 0.05;
                        fixed4 edgeColor = _EdgeColor;
                        edgeColor.a = smoothstep(0.0, 1.0, edgeFactor); // Fade edge color in
                        return edgeColor;
                    }

                    // Return black with adjusted alpha, ensuring it transitions from transparent to black
                    return fixed4(0, 0, 0, alpha);
                }
                ENDCG
            }
        }
}
