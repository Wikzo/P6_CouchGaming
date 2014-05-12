Shader "Cg shader using discard" {
        
        Properties
        {
        	_Color ("Main Color", Color) = (1.0, 1.0, 1.0, 0.5) 
        }
   SubShader {
      Pass {
         Cull Off // turn off triangle culling, alternatives are:
         // Cull Back (or nothing): cull only back faces 
         // Cull Front : cull only front faces
 
         CGPROGRAM 
 
         #pragma vertex vert  
         #pragma fragment frag 
 

            uniform float4 _Color;

         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posInObjectCoords : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
            output.posInObjectCoords = input.vertex + (_Time.x * -20); 
            //output.posInObjectCoords.x = sin(_Time.x);
 
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR 
         {
            if (abs(fmod(input.posInObjectCoords.y , 0.8)) < 0.1) 
            {
                // drop the fragment if y coordinate > 0
               return _Color;
            }
            else
            {
            discard;
            	return float4(0.0, 0.0, 0.0, 0.5); // black
               }
         }
 
         ENDCG  
      }
   }
}