using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Commands.Baseball;
using Agile.Minimalist.Models;
using Agile.Minimalist.Repository;
using Nancy;
using Nancy.ModelBinding;

namespace Agile.Minimalist.Modules
{
    public class CardModule : NancyModule
    {
        private readonly ICardRepository _repo;
        public CardModule(ICardRepository repo)
        {
            _repo = repo;

            Get["/card/search"] = args =>
            {
                return View["search.cshtml"];
            };

            Post["/card/search"] = criteria =>
            {
                var req = this.Bind<HitterSearch>();
                var searchModel = _repo.GetHitterSearch(req);
                return View["results.cshtml", searchModel];
            };

            Post["/card/facetsearch"] = criteria =>
            {
                var req = this.Bind<HitterFacetSearch>();
                var searchModel = _repo.GetFacetedHitterSearch(req.YearStart, req.YearEnd, req.MaxSalary, req.MinHomeRuns, req.field, req.criteria);
                return View["results.cshtml", searchModel];
            };

            Get["/newcard"] = _ =>
            {
                Agile.Minimalist.Commands.Writer.ICommandWriter writer =
                    new Agile.Minimalist.Commands.Writer.CommandWriter();
                using (var conn = new SqlConnection("server=AGILE-L03\\DSHSQL08_1;initial catalog=Lahman2012;integrated security=true;"))
                {
                    using (var cmd = new SqlCommand("SELECT * FROM dbo.Master", conn))
                    {
                        cmd.Connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var player = new CreatePlayer()
                                {
                                    Bats = reader["Bats"].ToString(),
                                    College = reader["College"].ToString(),
                                    Debut = reader["debut"] == DBNull.Value ? default(DateTime?) : Convert.ToDateTime(reader["debut"]),
                                    FinalGame = reader["finalGame"] == DBNull.Value ? default(DateTime?) : Convert.ToDateTime(reader["finalGame"]),
                                    FirstName = reader["nameFirst"].ToString(),
                                    Height = 75,
                                    LahmanId = reader.GetInt32(reader.GetOrdinal("lahmanID")),
                                    LastName = reader["nameLast"].ToString(),
                                    NickName = reader["nameNick"].ToString(),
                                    PlayerId = reader["playerID"].ToString(),
                                    Throws = reader["Throws"].ToString(),
                                    Weight = 150
                                };
                                writer.SendCommand(player);
                            }
                        }
                    }
                }

                return "new player submitted";
            };

            Get["/newcardstats"] = args =>
            {
                Agile.Minimalist.Commands.Writer.ICommandWriter writer =
                    new Agile.Minimalist.Commands.Writer.CommandWriter();
                using (var conn = new SqlConnection("server=AGILE-L03\\DSHSQL08_1;initial catalog=Lahman2012;integrated security=true;"))
                {
                    using (var cmd = new SqlCommand("SELECT * FROM dbo.BaseballHitters", conn))
                    {
                        cmd.Connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var playerYear = new AddPlayerYear()
                                {
                                    Year = Convert.ToInt32(reader["yearID"]),
                                    Average = Convert.ToDouble(reader["average"] == DBNull.Value ? 0 : reader["average"]),
                                    Doubles = Convert.ToInt32(reader["doubles"]),
                                    Hits = Convert.ToInt32(reader["H"] == DBNull.Value ? 0 : reader["H"]),
                                    HomeRuns = Convert.ToInt32(reader["HR"] == DBNull.Value ? 0 : reader["HR"]),
                                    LahmanId = Convert.ToInt32(reader["lahmanID"]),
                                    RunsBattedIn = Convert.ToInt32(reader["rbi"]),
                                    Salary = Convert.ToInt32(reader["salary"]),
                                    StrikeOuts = Convert.ToInt32(reader["so"]),
                                    TeamName = reader["teamName"].ToString(),
                                    Triples = Convert.ToInt32(reader["triples"])
                                };
                                writer.SendCommand(playerYear);
                            }
                        }
                    }
                }

                return "player year submitted";
            };

            Get["/testcard"] = _ =>
            {
                var player = new CreatePlayer()
                {
                    Bats = "R",
                    College = "University of Notre Dame",
                    Debut = DateTime.Now.AddYears(-10),
                    FinalGame = DateTime.Now.AddYears(-2),
                    FirstName = "David",
                    Height = 74,
                    LahmanId = 100000,
                    LastName = "Hoerster",
                    NickName = "Dave",
                    PlayerId = "davehoerster",
                    Throws = "R",
                    Weight = 180
                };
                Agile.Minimalist.Commands.Writer.ICommandWriter writer =
                    new Agile.Minimalist.Commands.Writer.CommandWriter();
                writer.SendCommand(player);

                return "test player created!";
            };

            Get["/testcard/{year}"] = args =>
            {
                var playerYear = new AddPlayerYear()
                {
                    Average = 0.275,
                    Doubles = 25,
                    Hits = 100,
                    HomeRuns = 50,
                    LahmanId = 100000,
                    RunsBattedIn = 75,
                    Salary = 10000000,
                    StrikeOuts = 3,
                    TeamName = "Pittsburgh Pirates",
                    Triples = 15,
                    Year = args.year
                };
                Agile.Minimalist.Commands.Writer.ICommandWriter writer =
                    new Agile.Minimalist.Commands.Writer.CommandWriter();
                writer.SendCommand(playerYear);

                return "player year for year " + args.year + " created!";
            };

        }
    }
}
