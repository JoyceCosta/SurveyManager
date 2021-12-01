using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SurveyManeger
{	
	/// Survey.
	partial class Survey : IStorable
	{
		/// Dictionary that maps an option ID to an option.
		private IDictionary<string, Option> options = new Dictionary<string, Option>();
		
		/// Reference object responsible for calculating votes.
		private Votes votes;
		
		/// Survey question.
		public string Question { get; set; }

		/// Survey vote counter.
		public int VoteCount
		{
			get
			{
				// Delegates the call to VoteCount from the Votes object.
				return votes.VoteCount;
			}
		}

		/// Constructor.
		public Survey()
		{
			// Instantiates the object that calculates votes.
			votes = new Votes(this);
		}

		/// Add or change a survey option. If the ID doesn't exist yet, add; otherwise it changes.
		public void SetOption(string id, string text)
		{
			// Creates the option by converting the ID to uppercase.
			Option option = new Option();
			option.Id = id.ToUpper();
			option.Text = text;

			if (!options.ContainsKey(id))
			{
				// Add if the ID does not exist.
				options.Add(id, option);
			}
			else
			{
				// Changes if the ID already exists.
				options[id] = option;
			}
		}

		/// Returns the survey in a string format.
		public string GetFormattedSurvey()
		{
			// Use a StringBuilder to avoid string concatenations.
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(Question);

			foreach (Option option in options.Values)
			{
				sb.Append(option.Id).Append(" - ").AppendLine(option.Text);
			}

			return sb.ToString();
		}

		
		/// Vote in the survey, by typing the option in the console.
		public bool Vote(out Option option, out string vote)
		{
			// Reads the vote.
			vote = Console.ReadLine();
			
			// Converts the vote to uppercase.
			vote = vote.ToUpper();
			
			// Look up the object in the dictionary.
			bool valid = options.TryGetValue(vote, out option);

			if (valid)
			{
				// If found, compute the vote.
				votes.AddVote(option);
			}

			return valid;
		}

		/// Calculates survey votes.
		public List<OptionScore> CalculateScores(bool sort = true)
		{
			// Delegates the calculation to the Votes object.
			return votes.CalculateScores(sort);
		}

		public void Save(BinaryWriter writer)
		{
			// Saves the question, the number of options and then each option.
			writer.Write(Question);
			writer.Write(options.Count);

			foreach (Option option in options.Values)
			{
				// Calls Save() from Option to save the option.
				option.Save(writer);
			}

			// Save the survey votes.
			votes.Save(writer);
		}

		public void Load(BinaryReader reader)
		{
			// Load the survey question and then each option.
			Question = reader.ReadString();

			options = new Dictionary<string, Option>();
			int count = reader.ReadInt32();

			for (int i = 0; i < count; i++)
			{
				Option option = new Option();

				// Call Load() from Option to read the option.
				option.Load(reader);

				options[option.Id] = option;
			}

			// Load the survey votes.
			votes.Load(reader);
		}
	}
}
