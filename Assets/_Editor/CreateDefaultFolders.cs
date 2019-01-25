using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

internal class CreateDefaultFolders
{
	[MenuItem("Custom/Create Default Folders")]
	private static void CreateFolders()
	{
		CreateDirectory("__Scenes");
		CreateDirectory("__Scenes/Levels");
		CreateDirectory("__Scenes/Other");
		CreateDirectory("Animations");
		CreateDirectory("_Editor");
		CreateDirectory("Materials");
		CreateDirectory("Models");
		CreateDirectory("Prefabs");
		CreateDirectory("Plugins");
		CreateDirectory("Resources");
		CreateDirectory("Resources/Sprites");
		CreateDirectory("Scripts");
		CreateDirectory("Scripts/Managers");
		CreateDirectory("Shaders");
		CreateDirectory("Audio");
		CreateDirectory("Textures");
		CreateDirectory("Fonts");
		AssetDatabase.Refresh();
	}

	private static void CreateDirectory(string name)
	{
		string path = Path.Combine(Application.dataPath, name);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
			var textFilePath = path + "/" + "readme.txt";
			System.IO.File.WriteAllText(textFilePath, "README: This is a temporary file for the " + path.ToString() + " folder to be saved in version control");
		}
	}
}