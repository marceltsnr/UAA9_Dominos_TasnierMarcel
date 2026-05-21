using System;
namespace UAA9_CODE
{
    internal class Fonctions
    {
        /// <summary>
        /// Crée et retourne un tableau contenant les 28 dominos standards (de [0|0] à [6|6]).
        /// </summary>
        public static string[] creationTalon()
        {
            string[] tableauDominos = new string[28];
            int compteurDomino = 0;
            for (int pointsFaceA = 0; pointsFaceA <= 6; pointsFaceA++)
            {
                for (int pointsFaceB = pointsFaceA; pointsFaceB <= 6; pointsFaceB++)
                {
                    string dominoActuel = "[" + pointsFaceA + "|" + pointsFaceB + "]";
                    tableauDominos[compteurDomino] = dominoActuel;
                    compteurDomino++;
                }
            }
            return tableauDominos;
        }

        /// <summary>
        /// Mélange aléatoirement les dominos du tableau en place via l'algorithme de Fisher-Yates.
        /// </summary>
        /// <param name="tableauDominos">Le tableau de dominos à mélanger.</param>
        public static void melangeAleatoire(string[] tableauDominos)
        {
            Random random = new Random();
            for (int i = 0; i < tableauDominos.Length; i++)
            {
                int indexAleatoire = random.Next(i, tableauDominos.Length);
                string temp = tableauDominos[i];
                tableauDominos[i] = tableauDominos[indexAleatoire];
                tableauDominos[indexAleatoire] = temp;
            }
        }

        /// <summary>
        /// Distribue les dominos mélangés entre le joueur humain et les robots,
        /// puis affiche la main de chaque joueur et le nombre de dominos restants dans le talon.
        /// 7 dominos par joueur si 2 joueurs, 6 sinon.
        /// </summary>
        /// <param name="tableauDominos">Le tableau de dominos mélangés.</param>
        /// <param name="pseudoJoueur">Le pseudo du joueur humain.</param>
        /// <param name="pseudosRobots">Le tableau des pseudos des robots.</param>
        public static void distributionCartes(string[] tableauDominos, string pseudoJoueur, string[] pseudosRobots)
        {
            int nombreJoueurs = pseudosRobots.Length + 1;
            int dominosParJoueur = (nombreJoueurs == 2) ? 7 : 6;
            int index = 0;
            Console.WriteLine("\n=== Distribution des dominos ===\n");

            Console.WriteLine(pseudoJoueur + " (" + dominosParJoueur + " dominos) :");
            for (int i = 0; i < dominosParJoueur; i++)
            {
                Console.Write(tableauDominos[index] + " ");
                index++;
            }
            Console.WriteLine("\n");

            for (int j = 0; j < pseudosRobots.Length; j++)
            {
                Console.WriteLine("ROBOTO | " + pseudosRobots[j] + " (" + dominosParJoueur + " dominos) :");
                for (int i = 0; i < dominosParJoueur; i++)
                {
                    Console.Write(tableauDominos[index] + " ");
                    index++;
                }
                Console.WriteLine("\n");
            }

            int talonRestant = tableauDominos.Length - index;
            Console.WriteLine("Dominos restants dans le talon : " + talonRestant + "\n");
        }

        /// <summary>
        /// Choisit un pseudo unique parmi les pseudos disponibles, soit aléatoirement,
        /// soit depuis la saisie du joueur, et l'enregistre dans le tableau des pseudos pris.
        /// </summary>
        /// <param name="pseudosDisponibles">La liste de tous les pseudos possibles.</param>
        /// <param name="pseudosPris">Le tableau des pseudos déjà attribués.</param>
        /// <param name="nbPseudosPris">Le nombre de pseudos déjà pris (mis à jour par référence).</param>
        /// <param name="generateur">L'instance Random partagée.</param>
        /// <returns>Le pseudo choisi.</returns>
        public static string choisirPseudo(string[] pseudosDisponibles, string[] pseudosPris, ref int nbPseudosPris, Random generateur)
        {
            Console.WriteLine("Entrez votre pseudo (ou tapez 'aleatoire') :");
            string input = Console.ReadLine();
            string pseudo;

            if (input.ToLower() == "aleatoire")
            {
                pseudo = choisirPseudoUnique(pseudosDisponibles, pseudosPris, nbPseudosPris, generateur);
                Console.WriteLine("Votre pseudo aléatoire est : " + pseudo);
            }
            else
            {
                pseudo = input;
                Console.WriteLine("Bienvenue, " + pseudo + " !");
            }

            pseudosPris[nbPseudosPris] = pseudo;
            nbPseudosPris++;
            return pseudo;
        }

