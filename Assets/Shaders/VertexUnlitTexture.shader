Shader "Custom/StandardVertex" {
     Properties {
         _Color ("Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _Glossiness ("Smoothness", Range(0,1)) = 0.5
         _Metallic ("Metallic", Range(0,1)) = 0.0
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf NoLighting vertex:vert 
         #pragma target 3.0
         struct Input {
             float2 uv_MainTex;
             float3 vertexColor; // Vertex color stored here by vert() method
         };
         
         struct v2f {
           float4 pos : SV_POSITION;
           fixed4 color : COLOR;
         };
 
         void vert (inout appdata_full v, out Input o)
         {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
         }
 
         sampler2D _MainTex;
 
         half _Glossiness;
         half _Metallic;
         fixed4 _Color;
 
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) 
        {
            return fixed4(0,0,0,0);
        }

         void surf (Input IN, inout SurfaceOutput o) 
         {
             // Albedo comes from a texture tinted by color
             fixed4 vColor =  fixed4(IN.vertexColor.r, IN.vertexColor.g, IN.vertexColor.b, 1);
             fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * vColor;
             o.Albedo = c.rgb * IN.vertexColor; // Combine normal color with the vertex color
             // Metallic and smoothness come from slider variables
            //  o.Metallic = _Metallic;
            //  o.Smoothness = _Glossiness;
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }