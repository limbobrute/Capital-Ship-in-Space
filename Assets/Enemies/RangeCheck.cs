using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class RangeCheck
{
    public const float DiskThickness = 0.1f;

    public static void DrawGizmoDisk(Transform t, float radius)
    {
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.2f);
        Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, new Vector3(1f, DiskThickness, 1f));
        Gizmos.DrawSphere(Vector3.zero, radius);
        Gizmos.matrix = oldMatrix;
    }
}