        /// <summary>
        /// Attribue aléatoirement un pseudo unique à chaque robot et retourne leurs pseudos.
        /// </summary>
        /// <param name="pseudosDisponibles">La liste de tous les pseudos possibles.</param>
        /// <param name="pseudosPris">Le tableau des pseudos déjà attribués.</param>
        /// <param name="nbPseudosPris">Le nombre de pseudos déjà pris (mis à jour par référence).</param>
        /// <param name="nombreRobots">Le nombre de robots à créer.</param>
        /// <param name="generateur">L'instance Random partagée.</param>
        /// <returns>Un tableau contenant les pseudos des robots.</returns>
        public static string[] attribuerPseudosRobots(string[] pseudosDisponibles, string[] pseudosPris, ref int nbPseudosPris, int nombreRobots, Random generateur)
        {
            string[] pseudosRobots = new string[nombreRobots];
            for (int i = 0; i < nombreRobots; i++)
            {
                string pseudo = choisirPseudoUnique(pseudosDisponibles, pseudosPris, nbPseudosPris, generateur);
                pseudosRobots[i] = pseudo;
                pseudosPris[nbPseudosPris] = pseudo;
                nbPseudosPris++;
                Console.WriteLine("Robot " + (i + 1) + " pseudo : " + pseudo);
            }
            return pseudosRobots;
        }

        /// <summary>
        /// Tire au sort un pseudo dans la liste des disponibles qui n'a pas encore été pris.
        /// </summary>
        /// <param name="pseudosDisponibles">La liste de tous les pseudos possibles.</param>
        /// <param name="pseudosPris">Le tableau des pseudos déjà attribués.</param>
        /// <param name="nbPseudosPris">Le nombre de pseudos déjà pris.</param>
        /// <param name="generateur">L'instance Random partagée.</param>
        /// <returns>Un pseudo unique non encore attribué.</returns>
        public static string choisirPseudoUnique(string[] pseudosDisponibles, string[] pseudosPris, int nbPseudosPris, Random generateur)
        {
            string pseudo;
            bool unique;
            do
            {
                pseudo = pseudosDisponibles[generateur.Next(pseudosDisponibles.Length)];
                unique = true;
                for (int i = 0; i < nbPseudosPris; i++)
                {
                    if (pseudosPris[i] == pseudo)
                    {
                        unique = false;
                    }
                }
            } while (!unique);
            return pseudo;
        }

        /// <summary>
        /// Affiche les règles de base du jeu de dominos et attend que le joueur appuie sur Entrée.
        /// </summary>
        public static void afficherTutoriel()
        {
            Console.WriteLine("\n=== Tutoriel du Jeu de Dominos ===");
            Console.WriteLine("1. Chaque joueur reçoit des dominos.");
            Console.WriteLine("2. Le but est de poser des dominos qui correspondent aux extrémités de la table.");
            Console.WriteLine("3. Les robots ont différents niveaux : Nul, Moyen, Fort.");
            Console.WriteLine("4. Le joueur humain joue à son tour.");
            Console.WriteLine("5. Le jeu continue jusqu'à ce qu'un joueur n'ait plus de dominos.");
            Console.WriteLine("Appuyez sur Entrée pour continuer...");
            Console.ReadLine();
        }
        // =======================================
        // VERIFICATION SI UN DOMINO EST VALIDE
        // =======================================
        // =======================================
        // VERIFICATION SI UN DOMINO EST VALIDE
        // =======================================

        public static bool dominoValide(string domino, string[] table, int nbDominosTable)
        {
            if (nbDominosTable == 0)
            {
                return true;
            }

            int gaucheTable = int.Parse(table[0][1].ToString());
            int droiteTable = int.Parse(table[nbDominosTable - 1][3].ToString());

            int gaucheDomino = int.Parse(domino[1].ToString());
            int droiteDomino = int.Parse(domino[3].ToString());

            if (gaucheDomino == gaucheTable ||
                droiteDomino == gaucheTable ||
                gaucheDomino == droiteTable ||
                droiteDomino == droiteTable)
            {
                return true;
            }

            return false;
        }

    }
}