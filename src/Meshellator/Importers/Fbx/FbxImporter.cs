using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Meshellator.Importers.Fbx
{
    [AssetImporter(".fbx", "Filmbox File Format")]
    public class FbxImporter : AssetImporterBase
    {
        public override Scene ImportFile(FileStream fileStream, string fileName)
        { 
            throw new NotImplementedException();
        }
    }
}
