﻿using System.IO;

namespace RainbowForge.Core.DataBlock
{
	/// <summary>
	///     No-op class to serve as a common parent for all ForgeAsset subcomponents
	/// </summary>
	public interface IAssetBlock
	{
		public Stream GetDataStream(BinaryReader r);
	}
}