using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Markov;

namespace OWASP_2013_Demo.Search
{
	public interface ISearchProvider
	{
		List<SearchResult> Search(string query);
	}

	public class SearchResult
	{
		public int Relevance { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }
	}

	public class DummySearchProvider : ISearchProvider
	{
		private static Random rng = new Random();
		private MarkovChain<string> titleChain;
		private MarkovChain<string> descriptionChain;

		public DummySearchProvider()
		{
			titleChain = BuildSentenceChain("sample-titles.txt", 1);
			descriptionChain = BuildSentenceChain("sample-descriptions.txt", 2);
		}

		public List<SearchResult> Search(string query)
		{
			var results = new List<SearchResult>();

			if (string.IsNullOrEmpty(query))
			{
				return results;
			}

			var titlesReturned = new HashSet<string>();
			var descriptionsReturned = new HashSet<string>();
			int resultsTried = 0;

			while (titlesReturned.Count() < 10 && resultsTried < 300)
			{
				resultsTried++;

				var randomTitle = string.Join(" ", GetSentence(titleChain, 12));
				var randomDescription = string.Join(" ", Enumerable.Range(0, rng.Next(1, 4)).SelectMany(_ => GetSentence(descriptionChain, 25)));

				if ((randomTitle.Contains(query.ToLower()) || randomDescription.Contains(query.ToLower()))
					&& !titlesReturned.Contains(randomTitle) && !descriptionsReturned.Contains(randomDescription))
				{
					titlesReturned.Add(randomTitle);
					descriptionsReturned.Add(randomDescription);

					results.Add(new SearchResult()
					{
						Title = TitleCase(randomTitle),
						Description = SentenceCase(randomDescription),
						Relevance = rng.Next(1, 6),
						Url = "#"
					});
				}
			}

			return results;
		}

		private static IEnumerable<string> GetSentence(MarkovChain<string> chain, int maxWordLength)
		{
			string[] sentence;
			while((sentence = chain.Chain(rng).ToArray()).Length > maxWordLength) { }

			return sentence;
		}

		private static MarkovChain<string> BuildSentenceChain(string resourceName, int order)
		{
			var sampleLines = SplitLines(ReadEmbeddedResource(resourceName).ToLower());
			var samples = sampleLines.Select(line => SplitWords(line));

			var chain = new MarkovChain<string>(order);
			foreach (var sample in samples)
			{
				chain.Add(sample, 1);
			}

			return chain;
		}

		private static string ReadEmbeddedResource(string filename)
		{
			var assembly = Assembly.GetAssembly(typeof(DummySearchProvider));
			var resourceName = typeof(DummySearchProvider).Namespace + "." + filename;

			using (var stream = assembly.GetManifestResourceStream(resourceName))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		private static string[] SplitLines(string input)
		{
			return input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
		}

		private static string[] SplitWords(string input)
		{
			return input.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
		}

		private static string TitleCase(string str)
		{
			if (str == null) return null;
			return Regex.Replace(str, @"(^|\s)\w", m => m.Value.ToUpper());
		}

		private static string SentenceCase(string str)
		{
			if (str == null) return null;
			return Regex.Replace(str, @"(^|(\.|\?|\!)\s+)\w", m => m.Value.ToUpper());
		}
	}
}
