//-----------------------------------------------------------------------
// <copyright file="ManetteService.cs" company="Marco Lavoie">
// Marco Lavoie, 2010. Tous droits réservés
// 
// L'utilisation de ce matériel pédagogique (présentations, code source 
// et autres) avec ou sans modifications, est permise en autant que les 
// conditions suivantes soient respectées:
//
// 1. La diffusion du matériel doit se limiter à un intranet dont l'accès
//    est imité aux étudiants inscrits à un cours exploitant le dit 
//    matériel. IL EST STRICTEMENT INTERDIT DE DIFFUSER CE MATÉRIEL 
//    LIBREMENT SUR INTERNET.
// 2. La redistribution des présentations contenues dans le matériel 
//    pédagogique est autorisée uniquement en format Acrobat PDF et sous
//    restrictions stipulées à la condition #1. Le code source contenu 
//    dans le matériel pédagogique peut cependant être redistribué sous 
//    sa forme  originale, en autant que la condition #1 soit également 
//    respectée.
// 3. Le matériel diffusé doit contenir intégralement la mention de 
//    droits d'auteurs ci-dessus, la notice présente ainsi que la
//    décharge ci-dessous.
// 
// CE MATÉRIEL PÉDAGOGIQUE EST DISTRIBUÉ "TEL QUEL" PAR L'AUTEUR, SANS 
// AUCUNE GARANTIE EXPLICITE OU IMPLICITE. L'AUTEUR NE PEUT EN AUCUNE 
// CIRCONSTANCE ÊTRE TENU RESPONSABLE DE DOMMAGES DIRECTS, INDIRECTS, 
// CIRCONSTENTIELS OU EXEMPLAIRES. TOUTE VIOLATION DE DROITS D'AUTEUR 
// OCCASIONNÉ PAR L'UTILISATION DE CE MATÉRIEL PÉDAGOGIQUE EST PRIS EN 
// CHARGE PAR L'UTILISATEUR DU DIT MATÉRIEL.
// 
// En utilisant ce matériel pédagogique, vous acceptez implicitement les
// conditions et la décharge exprimés ci-dessus.
// </copyright>
//-----------------------------------------------------------------------

