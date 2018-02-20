using Globals;
using System.Collections.Generic;

namespace Data
{
    public interface IMain
    {
        List<Game> Games { get; }
        List<Player> Players { get; }

        void ReadDatabase();
    }
}
