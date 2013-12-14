using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Commands.Baseball;
using Agile.Minimalist.Entity;
using Agile.Minimalist.Eventing.Baseball;
using Agile.Minimalist.Repository;

namespace Agile.Minimalist.Domain.Baseball
{
    public class Player : DomainBase
    {
        #region Domain Properties
        public Int32 Id { get; set; }
        public String PlayerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String NickName { get; set; }
        public String College { get; set; }
        public String Bats { get; set; }
        public String Throws { get; set; }
        public Int32 Height { get; set; }
        public Int32 Weight { get; set; }
        public DateTime? Debut { get; set; }
        public DateTime? FinalGame { get; set; }
        public IList<PlayerYear> Years { get; set; }
        #endregion

        public Player() { }

        public Player(CreatePlayer cmd)
        {
            Id = cmd.LahmanId;
            PlayerId = cmd.PlayerId;
            FirstName = cmd.FirstName;
            LastName = cmd.LastName;
            NickName = cmd.NickName;
            College = cmd.College;
            Bats = cmd.Bats;
            Throws = cmd.Throws;
            Height = cmd.Height;
            Weight = cmd.Weight;
            Debut = cmd.Debut;
            FinalGame = cmd.FinalGame;
            Years = new List<PlayerYear>();

            var repo = new CardRepository("mongodb://localhost:27017");
            repo.Save(this);

            ApplyEvent(new PlayerCreated()
            {
                FullName = String.Format("{0} {1}", FirstName, LastName),
                PlayerId = PlayerId
            });
        }

        public void AddPlayerYear(AddPlayerYear cmd)
        {
            var repo = PopulateMyself(cmd.LahmanId);

            var existingYear = this.Years.FirstOrDefault(p => p.Year == cmd.Year);
            if (existingYear==null)
            {
                this.Years.Add(new PlayerYear()
                {
                    Average = cmd.Average,
                    Doubles = cmd.Doubles,
                    Hits = cmd.Hits,
                    HomeRuns = cmd.HomeRuns,
                    LahmanId = cmd.LahmanId,
                    RunsBattedIn = cmd.RunsBattedIn,
                    Salary = cmd.Salary,
                    StrikeOuts = cmd.StrikeOuts,
                    Triples = cmd.Triples,
                    Year = cmd.Year,
                    TeamName = cmd.TeamName
                });
            }
            else
            {
                existingYear = new PlayerYear()
                {
                    Average = cmd.Average,
                    Doubles = cmd.Doubles,
                    Hits = cmd.Hits,
                    HomeRuns = cmd.HomeRuns,
                    LahmanId = cmd.LahmanId,
                    RunsBattedIn = cmd.RunsBattedIn,
                    Salary = cmd.Salary,
                    StrikeOuts = cmd.StrikeOuts,
                    Triples = cmd.Triples,
                    Year = cmd.Year,
                    TeamName = cmd.TeamName
                };
            }


            repo.Save(this);

            ApplyEvent(new PlayerYearAdded()
            {
                Average = cmd.Average,
                Doubles = cmd.Doubles,
                FirstName = FirstName,
                Hits = cmd.Hits,
                HomeRuns = cmd.HomeRuns,
                Id = Id,
                LastName = LastName,
                PlayerId = PlayerId,
                RunsBattedIn = cmd.RunsBattedIn,
                Salary = cmd.Salary,
                StrikeOuts = cmd.StrikeOuts,
                Team = cmd.TeamName,
                Triples = cmd.Triples,
                Year = cmd.Year
            });
        }

        private CardRepository PopulateMyself(Int32 id)
        {
            var repo = new CardRepository("mongodb://localhost:27017");
            var player = repo.GetPlayerById(id);

            this.Bats = player.Bats;
            this.College = player.College;
            this.Debut = player.Debut;
            this.FinalGame = player.FinalGame;
            this.FirstName = player.FirstName;
            this.Height = player.Height;
            this.Id = player.Id;
            this.LastName = player.LastName;
            this.NickName = player.NickName;
            this.PlayerId = player.PlayerId;
            this.Throws = player.Throws;
            this.Weight = player.Weight;
            this.Years = player.Years;
            return repo;
        }
    }
}
