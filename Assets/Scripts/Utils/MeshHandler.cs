using UnityEngine;

namespace Game.Utils {
    static class MeshHandler {
        public static void DrawLineMesh(Vector3 startPos, Vector3 endPos, float time,
                                        float startWidth = 0.006f, float endWidth = 0.006f,
                                        Material material = null,
                                        string sortingLayer = "Default", int sortingOrder = 0) {
            float length = Vector2.Distance(startPos, endPos);

            GameObject obj = new GameObject("Line");
            obj.transform.position = startPos;
            obj.transform.rotation = VectorHandler.RotationFromVector(endPos - startPos);

            MeshFilter filter = obj.AddComponent<MeshFilter>();
            MeshRenderer renderer = obj.AddComponent<MeshRenderer>();

            renderer.material = material != null ? material : new Material(Shader.Find("Sprites/Default"));

            // Set sorting layer and order
            renderer.sortingLayerName = sortingLayer;
            renderer.sortingOrder = sortingOrder;

            Mesh mesh = new Mesh();
            Vector3[] vertices = {
                new Vector3(0,-startWidth,0),
                new Vector3(0,startWidth,0),
                new Vector3(length,endWidth,0),
                new Vector3(length,-endWidth,0),
            };
            int[] triangles = { 0, 1, 2, 0, 2, 3 };
            Vector2[] uv = {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0),
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            filter.mesh = mesh;

            if (time >= 0) Object.Destroy(obj, time);
        }
    }
}
