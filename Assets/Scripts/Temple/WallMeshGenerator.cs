using System.Collections.Generic;
using UnityEngine;
using Utility;

public class WallMeshGenerator {
	private static readonly float FACTOR_UV_SCALING = 1.0f;

    public static Mesh GenerateArcWallMesh(float angleStart,
        float angleEnd,
        float innerRadius,
        float thickness,
        float height,
        float innerFaceArcLength = 1.0f,
        bool smoothInnerOuterNormals = false,
		bool doPolarUvMapping = false) {
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
		var polarVertices = new Polar[numVertices];
        for (var ivert = 0; ivert < numVertices; ivert++) {
            polarVertices[ivert] = new Polar(1.0f, angleStart + ivert * segmentAngle);
        }

		Mesh mesh = new Mesh();

		var vertices = new List<Vector3>();
		var normals = new List<Vector3>();
		var uvs = new List<Vector2>();

		var triangles = new List<int>();

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
		Vector3 normal;
		int ivertexstart;
		float uLeft = 0;
		float uRight = 0;
        for (var isegment = 0; isegment < numSegments; ++isegment) {
            var bottomRightPolar = polarVertices[isegment];
			var bottomLeftPolar = polarVertices[isegment + 1];

			var bottomRight = bottomRightPolar.Cartesian3D;
			var bottomLeft = bottomLeftPolar.Cartesian3D;

			uRight = uLeft;
			uLeft += FACTOR_UV_SCALING;

            // inner wall
            
			vertices.Add(innerRadius * bottomRight);
            vertices.Add(innerRadius * bottomLeft);
            vertices.Add(innerRadius * bottomRight + height * Vector3.up);
            vertices.Add(innerRadius * bottomLeft + height * Vector3.up);

			uvs.Add(new Vector2(uRight, 0.0f));
			uvs.Add(new Vector2(uLeft, 0.0f));
			uvs.Add(new Vector2(uRight, height));
			uvs.Add(new Vector2(uLeft, height));

            // outer wall

            vertices.Add(outerRadius * bottomRight);
            vertices.Add(outerRadius * bottomLeft);
            vertices.Add(outerRadius * bottomRight + height * Vector3.up);
            vertices.Add(outerRadius * bottomLeft + height * Vector3.up);

			uvs.Add(new Vector2(uRight, 0.0f));
			uvs.Add(new Vector2(uLeft, 0.0f));
			uvs.Add(new Vector2(uRight, height));
			uvs.Add(new Vector2(uLeft, height));
				

            // top part

            vertices.Add(innerRadius * bottomRight + height * Vector3.up);
            vertices.Add(innerRadius * bottomLeft + height * Vector3.up);
            vertices.Add(outerRadius * bottomRight + height * Vector3.up);
            vertices.Add(outerRadius * bottomLeft + height * Vector3.up);

			if (doPolarUvMapping) {
				uvs.Add(new Vector2(uRight, 0.0f));
				uvs.Add(new Vector2(uLeft, 0.0f));
				uvs.Add(new Vector2(uRight, 1.0f));
				uvs.Add(new Vector2(uLeft, 1.0f));
			} else {
				uvs.Add((innerRadius * bottomRight + height * Vector3.up).xz());
				uvs.Add((innerRadius * bottomLeft + height * Vector3.up).xz());
				uvs.Add((outerRadius * bottomRight + height * Vector3.up).xz());
				uvs.Add((outerRadius * bottomLeft + height * Vector3.up).xz());
			}


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
        var vert = polarVertices[0].Cartesian3D;
        
		vertices.Add(outerRadius * vert);
        vertices.Add(innerRadius * vert);
        vertices.Add(outerRadius * vert + height * Vector3.up);
        vertices.Add(innerRadius * vert + height * Vector3.up);

		uvs.Add(new Vector2(thickness, 0.0f));
		uvs.Add(new Vector2(0.0f, 0.0f));
		uvs.Add(new Vector2(thickness, height));
		uvs.Add(new Vector2(0.0f, height));

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
        vert = polarVertices[numVertices - 1].Cartesian3D;
        
		vertices.Add(innerRadius * vert);
        vertices.Add(outerRadius * vert);
        vertices.Add(innerRadius * vert + height * Vector3.up);
        vertices.Add(outerRadius * vert + height * Vector3.up);

		uvs.Add(new Vector2(0.0f, 0.0f));
		uvs.Add(new Vector2(thickness, 0.0f));
		uvs.Add(new Vector2(0.0f, height));
		uvs.Add(new Vector2(thickness, height));

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