Shader "Custom/DissolveURP"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DissolveTex("Dissolve Texture", 2D) = "white" {}
        _Cutoff("Dissolve Cutoff", Range(0,1)) = 0.5
        _EdgeColor("Edge Color", Color) = (1,0,0,1)
        _EdgeThickness("Edge Thickness", Float) = 0.1
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 position : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                float4 _EdgeColor;
                float _Cutoff;
                float _EdgeThickness;
                sampler2D _MainTex;
                sampler2D _DissolveTex;

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.position = TransformObjectToHClip(IN.position.xyz);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    float dissolveValue = tex2D(_DissolveTex, IN.uv).r;
                    float edge = smoothstep(_Cutoff - _EdgeThickness, _Cutoff, dissolveValue);
                    half4 color = tex2D(_MainTex, IN.uv);
                    clip(dissolveValue - _Cutoff);

                    color.rgb = lerp(color.rgb, _EdgeColor.rgb, edge);
                    return color;
                }
                ENDHLSL
            }
        }
}
