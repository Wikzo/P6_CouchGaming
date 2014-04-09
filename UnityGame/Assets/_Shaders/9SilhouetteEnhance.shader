Shader "Cg silhouette enhancement" {
   Properties {
      _Color ("Color", Color) = (1, 1, 1, 0.5) 
      _Exponent("Silhuette", Range(0.0,50.0)) = 1
      _MainTex ("Base (RGB)", 2D) = "white" {}
         // user-specified RGBA color including opacity
   }
   SubShader {
      Tags { "Queue" = "Overlay" } 
         // draw after all opaque geometry has been drawn
      Pass { 
         ZWrite Off // don't occlude other objects
         Blend One OneMinusSrcAlpha
         ZTest Greater
 
         CGPROGRAM 
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
 
         uniform float4 _Color; // define shader property for shaders
         uniform float _Exponent;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 normal : TEXCOORD;
            float3 viewDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.normal = normalize(float3(
               mul(float4(input.normal, 0.0), modelMatrixInverse)));
            output.viewDir = normalize(_WorldSpaceCameraPos 
               - float3(mul(modelMatrix, input.vertex)));
 
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normal);
            float3 viewDirection = normalize(input.viewDir);
 
            float newOpacity = min(1.0, _Color.a 
               / pow(abs(dot(viewDirection, normalDirection)), _Exponent));
            return float4(newOpacity * float3(_Color), newOpacity);
         }
 
         ENDCG
         }
Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}