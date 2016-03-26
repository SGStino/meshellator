using System;
using System.IO;

namespace Meshellator
{
	public abstract class AssetImporterBase : IAssetImporter
	{
		public abstract Scene ImportFile(Lazy<Stream> fileStream, string fileName);
	}
}