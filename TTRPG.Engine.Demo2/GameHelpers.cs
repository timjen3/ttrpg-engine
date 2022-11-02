using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTRPG.Engine.Demo2
{
	internal static class GameHelpers
	{
		internal static void MakeGrid(int gridSize, int tileSize, SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			for (int i = 0; i < gridSize; ++i)
			{
				for (int j = 0; j < gridSize; ++j)
				{
					spriteBatch.Draw(texture, new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), color);
				}
			}
		}
	}
}
