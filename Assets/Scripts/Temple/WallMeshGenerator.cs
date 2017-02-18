using System.Collections.Generic;
using UnityEngine;
using Utility;

public class WallMeshGenerator {
    public static Mesh GenerateArcWallMesh(float angleStart,
        float angleEnd,
        float innerRadius,
        float thickness,
        float height,
        float innerFaceArcLength = 1.0f,
        bool smoothInnerOuterNormals = false) {
        if (angleEnd < angleStart) {
            var temp = angleStart;
            angleStart = angleEnd;
            angleEnd = temp;
        }

        var outerRadius = innerRadius + thickness;
        var angle = angleEnd - angleStart;
        var totalInnerArcLength = innerRadius * angle;

        var numSegments = Mathf.CeilToInt(totalInnerArcLength / innerFaceArcLength);
        var segmentAngle = angle / numSegments;

        var numVertices = numSegments + 1;

        // Generate unit length polar vertices for the entire segment
        // v[3]    v[2]
        // o------o
        //         \
        //          \
        //           \
        //   z ^      o v[2]
        //     |      |
        //     0-->   |
        //        x   |
        //            o v[1]
        //           /
        //          /
        //         /
        //        o v[0]
        var abstractVerts = new Vector3[numVertices];
        for (var ivert = 0; ivert < numVertices; ivert++) {
            abstractVerts[ivert] = new Polar(1.0f, angleStart + ivert * segmentAngle).Cartesian3D;
        }

        // Generate wall segments
        //   s[3]
        // o------o
        //         \
        //          \ s[2]
        //           \
        //   z ^      o
        //     |      |
        //     0-->   | s[1]
        //        x   |
        //            o
        //           /
        //          / s[0]
        //         /
        //        o
        var segments = new Tuple<Vector3, Vector3>[numVertices];
        for (var isegment = 0; isegment < numSegments; ++isegment) {
            segments[isegment] = Tuple.Create(abstractVerts[isegment], abstractVerts[isegment + 1]);
        }

        Mesh mesh = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        var triangles = new List<int>();

        Vector3 normal;
        int ivertexstart;
        for (var isegment = 0; isegment < numSegments; ++isegment) {
            var segment = segments[isegment];

            var bottomRight = segment.Item1;
            var bottomLeft = segment.Item2;

            // inner wall
            vertices.Add(innerRadius * bottomRight);
            vertices.Add(innerRadius * bottomLeft);
            vertices.Add(innerRadius * bottomRight + height * Vector3.up);
            vertices.Add(innerRadius * bottomLeft + height * Vector3.up);
            // outer wall
            vertices.Add(outerRadius * bottomRight);
            vertices.Add(outerRadius * bottomLeft);
            vertices.Add(outerRadius * bottomRight + height * Vector3.up);
            vertices.Add(outerRadius * bottomLeft + height * Vector3.up);
            // top part
            vertices.Add(innerRadius * bottomRight + height * Vector3.up);
            vertices.Add(innerRadius * bottomLeft + height * Vector3.up);
            vertices.Add(outerRadius * bottomRight + height * Vector3.up);
            vertices.Add(outerRadius * bottomLeft + height * Vector3.up);

            if (smoothInnerOuterNormals) {
                // normals are generated so inner/outer walls are smoothly rounded
                // inner wall
                normals.Add(-bottomRight);
                normals.Add(-bottomLeft);
                normals.Add(-bottomRight);
                normals.Add(-bottomLeft);
                // outer wall
                normals.Add(bottomRight);
                normals.Add(bottomLeft);
                normals.Add(bottomRight);
                normals.Add(bottomLeft);
            }
            else {
                // normal is constant across segment
                normal = (bottomRight + bottomLeft).normalized;

                // inner wall
                for (var i = 0; i < 4; ++i) normals.Add(-normal);
                // outer wall
                for (var i = 0; i < 4; ++i) normals.Add(normal);
            }

            // top part normals
            for (var i = 0; i < 4; ++i) normals.Add(Vector3.up);

            ivertexstart = isegment * 12;

            // inner wall triangles
            triangles.Add(ivertexstart);
            triangles.Add(ivertexstart + 1);
            triangles.Add(ivertexstart + 3);
            triangles.Add(ivertexstart);
            triangles.Add(ivertexstart + 3);
            triangles.Add(ivertexstart + 2);
            // outer wall triangles
            triangles.Add(4 + ivertexstart);
            triangles.Add(4 + ivertexstart + 3);
            triangles.Add(4 + ivertexstart + 1);
            triangles.Add(4 + ivertexstart);
            triangles.Add(4 + ivertexstart + 2);
            triangles.Add(4 + ivertexstart + 3);
            // top part triangles
            triangles.Add(8 + ivertexstart);
            triangles.Add(8 + ivertexstart + 1);
            triangles.Add(8 + ivertexstart + 3);
            triangles.Add(8 + ivertexstart);
            triangles.Add(8 + ivertexstart + 3);
            triangles.Add(8 + ivertexstart + 2);
        }

        // start
        ivertexstart = vertices.Count;
        var vert = abstractVerts[0];
        vertices.Add(outerRadius * vert);
        vertices.Add(innerRadius * vert);
        vertices.Add(outerRadius * vert + height * Vector3.up);
        vertices.Add(innerRadius * vert + height * Vector3.up);

        normal = new Vector3(vert.z, 0.0f, -vert.x);
        for (var i = 0; i < 4; ++i) normals.Add(normal);

        triangles.Add(ivertexstart);
        triangles.Add(ivertexstart + 1);
        triangles.Add(ivertexstart + 3);
        triangles.Add(ivertexstart);
        triangles.Add(ivertexstart + 3);
        triangles.Add(ivertexstart + 2);

        // end
        ivertexstart = vertices.Count;
        vert = abstractVerts[numVertices - 1];
        vertices.Add(innerRadius * vert);
        vertices.Add(outerRadius * vert);
        vertices.Add(innerRadius * vert + height * Vector3.up);
        vertices.Add(outerRadius * vert + height * Vector3.up);

        normal = new Vector3(vert.z, 0.0f, -vert.x);
        for (var i = 0; i < 4; ++i) normals.Add(normal);

        triangles.Add(ivertexstart);
        triangles.Add(ivertexstart + 1);
        triangles.Add(ivertexstart + 3);
        triangles.Add(ivertexstart);
        triangles.Add(ivertexstart + 3);
        triangles.Add(ivertexstart + 2);

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();

        return mesh;
    }
}