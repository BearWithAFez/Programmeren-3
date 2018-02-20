using Globals;
using System.Collections.Generic;

namespace Logic
{
    public interface IMain
    {
        void ReadDatabase();

        List<Player> GetPlayerList();
        List<Game> GetGameList();

        List<string> GetPlayerScoreList(Player p);
        List<string> GetGameScoreList(Game game);
    }
}
