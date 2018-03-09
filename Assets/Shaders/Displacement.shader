Shader "Custom/Noise"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			#define PI 3.14159265358979323846

			half3 c_scale(float i)
			{
			    half3 c = half3(i, i, i);
			    return c;
			}

			half2 random(half2 st)
			{
			    st = mul(half2x2(1., 99., 0., 1.), st);
				st = frac(sin(st) * 999999.);
			    return st;
			}

			half2 noise(half2 st)
			{
			    return random(floor(st));
			}

			half2 smooth_h_noise(half2 st)
			{
			    return noise(st) * smoothstep(0., 1., frac(st))
			        + noise(st + half2(-1., 0.)) * (1. -smoothstep(0., 1., frac(st)));
			}

			half2 smooth_noise(half2 st)
			{
				return smooth_h_noise(st) * smoothstep(0., 1., frac(mul(half2x2(0., 1., -1., 0), st)))
			        + smooth_h_noise(st+half2(0., -1.)) * (1. -smoothstep(0., 1., frac(mul(half2x2(0., 1., -1., 0), st))));
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half2 st = i.uv * _ScreenParams.xy / _ScreenParams.xy;
			    half2 aspect_ratio = _ScreenParams / min(_ScreenParams.x, _ScreenParams.y);
			    st *= aspect_ratio;
			    st *= 2.;
//			    half2 move = half2(_Time.x*cos(_Time.x*0.112)*0.03, _Time.y*sin(_Time.y*0.1)*0.02);
			    half2 move = half2(_Time.x * 0.05, _Time.y * 0.02);
			    st += move;

			    half3 col = half3(0., 0., 0.);
			    
			    for (int i = 0; i < 15; i++)
			    {
			        float f = 1. / pow(2., float(i));
			        float x = (smooth_noise(move + st / f) * f).x;
			        col += x;
			    }
			    
			    col = col * col * col;
			    col *= 0.33;
			    return half4(col, 1.0);
			}
			ENDCG
		}
	}
}
