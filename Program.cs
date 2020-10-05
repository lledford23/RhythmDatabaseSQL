using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;


namespace RhythmDatabaseSQL
{
    class Band
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryOfOrgins { get; set; }
        public int NumberOfMembers { get; set; }
        public string Website { get; set; }
        public string Style { get; set; }
        public bool IsSigned { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }

    }
    class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Lyrics { get; set; }
        public string Length { get; set; }
        public string Genre { get; set; }
    }
    class RhythmDatabaseSQL : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Band> Bands { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(loggerFactory);

            optionsBuilder.UseNpgsql("server=localhost;database=RhythmDatabaseSQL");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var context = new RhythmDatabaseSQL();
            var ourBands = context.Bands;
            var ourAlbums = context.Albums;
            bool mainMenu = true;

            while (mainMenu)
            {
                Console.WriteLine("Add - (NB) New Band");
                Console.WriteLine("Add - (NA) New Album");
                Console.WriteLine("View - (VB) View All Bands");
                Console.WriteLine("View - (VA) View All Albums");
                Console.WriteLine("View - (AS) View all signed bands");
                Console.WriteLine("View - (NS) View all not signed bands");
                Console.WriteLine("Let band go - (CC) Cancel contract");
                Console.WriteLine("Resign band - (UC) Update Contract");
                Console.WriteLine("Exit - (E) Exit");
                Console.WriteLine("What would you like to do today?");
                var choice = Console.ReadLine();

                switch (choice.ToUpper())
                {
                    case "E":
                        mainMenu = false;
                        break;

                    case "NB":
                        Console.WriteLine("What is the name of the band you would like to add?:");
                        var newBandName = Console.ReadLine();
                        Console.WriteLine("What is the Country of Orgins?");
                        var newBandOrgins = Console.ReadLine();
                        Console.WriteLine("How many members?");
                        var newBandMembers = Console.ReadLine();
                        Console.WriteLine("Website?");
                        var newBandWebsite = Console.ReadLine();
                        Console.WriteLine("Style?");
                        var newBandStyle = Console.ReadLine();
                        Console.WriteLine("Signed?");
                        var newBandSigned = Console.ReadLine();
                        Console.WriteLine("Who is the contact for this band?");
                        var newBandContact = Console.ReadLine();
                        Console.WriteLine("Phone Number?");
                        var newBandPhoneNumber = Console.ReadLine();

                        var newBand = new Band()
                        {
                            Name = newBandName,
                            CountryOfOrgins = newBandOrgins,
                            NumberOfMembers = int.Parse(newBandMembers),
                            Website = newBandWebsite,
                            Style = newBandStyle,
                            IsSigned = newBandSigned,
                            ContactName = newBandContact,
                            ContactPhoneNumber = newBandPhoneNumber,
                        };
                        context.Bands.Add(newBand);
                        context.SaveChanges();
                        break;

                    case "NA":
                        Console.WriteLine("What is the name of the album title you would like to add?:");
                        var newAlbumTitle = Console.ReadLine();
                        Console.WriteLine("What are the lyrics?");
                        var newAlbumLyrics = Console.ReadLine();
                        Console.WriteLine("What is the length of album in minutes?");
                        var newAlbumLength = Console.ReadLine();
                        Console.WriteLine("Genre?");
                        var newAlbumGenre = Console.ReadLine();

                        var newAlbum = new Album()
                        {
                            Title = newAlbumTitle,
                            Lyrics = newAlbumLyrics,
                            Length = newAlbumLength,
                            Genre = newAlbumGenre,
                        };
                        context.Albums.Add(newAlbum);
                        context.SaveChanges();
                        break;

                    case "VB":
                        foreach (var band in ourBands)
                        {
                            Console.WriteLine($"These are the bands within our database: {band.Name}");
                        }
                        break;

                    case "VA":
                        foreach (var album in ourAlbums)
                        {
                            Console.WriteLine($"These are the albums within our database: {album.Title}");
                        }
                        break;

                    case "AS":
                        var signedBands = ourBands.Where(band => band.IsSigned == true);
                        Console.WriteLine($"{signedBands}");
                        break;

                    case "NS":
                        var notSignedBands = ourBands.Where(band => band.IsSigned == false);
                        Console.WriteLine($"{notSignedBands}");
                        break;

                    case "CC":
                        Console.WriteLine("What is the name of the band you'd like to let go?");
                        var bandLetGo = Console.ReadLine();
                        var cancelContract = context.Bands.FirstOrDefault(band => band.Name == bandLetGo);
                        if (cancelContract != null)
                        {
                            cancelContract.IsSigned = false;
                            context.Entry(cancelContract).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        break;

                    case "UC":
                        Console.WriteLine("What is the name of the band you'd like to re-sign");
                        var bandResign = Console.ReadLine();
                        var resignBand = context.Bands.FirstOrDefault(band => band.Name == bandResign);
                        if (resignBand != null)
                        {
                            resignBand.IsSigned = true;
                            context.Entry(resignBand).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        break;
                }

            }
        }

    }
}
