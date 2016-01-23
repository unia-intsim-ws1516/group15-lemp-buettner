Shader "Hidden/CVDMachado"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlindIntensity ("Color-Blind Intensity", Range(0,1)) = 1
	}
	
	SubShader
	{
		Pass
		{
			CGPROGRAM
#include "UnityCG.cginc"

#pragma target 3.0
#pragma multi_compile CB_TYPE_ONE CB_TYPE_TWO
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
			
			uniform sampler2D _MainTex;
			uniform fixed _BlindIntensity;
			float4 _MainTex_TexelSize;
			
			fixed4 frag(v2f_img i) : COLOR
			{
                const int NUM_TRAFOS = 11;
                fixed3x3 trafos[NUM_TRAFOS];
                
                trafos[0] = fixed3x3(
                    1.0, 0.0, 0.0,
                    0.0, 1.0, 0.0, 
                    0.0, 0.0, 1.0
                );

#ifdef CB_TYPE_ONE
                trafos[1] = fixed3x3(
                    0.856167,  0.182038, -0.038205,
                    0.029342,  0.955115,  0.015544,
                   -0.002880, -0.001563,  1.004443
                );
                trafos[2] = fixed3x3(
                    0.734766,  0.334872, -0.069637,
                    0.051840,  0.919198,  0.028963,
                   -0.004928, -0.004209,  1.009137
                );
                trafos[3] = fixed3x3(
                    0.630323,  0.465641, -0.095964,
                    0.069181,  0.890046,  0.040773,
                   -0.006308, -0.007724,  1.014032
                );
                trafos[4] = fixed3x3(
                    0.539009,  0.579343, -0.118352,
                    0.082546,  0.866121,  0.051332,
                   -0.007136, -0.011959,  1.019095
                );
                trafos[5] = fixed3x3(
                    0.458064,  0.679578, -0.137642,
                    0.092785,  0.846313,  0.060902,
                   -0.007494, -0.016807,  1.024301
                );
                trafos[6] = fixed3x3(
                    0.385450,  0.769005, -0.154455,
                    0.100526,  0.829802,  0.069673,
                   -0.007442, -0.022190,  1.02963
                );
                trafos[7] = fixed3x3(
                    0.319627,    0.849633,  -0.169261,
                    0.106241,    0.815969,   0.077790,
                   -0.007025,   -0.028051,   1.035076
                );
                trafos[8] = fixed3x3(
                    0.259411,    0.923008,  -0.182420,
                    0.110296,    0.804340,   0.085364,
                   -0.006276,   -0.034346,   1.040622
                );
                trafos[9] = fixed3x3(
                    0.203876,    0.990338,  -0.194214,
                    0.112975,    0.794542,   0.092483,
                   -0.005222,   -0.041043,   1.046265
                );
                trafos[10] = fixed3x3(
                    0.152286,  1.052583, -0.204868,
                    0.114503,  0.786281,  0.099216,
                   -0.003882, -0.048116,  1.051998
                );
#else
                trafos[1] = fixed3x3(
                    0.866435,   0.177704,   -0.044139,
                    0.049567,   0.939063,    0.011370,
                   -0.003453,   0.007233,    0.996220
                );
                trafos[2] = fixed3x3(
                    0.760729,   0.319078,   -0.079807,
                    0.090568,   0.889315,    0.020117,
                   -0.006027,   0.013325,    0.992702
                );
                trafos[3] = fixed3x3(
                    0.675425,   0.433850,   -0.109275,
                    0.125303,   0.847755,    0.026942,
                   -0.007950,   0.018572,    0.989378
                );
                trafos[4] = fixed3x3(
                    0.605511,   0.528560,   -0.134071,
                    0.155318,   0.812366,    0.032316,
                   -0.009376,   0.023176,    0.986200
                );
                trafos[5] = fixed3x3(
                    0.547494,   0.607765,   -0.155259,
                    0.181692,   0.781742,    0.036566,
                   -0.010410,   0.027275,    0.983136
                );
                trafos[6] = fixed3x3(
                    0.498864,   0.674741,   -0.173604,
                    0.205199,   0.754872,    0.039929,
                   -0.011131,   0.030969,    0.980162
                );
                trafos[7] = fixed3x3(
                    0.457771,   0.731899,   -0.189670,
                    0.226409,   0.731012,    0.042579,
                   -0.011595,   0.034333,    0.977261
                );
                trafos[8] = fixed3x3(
                    0.422823,   0.781057,   -0.203881,
                    0.245752,   0.709602,    0.044646,
                   -0.011843,   0.037423,    0.97442
                );
                trafos[9] = fixed3x3(
                    0.392952,   0.823610,   -0.216562,
                    0.263559,   0.690210,    0.046232,
                   -0.011910,   0.040281,    0.971630
                );
                trafos[10] = fixed3x3(
                    0.367322,   0.860646,   -0.227968,
                    0.280085,   0.672501,    0.047413,
                   -0.011820,   0.042940,    0.968881
                );
#endif

				// Direct3D9 needs texel offset
				float2 uv = i.uv;
#ifdef UNITY_HALF_TEXEL_OFFSET
				uv.y += _MainTex_TexelSize.y;
#endif
				//Original Color
				fixed4 origColor = tex2D(_MainTex, uv);
                
                const int a = _BlindIntensity * (NUM_TRAFOS - 1);
                const int b = a + 1;
                const fixed s = (_BlindIntensity * (NUM_TRAFOS - 1)) - a;
                const fixed3x3 T = (1.0 - s) * trafos[a] + s * trafos[b];

				fixed4 retColor;
				retColor.rgb = mul(T, origColor.rgb);
				retColor.a = origColor.a;
                
                retColor = saturate(retColor);
				
				return retColor;
			}
			ENDCG
		}
	}
	FallBack off
}
