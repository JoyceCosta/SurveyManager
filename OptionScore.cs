using System;

namespace SurveyManager
{
	/// Votes for an option.
	class OptionScore : IComparable<OptionScore>
	{
		/// Option.
		public Option Option { get; private set; }
		
		/// Number of votes.
		public int Count { get; private set; }

		/// Constructor.
		public OptionScore(Option option, int score)
		{
			this.Option = option;
			this.Count = score;
		}

		/// Defines the comparison as descending order of votes. If two options have the same number of votes,
		/// uses the alphabetical order of the option text.
		public int CompareTo(OptionScore other)
		{
			int comp = -Count.CompareTo(other.Count);

			if (comp == 0)
			{
				return Option.Text.CompareTo(other.Option.Text);
			}

			return comp;
		}
	}
}
