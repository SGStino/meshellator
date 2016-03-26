using Nexus;

namespace Meshellator.Primitives
{
    public class PlaneTessellator : BasicPrimitiveTessellator
    {
        private readonly int _width;
        private readonly int _length;

        protected override Vector3D PositionOffset
        {
            get { return new Vector3D(-(_width - 1) / 2f, -(_length - 1) / 2f, 0); }
        }

        public PlaneTessellator(int width, int length)
        {
            _width = width;
            _length = length;
        }

        public override void Tessellate()
        {
            // Create vertices.
            float oX = _width / 2.0f;
            float oY = _length / 2.0f;
            Vector3D normal = new Vector3D(0, 0, 1);
            for (int z = 0; z < _length; ++z)
                for (int x = 0; x < _width; ++x)
                {
                    float u = x / (float)(_width - 1);
                    float v = z / (float)(_length - 1);
                    float w = 0;
                    AddVertex(new Point3D(x, ((_length - 1) - z), 0), normal, new Point3D(u, v, w)); // Invert z so that winding order is correct.
                }


            for (int z = 0; z < _length - 1; ++z)
                for (int x = 0; x < _width - 1; ++x)
                {
                    int i00 = x + z * (_width);
                    int i01 = x + z * (_width) + 1;
                    int i10 = x + (z + 1) * (_width);
                    int i11 = x + (z + 1) * (_width) + 1;

                    AddIndex(i00);
                    AddIndex(i01);
                    AddIndex(i11);

                    AddIndex(i00);
                    AddIndex(i11);
                    AddIndex(i10);
                }


            /*

            // Create indices.
            for (int z = 0; z < _length - 1; ++z)
                for (int x = 0; x < _width; ++x)
                {
                    // Create vertex for degenerate triangle.
                    if (x == 0 && z > 0)
                        AddIndex(((z + 0) * _width) + x);

                    AddIndex(((z + 0) * _width) + x);
                    AddIndex(((z + 1) * _width) + x);

                    // Create vertex for degenerate triangle.
                    if (x == _width - 1 && z < _length - 2)
                        AddIndex(((z + 1) * _width) + x);
                }

            // It's easiest to define the plane in terms of a triangle strip,
            // and then convert it.
            Int32Collection newIndices = MeshUtility.ConvertTriangleStripToTriangleList(Indices);
            Indices.Clear();
            Indices.AddRange(newIndices);
            */
        }
    }
}