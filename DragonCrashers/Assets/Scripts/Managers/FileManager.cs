using UnityEngine;
using System;
using System.IO;

namespace UIToolkitDemo
{
    // writes and read SaveData to and from disk
    public class FileManager
    {
		public static bool WriteToFile(string fileName, string fileContents)
		{
			var fullPath = Path.Combine(Application.persistentDataPath, fileName);

			try
			{
				File.WriteAllText(fullPath, fileContents);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to write to {fullPath} with exception {e}");
				return false;
			}
		}

		public static bool LoadFromFile(string fileName, out string result)
		{
			var fullPath = Path.Combine(Application.persistentDataPath, fileName);
			if (!File.Exists(fullPath))
			{
				File.WriteAllText(fullPath, "");
			}
			try
			{
				result = File.ReadAllText(fullPath);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to read from {fullPath} with exception {e}");
				result = "";
				return false;
			}
		}

	}
}
