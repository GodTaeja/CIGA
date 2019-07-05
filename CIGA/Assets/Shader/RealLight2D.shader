// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RailGun/RGCharacterVertexLit" {
	Properties{
		_MainTex("Texture (RGB) Alpha (A)", 2D) = "white" {}
		_MaskTex("Mask Tex",2D) = "white"{}
		_NoiseTex("Noise Tex",2D) = "white"{}
		_LightScale("Light Scale", Range(0.0,5.0)) = 1
		_Color("Tint", Color) = (1,1,1,1)

		[Header(Extend)]
		_AddColor("AddColor", Color) = (1,1,1,1)
		_AddColorScale("_AddColorScale",Range(0.0,10.0)) = 0
		_RimColor("RimColor",Color) = (1,1,1,1)
		_RimPower("RimPower",Range(0.0,100.0)) = 0.0

		[Header(Light)]
		_photosensitive("photosensitive", Float) = 0.05
		_ColorScale("ColorScale",Range(0,10)) = 1
		_DirectScale("Direct Scale",Float) = 1
		_PointScale("Point Scale", Float) = 0.5

		[Header(Hurt)]
		_SizeX("Size X", Float) = 0
		_SizeY("Size Y", Float) = 0
		_AtmoColor("Atmosphere Color", Color) = (0, 0.4, 1.0, 1)
		_Size("Size",Float) = 0
		_Alpha("Alpha",Range(0,1)) = 0

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

			sampler2D _MainTex,_MaskTex,_NoiseTex;
	half4    _MainTex_ST;

	fixed 	 _White;
	float    _LightScale;
	float    _photosensitive;
	float	 _RimPower;
	float    _ColorScale;
	float    _DirectScale;
	float	 _PointScale, _Alpha;
	float4 _AddColor;
	float	_AddColorScale, _Size;

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

			if (unity_LightPosition[i].w != 0 && distance + unity_LightAtten[i].w/5.0f > unity_LightAtten[i].w )
			{
				half   atten = 1.0 / ((distSq * fDistance*_LightScale * (unity_LightAtten[i].z))+3.0f);
				return ((unity_LightColor[i].rgb * pow(atten, 1.5f)))*_PointScale;
			}
			half   atten = 1.0 / ((distSq * fDistance*_LightScale * (unity_LightAtten[i].z)) + 1.0f);
			return ((unity_LightColor[i].rgb * pow(atten, 1.5f)))*_PointScale;

		}

	}

#pragma multi_compile __ POINT SPOT

	half3 computeLighting(int idx, half3 dirToLight, half3 eyeNormal, half3 viewDir, half4 diffuseColor, half atten) {
		half NdotL = max(dot(eyeNormal, dirToLight), 0.0);
		// diffuse
		half3 color = NdotL * diffuseColor.rgb * unity_LightColor[idx].rgb;


		if (unity_LightPosition[idx].w == 0)
		{
			return color * atten*_DirectScale;
		}

		return color * atten;
	}

	half3 computeOneLight(int idx, float3 eyePosition, half3 eyeNormal, half3 viewDir, half4 diffuseColor) {
		float3 dirToLight = unity_LightPosition[idx].xyz;
		half att = 1.0;


#if defined(POINT) || defined(SPOT)
		dirToLight -= eyePosition * unity_LightPosition[idx].w;
		float fDistance = length(dirToLight);
		// distance attenuation
		float distSqr = dot(dirToLight, dirToLight);
		if (unity_LightPosition[idx].w != 0)
		{
			att *= fDistance*_PointScale;
		}


		att /= (1.0f + unity_LightAtten[idx].z * distSqr);
		if (unity_LightPosition[idx].w != 0 && distSqr > unity_LightAtten[idx].w+0.3f) att = 0.0; // set to 0 if outside of range
	/*	distSqr = max(distSqr, 1);*/ // don't produce NaNs if some vertex position overlaps with the light
		dirToLight *= rsqrt(distSqr);
#if defined(SPOT)

		// spot angle attenuation
		half rho = max(dot(dirToLight, unity_SpotDirection[idx].xyz), 0.0);
		half spotAtt = (rho - unity_LightAtten[idx].x) * unity_LightAtten[idx].y;
		att *= saturate(spotAtt);
#endif
#endif

		/*att *= 5;*/ // passed in light colors are 2x brighter than what used to be in FFP
		return min(computeLighting(idx, dirToLight, eyeNormal, viewDir, diffuseColor, att), 1.0);
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
	//color = color * _Color;
	color = fixed4(color.rgb * (1 - _White) + fixed3(0.6,0.6,0.6) * _White,color.a);

	//half4 color1 = half4(1, 1, 1, 1);

	//half3 fixedNormal = half3(0, 0, -1);
	//half3 eyeNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, fixedNormal));
	////half3 eyeNormal = half3(0,0,1);
	//half3 viewDir = 0.0;

	//for (int il = 0; il < MAX_LIGHTS; ++il) {
	//	light += computeOneLight(il, inp.viewpos, eyeNormal, viewDir, color1);
	//}

#if MAX_LIGHTS > 0
	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		light += GetLight(i, inp.viewpos);
	}
#endif
//
//
#if MAX_LIGHTS > 0
	color.rgb *= light;
#endif

	fixed rimValue = tex2D(_MainTex, inp.uv).a;
	fixed noiseValue = tex2D(_NoiseTex, inp.uv).a;
	fixed4 maskValue = tex2D(_MaskTex, inp.uv);

	color.rgb *= _ColorScale;
	color.rgb = color.rgb + _AddColor*_AddColorScale;
	return color;

	return color;
	}


		ENDCG
	}

		Pass
	{
		Name "Plane"
		Tags{ LightMode = Vertex }

		//Cull Front
		Blend SrcAlpha one

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
	sampler2D _RimTex, _MaskTex, _AlphaTex;


	fixed4   _AtmoColor;
	float	  _Alpha;
	float	 _SizeX, _SizeY, _Size;


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
		half2  uv3     : TEXCOORD0;
#if MAX_LIGHTS > 0
		half3  viewpos : TEXCOORD1;
#endif
		half3 worldPos:TEXCOORD2;
		fixed4 color : COLOR;
		float3 normal:TEXCOORD3;
	};


	VS_OUT vert(VS_INPUT input)
	{
		VS_OUT result;

		input.vertex.xyz += float3(_SizeX, _SizeY,0 );
		input.vertex.xyz *= _Size;
		result.position = UnityObjectToClipPos(input.vertex);
		//result.color = input.color * _Color;

		result.uv3 = TRANSFORM_TEX(input.texcoord, _MainTex);
		return result;
	}

	fixed4 frag(VS_OUT i) : SV_Target
	{
		float4 color = _AtmoColor;
		fixed rimValue = tex2D(_MainTex, i.uv3).a;

		return color*rimValue*_Alpha;
	}


		ENDCG
	}
	}
}