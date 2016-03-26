using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FbxScene = ManagedFbx.Scene;
using FbxMesh = ManagedFbx.Mesh;
using FbxSceneNode = ManagedFbx.SceneNode;
using ManagedFbx;
using Nexus;
using Nexus.Graphics.Colors;

namespace Meshellator.Importers.Fbx
{
    [AssetImporter(".fbx", "Filmbox File Format")]
    public class FbxImporter : AssetImporterBase
    {
        public override Scene ImportFile(Lazy<Stream> fileStream, string fileName)
        {
            var fbxScene = FbxScene.Import(fileName);



            var scene = new Scene();

            walkTree(fbxScene, fbxScene.RootNode, scene);



            return scene;
        }

        private void walkTree(FbxScene fbxScene, FbxSceneNode node, Scene scene)
        {
            if (node.Mesh != null)
                scene.Meshes.Add(createMesh(fbxScene, node));
        }

        private Mesh createMesh(FbxScene fbxScene, FbxSceneNode fbxNode)
        {
            var fbxMesh = fbxNode.Mesh;

            fbxScene.BakeTransform(fbxNode);

            if (!fbxMesh.Triangulated)
                fbxMesh = fbxMesh.Triangulate();








            var mesh = new Mesh();


            convert(fbxMesh.Vertices, mesh.Positions);
            convert(fbxMesh.TextureCoords, mesh.TextureCoordinates);
            convert(fbxMesh.VertexColours, mesh.Colors);
            mesh.Name = fbxNode.Name;


            return mesh;
        }

        private void convert(Colour[] source, ColorFCollection target)
        {
            target.Clear();
            target.Capacity = source.Length;
            target.AddRange(source.Select(v => new ColorF((float)v.A, (float)v.R, (float)v.G, (float)v.B)));
        }

        private void convert(Vector2[] source, Point3DCollection target)
        {
            target.Clear();
            target.Capacity = source.Length;
            target.AddRange(source.Select(v => new Point3D((float)v.X, (float)v.Y, 0.0f)));
        }
        private void convert(Vector3[] source, Point3DCollection target)
        {
            target.Clear();
            target.Capacity = source.Length;
            target.AddRange(source.Select(v => new Point3D((float)v.X, (float)v.Y, (float)v.Z)));
        }
        private void convert(Vector2[] source, Point2DCollection target)
        {
            target.Clear();
            target.Capacity = source.Length;
            target.AddRange(source.Select(v => new Point2D((float)v.X, (float)v.Y)));
        }

    }
}
