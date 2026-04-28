using NLEastChallenge;

namespace NLEastTests
{
    public class Tests
    {
        [Test]
        public void NormalScoring_KeepsExactRankScoring()
        {
            var data = DivisionData.GetData(BuildPlayers(), BuildActual(), ScoringMode.Normal);

            var totals = PlayerTotals(data);

            Assert.That(totals.Values, Is.All.EqualTo(0));
        }

        [Test]
        public void NormalScoring_SortsTiesByEachPlayersPredictedTopTeamWinPercentage()
        {
            var actual = new DivisionData
            {
                Name = "Actual",
                Teams =
                [
                    ActualTeam("ATL", 100, 62),
                    ActualTeam("MIA", 90, 72),
                    ActualTeam("WAS", 80, 82),
                    ActualTeam("PHI", 92, 70),
                    ActualTeam("NYM", 70, 92)
                ]
            };
            var players = new[]
            {
                Player("PhiPick", ("PHI", 92), ("NYM", 0), ("ATL", 0), ("MIA", 0), ("WAS", 0)),
                Player("NymPick", ("NYM", 92), ("PHI", 0), ("ATL", 0), ("MIA", 0), ("WAS", 0))
            };

            var data = DivisionData.GetData(players, actual, ScoringMode.Normal);

            Assert.That(data.Headers.Skip(3), Is.EqualTo(new[] { "PhiPick", "NymPick" }));
        }

        [Test]
        public void HorseshoesScoring_UsesRankDistanceAndPairwiseBonus()
        {
            var data = DivisionData.GetData(BuildPlayers(), BuildActual(), ScoringMode.Horseshoes);

            var totals = PlayerTotals(data);
            var wayneColumn = Array.IndexOf(data.Headers, "Wayne");
            var wayneFirstPick = data.Rows[0][wayneColumn];
            var wayneAtlantaPick = data.Rows[2][wayneColumn];

            Assert.Multiple(() =>
            {
                Assert.That(totals["Greg"], Is.EqualTo(10));
                Assert.That(totals["Kevin"], Is.EqualTo(10));
                Assert.That(totals["Wayne"], Is.EqualTo(10));
                Assert.That(totals["Sarge"], Is.EqualTo(9));
                Assert.That(totals["Jeff"], Is.EqualTo(9));
                Assert.That(totals["Brenna"], Is.EqualTo(8));
                Assert.That(wayneFirstPick.Team, Is.EqualTo("PHI"));
                Assert.That(wayneFirstPick.RankDistanceValue, Is.EqualTo(0));
                Assert.That(wayneFirstPick.PairwiseBonusValue, Is.EqualTo(4));
                Assert.That(wayneFirstPick.Value, Is.EqualTo(4));
                Assert.That(wayneAtlantaPick.Team, Is.EqualTo("ATL"));
                Assert.That(wayneAtlantaPick.RankDistanceValue, Is.EqualTo(3));
                Assert.That(wayneAtlantaPick.PairwiseBonusValue, Is.EqualTo(0));
                Assert.That(wayneAtlantaPick.Value, Is.EqualTo(3));
            });
        }

        [Test]
        public void HorseshoesScoring_SortsTiesByPredictedTopTeamWinPercentage()
        {
            var data = DivisionData.GetData(BuildPlayers(), BuildActual(), ScoringMode.Horseshoes);

            Assert.That(data.Headers.Skip(3), Is.EqualTo(new[]
            {
                "Wayne",
                "Kevin",
                "Greg",
                "Jeff",
                "Sarge",
                "Brenna"
            }));
        }

        private static Dictionary<string, int> PlayerTotals(DivisionDataVm data)
        {
            return data.Headers
                .Select((header, index) => new { header, index })
                .Where(column => column.index >= 3)
                .ToDictionary(
                    column => column.header,
                    column => int.Parse(data.Footers[column.index]));
        }

        private static DivisionData BuildActual()
        {
            return new DivisionData
            {
                Name = "Actual",
                Teams =
                [
                    ActualTeam("ATL", 20, 9),
                    ActualTeam("MIA", 13, 16),
                    ActualTeam("WAS", 13, 16),
                    ActualTeam("PHI", 9, 19),
                    ActualTeam("NYM", 9, 19)
                ]
            };
        }

        private static TeamData ActualTeam(string team, int wins, int losses)
        {
            return new TeamData
            {
                Team = team,
                Wins = wins,
                Losses = losses,
                Record = $"{wins}-{losses}"
            };
        }

        private static DivisionData[] BuildPlayers()
        {
            return
            [
                Player("Greg", ("PHI", 95), ("NYM", 92), ("ATL", 0), ("MIA", 0), ("WAS", 0)),
                Player("Sarge", ("NYM", 93), ("PHI", 88), ("ATL", 0), ("MIA", 0), ("WAS", 0)),
                Player("Kevin", ("PHI", 92), ("NYM", 89), ("ATL", 85), ("MIA", 78), ("WAS", 65)),
                Player("Jeff", ("NYM", 91), ("PHI", 90), ("ATL", 0), ("MIA", 0), ("WAS", 0)),
                Player("Brenna", ("NYM", 94), ("PHI", 90), ("ATL", 0), ("WAS", 0), ("MIA", 0)),
                Player("Wayne", ("PHI", 91), ("NYM", 89), ("ATL", 0), ("MIA", 0), ("WAS", 0))
            ];
        }

        private static DivisionData Player(string name, params (string Team, int WinsGuess)[] teams)
        {
            return new DivisionData
            {
                Name = name,
                Teams = teams.Select(team => new TeamData
                {
                    Team = team.Team,
                    WinsGuess = team.WinsGuess
                }).ToArray()
            };
        }
    }
}
