using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            // Check if the database exists and create a new one if not
            _context.Database.EnsureCreated();

            //InsertNewSamuraiWithQuote("Mark", "Thanks for dinner!");
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ExplicitLoadQuotes();
            //AddNewSamuraiToAnExistingBattle();
            RemoveSamuraiFromBattle();

            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void InsertNewSamuraiWithQuote(string name, string quote)
        {
            var samurai = new Samurai
            {
                Name = name,
                Quotes = new List<Quote>
                {
                    new Quote { Text = quote }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void GetAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";

            _context.SaveChanges();
        }

        private static void DeleteSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraisWithQuotes = _context.Samurais
                .Include(s => s.Quotes)
                .ToList();

            var samuraisWithDinnerQuotes = _context.Samurais
                .Where(s => s.Quotes.Any(Queryable => Queryable.Text.Contains("dinner")))
                .Include(s => s.Quotes)
                .ToList();
        }

        private static void ProjectSomeProperties()
        {
            var someProps = _context.Samurais
                .Select(s => new { s.Id, s.Name })
                .ToList();

            var somePropsWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name, s.Quotes })
                .ToList();

            var somePropsWithDinnerQuotes = _context.Samurais
               .Select(s => new { s.Id, s.Name, DinnerQuotes = s.Quotes.Where(q => q.Text.Contains("dinner")) })
               .ToList();
        }

        private static void ExplicitLoadQuotes()
        {
            //_context
            //    .Set<Horse>()
            //    .Add(new Horse { SamuraiId = 10, Name = "Mr. Ed" });
            //_context.SaveChanges();
            //_context.ChangeTracker.Clear();

            var samurai = _context.Samurais.Find(10);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void AddNewSamuraiToAnExistingBattle()
        {
            var samurai = _context.Samurais
                .FirstOrDefault();
            var battle = _context.Battles
                .Include(b => b.Samurais)
                .FirstOrDefault();
            battle.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromBattle()
        {
            var battle = _context.Battles
                .Include(b => b.Samurais)
                .FirstOrDefault();
            var samurai = battle.Samurais.First();
            battle.Samurais.Remove(samurai);

            _context.SaveChanges();
        }


    }
}
