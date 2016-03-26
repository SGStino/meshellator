using System;
using System.IO;

namespace Meshellator
{
	public interface IAssetImporter
	{
		Scene ImportFile(Lazy<Stream> streamSource, string fileName);
	}
}