using System;
using System.Collections.Generic;
using System.IO;

namespace SurveyManeger
{
	/// Manages the application's graphical interface.
	class SurveyUI
	{
		/// Active survey.
		private Survey survey;

		/// File associated with the survey.
		private string? surveyFile;

		/// Starts the application execution.
		public void Start()
		{
			while (true)
			{
				// Shows the main menu. The return is the chosen option.
				string option = ShowMainMenu();

				if (option == "1")
				{
					// Create a survey and show the survey menu.
					ShowCreateMenu();
					ShowSurveyMenu();
				}
				else if (option == "2")
				{
					// Load a survey and show the survey menu.
					ShowLoadMenu();
					ShowSurveyMenu();
				}
				else if (option == "3")
				{
					// Exit the application.
					return;
				}
			}
		}

		/// Shows the main menu.
		private string ShowMainMenu()
		{
			while (true)
			{
				Console.Clear();

				Console.WriteLine("MAIN MENU");
				Console.WriteLine("--------------\n");

				Console.WriteLine("1 - Create a survey");
				Console.WriteLine("2 - Load a survey");
				Console.WriteLine("3 - Exit");
				Console.Write("What do you want to do? => ");

				string? option = Console.ReadLine();

				if (option != "1" && option != "2" && option != "3")
				{
					// As long as the option entered is invalid, it remains in the loop.
					continue;
				}

				return option;
			}
		}

		/// Shows the create survey menu.
		private void ShowCreateMenu()
		{
			survey = new Survey();
			surveyFile = null;

			Console.Clear();

			Console.WriteLine("CREATE A NEW SURVEY.");
			Console.WriteLine("----------------------\n");

			while (true)
			{
				// Asks the survey question.
				Console.Write("Question: ");
				string? question = Console.ReadLine();
				if (!String.IsNullOrEmpty(question))
				{
					survey.Question = question;
					break;
				}
			}

			int numOptions;
			while (true)
			{
				// Requests the number of options.
				Console.Write("How many options will the question have? ");

				try
				{
					numOptions = int.Parse(Console.ReadLine());
					break;
				}
				catch (FormatException)
				{
				}
			}

			// Requests for each of the options (ID and text).
			for (int i = 0; i < numOptions; i++)
			{
				string id;
				string text;

				while (true)
				{
					Console.Write("Option ID {0}: ", i + 1);
					id = Console.ReadLine();
					if (!String.IsNullOrEmpty(id))
					{
						break;
					}
				}

				while (true)
				{
					Console.Write("Option text {0}: ", i + 1);
					text = Console.ReadLine();
					if (!String.IsNullOrEmpty(text))
					{
						break;
					}
				}

				// Add the option to the survey.
				survey.SetOption(id, text);
			}

			// Show the survey.
			Console.WriteLine("Options successfully added! see the survey:\n");
			Console.WriteLine(survey.GetFormattedSurvey());

			while (true)
			{
				// Request a file for recording the new survey.
				Console.Write("Enter the file path to save the survey: ");
				string filePath = Console.ReadLine();

				if (!String.IsNullOrWhiteSpace(filePath))
				{
					try
					{
						// Save the survey in the file.
						SurveyIO.SaveToFile(survey, filePath);
						surveyFile = filePath;
						break;
					}
					catch (IOException e)
					{
						Console.WriteLine("There was an error saving the file: {0}", e.Message);
					}
				}
			}

			Console.WriteLine("Survey saved in \"{0}\". Press ENTER to continue...", surveyFile);
			Console.ReadLine();
		}

		/// Shows the voting menu in the survey.
		private void ShowVoteMenu()
		{
			while (true)
			{
				Console.Clear();

				Console.WriteLine("VOTE");
				Console.WriteLine("-----\n");

				Console.WriteLine("Number of votes: {0}\n", survey.VoteCount);

				Console.WriteLine(survey.GetFormattedSurvey());
				Console.Write("Choose an option => ");

				Option option;
				string vote;

				// Request the vote.
				bool valid = survey.Vote(out option, out vote);

				if (valid)
				{
					Console.Write("Thanks for your vote! Do you want to continue voting? (Y/N): ");
					string yn = Console.ReadLine();

					if (yn != "Y" && yn != "y")
					{
						break;
					}
				}
			}

			// At the end of the survey, save the survey in the associated file.
			SurveyIO.SaveToFile(survey, surveyFile);

			Console.Write("End of voting. Press ENTER to continue...");
			Console.ReadLine();
		}

		/// Shows the survey loading menu.
		private void ShowLoadMenu()
		{
			survey = new Survey();
			
			Console.Clear();

			Console.WriteLine("LOAD A SURVEY");
			Console.WriteLine("--------------------\n");

			while (true)
			{
				// Requests the path where the survey is saved.
				Console.Write("Enter the survey file name: ");
				string filePath = Console.ReadLine();
				if (!String.IsNullOrEmpty(filePath))
				{

					try
					{
						// Load the survey from the file.
						SurveyIO.LoadFromFile(survey, filePath);
						surveyFile = filePath;
						Console.Write("The survey was successfully loaded! Press ENTER to continue...");
						Console.ReadLine();
						break;
					}
					catch (IOException e)
					{
						Console.WriteLine("There was an error opening the file: {0}", e.Message);
					}
				}
			}
		}

		/// Shows the survey menu, where you can vote or view the result.
		private void ShowSurveyMenu()
		{
			while (true)
			{
				Console.Clear();

				Console.WriteLine("SURVEY MENU");
				Console.WriteLine("---------------\n");
				Console.WriteLine("Active survey: \"{0}\"", survey.Question);
				Console.WriteLine("Number of votes: {0}\n", survey.VoteCount);

				Console.WriteLine("1 - Vote in the survey");
				Console.WriteLine("2 - View survey results");
				Console.WriteLine("3 - Back to main menu");
				Console.Write("Choose an option => ");
				string option = Console.ReadLine();

				if (option == "1")
				{
					// Vote in the survey.
					ShowVoteMenu();
				}
				else if (option == "2")
				{
					// Show survey results.
					ShowSurveyResults();	
				}
				else if (option == "3")
				{
					// Return to main menu.
					break;
				}
			}
		}
		
		/// Shows the result of the survey.
		private void ShowSurveyResults()
		{
			Console.Clear();

			Console.WriteLine("SURVEY RESULT");
			Console.WriteLine("--------------------\n");

			// Calculates the result.
			List<OptionScore> scores = survey.CalculateScores();

			Console.WriteLine("{0,-23} | {1,-5}", "Option", "Votes");
			Console.WriteLine("-------------------------------");

			foreach (OptionScore score in scores)
			{
				Console.WriteLine("{0,-3}{1,-20} | {2,5}", score.Option.Id, score.Option.Text, score.Count);
			}

			Console.Write("\nPress ENTER to continue...");
			Console.ReadLine();
		}
	}
}
