Shader "Hidden/GlaucomaSimulator"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurIntensity ("Intensity of the blur", 2D) = "white" {}
        _Radius ("Maximal radius of the blur", Range(0,0.5)) = 0.1
    }
    
    SubShader
    {
        Pass
        {
            CGPROGRAM
#include "UnityCG.cginc"

//#pragma multi_compile CB_TYPE_ONE CB_TYPE_TWO
#pragma target 3.0 // use shader model 3
#pragma vertex vert_img // this defines the vert_img function as the vertex shader function
#pragma fragment frag // this defines the frag function as the fragment shader function
//#pragma fragmentoption ARB_precision_hint_fastest
            
            uniform sampler2D _MainTex;
            uniform sampler2D _BlurIntensity;
            uniform fixed _Radius;
            float4 _MainTex_TexelSize;
            
            fixed4 frag(v2f_img i) : COLOR
            {
                // Direct3D9 needs texel offset
                float2 tc = i.uv;
#ifdef UNITY_HALF_TEXEL_OFFSET
                tc.y += _MainTex_TexelSize.y;
#endif

                // _ScreenParams is predefined in UnityShaderVariables.cginc included by UnityCG.cginc
                float aspect = _ScreenParams.y / _ScreenParams.x;
                fixed4 tmp = tex2D(_BlurIntensity, tc);
                float radius = (tmp.r + tmp.g + tmp.b) / 3.0 * _Radius;
                float4 sum = float4(0.0, 0.0, 0.0, 0.0);

                sum += tex2D(_MainTex, float2(tc.x + (0-2)/2.0*radius*aspect, tc.y + (0-2)/2.0*radius)) * 1;
                sum += tex2D(_MainTex, float2(tc.x + (1-2)/2.0*radius*aspect, tc.y + (0-2)/2.0*radius)) * 4;
                sum += tex2D(_MainTex, float2(tc.x + (2-2)/2.0*radius*aspect, tc.y + (0-2)/2.0*radius)) * 7;
                sum += tex2D(_MainTex, float2(tc.x + (3-2)/2.0*radius*aspect, tc.y + (0-2)/2.0*radius)) * 4;
                sum += tex2D(_MainTex, float2(tc.x + (4-2)/2.0*radius*aspect, tc.y + (0-2)/2.0*radius)) * 1;

                sum += tex2D(_MainTex, float2(tc.x + (0-2)/2.0*radius*aspect, tc.y + (1-2)/2.0*radius)) * 4;
                sum += tex2D(_MainTex, float2(tc.x + (1-2)/2.0*radius*aspect, tc.y + (1-2)/2.0*radius)) *16;
                sum += tex2D(_MainTex, float2(tc.x + (2-2)/2.0*radius*aspect, tc.y + (1-2)/2.0*radius)) *26;
                sum += tex2D(_MainTex, float2(tc.x + (3-2)/2.0*radius*aspect, tc.y + (1-2)/2.0*radius)) *16;
                sum += tex2D(_MainTex, float2(tc.x + (4-2)/2.0*radius*aspect, tc.y + (1-2)/2.0*radius)) * 4;

                sum += tex2D(_MainTex, float2(tc.x + (0-2)/2.0*radius*aspect, tc.y + (2-2)/2.0*radius)) * 7;
                sum += tex2D(_MainTex, float2(tc.x + (1-2)/2.0*radius*aspect, tc.y + (2-2)/2.0*radius)) *26;
                sum += tex2D(_MainTex, float2(tc.x + (2-2)/2.0*radius*aspect, tc.y + (2-2)/2.0*radius)) *41;
                sum += tex2D(_MainTex, float2(tc.x + (3-2)/2.0*radius*aspect, tc.y + (2-2)/2.0*radius)) *26;
                sum += tex2D(_MainTex, float2(tc.x + (4-2)/2.0*radius*aspect, tc.y + (2-2)/2.0*radius)) * 7;

                sum += tex2D(_MainTex, float2(tc.x + (0-2)/2.0*radius*aspect, tc.y + (3-2)/2.0*radius)) * 4;
                sum += tex2D(_MainTex, float2(tc.x + (1-2)/2.0*radius*aspect, tc.y + (3-2)/2.0*radius)) *16;
                sum += tex2D(_MainTex, float2(tc.x + (2-2)/2.0*radius*aspect, tc.y + (3-2)/2.0*radius)) *26;
                sum += tex2D(_MainTex, float2(tc.x + (3-2)/2.0*radius*aspect, tc.y + (3-2)/2.0*radius)) *16;
                sum += tex2D(_MainTex, float2(tc.x + (4-2)/2.0*radius*aspect, tc.y + (3-2)/2.0*radius)) * 4;

                sum += tex2D(_MainTex, float2(tc.x + (0-2)/2.0*radius*aspect, tc.y + (4-2)/2.0*radius)) * 1;
                sum += tex2D(_MainTex, float2(tc.x + (1-2)/2.0*radius*aspect, tc.y + (4-2)/2.0*radius)) * 4;
                sum += tex2D(_MainTex, float2(tc.x + (2-2)/2.0*radius*aspect, tc.y + (4-2)/2.0*radius)) * 7;
                sum += tex2D(_MainTex, float2(tc.x + (3-2)/2.0*radius*aspect, tc.y + (4-2)/2.0*radius)) * 4;
                sum += tex2D(_MainTex, float2(tc.x + (4-2)/2.0*radius*aspect, tc.y + (4-2)/2.0*radius)) * 1;

                sum /= 273.0;
                return sum;
            }
            ENDCG
        }
    }
    FallBack off
}