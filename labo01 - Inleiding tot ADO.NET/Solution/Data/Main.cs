using System.Collections.Generic;
using System.Data;
using System.Linq;
using Globals;
using MySql.Data.MySqlClient;

namespace Data
{
    public class Main : IMain
    {
        private const string LOCAL_DATABASE = "labohighscores";
        private const string LOCAL_USER = "root";
        private const string LOCAL_PASSWORD = "root";
        private const string REMOTE_SERVER = "10.129.28.180";
        private const string REMOTE_DATABASE = "r0579578_C#ProgrammingTechniques_Labo1";
        private const string REMOTE_USER = "r0579578_Dwight";
        private const string REMOTE_PASSWORD = "C#isEenZeerLeukVak";
        private const string SELECT_ALL_FROM = "SELECT * FROM ";

        private MySqlConnection conn;

        public List<Game> Games { get; private set; }
        public List<Player> Players { get; private set; }

        public Main()
        {
            Games = new List<Game>();
            Players = new List<Player>();
            
            // Local
            conn = new MySqlConnection(
                "SERVER=localhost;" +
                "DATABASE=" + LOCAL_DATABASE + ";" +
                "UID=" + LOCAL_USER +";" + 
                "PASSWORD=" + LOCAL_PASSWORD + ";"
            );

            // Remote
            conn = new MySqlConnection(
                "SERVER=" + REMOTE_SERVER + ";" +
                "DATABASE=" + REMOTE_DATABASE + ";" +
                "UID=" + REMOTE_USER + ";" +
                "PASSWORD=" + REMOTE_PASSWORD + ";"
            );
        }

        public void ReadDatabase()
        {
            DataSet dsPlayers = new DataSet();
            DataSet dsGames = new DataSet();
            DataSet dsScores = new DataSet();

            conn.Open();

            // Players
            MySqlDataAdapter adapter = new MySqlDataAdapter(SELECT_ALL_FROM + "players", conn);
            adapter.Fill(dsPlayers, "players");
            Players = dsPlayers.Tables[0].AsEnumerable().Select(dataRow => new Player(dataRow.Field<int>("id"), dataRow.Field<string>("name"), dataRow.Field<string>("nickname"))).ToList();

            // Games
            adapter = new MySqlDataAdapter(SELECT_ALL_FROM + "games", conn);
            adapter.Fill(dsGames, "games");
            Games = dsGames.Tables[0].AsEnumerable().Select(dataRow => new Game(dataRow.Field<int>("id"), dataRow.Field<string>("name"))).ToList();

            // Scores
            adapter = new MySqlDataAdapter(SELECT_ALL_FROM + "scores", conn);
            adapter.Fill(dsScores, "scores");
            List<Score> scores = dsScores.Tables[0].AsEnumerable().Select(dataRow => new Score(dataRow.Field<int>("id"), dataRow.Field<int>("playerId"), dataRow.Field<int>("gameId"), dataRow.Field<int>("points"))).ToList();

            AssignScores(scores);
        }

        private void AssignScores(List<Score> scores)
        {
            foreach (Score s in scores)
            {
                Games.Find(x => x.Id == s.GameId).AddScore(s);
                Players.Find(x => x.Id == s.PlayerId).AddScoreForGame(s);
            }
        }
    }
}
