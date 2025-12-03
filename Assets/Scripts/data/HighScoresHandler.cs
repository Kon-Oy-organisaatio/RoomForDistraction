using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    [Serializable]
    class HighScores
    {
        public HighScore[] scores;
    }
    [Serializable]
    public class HighScore
    {
        public int id = 0;
        public string playerName = "";
        public int score = 0;
        public int mstime = 0;
        public string collectedItems = "";
    }
}
