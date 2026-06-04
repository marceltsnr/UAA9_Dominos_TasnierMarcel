using System;

namespace UAA9_CODE
{
    internal class Fonctions
    {
        // =======================================
        // CREATION DU TALON
        // =======================================

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

        // =======================================
        // MELANGE ALEATOIRE
        // =======================================

        /// <summary>
        /// Mélange aléatoirement les dominos du tableau en place via l'algorithme de Fisher-Yates.
        /// </summary>
        /// <param name="tableauDominos">Le tableau de dominos à mélanger.</param>
        /// <param name="generateur">L'instance Random partagée.</param>
        public static void melangeAleatoire(string[] tableauDominos, Random generateur)
        {
            for (int i = 0; i < tableauDominos.Length; i++)
            {
                int indexAleatoire = generateur.Next(i, tableauDominos.Length);
                string temp = tableauDominos[i];
                tableauDominos[i] = tableauDominos[indexAleatoire];
                tableauDominos[indexAleatoire] = temp;
            }
        }

        // =======================================
        // CHOIX DE PSEUDO JOUEUR
        // =======================================

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
            afficherEncadre("BIENVENUE AU JEU DE DOMINOS");

            Console.Write("  Entrez votre pseudo (ou tapez 'aleatoire') : ");
            string input = Console.ReadLine();
            string pseudo;

            if (input.ToLower() == "aleatoire")
            {
                pseudo = choisirPseudoUnique(pseudosDisponibles, pseudosPris, nbPseudosPris, generateur);
                Console.WriteLine("  Pseudo attribué : " + pseudo);
            }
            else
            {
                pseudo = input;
                Console.WriteLine("  Bienvenue, " + pseudo + " !");
            }

            pseudosPris[nbPseudosPris] = pseudo;
            nbPseudosPris++;
            return pseudo;
        }

