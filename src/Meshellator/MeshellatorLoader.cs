using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Meshellator.Primitives;
using Nexus;
using Nexus.Graphics.Colors;

namespace Meshellator
{
    public class MeshellatorLoader
    {
        private static MeshellatorLoader _instance;
        private static MeshellatorLoader Instance
        {
            get { return _instance ?? (_instance = new MeshellatorLoader()); }
        }

        private readonly CompositionContainer _container;

        [ImportMany]
        private Lazy<IAssetImporter, IAssetImporterMetadata>[] AssetImporters { get; set; }

        private MeshellatorLoader()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            //catalog.Catalogs.Add(new DirectoryCatalog(".", "*.dll"));
            //catalog.Catalogs.Add(new DirectoryCatalog(".", "*.exe"));

            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public static Scene ImportFromFile(string fileName)
        {
            // Look for importer which handles this file extension.
            string fileExtension = Path.GetExtension(fileName).ToUpper();
            var lazyAssetImporter = Instance.AssetImporters.SingleOrDefault(l => l.Metadata.Extension.ToUpper() == fileExtension);
            if (lazyAssetImporter == null)
                throw new ArgumentException("Could not find importer for extension '" + fileExtension + "'");

            IAssetImporter assetImporter = lazyAssetImporter.Value;
            var fileStream = new Lazy<Stream>(() => File.OpenRead(fileName));
            return assetImporter.ImportFile(fileStream, fileName);
        }

        public static Scene CreateFromCube(float size)
        {
            return CreateFromPrimitive(new CubeTessellator(size));
        }

        public static Scene CreateFromCylinder(float radius, float height, int tessellationLevel)
        {
            return CreateFromPrimitive(new CylinderTessellator(radius, height, tessellationLevel));
        }

        public static Scene CreateFromPlane(int width, int length)
        {
            return CreateFromPrimitive(new PlaneTessellator(width, length));
        }

        public static Scene CreateFromSphere(float radius, int tessellationLevel)
        {
            return CreateFromPrimitive(new SphereTessellator(radius, tessellationLevel));
        }

        public static Scene CreateFromTeapot(float size, int tessellationLevel)
        {
            return CreateFromPrimitive(new TeapotTessellator(size, tessellationLevel));
        }

        public static Scene CreateFromTorus(float radius, float thickness, int tessellationLevel)
        {
            return CreateFromPrimitive(new TorusTessellator(radius, thickness, tessellationLevel));
        }

        private static Scene CreateFromPrimitive(BasicPrimitiveTessellator tessellator)
        {
            tessellator.Tessellate();

            Material material = new Material();
            material.Name = "Default";
            material.DiffuseColor = ColorsRgbF.Blue;
            material.SpecularColor = ColorsRgbF.White;

            Mesh mesh = new Mesh();
            mesh.Positions.AddRange(tessellator.Positions);
            mesh.Indices.AddRange(tessellator.Indices);
            mesh.Normals.AddRange(tessellator.Normals);
            mesh.Material = material;

            Scene scene = new Scene { FileName = "[New Sphere]" };
            scene.Materials.Add(material);
            scene.Meshes.Add(mesh);

            return scene;
        }

        public static bool IsSupportedFormat(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToUpper();
            return Instance.AssetImporters.Any(l => l.Metadata.Extension.ToUpper() == fileExtension);
        }
    }
}