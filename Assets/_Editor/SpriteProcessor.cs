using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpriteProcessor : AssetPostprocessor {

//	void OnPostprocessTexture(Texture2D texture)
	void OnPreprocessTexture()
	{
		string lowerCaseAssetPath = assetPath.ToLower ();
		bool isInSpritesDirectory = lowerCaseAssetPath.IndexOf ("/sprites/") != -1;

		if (isInSpritesDirectory) 
		{
			TextureImporter textureImporter = (TextureImporter) assetImporter;
			TextureImporterSettings textureImporterSettings = new TextureImporterSettings ();

			textureImporter.ReadTextureSettings (textureImporterSettings);

			textureImporterSettings.spriteMeshType = SpriteMeshType.FullRect;
			textureImporterSettings.mipmapEnabled = false;
			textureImporterSettings.filterMode = FilterMode.Point;
			textureImporter.SetTextureSettings (textureImporterSettings);

			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.spritePackingTag = "SpriteSheet";
			textureImporter.maxTextureSize = 512;
		}
	}
}