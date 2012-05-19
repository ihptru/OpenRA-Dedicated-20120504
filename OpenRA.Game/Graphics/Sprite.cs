#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System.Drawing;

namespace OpenRA.Graphics
{
	public class Sprite
	{
		public readonly Rectangle bounds;
		public readonly Sheet sheet;
		public readonly TextureChannel channel;
		public readonly RectangleF uv;
		public readonly float2 size;

		readonly float2[] uvhax;

		public Sprite(Sheet sheet, Rectangle bounds, TextureChannel channel)
		{
			this.bounds = bounds;
			this.sheet = sheet;
			this.channel = channel;

			uv = new RectangleF(
					(float)(bounds.Left) / sheet.Size.Width,
					(float)(bounds.Top) / sheet.Size.Height,
					(float)(bounds.Width) / sheet.Size.Width,
					(float)(bounds.Height) / sheet.Size.Height);

			uvhax = new float2[]
			{
				new float2( uv.Left, uv.Top ),
				new float2( uv.Right, uv.Top ),
				new float2( uv.Left, uv.Bottom ),
				new float2( uv.Right, uv.Bottom ),
			};

			this.size = new float2(bounds.Size);
		}

		public float2 FastMapTextureCoords( int k )
		{
			return uvhax[ k ];
		}

		public void DrawAt( WorldRenderer wr, float2 location, string palette )
		{
			Game.Renderer.WorldSpriteRenderer.DrawSprite( this, location, wr, palette, this.size );
		}

		public void DrawAt( float2 location, int paletteIndex )
		{
			Game.Renderer.WorldSpriteRenderer.DrawSprite( this, location, paletteIndex, this.size );
		}

		public void DrawAt(float2 location, int paletteIndex, float scale)
		{
			Game.Renderer.WorldSpriteRenderer.DrawSprite(this, location, paletteIndex, this.size * scale);
		}

		public void DrawAt( float2 location, int paletteIndex, float2 size )
		{
			Game.Renderer.WorldSpriteRenderer.DrawSprite( this, location, paletteIndex, size );
		}
	}

	public enum TextureChannel
	{
		Red = 0,
		Green = 1,
		Blue = 2,
		Alpha = 3,
	}
}
