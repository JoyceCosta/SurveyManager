using System;
using System.IO;

namespace SurveyManeger
{
	/// Group application I/O operations.
	static class SurveyIO
	{
		/// Saves an object to a file.
		public static void SaveToFile(IStorable obj, string filePath)
		{
			FileInfo file = new FileInfo(filePath);

			using (BinaryWriter writer = new BinaryWriter(file.OpenWrite()))
			{
				obj.Save(writer);
			}
		}

		/// Loads an object with data from a file.
		public static void LoadFromFile(IStorable obj, string filePath)
		{
			FileInfo file = new FileInfo(filePath);

			using (BinaryReader reader = new BinaryReader(file.OpenRead()))
			{
				obj.Load(reader);
			}
		}
	}

	/// Interface that defines methods of saving and loading data using files.
	interface IStorable
	{
		/// Record the data.
		void Save(BinaryWriter writer);

		/// Reads the data.
		void Load(BinaryReader reader);
	}
}
