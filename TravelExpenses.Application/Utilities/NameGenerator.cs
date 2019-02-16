using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Application.Interfaces;

namespace TravelExpenses.Application.Utilities
{
    public class NameGenerator : INameGenerator
    {
        private readonly string[] first = new string[]
        {
            "Eilene",
            "Camellia",
            "Williams",
            "Carlena",
            "Arthur",
            "Tamala",
            "Daysi",
            "Cedric",
            "Lenard",
            "Luba",
            "Iluminada",
            "Eura",
            "Janae",
            "Brenton",
            "Salvatore",
            "Shirleen",
            "Marcell",
            "Terrilyn",
            "Booker",
            "Adeline",
            "Margarita",
            "Hilaria",
            "Randolph",
            "Zita",
            "Milan",
            "Eden",
            "Russ",
            "Alesia",
            "Gene",
            "Jerri",
            "Arnetta",
            "Darren",
            "Maybell",
            "Cynthia",
            "Sharmaine",
            "Chassidy",
            "Mathilde",
            "Sang",
            "Wiley",
            "Yelena",
            "Jonnie",
            "Jaleesa",
            "Renea",
            "Silva",
            "Emilie",
            "Randy",
            "Lucy",
            "Leif",
            "Angelyn"
        };

        private readonly string[] last = new string[]
        {
            "Mack",
            "Reyes",
            "Cherry",
            "Reynolds",
            "Barajas",
            "Mcknight",
            "Stewart",
            "Avery",
            "Ross",
            "Hendrix",
            "Mcgee",
            "Mann",
            "Pennington",
            "Hunt",
            "Hayes",
            "Ellison",
            "Foley",
            "Benton",
            "Little",
            "Robinson",
            "Levine",
            "Hull",
            "Howe",
            "Irwin",
            "Wong",
            "Stokes",
            "Hatfield",
            "Mueller",
            "Schneider",
            "Sheppard",
            "Esparza",
            "Rush",
            "Savage",
            "Wolf",
            "Le",
            "Lowe",
            "Potts",
            "Church",
            "Deleon",
            "Watkins",
            "Strickland",
            "Berger",
            "Craig",
            "Miles",
            "Bradley",
            "Dean",
            "Barrera",
            "Cohen",
            "Espinoza",
            "Lindsey",
        };

        private readonly Random rand = new Random();

        public string FirstName()
        {
            return first[rand.Next(first.Length)];
        }

        public string Surname()
        {
            return last[rand.Next(last.Length)];
        }

        public string FullName()
        {
            return $"{FirstName()} {Surname()}";
        }
    }
}
