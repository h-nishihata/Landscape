Shader "Custom/Displacement"
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
			// make fog work
			#pragma multi_compile_fog
			
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

			#define NUM_OCTAVES 16

			half3x3 rotX(float a) {
			    float c = cos(a);
			    float s = sin(a);
			    return half3x3(
	                1, 0, 0,
	                0, c, -s,
	                0, s, c
                );
			}

			half3x3 rotY(float a) {
			    float c = cos(a);
			    float s = sin(a);
			    return half3x3(
		            c, 0, -s,
		            0, 1, 0,
		            s, 0, c
	            );
			}

			float random(half2 pos) {
			    return frac(sin(dot(pos.xy, half2(12.9898, 78.233))) * 43758.5453123);
			}

			float noise(half2 pos) {
			    half2 i = floor(pos);
			    half2 f = frac(pos);
			    float a = random(i + half2(0.0, 0.0));
			    float b = random(i + half2(1.0, 0.0));
			    float c = random(i + half2(0.0, 1.0));
			    float d = random(i + half2(1.0, 1.0));
			    half2 u = f * f * (3.0 - 2.0 * f);
			    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
			}

			float fbm(half2 pos) {
			    float v = 0.0;
			    float a = 0.5;
			    half2 shift = half2(100.0, 100.0);
			    half2x2 rot = half2x2(cos(0.5), sin(0.5), -sin(0.5), cos(0.5));
			    for (int i=0; i<NUM_OCTAVES; i++) {
			        v += a * noise(pos);
			        pos = mul(rot, pos) * 2 + shift;
			        a *= 0.5;
			    }
			    return v;
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
			    half2 p = (i.uv * _ScreenParams.xy * 2.0 - _ScreenParams.xy) / min(_ScreenParams.x, _ScreenParams.y);
			    float t = 0.0, d;
			    float time = _Time * 15.0;
			    
			    half2 q = half2(0.0, 0.0);
			    q.x = fbm(p + 0.00 * time);
			    q.y = fbm(p + half2(1.0, 1.0));
			    half2 r = half2(0.0, 0.0);
			    r.x = fbm(p + 1.0 * q + half2(1.7, 9.2) + 0.15 * time);
			    r.y = fbm(p + 1.0 * q + half2(8.3, 2.8) + 0.126 * time);
			    float f = fbm(p + r);
			    half3 v = half3(f, f, f);
				return half4(v, 1.0);
			}
			ENDCG
		}
	}
}
