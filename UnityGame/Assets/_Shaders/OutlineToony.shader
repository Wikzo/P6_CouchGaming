Shader "Custom/OutlineToonShader" {
    Properties {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _Outline ("Outline", Range(0, 0.15)) = 0.08
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        Pass {
 
            Cull Front
            Lighting Off
            ZWrite On
            Tags { "LightMode"="ForwardBase" }
 
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
 
            Cull Back 
            Lighting On
            Tags { "LightMode"="ForwardBase" }
 
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members lightDirection)
#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            uniform float4 _LightColor0;
 

 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
 
            }; 
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 lightDirection;
 
            };
 
            v2f vert (a2v v)
            {
                v2f o;
                //Create a rotation matrix for tangent space
                TANGENT_SPACE_ROTATION; 
                //Store the light's direction in tangent space
                o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
                //Transform the vertex to projection space
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex); 
                //Get the UV coordinates
                return o;
            }
 
            float4 frag(v2f i) : COLOR  
            { 
                
                return float4(0);
 
            } 
 
            ENDCG
        }
 
    }
    FallBack "Diffuse"
}