// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/DiffuseTextureScale" 
{
	Properties
	{
		_BaseColor("Base Color", Color) = (1, 1, 1, 1)
		[NoScaleOffset]_MainTex ("Base (RGB)", 2D) = "white" {}
		_TextureScaleX("Texture Scale X", Float) = 1
		_TextureScaleY("Texture Scale Y", Float) = 1
	}
		
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		float4 _BaseColor;
		float _TextureScaleX;
		float _TextureScaleY;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 uv = float2(IN.uv_MainTex.x * _TextureScaleX, IN.uv_MainTex.y * _TextureScaleY);
			fixed4 c = tex2D(_MainTex, uv) * _BaseColor;			
			o.Albedo = c.rgb;			
			//o.Alpha = c.a;
			o.Alpha = 1;
		}
		ENDCG
	}

Fallback "Mobile/VertexLit"
}
