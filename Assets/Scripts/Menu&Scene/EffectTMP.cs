using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
 
public class EffectTMP : MonoBehaviour
{
    public TMP_Text tmp;

    void Update()
    {
        tmp.ForceMeshUpdate();//ˢ������Mesh

        var textInfo = tmp.textInfo;//��ȡ������Ϣ

        for (int i = 0; i < textInfo.characterCount; i++)//ѭ������ÿ������
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;        //��ȡÿ�����ֶ�����Ϣ
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                //����
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 3f + orig.x * 1.0f) * 2.0f, 0);//���ĸ��������ƫ�ƣ�ʹ��sin����ʵ�֣�

            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)//д��ո���Ϣ
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            tmp.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
