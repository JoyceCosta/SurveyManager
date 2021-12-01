using System;
using System.IO;

namespace SurveyManager
{
	
	/// Option of a survey.
	class Option : IStorable, IEquatable<Option>
	{
		/// Option ID (which must be entered to choose the option).
		public string? Id { get; set; }

		/// Text associated with the option.
		public string? Text { get; set; }

		public void Save(BinaryWriter writer)
		{
			writer.Write(Id);
			writer.Write(Text);
		}

		public void Load(BinaryReader reader)
		{
			Id = reader.ReadString();
			Text = reader.ReadString();
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Option);
		}

		public bool Equals(Option other)
		{
			if (other == null)
			{
				return false;
			}

			return this.Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
