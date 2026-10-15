Shader "Hidden/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _Outline ("Outline Width", Range(0, 5)) = 0.1
    }
    SubShader
    {
        Tags{
            "Queue" = "Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float3 _OutlineColor;
            float _Outline;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 uv_up = i.uv + _MainTex_TexelSize.xy*float2(0, 1) * _Outline;
                float2 uv_down = i.uv + _MainTex_TexelSize.xy*float2(0, -1) * _Outline;
                float2 uv_left = i.uv + _MainTex_TexelSize.xy*float2(-1, 0) * _Outline;
                float2 uv_right = i.uv + _MainTex_TexelSize.xy*float2(1, 0) * _Outline;

                float w = tex2D(_MainTex, uv_up).a * tex2D(_MainTex, uv_down).a
                         * tex2D(_MainTex, uv_left).a * tex2D(_MainTex, uv_right).a;
                float t = _Time.y; // 使用时间变量来实现动态效果
                _OutlineColor.r = sin(t + i.uv.x); // 红色通道使用正弦函数变化，与x纹理坐标有关
                _OutlineColor.g = cos(t + i.uv.y); // 绿色通道使用余弦函数变化，与y纹理坐标有关
                _OutlineColor.b = 0.5;
                col.rgb = lerp(_OutlineColor, col.rgb, w);
                
                

                return col;
            }
            ENDCG
        }
    }
}