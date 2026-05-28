using System;

namespace UAA9_CODE
{
    public class Program
    {
        static void Main()
        {
            string reponse;

            do
            {
                Console.Clear();
                Console.WriteLine("=== Jeu de Dominos ===\n");

                Random generateur = new Random();

                string[] pseudosDisponibles =
                {
                    "Bob",
                    "Titan",
                    "Bertrand",
                    "Pixel",
                    "Nova",
                    "Luna",
                    "Prout",
                    "Vortex"
                };

                string[] pseudosPris = new string[4];
                int nbPseudosPris = 0;

                // =======================================
                // PSEUDO HUMAIN
                // =======================================

                string pseudoHumain = Fonctions.choisirPseudo(
                    pseudosDisponibles,
                    pseudosPris,
                    ref nbPseudosPris,
                    generateur
                );

                // =======================================
                // TUTORIEL
                // =======================================

                Console.WriteLine("\nVoulez-vous voir le tutoriel du jeu ? (o/n)");

                if (Console.ReadLine().ToLower() == "o")
                {
                    Fonctions.afficherTutoriel();
                }

                // =======================================
                // NOMBRE DE ROBOTS
                // =======================================

                int nombreRobots;

                do
                {
                    Console.WriteLine("\nCombien de robots dans la partie ? (1 à 3)");

                } while (
                    !int.TryParse(Console.ReadLine(), out nombreRobots)
                    || nombreRobots < 1
                    || nombreRobots > 3
                );

                // =======================================
                // NIVEAU ROBOTS
                // =======================================

                int niveauRobots;

                do
                {
                    Console.WriteLine("\nChoisissez le niveau des robots :");
                    Console.WriteLine("1 - Nul");
                    Console.WriteLine("2 - Moyen");
                    Console.WriteLine("3 - Fort");

                } while (
                    !int.TryParse(Console.ReadLine(), out niveauRobots)
                    || niveauRobots < 1
                    || niveauRobots > 3
                );

                Console.WriteLine("\nNiveau choisi : " + niveauRobots);

                // =======================================
                // PSEUDOS ROBOTS
                // =======================================

                string[] pseudosRobots = Fonctions.attribuerPseudosRobots(
                    pseudosDisponibles,
                    pseudosPris,
                    ref nbPseudosPris,
                    nombreRobots,
                    generateur
                );

                // =======================================
                // TALON
                // =======================================

                string[] talon = Fonctions.creationTalon();
                Fonctions.melangeAleatoire(talon);

                // =======================================
                // CREATION DES MAINS
                // =======================================

                int dominosParJoueur;

                if (nombreRobots + 1 == 2)
                    dominosParJoueur = 7;
                else
                    dominosParJoueur = 6;

                string[] mainJoueur = new string[7];
                int nbDominosJoueur = dominosParJoueur;

                string[,] mainsRobots = new string[nombreRobots, 7];

                int indexDistribution = 0;

                for (int i = 0; i < dominosParJoueur; i++)
                {
                    mainJoueur[i] = talon[indexDistribution];
                    indexDistribution++;
                }

                for (int robot = 0; robot < nombreRobots; robot++)
                {
                    for (int i = 0; i < dominosParJoueur; i++)
                    {
                        mainsRobots[robot, i] = talon[indexDistribution];
                        indexDistribution++;
                    }
                }

                // =======================================
                // TALON RESTANT
                // =======================================

                string[] talonRestant = new string[28];
                int nbDominosTalon = 0;

                for (int i = indexDistribution; i < talon.Length; i++)
                {
                    talonRestant[nbDominosTalon] = talon[i];
                    nbDominosTalon++;
                }

                // =======================================
                // AFFICHAGE DES MAINS
                // =======================================

                Console.WriteLine("\n=== Distribution ===\n");

                Console.WriteLine(pseudoHumain + " :");

                for (int i = 0; i < nbDominosJoueur; i++)
                    Console.WriteLine(i + " : " + mainJoueur[i]);

                Console.WriteLine();

                for (int robot = 0; robot < nombreRobots; robot++)
                {
                    Console.WriteLine(pseudosRobots[robot] + " :");

                    for (int i = 0; i < dominosParJoueur; i++)
                        Console.Write(mainJoueur[i] + " ");

                    Console.WriteLine("\n");
                }

                // =======================================
                // TABLE DE JEU
                // =======================================

                string[] table = new string[28];
                int nbDominosTable = 0;

                table[0] = mainJoueur[0];
                nbDominosTable++;

                for (int i = 0; i < nbDominosJoueur - 1; i++)
                    mainJoueur[i] = mainJoueur[i + 1];

                nbDominosJoueur--;

                // =======================================
                // AFFICHAGE MAIN (remplacement de la fonction afficherMain)
                // =======================================

                Console.WriteLine("=== VOS DOMINOS ===");

                for (int i = 0; i < nbDominosJoueur; i++)
                {
                    Console.WriteLine(i + " : " + mainJoueur[i]);
                }

                Console.WriteLine();

                // =======================================
                // FIN DE PARTIE
                // =======================================

                bool partieTerminee = false;

                while (!partieTerminee)
                {
                    Console.Clear();

                    // remplacement de afficherTable
                    Console.WriteLine("\n=== TABLE DE JEU ===");

                    for (int i = 0; i < nbDominosTable; i++)
                    {
                        Console.Write(table[i]);

                        if (i < nbDominosTable - 1)
                            Console.Write(" - ");
                    }

                    Console.WriteLine("\n");

                    // main joueur
                    Console.WriteLine("=== VOS DOMINOS ===");

                    for (int i = 0; i < nbDominosJoueur; i++)
                        Console.WriteLine(i + " : " + mainJoueur[i]);

                    Console.WriteLine();

                    if (nbDominosJoueur == 0)
                    {
                        Console.WriteLine("\nVous avez gagné !");
                        partieTerminee = true;
                    }

                    bool robotsVides = false;

                    for (int robot = 0; robot < nombreRobots; robot++)
                    {
                        int nbRobot = 0;

                        for (int i = 0; i < dominosParJoueur; i++)
                        {
                            if (mainsRobots[robot, i] != null)
                                nbRobot++;
                        }

                        if (nbRobot == 0)
                        {
                            Console.WriteLine("\n" + pseudosRobots[robot] + " gagne !");
                            robotsVides = true;
                        }
                    }

                    if (robotsVides)
                        partieTerminee = true;

                    if (!partieTerminee)
                    {
                        Console.WriteLine("\nTour suivant...");
                        Console.ReadLine();
                        partieTerminee = true;
                    }
                }

                Console.WriteLine("\nVoulez-vous rejouer ? (o/n)");
                reponse = Console.ReadLine().ToLower();

            } while (reponse == "o");

            Console.WriteLine("\nMerci d'avoir joué !");
        }
    }
}