// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RealLight2D" {
	Properties{
		_MainTex("Texture (RGB) Alpha (A)", 2D) = "white" {}

	_LightScale("Light Scale", Range(0.0,5.0)) = 1
		_Color("Tint", Color) = (1,1,1,1)

		[Header(Light)]
	_photosensitive("photosensitive", Float) = 0.05
		_ColorScale("ColorScale",Range(0,10)) = 1
		_DirectScale("Direct Scale",Float) = 1
		_PointScale("Point Scale", Float) = 1

		[MaterialToggle] _White("White", Float) = 0
	}

		SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend  SrcAlpha OneMinusSrcAlpha

		LOD 100
		ZWrite    Off
		Cull      Off
		Fog{ Mode Off }


		Pass
	{
		Tags{ LightMode = Vertex }
		CGPROGRAM
#pragma  vertex   vert  
#pragma  fragment frag
#pragma  fragmentoption ARB_precision_hint_fastest
#pragma  target 3.0

#define  MAX_LIGHTS 8

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"

		sampler2D _MainTex;
	half4    _MainTex_ST;

	fixed 	 _White;
	float    _LightScale;
	float    _photosensitive;
	float    _ColorScale;
	float    _DirectScale;
	float	 _PointScale, _Alpha;

	/*uniform float4 _OutlineColor;
	uniform float _Outline;*/
	struct VS_INPUT
	{
		float4 vertex     : POSITION;
		float2 texcoord   : TEXCOORD0;
		fixed4 color : COLOR;
		float3 normal     : NORMAL;

	};

	struct VS_OUT
	{
		float4 position: SV_POSITION;
		half2  uv      : TEXCOORD0;
#if MAX_LIGHTS > 0
		half3  viewpos : TEXCOORD1;
#endif
		half3 worldPos:TEXCOORD2;
		fixed4 color : COLOR;
	};

	float3 GetLight(int i, float3 aViewPos)
	{
		if (unity_LightPosition[i].w == 0)
		{
			return unity_LightColor[i].rgb*_DirectScale;
		}
		else
		{
			half3  toLight = unity_LightPosition[i].xyz - aViewPos;

			float fDistance = length(toLight) /*/ 2*/;
			half   distSq = dot(toLight, toLight)*_photosensitive;
			half distance = dot(toLight, toLight);

			if (fDistance>sqrt(unity_LightAtten[i].w))
			{
				return half3(0, 0, 0);
			}
			half   atten = (sqrt(unity_LightAtten[i].w) - fDistance) / (sqrt(unity_LightAtten[i].w));
			return unity_LightColor[i].rgb * atten*_PointScale;

		}

	}


	VS_OUT vert(VS_INPUT input)
	{
		VS_OUT result;

		result.position = UnityObjectToClipPos(input.vertex);
		result.color = input.color;

#if MAX_LIGHTS > 0
		result.viewpos = mul(UNITY_MATRIX_MV, input.vertex).xyz;
#endif

		result.worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
		result.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
		return result;
	}

	fixed4 frag(VS_OUT inp) : SV_Target
	{
		fixed4 color = tex2D(_MainTex, inp.uv);
	fixed3 light = float3(0,0,0);


#if MAX_LIGHTS > 0
	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		light += GetLight(i, inp.viewpos);

	}
#endif

#if MAX_LIGHTS > 0
	color.rgb *= light;
#endif

	color.rgb *= _ColorScale;
	color.rgb = color.rgb*(1 - _White) + fixed3(1, 1, 1)*_White;
	return color;
	}

		ENDCG
	}

	}
}