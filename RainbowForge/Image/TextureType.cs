﻿namespace RainbowForge.Image
{
	public enum TextureType : uint
	{
		Diffuse = 0x0, // older GUI tetxures
		Normal = 0x1, // not just yellow (RG = XY) ones, head detail (RGA = XYZ) as well
		Specular = 0x2, // usually holds gloss, metalness and cavity
		Diffuse2 = 0x3, // GUI texture, cubemaps, spritesheets
		Normal2 = 0x5,
		ColorMask = 0x7
	}
}