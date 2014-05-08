Shader "Cg shader using blending" {
   Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _Bump ("Bump", 2D) = "bump" {}
        _ColorMerge ("Color Merge", Range(0.1,20000)) = 8
        _Ramp ("Ramp Texture", 2D) = "white" {}
        _Outline ("Outline", Range(0, 0.15)) = 0.08
    }

   SubShader {
      Tags { "Queue" = "Transparent" } 
         // draw after all opaque geometry has been drawn
      
      Pass {
 
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
            
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 tangent : TANGENT;
            }; 
 
            struct v2f
            {
                float4 pos : POSITION;
            };
 
            float _Outline;
            uniform float4 _Color;
 
            v2f vert (a2v v)
            {
                v2f o;
                float4 pos = mul( UNITY_MATRIX_MV, v.vertex); 
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);  
                normal.z = -0.4;
                pos = pos + float4(normalize(normal),0) * _Outline;
                o.pos = mul(UNITY_MATRIX_P, pos);
 
                return o;
            }
 
            float4 frag (v2f IN) : COLOR
            {
            return _Color;
            	
            }
 
            ENDCG
 
        }

      Pass {
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
 
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
 
         CGPROGRAM 
 
         #pragma vertex vert 
         #pragma fragment frag
 
         float4 vert(float4 vertexPos : POSITION) : SV_POSITION 
         {
            return mul(UNITY_MATRIX_MVP, vertexPos);
         }
 
         float4 frag(void) : COLOR 
         {
            return float4(1.0, 1.0, 1.0, 0.0); 
               // the fourth component (alpha) is important: 
               // this is semitransparent green
         }
 
         ENDCG  
      }

      
   }
}