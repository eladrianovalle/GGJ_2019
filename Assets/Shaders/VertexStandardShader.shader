Shader "Custom/VertexUnlitTexture" {
     Properties 
     {
         _Color ("Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _Brightness ("Brightness", Vector) = (1,1,1,1)
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf NoLighting vertex:vert 
         #pragma target 3.0
         
         struct Input 
         {
             float2 uv_MainTex;
             float3 vertexColor; // Vertex color stored here by vert() method
         };
         
         struct v2f 
         {
           float4 pos : SV_POSITION;
           fixed4 color : COLOR;
         };
 
         void vert (inout appdata_full v, out Input o)
         {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.vertexColor = v.color;
         }
 
         sampler2D _MainTex;
         fixed4 _Color;
         fixed4 _Brightness;
 
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) 
        {
            return fixed4(0,0,0,0);
        }

         void surf (Input IN, inout SurfaceOutput o) 
         {
             // Albedo comes from a texture tinted by color
             fixed4 vColor =  fixed4(IN.vertexColor.r, IN.vertexColor.g, IN.vertexColor.b, 1);
             fixed4 c = (tex2D (_MainTex, IN.uv_MainTex) * _Color * vColor) + _Brightness;
             o.Albedo = c.rgb * IN.vertexColor; // Combine normal color with the vertex color
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }