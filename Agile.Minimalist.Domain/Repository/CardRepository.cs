using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Domain.Baseball;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Agile.Minimalist.Repository
{
    public class CardRepository
    {
        private readonly MongoServer _server;
        private readonly MongoDatabase _db;

        public CardRepository(String connString)
        {
            var client = new MongoClient(connString);
            _server = client.GetServer();
            _db = _server.GetDatabase("baseball");
        }

        public Player GetPlayerById(Int32 id)
        {
            var player = _db.GetCollection<Player>("players")
                            .AsQueryable()
                            .FirstOrDefault(p => p.Id == id);
            return player;
        }

        public void Save(Player player)
        {
            _db.GetCollection<Player>("players").Save(player);
        }
    }
}
