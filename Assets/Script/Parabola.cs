using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class Parabola : MonoBehaviour
{
    //重力
    [Range(-10, 10)]
    public float mass = 1.3f;
    //抛射速度
    [Range(0, 100)]
    public float shootForce = 1;
    //最大长度
    public float maxLength = 30;
    //两点之间的距离
    const float length = 0.01f;
    //点集合
    List<Vector3> m_List = new List<Vector3>();
    Material m_LineMat;

    public void OnRenderObject()
    {
        CreateLineMaterial();
        m_LineMat.SetPass(0);

        Vector3 newPos = transform.position;
        Vector3 forward = transform.rotation * Vector3.right * length * shootForce;
        Vector3 lastPos = newPos;
        m_List.Add(newPos);
        int i = 0, iMax = 0;
        float dis = 0;
        while (dis < maxLength)
        {
            i++;
            newPos = lastPos + forward + Vector3.up * i * -mass * 0.01f;
            dis += Vector3.Distance(lastPos, newPos);
            m_List.Add(newPos);
            lastPos = newPos;
        }

        GL.Begin(GL.LINES);
        GL.Color(Color.yellow);
        i = 0;
        iMax = m_List.Count;
        for (i = 0; i < iMax; i++)
        {
            GL.Vertex(m_List[i]);
        }
        GL.End();

        m_List.Clear();

    }

    void CreateLineMaterial()
    {
        if (!m_LineMat)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            m_LineMat = new Material(shader);
            m_LineMat.hideFlags = HideFlags.HideAndDontSave;
            m_LineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m_LineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m_LineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            m_LineMat.SetInt("_ZWrite", 0);
        }
    }
}