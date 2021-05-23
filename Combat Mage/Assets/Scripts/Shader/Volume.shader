Shader "Custom/Volume"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "black" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _MarchSteps("March steps", Range(1, 100)) = 100
		_MarchStepSize("March step size", Range(0, 0.2)) = 0.01
		_MarchStepValue("March step value", Range(0, 1)) = 0.01
		_MediumColor("Medium color", Color) = (1,1,1,1)
		_MediumThreshold("Medium threshold", Range(0, 1)) = 0.5

    }
    SubShader
    {

	Tags{ "Queue" = "Transparent" }
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	Pass
		{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


           #include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float3 worldPos : TEXCOORD1;
		};
    

		sampler3D _MainTex;
		float _MarchSteps;
		float _MarchStepsSize;
		float _MarchStepValue;
		float4 _MediumColor;
		float _MediumThreshold;

	appdata vert(appdata vertice) //Retorna 
		{
			appdata o;
			o.worldPos = mul(UNITY_MATRIX_M, vertice.vertex);
			o.vertex = UnityObjectToClipPos(vertice.vertex);
			return o;
        }

	 bool isInsidePlane(float3 posicao)
	 {
		 float largura = 1.0f;
		 float altura = 30.0f;

		 if (altura <posicao.y)
		 {
			 return false;
		 }

		 else
		 {
             return true;
		 }
		
     }

	 fixed4 frag(appdata i) : SV_Target
	 {
		 float3 View = normalize(i.worldPos - _WorldSpaceCameraPos); //float3->Direção dos raios
	     float3 currentPos = i.worldPos; //Posição do raio

	 float density = 0.0f;
	 for (int i = 0; i < _MarchSteps; i++) { //Loop de maximo de passos
		 currentPos += View * _MarchStepsSize; //Utiliza uma posiçao do raio

		 float noise = tex3D(_MainTex, currentPos + _Time.xxx).r;
		 if (noise > _MediumThreshold && isInsidePlane(currentPos))
			 density += _MarchStepValue * noise;
	 }

	 return saturate(float4(_MediumColor.rgb * density, density));
     }
	 ENDCG
    }

  }
}