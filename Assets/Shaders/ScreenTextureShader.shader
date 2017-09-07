Shader "Custom/ScreenTextureShader" {
	 Properties {
	 _Color ("Color", Color) = (1,1,1,1)
//	 _MainTex ("Albedo (RGB)", 2D) = "white" {}
      _Detail ("Detail", 2D) = "gray" {}
//      _TransVal ("Transparency Value", Range(0,1)) = 0.5

    }
    SubShader {
//      Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
Tags { "RenderType" = "Transparent" }
      CGPROGRAM
      #pragma surface surf Lambert alpha
//      #pragma surface surf Lambert

      struct Input {

          float4 screenPos;
          float2 uv_MainTex;
      };
      sampler2D _MainTex;
      sampler2D _Detail;
      fixed4 _Color;
      void surf (Input IN, inout SurfaceOutput o) {
//          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;

		
//		  fixed4 c = tex2D (_Detail, IN.uv_MainTex) * _Color;
		  
          float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
           fixed4 c = tex2D (_Detail, screenUV) * _Color;
          o.Albedo = tex2D (_Detail, screenUV).rgb;

//          o.Alpha = c.b * _TransVal;
//o.Alpha = tex2D (_Detail, screenUV).a;
     	o.Alpha = c.a;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }