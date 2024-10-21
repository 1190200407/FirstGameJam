Shader "Custom/SpriteMaskByScreen"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {} // 精灵的主纹理
        _ScreenMask ("Screen Mask", 2D) = "white" {} // 屏幕遮罩纹理
        _LightColor ("LightColor", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1; // 用于屏幕坐标计算
            };

            sampler2D _MainTex;
            sampler2D _ScreenMask;
            float4 _MainTex_ST;
            float4 _LightColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // 将顶点转换为剪裁空间
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);  // 精灵的 UV 坐标
                o.screenPos = ComputeScreenPos(o.pos);  // 保存剪裁空间位置用于后续计算
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 采样精灵纹理
                fixed4 spriteColor = tex2D(_MainTex, i.uv) * _LightColor;

                // 计算屏幕坐标的 UV 值，范围为 [0, 1]，用于采样屏幕遮罩
                float2 screenUV = i.screenPos.xy / i.screenPos.w;  // Clip 空间 -> 屏幕空间

                // 判断像素是否在屏幕内，防止超出屏幕范围时处理
                if (screenUV.x < 0.0 || screenUV.x > 1.0 || screenUV.y < 0.0 || screenUV.y > 1.0)
                {
                    return spriteColor; // 如果在屏幕外，直接返回原始颜色
                }

                // 采样屏幕遮罩纹理
                float maskValue = tex2D(_ScreenMask, screenUV).r; // 读取遮罩的红色通道（假设遮罩是灰度图）

                // 如果遮罩区域是白色（值接近1），则设置 alpha 为 0，不显示像素
                spriteColor.a *= step(maskValue, 0.5); // maskValue > 0.5 为白色区域

                return spriteColor;  // 返回计算后的颜色
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
