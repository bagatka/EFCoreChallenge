using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TempEfCoreLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var summary = BenchmarkRunner.Run<PerformanceTest>();*/

            /*var pc = new PerformanceTest();
            var data1 = pc.BagatkaGetCinemasByMovieId();
            var data2 = pc.GuysGetCinemasByMovieId();*/
        }
    }

    public class PerformanceTest
    {
        [Benchmark]
        public List<Cinema> BagatkaGetCinemasByShowtimeMovieId()
        {
            const int movieId = 2;

            using var context = new CinemasContext();

            return context.Showtime
                .Include(showtime => showtime.Hall)
                    .ThenInclude(hall => hall.Cinema)
                .Where(showtime => showtime.MovieId == movieId)
                .AsEnumerable()
                .Select(showtime => showtime.Hall.Cinema)
                .Distinct()
                .ToList();
        }

        [Benchmark]
        public List<Cinema> GuysGetCinemasByShowtimeMovieId()
        {
            const int movieId = 2;

            using var context = new CinemasContext();

            return context.Cinemas
                .Include(
                    cinema =>
                        cinema.Halls.Where(
                            hall =>
                                hall.Showtimes.Any(
                                    showtime => showtime.MovieId == movieId
                                )
                        )
                )
                    .ThenInclude(
                        hall =>
                            hall.Showtimes.Where(
                                showtime => showtime.MovieId == movieId
                            )
                    )
                .Where(
                    cinema =>
                        cinema.Halls.Any(
                            hall =>
                                hall.Showtimes.Any(
                                    showtime => showtime.MovieId == movieId
                                )
                        )
                )
                .ToList();
        }
    }

    #region DatabaseConfiguration

    public class CinemasContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Showtime> Showtime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=Cinemas;Trusted_Connection=True;");
        }
    }

    [Table("Movie", Schema = "dbo")]
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Table("Cinema", Schema = "dbo")]
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Hall> Halls { get; set; }
    }

    [Table("Hall", Schema = "dbo")]
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }

        public ICollection<Showtime> Showtimes { get; set; }
    }

    [Table("Showtime", Schema = "dbo")]
    public class Showtime
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HallId { get; set; }
        public int MovieId { get; set; }
        public Hall Hall { get; set; }
        public Movie Movie { get; set; }
    }

    #endregion
}