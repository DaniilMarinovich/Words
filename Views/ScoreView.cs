using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Words.Views
{
    public class ScoreView
    {
        private readonly string _filePath;

        public ScoreView(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "{}");
            }
        }

        public int GetScore(string player)
        {
            var scores = LoadScores();
            return scores.ContainsKey(player) ? scores[player] : 0;
        }

        public string GetAllScores()
        {
            return string.Join(",", LoadScores().Select(kv => $"\n{kv.Key}: {kv.Value}"));
        }

        public void AddWin(string player)
        {
            var scores = LoadScores();

            if (scores.ContainsKey(player))
            {
                ++scores[player];
            }
            else
            {
                scores[player] = 1;
            }

            SaveScores(scores);
        }

        private Dictionary<string, int> LoadScores()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
        }

        private void SaveScores(Dictionary<string, int> scores)
        {
            var json = JsonSerializer.Serialize(scores);
            File.WriteAllText(_filePath, json);
        }
    }
}