        // =======================================
        // ATTRIBUTION DES PSEUDOS ROBOTS
        // =======================================

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
            }

            return pseudosRobots;
        }

        // =======================================
        // CHOIX D'UN PSEUDO UNIQUE
        // =======================================

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

        // =======================================
        // TUTORIEL
        // =======================================

        /// <summary>
        /// Affiche les règles de base du jeu de dominos et attend que le joueur appuie sur Entrée.
        /// </summary>
        public static void afficherTutoriel()
        {
            Console.Clear();
            afficherEncadre("TUTORIEL DU JEU DE DOMINOS");

            Console.WriteLine("  1. Chaque joueur reçoit 6 ou 7 dominos selon le nombre de joueurs.");
            Console.WriteLine("  2. Le premier domino est posé automatiquement sur la table.");
            Console.WriteLine("  3. A votre tour, choisissez un domino dont une valeur correspond");
            Console.WriteLine("     à l'une des deux extremites de la table.");
            Console.WriteLine("  4. Si vous ne pouvez pas jouer, piochez dans le talon (touche P).");
            Console.WriteLine("  5. Si le talon est vide et que vous ne pouvez pas jouer, passez.");
            Console.WriteLine("  6. Le premier joueur a vider sa main gagne la partie !");
            Console.WriteLine("  7. Les robots ont differents niveaux :");
            Console.WriteLine("     - Nul   : joue un domino valide au hasard");
            Console.WriteLine("     - Moyen : prefere les doubles, sinon joue au hasard");
            Console.WriteLine("     - Fort  : choisit le domino avec la valeur la plus haute");
            Console.WriteLine();
            Console.Write("  Appuyez sur Entree pour continuer...");
            Console.ReadLine();
        }

        // =======================================
        // VERIFICATION SI UN DOMINO EST VALIDE
        // =======================================

        /// <summary>
        /// Vérifie si un domino donné peut être posé sur la table de jeu.
        /// Si la table est vide, tout domino est valide.
        /// Sinon, le domino doit correspondre à l'une des deux extrémités de la table.
        /// </summary>
        /// <param name="domino">Le domino à vérifier, sous la forme "[x|y]".</param>
        /// <param name="table">Le tableau des dominos actuellement sur la table.</param>
        /// <param name="nbDominosTable">Le nombre de dominos actuellement posés sur la table.</param>
        /// <returns>True si le domino peut être posé, false sinon.</returns>
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

        // =======================================
        // AFFICHER LA TABLE
        // =======================================

        /// <summary>
        /// Affiche dans la console tous les dominos actuellement posés sur la table de jeu,
        /// séparés par des tirets, avec les extrémités en évidence.
        /// </summary>
        /// <param name="table">Le tableau des dominos posés sur la table.</param>
        /// <param name="nbDominosTable">Le nombre de dominos actuellement posés sur la table.</param>
        public static void afficherTable(string[] table, int nbDominosTable)
        {
            afficherLigne();
            Console.WriteLine("  TABLE DE JEU");
            afficherLigne();

            Console.Write("  ");

            for (int i = 0; i < nbDominosTable; i++)
            {
                Console.Write(table[i]);

                if (i < nbDominosTable - 1)
                {
                    Console.Write("--");
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            if (nbDominosTable > 0)
            {
                int gaucheTable = int.Parse(table[0][1].ToString());
                int droiteTable = int.Parse(table[nbDominosTable - 1][3].ToString());

                Console.WriteLine("  Extremite gauche : [" + gaucheTable + "]   Extremite droite : [" + droiteTable + "]");
            }

            afficherLigne();
        }

        // =======================================
        // AFFICHER LA MAIN DU JOUEUR
        // =======================================

        /// <summary>
        /// Affiche dans la console les dominos en main d'un joueur, avec leur indice.
        /// Les dominos jouables sont signalés.
        /// </summary>
        /// <param name="mainJoueur">Le tableau des dominos en main du joueur.</param>
        /// <param name="nbDominosMain">Le nombre de dominos actuellement en main.</param>
        /// <param name="table">Le tableau des dominos sur la table (pour identifier les jouables).</param>
        /// <param name="nbDominosTable">Le nombre de dominos sur la table.</param>
        public static void afficherMain(string[] mainJoueur, int nbDominosMain, string[] table, int nbDominosTable)
        {
            Console.WriteLine("  VOTRE MAIN");
            afficherLigne();

            for (int i = 0; i < nbDominosMain; i++)
            {
                Console.Write("  " + i + " : " + mainJoueur[i]);

                if (dominoValide(mainJoueur[i], table, nbDominosTable))
                {
                    Console.Write(" <-- jouable");
                }

                Console.WriteLine();
            }

            afficherLigne();
        }

        // =======================================
        // PLACER UN DOMINO
        // =======================================

        /// <summary>
        /// Place un domino sur la table de jeu, à gauche ou à droite selon la correspondance des valeurs.
        /// Si nécessaire, le domino est retourné avant d'être posé.
        /// </summary>
        /// <param name="domino">Le domino à placer, sous la forme "[x|y]".</param>
        /// <param name="table">Le tableau représentant la table de jeu.</param>
        /// <param name="nbDominosTable">Le nombre de dominos actuellement sur la table (mis à jour par référence).</param>
        public static void placerDomino(string domino, string[] table, ref int nbDominosTable)
        {
            if (nbDominosTable == 0)
            {
                table[0] = domino;
                nbDominosTable++;
                return;
            }

            int gaucheTable = int.Parse(table[0][1].ToString());
            int droiteTable = int.Parse(table[nbDominosTable - 1][3].ToString());

            int gaucheDomino = int.Parse(domino[1].ToString());
            int droiteDomino = int.Parse(domino[3].ToString());

            if (droiteDomino == gaucheTable)
            {
                for (int i = nbDominosTable; i > 0; i--)
                {
                    table[i] = table[i - 1];
                }

                table[0] = domino;
                nbDominosTable++;
            }
            else if (gaucheDomino == gaucheTable)
            {
                string dominoInverse = "[" + droiteDomino + "|" + gaucheDomino + "]";

                for (int i = nbDominosTable; i > 0; i--)
                {
                    table[i] = table[i - 1];
                }

                table[0] = dominoInverse;
                nbDominosTable++;
            }
            else if (gaucheDomino == droiteTable)
            {
                table[nbDominosTable] = domino;
                nbDominosTable++;
            }
            else if (droiteDomino == droiteTable)
            {
                string dominoInverse = "[" + droiteDomino + "|" + gaucheDomino + "]";
                table[nbDominosTable] = dominoInverse;
                nbDominosTable++;
            }
        }

        // =======================================
        // RETIRER UN DOMINO DE LA MAIN
        // =======================================

        /// <summary>
        /// Retire un domino de la main d'un joueur en décalant les éléments suivants vers la gauche.
        /// </summary>
        /// <param name="main">Le tableau représentant la main du joueur.</param>
        /// <param name="nbDominos">Le nombre de dominos en main (mis à jour par référence).</param>
        /// <param name="index">L'indice du domino à retirer.</param>
        public static void retirerDominoMain(string[] main, ref int nbDominos, int index)
        {
            for (int i = index; i < nbDominos - 1; i++)
            {
                main[i] = main[i + 1];
            }

            main[nbDominos - 1] = null;
            nbDominos--;
        }

        // =======================================
        // RETIRER UN DOMINO DE LA MAIN ROBOT
        // =======================================

        /// <summary>
        /// Retire un domino de la main d'un robot en décalant les éléments suivants vers la gauche.
        /// </summary>
        /// <param name="mainsRobots">Le tableau 2D des mains des robots.</param>
        /// <param name="indexRobot">L'indice du robot concerné.</param>
        /// <param name="nbDominos">Le nombre de dominos en main du robot (mis à jour par référence).</param>
        /// <param name="indexDomino">L'indice du domino à retirer.</param>
        /// <param name="tailleMax">La taille maximale de la main (nombre de colonnes du tableau 2D).</param>
        public static void retirerDominoRobot(string[,] mainsRobots, int indexRobot, ref int nbDominos, int indexDomino, int tailleMax)
        {
            for (int i = indexDomino; i < nbDominos - 1; i++)
            {
                mainsRobots[indexRobot, i] = mainsRobots[indexRobot, i + 1];
            }

            mainsRobots[indexRobot, nbDominos - 1] = null;
            nbDominos--;
        }

        // =======================================
        // PIOCHER DANS LE TALON
        // =======================================

        /// <summary>
        /// Pioche un domino dans le talon et l'ajoute à la main du joueur.
        /// Retourne true si la pioche a réussi, false si le talon est vide.
        /// </summary>
        /// <param name="main">La main du joueur.</param>
        /// <param name="nbDominosMain">Le nombre de dominos en main (mis à jour par référence).</param>
        /// <param name="talonRestant">Le talon de dominos restants.</param>
        /// <param name="nbDominosTalon">Le nombre de dominos dans le talon (mis à jour par référence).</param>
        /// <returns>True si un domino a été pioché, false si le talon est vide.</returns>
        public static bool piocher(string[] main, ref int nbDominosMain, string[] talonRestant, ref int nbDominosTalon)
        {
            if (nbDominosTalon == 0)
            {
                return false;
            }

            main[nbDominosMain] = talonRestant[0];
            nbDominosMain++;

            for (int i = 0; i < nbDominosTalon - 1; i++)
            {
                talonRestant[i] = talonRestant[i + 1];
            }

            talonRestant[nbDominosTalon - 1] = null;
            nbDominosTalon--;

            return true;
        }

        // =======================================
        // PIOCHER ROBOT
        // =======================================

        /// <summary>
        /// Pioche un domino dans le talon et l'ajoute à la main d'un robot.
        /// Retourne true si la pioche a réussi, false si le talon est vide.
        /// </summary>
        /// <param name="mainsRobots">Le tableau 2D des mains des robots.</param>
        /// <param name="indexRobot">L'indice du robot concerné.</param>
        /// <param name="nbDominosMain">Le nombre de dominos en main du robot (mis à jour par référence).</param>
        /// <param name="talonRestant">Le talon de dominos restants.</param>
        /// <param name="nbDominosTalon">Le nombre de dominos dans le talon (mis à jour par référence).</param>
        /// <returns>True si un domino a été pioché, false si le talon est vide.</returns>
        public static bool piocherRobot(string[,] mainsRobots, int indexRobot, ref int nbDominosMain, string[] talonRestant, ref int nbDominosTalon)
        {
            if (nbDominosTalon == 0)
            {
                return false;
            }

            mainsRobots[indexRobot, nbDominosMain] = talonRestant[0];
            nbDominosMain++;

            for (int i = 0; i < nbDominosTalon - 1; i++)
            {
                talonRestant[i] = talonRestant[i + 1];
            }

            talonRestant[nbDominosTalon - 1] = null;
            nbDominosTalon--;

            return true;
        }

        // =======================================
        // VERIFIER SI UN JOUEUR PEUT JOUER
        // =======================================

        /// <summary>
        /// Vérifie si un joueur possède au moins un domino jouable dans sa main.
        /// </summary>
        /// <param name="main">La main du joueur.</param>
        /// <param name="nbDominos">Le nombre de dominos en main.</param>
        /// <param name="table">Le tableau des dominos sur la table.</param>
        /// <param name="nbDominosTable">Le nombre de dominos sur la table.</param>
        /// <returns>True si le joueur peut jouer, false sinon.</returns>
        public static bool peutJouer(string[] main, int nbDominos, string[] table, int nbDominosTable)
        {
            for (int i = 0; i < nbDominos; i++)
            {
                if (dominoValide(main[i], table, nbDominosTable))
                {
                    return true;
                }
            }

            return false;
        }

        // =======================================
        // VERIFIER SI UN ROBOT PEUT JOUER
        // =======================================

        /// <summary>
        /// Vérifie si un robot possède au moins un domino jouable dans sa main.
        /// </summary>
        /// <param name="mainsRobots">Le tableau 2D des mains des robots.</param>
        /// <param name="indexRobot">L'indice du robot.</param>
        /// <param name="nbDominos">Le nombre de dominos en main du robot.</param>
        /// <param name="table">Le tableau des dominos sur la table.</param>
        /// <param name="nbDominosTable">Le nombre de dominos sur la table.</param>
        /// <returns>True si le robot peut jouer, false sinon.</returns>
        public static bool robotPeutJouer(string[,] mainsRobots, int indexRobot, int nbDominos, string[] table, int nbDominosTable)
        {
            for (int i = 0; i < nbDominos; i++)
            {
                if (dominoValide(mainsRobots[indexRobot, i], table, nbDominosTable))
                {
                    return true;
                }
            }

            return false;
        }

        // =======================================
        // TOUR DU ROBOT - CHOISIR UN DOMINO
        // =======================================

        /// <summary>
        /// Choisit l'indice du domino que le robot va jouer, selon son niveau.
        /// Niveau 1 (Nul)   : domino valide aléatoire.
        /// Niveau 2 (Moyen) : préfère les doubles, sinon aléatoire.
        /// Niveau 3 (Fort)  : choisit la somme la plus haute parmi les valides.
        /// Retourne -1 si aucun domino n'est jouable.
        /// </summary>
        /// <param name="mainsRobots">Le tableau 2D des mains des robots.</param>
        /// <param name="indexRobot">L'indice du robot.</param>
        /// <param name="nbDominos">Le nombre de dominos en main du robot.</param>
        /// <param name="table">Le tableau des dominos sur la table.</param>
        /// <param name="nbDominosTable">Le nombre de dominos sur la table.</param>
        /// <param name="niveauRobot">Le niveau du robot (1, 2 ou 3).</param>
        /// <param name="generateur">L'instance Random partagée.</param>
        /// <returns>L'indice du domino choisi, ou -1 si aucun n'est jouable.</returns>
        public static int choisirDominoRobot(string[,] mainsRobots, int indexRobot, int nbDominos, string[] table, int nbDominosTable, int niveauRobot, Random generateur)
        {
            int[] indicesValides = new int[nbDominos];
            int nbValides = 0;

            for (int i = 0; i < nbDominos; i++)
            {
                if (dominoValide(mainsRobots[indexRobot, i], table, nbDominosTable))
                {
                    indicesValides[nbValides] = i;
                    nbValides++;
                }
            }

            if (nbValides == 0)
            {
                return -1;
            }

            if (niveauRobot == 1)
            {
                return indicesValides[generateur.Next(nbValides)];
            }

            if (niveauRobot == 2)
            {
                for (int i = 0; i < nbValides; i++)
                {
                    string domino = mainsRobots[indexRobot, indicesValides[i]];
                    int gaucheD = int.Parse(domino[1].ToString());
                    int droiteD = int.Parse(domino[3].ToString());

                    if (gaucheD == droiteD)
                    {
                        return indicesValides[i];
                    }
                }

                return indicesValides[generateur.Next(nbValides)];
            }

            if (niveauRobot == 3)
            {
                int meilleurIndex = indicesValides[0];
                int meilleureValeur = -1;

                for (int i = 0; i < nbValides; i++)
                {
                    string domino = mainsRobots[indexRobot, indicesValides[i]];
                    int gaucheD = int.Parse(domino[1].ToString());
                    int droiteD = int.Parse(domino[3].ToString());
                    int somme = gaucheD + droiteD;

                    if (somme > meilleureValeur)
                    {
                        meilleureValeur = somme;
                        meilleurIndex = indicesValides[i];
                    }
                }

                return meilleurIndex;
            }

            return indicesValides[0];
        }

        // =======================================
        // COMPTER LES DOMINOS D'UN ROBOT
        // =======================================

        /// <summary>
        /// Compte le nombre de dominos non null dans la main d'un robot.
        /// </summary>
        /// <param name="mainsRobots">Le tableau 2D des mains des robots.</param>
        /// <param name="indexRobot">L'indice du robot.</param>
        /// <param name="tailleMax">La taille maximale de la main.</param>
        /// <returns>Le nombre de dominos en main du robot.</returns>
        public static int compterDominosRobot(string[,] mainsRobots, int indexRobot, int tailleMax)
        {
            int count = 0;

            for (int i = 0; i < tailleMax; i++)
            {
                if (mainsRobots[indexRobot, i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        // =======================================
        // AFFICHER LE RECAP DES JOUEURS
        // =======================================

        /// <summary>
        /// Affiche le nombre de dominos en main de chaque robot et le nombre de dominos dans le talon.
        /// </summary>
        /// <param name="pseudosRobots">Les pseudos des robots.</param>
        /// <param name="nbDominosRobots">Le tableau des compteurs de dominos par robot.</param>
        /// <param name="nombreRobots">Le nombre de robots.</param>
        /// <param name="nbDominosTalon">Le nombre de dominos restants dans le talon.</param>
        public static void afficherRecapJoueurs(string[] pseudosRobots, int[] nbDominosRobots, int nombreRobots, int nbDominosTalon)
        {
            Console.WriteLine("  ADVERSAIRES");
            afficherLigne();

            for (int i = 0; i < nombreRobots; i++)
            {
                Console.WriteLine("  [Robot] " + pseudosRobots[i] + " : " + nbDominosRobots[i] + " domino(s)");
            }

            Console.WriteLine("  [Talon] " + nbDominosTalon + " domino(s) restant(s)");
            afficherLigne();
        }

        // =======================================
        // AFFICHER UN ENCADRE
        // =======================================

        /// <summary>
        /// Affiche un titre encadré dans la console.
        /// </summary>
        /// <param name="titre">Le texte du titre à encadrer.</param>
        public static void afficherEncadre(string titre)
        {
            int largeur = titre.Length + 4;
            string ligne = new string('=', largeur);

            Console.WriteLine();
            Console.WriteLine("  " + ligne);
            Console.WriteLine("  = " + titre + " =");
            Console.WriteLine("  " + ligne);
            Console.WriteLine();
        }

        // =======================================
        // AFFICHER UNE LIGNE SEPARATRICE
        // =======================================

        /// <summary>
        /// Affiche une ligne de séparation dans la console.
        /// </summary>
        public static void afficherLigne()
        {
            Console.WriteLine("  " + new string('-', 50));
        }

        // =======================================
        // AFFICHER LE TUTORIEL
        // =======================================

        /// <summary>
        /// Affiche les règles de base du jeu de dominos et attend que le joueur appuie sur Entrée.
        /// </summary>
        public static void afficherTutorielSimple()
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
    }
}