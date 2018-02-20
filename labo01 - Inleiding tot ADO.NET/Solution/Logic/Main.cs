using System.Collections.Generic;
using Globals;

namespace Logic
{
    public class Main : IMain
    {
        private Data.IMain data;

        public Main()
        {
            data = new Data.Main();
        }

        public void ReadDatabase() => data.ReadDatabase();

        public List<Player> GetPlayerList()
        {
            List<Player> players = new List<Player>(data.Players);
            players.Sort((x,y) => y.GetAverageTopScores(new List<Game>(data.Games)).CompareTo(x.GetAverageTopScores(new List<Game>(data.Games))));
            return players;
        }

        public List<Game> GetGameList() => new List<Game>(data.Games);

        public List<string> GetPlayerScoreList(Player p)
        {
            List<string> returnable = new List<string>();

            foreach (Game g in data.Games)
            {
                Score s = p.MaxScoreForGame(g);
                if (s == null) continue;
                returnable.Add(g.Name.PadRight(15) + "\t" + s.Points + "\t" + g.AllTimeTopScore.Points);
            }

            return returnable;
        }

        public List<string> GetGameScoreList(Game g)
        {
            List<string> returnable = new List<string>();

            foreach (Score s in g.Scores) returnable.Add(data.Players.Find(x => x.Id == s.PlayerId).NickName.PadRight(15) + "\t" + s.Points);

            return returnable;
        }
    }
}
