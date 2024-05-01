using MelonLoader;
using MelonLoader.TinyJSON;
using MelonLoader.Utils;
using System.Reflection;
using System.Text;

namespace TLD_Twitch_Integration
{
	/// <summary>
	/// shamelessly repurposed from 
	/// https://github.com/DigitalzombieTLD/ModSettings/blob/master/JsonModSettings.cs
	/// </summary>
	public abstract class JsonFile
	{

		private readonly string _jsonPath;
		private readonly FieldInfo[] _fields;
		private readonly Dictionary<FieldInfo, object?> _confirmedValues;

		public JsonFile() : this(null)
		{ }

		public JsonFile(string? relativeJsonFilePath)
		{
			_fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			_confirmedValues = new Dictionary<FieldInfo, object?>(_fields.Length);

			_jsonPath = ToAbsoluteJsonPath(relativeJsonFilePath ??
				nameof(TLD_Twitch_Integration));

			LoadOrCreate();
		}

		private static string ToAbsoluteJsonPath(string relativePath)
		{
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentException("JSON file path cannot be null or empty");

			else if (relativePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
				throw new ArgumentException($"JSON file path contains an invalid path character: {relativePath}");

			else if (Path.IsPathRooted(relativePath))
				throw new ArgumentException("JSON file path must be relative. Absolute paths are not allowed.");

			if (Path.GetExtension(relativePath) != ".json")
				relativePath += ".json";

			return Path.Combine(MelonEnvironment.ModsDirectory, relativePath);
		}

		public void Save()
		{
			try
			{
				string json = JSON.Dump(this, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
				File.WriteAllText(_jsonPath, json, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error($"Error while trying to write config file {_jsonPath}: {ex}");
			}
		}

		public void Reload()
		{
			try
			{
				string json = File.ReadAllText(_jsonPath, Encoding.UTF8);
				Variant parsed = JSON.Load(json);
				MethodInfo populateMethod = typeof(JSON).GetMethod(nameof(JSON.Populate))!.MakeGenericMethod(GetType());
				populateMethod.Invoke(null, new object[] { parsed, this });

				foreach (FieldInfo field in _fields)
				{
					_confirmedValues[field] = field.GetValue(this);
				}
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error($"Error while trying to read config file {_jsonPath}: {ex}");
			}
		}

		private void LoadOrCreate()
		{
			if (File.Exists(_jsonPath))
				Reload();

			else
			{
				foreach (FieldInfo field in _fields)
				{
					_confirmedValues[field] = field.GetValue(this);
				}

				string json = JSON.Dump(this, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
				File.WriteAllText(_jsonPath, json, Encoding.UTF8);
			}
		}

	}
}
