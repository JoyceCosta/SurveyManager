using System;
using System.Collections.Generic;
using System.IO;

namespace SurveyManager
{
	/// Survey.
	partial class Survey
	{
		/// Votes from a survey. Implements IStorable.
		private class Votes : IStorable
		{
			/// Associated survey.
			private Survey survey;

			/// Dictionary that maps a survey option to a number of votes.
			private Dictionary<Option, int> votes = new Dictionary<Option, int>();

			/// Survey vote counter.
			public int VoteCount { get; private set; }

			/// Constructor.
			public Votes(Survey survey)
			{
				this.survey = survey;
			}

			/// Add a vote to the survey.
			public void AddVote(Option option)
			{
				int count;
				if (votes.TryGetValue(option, out count))
				{
					// If the option already had any votes, it increases the number of votes.
					count++;
					votes[option] = count;
				}
				else
				{
					// If the option had not yet been voted on, it considers 1 vote.
					votes[option] = 1;
				}

				// Increases the total number of votes in the survey.
				VoteCount++;
			}

			/// Calculates survey votes.
			public List<OptionScore> CalculateScores(bool sort = true)
			{
				List<OptionScore> scores = new List<OptionScore>();

				foreach (KeyValuePair<Option, int> entry in votes)
				{
					scores.Add(new OptionScore(entry.Key, entry.Value));
				}

				if (sort)
				{
					// Sort the list if necessary.
					scores.Sort();
				}

				return scores;
			}

			public void Save(BinaryWriter writer)
			{
				// Save dictionary size
				writer.Write(votes.Count);

				foreach (KeyValuePair<Option, int> entry in votes)
				{
					// Record each of the dictionary elements: the option and then the number of votes.
					Option option = entry.Key;
					int numVotes = entry.Value;

					// Calls Option Save() to save the option.
					option.Save(writer);

					writer.Write(numVotes);
				}
			}

			public void Load(BinaryReader reader)
			{
				// Read dictionary size
				int count = reader.ReadInt32();

				// Iterates creating the options and their respective votes.
				for (int i = 0; i < count; i++)
				{
					Option option = new Option();

					// Call Load() from Option to read the option.
					option.Load(reader);

					int numVotes = reader.ReadInt32();

					// Accumulates the total number of votes
					VoteCount += numVotes;

					votes.Add(option, numVotes);
				}
			}
		}
	}
}