namespace IFM20884
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Classe implantant un service XNA de lecture des entrées via la manette.
    /// </summary>
    public class ManetteService : GameComponent, IInputService
    {
        /// <summary>
        /// Pour récupérer l'état de chaque manette.
        /// </summary>
        private GamePadState[] etatManette;

        /// <summary>
        /// Heure à laquelle la dernière pression de chaque bouton est occasionné
        /// pour chaque manette.
        /// Premier indice  : numéro de périphérique.
        /// Deuxième indice : bouton.
        /// </summary>
        private DateTime[,] heureDernierePression;

        /// <summary>
        /// Tableau stockant l'état (pressée ou pas) de chaque bouton de chaque manette. Ce
        /// tableau permet de prévenir la répétition automatisée des touches au besoin.
        /// Premier indice  : numéro de périphérique.
        /// Deuxième indice : bouton.
        /// </summary>
        private bool[,] etatPrecedent;

        /// <summary>
        /// Initialise une nouvelle instance de la classe ManetteService.
        /// </summary>
        /// <param name="game">Instance de Game responsable du gestionnaire de services.</param>
        public ManetteService(Game game)
            : base(game)
        {
            // Créer les tableaux d'attributs.
            this.etatManette = new GamePadState[this.NombreMaxManettes];                                 // l'état de chaque manette
            this.heureDernierePression = new DateTime[this.NombreMaxManettes, this.NombreMaxBoutons];    // l'heure de la dernière pression de chaque bouton pour chaque manette
            this.etatPrecedent = new bool[this.NombreMaxManettes, this.NombreMaxBoutons];                // état précédent (pressé ou pas) de chaque bouton pour chaque manette

            this.Initialize(game);
        }

        /// <summary>
        /// Retourne le nombre maximum de boutons qu'on retrouve sur une manette. Cette valeur est
        /// extraite de l'énumération Buttons.
        /// </summary>
        /// <returns>Nombre maximum de boutons sur une manette.</returns>
        private int NombreMaxBoutons
        {
            get { return Enum.GetValues(typeof(Buttons)).Length; }
        }

        /// <summary>
        /// Retourne le nombre maximum de manettes. Une console de jeu ne peut physsiquement pas 
        /// gérer plus de 4 manettes.
        /// </summary>
        /// <returns>Nombre maximum de manettes.</returns>
        private int NombreMaxManettes
        {
            get { return 4; }
        }

        /// <summary>
        /// Retourne 1.0f si le thumbstick gauche ou la flèche gauche du pad est pressée, 0.0f sinon.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vitesse entre 0.0f et 1.0f.</returns>
        public float DeplacementGauche(int device)
        {
            // S'assurer que le numéro de manette fourni est valide.
            this.ValiderDevice(ref device);

            // Premièrement vérifier le thumbstick gauche
            if (this.etatManette[device - 1].IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                return Math.Abs(this.etatManette[device - 1].ThumbSticks.Left.X);
            }

            // Puisque le thumbstick gauche n'est pas utilisé, essayons le pad
            if (this.etatManette[device - 1].DPad.Left == ButtonState.Pressed)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Retourne 1.0f si le thumbstick droite ou la flèche droite du pad est pressée, 0.0f sinon.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vitesse entre 0.0f et 1.0f.</returns>
        public float DeplacementDroite(int device)
        {
            // S'assurer que le numéro de manette fourni est valide.
            this.ValiderDevice(ref device);

            // Premièrement vérifier le thumbstick gauche
            if (this.etatManette[device - 1].IsButtonDown(Buttons.LeftThumbstickRight))
            {
                return this.etatManette[device - 1].ThumbSticks.Left.X;
            }

            // Puisque le thumbstick gauche n'est pas utilisé, essayons le pad
            if (this.etatManette[device - 1].DPad.Right == ButtonState.Pressed)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Indique si le personnage doit sauter (si le bouton A de la manette est pressé). La
        /// fonction est écrite de sorte que si la touche est tenue pressée, le prochain saut
        /// ne sera pas occasionné avant un délai d'attente d'une seconde. L'objectif est d'avoir un
        /// délai d'une seconde entre le premier et le second saut, mais aucun délai par la suite tant
        /// que la touche espace est tenue pressée.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vrai si la barre d'espacement est pressée.</returns>
        public bool Sauter(int device)
        {
            // Imposer un délai de 0.2 seconde entre les sauts subséquents.
            return this.DelaiDuplicationExpire(device, Buttons.A, 200);
        }

        /// <summary>
        /// Indique la fin d'exécution du jeu doit être suspendue (si B de la manette pressée).
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vrai si le bouton B est pressé.</returns>
        public bool Pause(int device)
        {
            return this.NouvellePression(device, Buttons.Start);
        }

        /// <summary>
        /// Indique la fin d'exécution du programme (si le bouton Y de la manette est pressé).
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vrai si la touche Esc est pressée.</returns>
        public bool Quitter(int device)
        {
            return this.NouvellePression(device, Buttons.Back);
        }

        /// <summary>
        /// Indique si l'item de menu précédent doit être activé pour sélection (si flèche haut pressée).
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vrai si l'item de menu précédent doit être activé; faux sinon.</returns>
        public bool MenuItemPrecedent(int device)
        {
            // Ralentir les répétitions de pressions (400 millisecondes de délai)
            return this.DelaiDuplicationExpire(device, Buttons.DPadUp, 400);
        }

        /// <summary>
        /// Indique si l'item de menu suivant doit être activé pour sélection (si flèche bas pressée).
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vrai si l'item de menu suivant doit être activé; faux sinon.</returns>
        public bool MenuItemSuivant(int device)
        {
            // Ralentir les répétitions de pressions (400 millisecondes de délai)
            return this.DelaiDuplicationExpire(device, Buttons.DPadDown, 400);
        }

        /// <summary>
        /// Indique si l'item de menu présentement activé doit être sélectionné (bouton A pressé).
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vrai si l'item de menu actif doit être sélectionné; faux sinon.</returns>
        public bool MenuItemSelection(int device)
        {
            // Ne pas tenir compte des répétitions de pressions
            return this.NouvellePression(device, Buttons.A);
        }

        /// <summary>
        /// Indique si la barre d'espacement fut pressée.
        /// Le paramètre device est ignoré (un seul clavier).
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Vrai la barre d'espacement est pressée; faux sinon.</returns>
        public bool TirerObus(int device)
        {
            // Ralentir les répétitions de pressions (200 millisecondes de délai)
            return this.DelaiDuplicationExpire(device, Buttons.X, 300);
        }

        /// <summary>
        /// Retourne 1.0f si le LeftTrigger est pressée, 0.0f sinon.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vitesse entre 0.0f et 1.0f.</returns>
        public float DashingGauche(int device)
        
        {
            if (this.etatManette[device - 1].IsButtonDown(Buttons.LeftTrigger))
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Retourne 1.0f si le RightTrigger est pressée, 0.0f sinon.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <returns>Vitesse entre 0.0f et 1.0f.</returns>
        public float DashingDroite(int device)
        {
            if (this.etatManette[device - 1].IsButtonDown(Buttons.RightTrigger))
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Récupère l'état du clavier.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        public override void Update(GameTime gameTime)
        {
            // Extraire l'état de chaque manette.
            for (int device = 1; device <= this.NombreMaxManettes; device++)
            {
                this.etatManette[device - 1] = GamePad.GetState(this.GetPlayerIndex(device));
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Initialise l'instance de la classe ManetteService.
        /// </summary>
        /// <param name="game">Instance de Game responsable du gestionnaire de services.</param>
        private void Initialize(Game game)
        {
            // Créer les attributs vecteurs.
            for (int device = 1; device <= this.NombreMaxManettes; device++)
            {
                this.etatManette[device - 1] = GamePad.GetState(this.GetPlayerIndex(device));

                // Initialiser les tableaux d'état des boutons.
                for (int bouton = 0; bouton < this.NombreMaxBoutons; bouton++)
                {
                    this.heureDernierePression[device - 1, bouton] = new DateTime(0);
                    this.etatPrecedent[device - 1, bouton] = false;
                }
            }

            ServiceHelper.Add<IInputService>(this);     // activer le service
        }

        /// <summary>
        /// S'assure que le numéro de manette indiqué est valide, et sinon le change au numéro
        /// de manette valide le plus près.
        /// </summary>
        /// <param name="device">Le numéro de manette à valider; sa valeur est corrigée au 
        /// besoin.</param>
        /// <returns>Numéro de manette validé.</returns>
        private int ValiderDevice(ref int device)
        {
            // Corriger au besoin, et retourner la valeur corrigée.
            device = (int)MathHelper.Clamp(device, 1, this.NombreMaxManettes);
            return device;
        }

        /// <summary>
        /// Retourne un entier correspondant au bouton donné (indépendant de la valeur entière
        /// attribuée à celui-ci dan l'énumération Buttons).
        /// </summary>
        /// <param name="bouton">Le bouton dont on veut obtenir la position dans l'énumération 
        /// Buttons.</param>
        /// <returns>La position du bouton dans l'énumération Buttons.</returns>
        private int IndexBouton(Buttons bouton)
        {
            return Array.IndexOf(Enum.GetValues(bouton.GetType()), bouton);
        }

        /// <summary>
        /// Retourne l'identificateur de manette correspondant au numéro de manette indiqué.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire.</param>
        /// <returns>Identificateur de manette.</returns>
        private PlayerIndex GetPlayerIndex(int device)
        {
            // S'assurer que le numéro de manette fourni est valide.
            this.ValiderDevice(ref device);

            switch (device)
            {
                case 2:
                    return PlayerIndex.Two;
                case 3:
                    return PlayerIndex.Three;
                case 4:
                    return PlayerIndex.Four;
                default:
                    return PlayerIndex.One;
            }
        }

        /// <summary>
        /// Récupère l'état des boutons de la manette tout en considérant un délai de temps minimum
        /// entre deux pressions consécutives de ce bouton.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <param name="bouton">Bouton de la manette à considérer.</param>
        /// <param name="delai">Délai d'expiration à considérer, en millisecondes.</param>
        /// <returns>Vrai si le bouton visé est pressée et que le délai minimum entre deux
        /// pressions de ce bouton est écoulé.</returns>
        private bool DelaiDuplicationExpire(int device, Buttons bouton, int delai)
        {
            // Premièrement s'assurer que le bouton est pressé
            if (!this.etatManette[device - 1].IsButtonDown(bouton))
            {
                return false;
            }

            // Vérifier si le délai minimum entre deux pression du bouton est expiré
            DateTime now = DateTime.Now;        // heure courante
            if ((now - this.heureDernierePression[device - 1, this.IndexBouton(bouton)]).TotalMilliseconds < delai)
            {
                return false;
            }

            // Le délai étant expiré, on note l'heure de la pression du bouton
            this.heureDernierePression[device - 1, this.IndexBouton(bouton)] = now;
            return true;
        }

        /// <summary>
        /// Récupère l'état d'un bouton de la manette tout en considérant son état précédent
        /// de façon à s'assurer que c'est une nouvelle pression du bouton.
        /// </summary>
        /// <param name="device">Le numéro de manette à lire (1 à 4).</param>
        /// <param name="bouton">Bouton de la manette à considérer.</param>
        /// <returns>Vrai si le bouton visé est nouvellement pressé.</returns>
        private bool NouvellePression(int device, Buttons bouton)
        {
            bool etaitPresse = this.etatPrecedent[device - 1, this.IndexBouton(bouton)];        // stocker l'état précédent
            bool estPresse = this.etatManette[device - 1].IsButtonDown(bouton);   // état actuel de la touche

            // Mettre à jour le tableau d'états précédents
            this.etatPrecedent[device - 1, this.IndexBouton(bouton)] = estPresse;

            // La touche est pressée seulement si c'est une nouvelle pression
            return !etaitPresse && estPresse;
        }
    }
}