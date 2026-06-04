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
                // CHOIX DU PSEUDO HUMAIN
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

                Console.WriteLine();
                Console.Write("  Voulez-vous voir le tutoriel du jeu ? (o/n) : ");

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
                    Console.Write("\n  Combien de robots dans la partie ? (1 a 3) : ");

                } while (
                    !int.TryParse(Console.ReadLine(), out nombreRobots)
                    || nombreRobots < 1
                    || nombreRobots > 3
                );

                // =======================================
                // NIVEAU DES ROBOTS
                // =======================================

                int niveauRobots;

                do
                {
                    Console.WriteLine();
                    Console.WriteLine("  Choisissez le niveau des robots :");
                    Console.WriteLine("  1 - Nul   (joue au hasard)");
                    Console.WriteLine("  2 - Moyen (prefere les doubles)");
                    Console.WriteLine("  3 - Fort  (joue la plus haute valeur)");
                    Console.Write("  Votre choix : ");

                } while (
                    !int.TryParse(Console.ReadLine(), out niveauRobots)
                    || niveauRobots < 1
                    || niveauRobots > 3
                );

                string nomNiveau = niveauRobots == 1 ? "Nul" : niveauRobots == 2 ? "Moyen" : "Fort";
                Console.WriteLine("  Niveau choisi : " + nomNiveau);

                // =======================================
                // ATTRIBUTION DES PSEUDOS ROBOTS
                // =======================================

                string[] pseudosRobots = Fonctions.attribuerPseudosRobots(
                    pseudosDisponibles,
                    pseudosPris,
                    ref nbPseudosPris,
                    nombreRobots,
                    generateur
                );

                Console.WriteLine();
                Console.Write("  Adversaires : ");

                for (int i = 0; i < nombreRobots; i++)
                {
                    Console.Write(pseudosRobots[i]);

                    if (i < nombreRobots - 1)
                    {
                        Console.Write(", ");
                    }
                }

                Console.WriteLine();

                // =======================================
                // CREATION ET MELANGE DU TALON
                // =======================================

                string[] talon = Fonctions.creationTalon();
                Fonctions.melangeAleatoire(talon, generateur);

                int dominosParJoueur = (nombreRobots + 1 == 2) ? 7 : 6;

                // =======================================
                // DISTRIBUTION DES DOMINOS
                // =======================================

                string[] mainJoueur = new string[28];
                int nbDominosJoueur = dominosParJoueur;

                string[,] mainsRobots = new string[nombreRobots, 28];
                int[] nbDominosRobots = new int[nombreRobots];

                int indexDistribution = 0;

                for (int i = 0; i < dominosParJoueur; i++)
                {
                    mainJoueur[i] = talon[indexDistribution++];
                }

                for (int robot = 0; robot < nombreRobots; robot++)
                {
                    nbDominosRobots[robot] = dominosParJoueur;

                    for (int i = 0; i < dominosParJoueur; i++)
                    {
                        mainsRobots[robot, i] = talon[indexDistribution++];
                    }
                }

                string[] talonRestant = new string[28];
                int nbDominosTalon = 0;

                for (int i = indexDistribution; i < talon.Length; i++)
                {
                    talonRestant[nbDominosTalon++] = talon[i];
                }

                // =======================================
                // PREMIER DOMINO SUR LA TABLE
                // =======================================

                string[] table = new string[56];
                int nbDominosTable = 0;

                Fonctions.placerDomino(mainJoueur[0], table, ref nbDominosTable);
                Fonctions.retirerDominoMain(mainJoueur, ref nbDominosJoueur, 0);

                // =======================================
                // BOUCLE DE JEU PRINCIPALE
                // =======================================

                bool partieTerminee = false;
                string gagnant = "";

                // Tour 0 = joueur humain, tours 1..N = robots
                int tourActuel = 0;
                int nbJoueurs = 1 + nombreRobots;

                while (!partieTerminee)
                {
                    Console.Clear();

                    Fonctions.afficherTable(table, nbDominosTable);

                    // =======================================
                    // TOUR DU JOUEUR HUMAIN
                    // =======================================

                    if (tourActuel == 0)
                    {
                        Fonctions.afficherMain(mainJoueur, nbDominosJoueur, table, nbDominosTable);
                        Fonctions.afficherRecapJoueurs(pseudosRobots, nbDominosRobots, nombreRobots, nbDominosTalon);

                        Console.WriteLine("  Votre tour, " + pseudoHumain + " !");
                        Fonctions.afficherLigne();

                        bool aJoue = false;

                        while (!aJoue)
                        {
                            if (!Fonctions.peutJouer(mainJoueur, nbDominosJoueur, table, nbDominosTable))
                            {
                                if (nbDominosTalon > 0)
                                {
                                    Console.WriteLine("  Aucun domino jouable. Pioche automatique...");
                                    bool piocheReussie = Fonctions.piocher(mainJoueur, ref nbDominosJoueur, talonRestant, ref nbDominosTalon);

                                    if (piocheReussie)
                                    {
                                        Console.WriteLine("  Vous avez pioché : " + mainJoueur[nbDominosJoueur - 1]);
                                        Console.Write("  Appuyez sur Entree...");
                                        Console.ReadLine();

                                        Console.Clear();
                                        Fonctions.afficherTable(table, nbDominosTable);
                                        Fonctions.afficherMain(mainJoueur, nbDominosJoueur, table, nbDominosTable);
                                        Fonctions.afficherRecapJoueurs(pseudosRobots, nbDominosRobots, nombreRobots, nbDominosTalon);
                                        Console.WriteLine("  Votre tour, " + pseudoHumain + " !");
                                        Fonctions.afficherLigne();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("  Aucun domino jouable et talon vide. Vous passez votre tour.");
                                    Console.Write("  Appuyez sur Entree...");
                                    Console.ReadLine();
                                    aJoue = true;
                                }
                            }
                            else
                            {
                                Console.Write("  Choisissez un domino (0-" + (nbDominosJoueur - 1) + ") ou P pour piocher : ");
                                string saisie = Console.ReadLine().ToLower();

                                if (saisie == "p")
                                {
                                    if (nbDominosTalon == 0)
                                    {
                                        Console.WriteLine("  Le talon est vide !");
                                    }
                                    else
                                    {
                                        bool piocheReussie = Fonctions.piocher(mainJoueur, ref nbDominosJoueur, talonRestant, ref nbDominosTalon);

                                        if (piocheReussie)
                                        {
                                            Console.WriteLine("  Vous avez pioché : " + mainJoueur[nbDominosJoueur - 1]);
                                            Console.Write("  Appuyez sur Entree...");
                                            Console.ReadLine();

                                            Console.Clear();
                                            Fonctions.afficherTable(table, nbDominosTable);
                                            Fonctions.afficherMain(mainJoueur, nbDominosJoueur, table, nbDominosTable);
                                            Fonctions.afficherRecapJoueurs(pseudosRobots, nbDominosRobots, nombreRobots, nbDominosTalon);
                                            Console.WriteLine("  Votre tour, " + pseudoHumain + " !");
                                            Fonctions.afficherLigne();
                                        }
                                    }
                                }
                                else
                                {
                                    int indexChoisi;

                                    if (!int.TryParse(saisie, out indexChoisi) || indexChoisi < 0 || indexChoisi >= nbDominosJoueur)
                                    {
                                        Console.WriteLine("  Saisie invalide.");
                                    }
                                    else if (!Fonctions.dominoValide(mainJoueur[indexChoisi], table, nbDominosTable))
                                    {
                                        Console.WriteLine("  Ce domino ne peut pas etre pose ici.");
                                    }
                                    else
                                    {
                                        string dominoJoue = mainJoueur[indexChoisi];
                                        Fonctions.placerDomino(dominoJoue, table, ref nbDominosTable);
                                        Fonctions.retirerDominoMain(mainJoueur, ref nbDominosJoueur, indexChoisi);

                                        Console.WriteLine("  Vous avez joue : " + dominoJoue);
                                        aJoue = true;
                                    }
                                }
                            }
                        }

                        // Vérifie si le joueur a gagné
                        if (nbDominosJoueur == 0)
                        {
                            gagnant = pseudoHumain;
                            partieTerminee = true;
                        }
                    }

                    // =======================================
                    // TOUR D'UN ROBOT
                    // =======================================

                    else
                    {
                        int indexRobot = tourActuel - 1;
                        string nomRobot = pseudosRobots[indexRobot];

                        Console.WriteLine("  Tour de " + nomRobot + " [Robot - " + nomNiveau + "]");
                        Fonctions.afficherLigne();

                        bool aJoueRobot = false;

                        while (!aJoueRobot)
                        {
                            if (!Fonctions.robotPeutJouer(mainsRobots, indexRobot, nbDominosRobots[indexRobot], table, nbDominosTable))
                            {
                                if (nbDominosTalon > 0)
                                {
                                    bool piocheReussie = Fonctions.piocherRobot(mainsRobots, indexRobot, ref nbDominosRobots[indexRobot], talonRestant, ref nbDominosTalon);

                                    if (piocheReussie)
                                    {
                                        Console.WriteLine("  " + nomRobot + " pioche un domino. (" + nbDominosTalon + " restants dans le talon)");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("  " + nomRobot + " ne peut pas jouer et passe son tour.");
                                    aJoueRobot = true;
                                }
                            }
                            else
                            {
                                int indexDomino = Fonctions.choisirDominoRobot(
                                    mainsRobots,
                                    indexRobot,
                                    nbDominosRobots[indexRobot],
                                    table,
                                    nbDominosTable,
                                    niveauRobots,
                                    generateur
                                );

                                if (indexDomino == -1)
                                {
                                    Console.WriteLine("  " + nomRobot + " ne peut pas jouer.");
                                    aJoueRobot = true;
                                }
                                else
                                {
                                    string dominoJoue = mainsRobots[indexRobot, indexDomino];
                                    Fonctions.placerDomino(dominoJoue, table, ref nbDominosTable);
                                    Fonctions.retirerDominoRobot(mainsRobots, indexRobot, ref nbDominosRobots[indexRobot], indexDomino, 28);

                                    Console.WriteLine("  " + nomRobot + " joue : " + dominoJoue + " (" + nbDominosRobots[indexRobot] + " domino(s) restant(s))");
                                    aJoueRobot = true;
                                }
                            }
                        }

                        // Vérifie si le robot a gagné
                        if (nbDominosRobots[indexRobot] == 0)
                        {
                            gagnant = nomRobot;
                            partieTerminee = true;
                        }
                    }

                    // =======================================
                    // DETECTION DU BLOCAGE GENERAL
                    // =======================================

                    if (!partieTerminee && nbDominosTalon == 0)
                    {
                        bool joueurBloque = !Fonctions.peutJouer(mainJoueur, nbDominosJoueur, table, nbDominosTable);
                        bool tousRobotsBloques = true;

                        for (int r = 0; r < nombreRobots; r++)
                        {
                            if (Fonctions.robotPeutJouer(mainsRobots, r, nbDominosRobots[r], table, nbDominosTable))
                            {
                                tousRobotsBloques = false;
                            }
                        }

                        if (joueurBloque && tousRobotsBloques)
                        {
                            partieTerminee = true;
                            gagnant = "";
                        }
                    }

                    // =======================================
                    // PASSAGE AU TOUR SUIVANT
                    // =======================================

                    if (!partieTerminee)
                    {
                        Console.Write("\n  Appuyez sur Entree pour continuer...");
                        Console.ReadLine();

                        tourActuel = (tourActuel + 1) % nbJoueurs;
                    }
                }

                // =======================================
                // FIN DE PARTIE
                // =======================================

                Console.Clear();
                Fonctions.afficherTable(table, nbDominosTable);
                Fonctions.afficherEncadre("FIN DE PARTIE");

                if (gagnant == "")
                {
                    Console.WriteLine("  Partie bloquee ! Personne ne peut jouer.");
                    Console.WriteLine("  Calcul des points restants :");
                    Console.WriteLine();

                    int pointsJoueur = 0;

                    for (int i = 0; i < nbDominosJoueur; i++)
                    {
                        pointsJoueur += int.Parse(mainJoueur[i][1].ToString()) + int.Parse(mainJoueur[i][3].ToString());
                    }

                    Console.WriteLine("  " + pseudoHumain + " : " + pointsJoueur + " points");

                    int indexGagnantBlocage = 0;
                    int minPoints = pointsJoueur;

                    for (int r = 0; r < nombreRobots; r++)
                    {
                        int pointsRobot = 0;

                        for (int i = 0; i < nbDominosRobots[r]; i++)
                        {
                            pointsRobot += int.Parse(mainsRobots[r, i][1].ToString()) + int.Parse(mainsRobots[r, i][3].ToString());
                        }

                        Console.WriteLine("  " + pseudosRobots[r] + " : " + pointsRobot + " points");

                        if (pointsRobot < minPoints)
                        {
                            minPoints = pointsRobot;
                            indexGagnantBlocage = r + 1;
                        }
                    }

                    Console.WriteLine();

                    if (indexGagnantBlocage == 0)
                    {
                        Console.WriteLine("  " + pseudoHumain + " gagne avec le moins de points !");
                    }
                    else
                    {
                        Console.WriteLine("  " + pseudosRobots[indexGagnantBlocage - 1] + " gagne avec le moins de points !");
                    }
                }
                else if (gagnant == pseudoHumain)
                {
                    Console.WriteLine("  Felicitations " + pseudoHumain + " ! Vous avez gagne !");
                }
                else
                {
                    Console.WriteLine("  " + gagnant + " (Robot) gagne la partie !");
                }

                Console.WriteLine();
                Console.Write("  Voulez-vous rejouer ? (o/n) : ");
                reponse = Console.ReadLine().ToLower();

            } while (reponse == "o");

            Fonctions.afficherEncadre("MERCI D'AVOIR JOUE !");
        }
    }
}
