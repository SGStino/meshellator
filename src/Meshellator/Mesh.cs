using System;
using Nexus;
using Nexus.Graphics.Transforms;
using Nexus.Objects3D;
using Nexus.Graphics.Colors;

namespace Meshellator
{
	public class Mesh
	{
		private AxisAlignedBox3D? _bounds;

		public string Name { get; set; }
		public Point3DCollection Positions { get; private set; }
        public ColorFCollection Colors { get; private set; }

        public Vector3DCollection Normals { get; private set; }
        public Vector3DCollection Tangents { get; private set; }
        public Vector3DCollection Bitangents { get; private set; }

        public Point3DCollection TextureCoordinates { get; private set; }
		public Int32Collection Indices { get; private set; }

		public Transform3D Transform { get; set; }

		public int PrimitiveCount
		{
			get { return Indices.Count / 3; }
		}

		public Material Material { get; set; }

		public AxisAlignedBox3D Bounds
		{
			get
			{
				if (_bounds == null)
					_bounds = (Positions.Count > 0)
						? new AxisAlignedBox3D(Positions)
						: AxisAlignedBox3D.Empty;
				return _bounds.Value;
			}
		}

		public Mesh()
		{
            Colors = new ColorFCollection();
			Positions = new Point3DCollection();
			Normals = new Vector3DCollection();
            Tangents = new Vector3DCollection();
            Bitangents = new Vector3DCollection();
            TextureCoordinates = new Point3DCollection();
			Indices = new Int32Collection();
			Transform = new MatrixTransform(Matrix3D.Identity);
		}
	}
}