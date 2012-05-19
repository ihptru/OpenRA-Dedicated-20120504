#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenRA.FileFormats;
using OpenRA.FileFormats.Graphics;
using Tao.OpenGl;

namespace OpenRA.Renderer.SdlCommon
{
	public class Texture : ITexture
	{
		public int texture;		/* temp: can be internal again once shaders are in shared code */

		public Texture()
		{
			Gl.glGenTextures(1, out texture);
			ErrorHandler.CheckGlError();
		}

		public Texture(Bitmap bitmap)
		{
			Gl.glGenTextures(1, out texture);
			ErrorHandler.CheckGlError();
			SetData(bitmap);
		}

		void FinalizeInner() { Gl.glDeleteTextures(1, ref texture); }
		~Texture() { Game.RunAfterTick(FinalizeInner); }

		void PrepareTexture()
		{
			ErrorHandler.CheckGlError();
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
			ErrorHandler.CheckGlError();
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
			ErrorHandler.CheckGlError();
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
			ErrorHandler.CheckGlError();

			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_BASE_LEVEL, 0);
			ErrorHandler.CheckGlError();
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_LEVEL, 0);
			ErrorHandler.CheckGlError();
		}

		public void SetData(byte[] colors, int width, int height)
		{
			if (!IsPowerOf2(width) || !IsPowerOf2(height))
				throw new InvalidDataException("Non-power-of-two array {0}x{1}".F(width, height));

			unsafe
			{
				fixed (byte* ptr = &colors[0])
				{
					IntPtr intPtr = new IntPtr((void*)ptr);
					PrepareTexture();
					Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, width, height,
						0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, intPtr);
					ErrorHandler.CheckGlError();
				}
			}
		}

		// An array of RGBA
		public void SetData(uint[,] colors)
		{
			int width = colors.GetUpperBound(1) + 1;
			int height = colors.GetUpperBound(0) + 1;

			if (!IsPowerOf2(width) || !IsPowerOf2(height))
				throw new InvalidDataException("Non-power-of-two array {0}x{1}".F(width,height));

			unsafe
			{
				fixed (uint* ptr = &colors[0,0])
				{
					IntPtr intPtr = new IntPtr((void *) ptr);
					PrepareTexture();
					Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, width, height,
						0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, intPtr);
					ErrorHandler.CheckGlError();
				}
			}
		}

		public void SetData(Bitmap bitmap)
		{
			if (!IsPowerOf2(bitmap.Width) || !IsPowerOf2(bitmap.Height))
			{
				//throw new InvalidOperationException( "non-power-of-2-texture" );
				bitmap = new Bitmap(bitmap, bitmap.Size.NextPowerOf2());
			}

			var bits = bitmap.LockBits(bitmap.Bounds(),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			PrepareTexture();
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, bits.Width, bits.Height,
				0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bits.Scan0);        // todo: weird strides
			ErrorHandler.CheckGlError();
			bitmap.UnlockBits(bits);
		}

		bool IsPowerOf2(int v)
		{
			return (v & (v - 1)) == 0;
		}
	}
}
