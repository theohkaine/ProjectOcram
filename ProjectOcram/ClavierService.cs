//-----------------------------------------------------------------------
// <copyright file="ClavierService.cs" company="Marco Lavoie">
// Marco Lavoie, 2010-2016. Tous droits réservés
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
    /// Classe implantant un service de gestion de clavier.
    /// </summary>
    public class ClavierService : GameComponent, IInputService
    {
        /// <summary>
        /// Instance permettant d'extraire l'état des touches du clavier.
        /// </summary>
        private KeyboardState etatClavier;

        /// <summary>
        /// Tableau stockant l'heure à laquelle chaque touche fut pressée pour la dernière fois. Ce
        /// tableau permet de prévenir la répétition automatisée des touches au besoin.
        /// </summary>
        private DateTime[] heureDernierePression;

        /// <summary>
        /// Tableau stockant l'état (pressée ou pas) de chaque touche. Ce
        /// tableau permet de prévenir la répétition automatisée des touches au besoin.
        /// </summary>
        private bool[] etatPrecedent;

        /// <summary>
        /// Heure à laquelle un premier saut (d'une série de sauts continue) est occasionné.
        /// </summary>
        private DateTime heureSaut;

        /// <summary>
        /// Indique si le saut est le premier d'une série de sauts continue.
        /// </summary>
        private bool premierSaut = true;

        /// <summary>
        /// Constructeur paramétré.
        /// </summary>
        /// <param name="game">Instance de la classe de jeu gérant la partie.</param>
        public ClavierService(Game game)
            : base(game)
        {
            // Initialise les tableaux des heures de dernière pression et d'état des touches
            this.heureDernierePression = new DateTime[this.NombreMaxTouches];
            this.etatPrecedent = new bool[this.NombreMaxTouches];
            for (int key = 0; key < this.NombreMaxTouches; key++)
            {
                this.heureDernierePression[key] = new DateTime(0);
                this.etatPrecedent[key] = false;
            }
            this.heureSaut = new DateTime(0);           // initialisé l'objet

            ServiceHelper.Add<IInputService>(this);
        }

        /// <summary>
        /// Retourne le nombre maximum de touches qu'on retrouve sur un clavier. Cette valeur est
        /// extraite de l'énumération Keys.
        /// </summary>
        /// <returns>Nombre maximum de boutons sur une manette.</returns>
        private int NombreMaxTouches
        {
            get { return (int)Keys.Zoom + 1; }
        }
        /// <summary>
        /// Retourne 1.0f si la flèche gauche du clavier est pressée, 0.0 sinon.
        /// Le paramètre device est ignoré (un seul clavier).
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Valeur entre 0.0 (aucun mouvement) et 1.0 (vitesse maximale).</returns>
        public float DeplacementGauche(int device)
        {
            if (this.etatClavier.IsKeyDown(Keys.Left))
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Retourne 1.0f si la flèche droite du clavier est pressée, 0.0 sinon.
        /// Le paramètre device est ignoré (un seul clavier).
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Valeur entre 0.0 (aucun mouvement) et 1.0 (vitesse maximale).</returns>
        public float DeplacementDroite(int device)
        {
            if (this.etatClavier.IsKeyDown(Keys.Right))
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }


        /// <summary>
        /// Indique si le personnage doit sauter (si la barre d'espacement du clavier pressée). La
        /// fonction est écrite de sorte que si la touche est tenue pressée, le prochain saut
        /// ne sera pas occasionné avant un délai d'attente d'une seconde. L'objectif est d'avoir un
        /// délai d'une seconde entre le premier et le second saut, mais aucun délai par la suite tant
        /// que la touche espace est tenue pressée.
        /// Le paramètre device est ignoré (un seul clavier).
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Vrai si la barre d'espacement est pressée.</returns>
        public bool Sauter(int device)
        {
            DateTime now = DateTime.Now;        // heure courante

            // Vérifier si un saut est occasionné (touche espace pressée)
            if (!this.etatClavier.IsKeyDown(Keys.Space))
            {
                this.premierSaut = true;        // le prochain saut sera le premier d'une série
                return false;
            }

            // Est-ce le premier saut de la série. Si c'est le cas, initialiser heureSaut de sorte
            // que le prochain saut ne sera pas occasionné avant un délai d'attente
            if (this.premierSaut)
            {
                this.premierSaut = false;       // le prochain saut ne sera pas le premier de la série
                this.heureSaut = DateTime.Now;
                return true;                    // activer le saut
            }
            else
            {
                // Attendre un délai d'une seconde entre le premier et le second saut. Après quoi
                // ne pas imposer de délai entre les sauts
                return (now - this.heureSaut).TotalMilliseconds > 1000;
            }
        }

        /// <summary>
        /// Retourne 1.0f si la touche Esc du clavier est pressée; 0.0 sinon.
        /// Le paramètre device est ignoré (un seul clavier).
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Booléen indiquant si on doit terminer la partie en cours.</returns>
        public bool Quitter(int device)
        {
            return this.etatClavier.IsKeyDown(Keys.Escape);
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
            return this.DelaiDuplicationExpire(Keys.W, 300);
        }



        public float DashingDroite(int device)
        {
            if (this.etatClavier.IsKeyDown(Keys.Q))
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        public float DashingGauche(int device)
        {
            if (this.etatClavier.IsKeyDown(Keys.E))
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
        /// <param name="gameTime">Gestionnaire de temps.</param>
        public override void Update(GameTime gameTime)
        {
            this.etatClavier = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// Récupère l'état d'une touche du clavier tout en considérant un délai de temps minimum
        /// entre deux pressions consécutives de cette touche.
        /// </summary>
        /// <param name="touche">Touche du clavier à considérer.</param>
        /// <param name="delai">Délai d'expiration à considérer, en millisecondes.</param>
        /// <returns>Vrai si la touche visée est pressée et que le délai minimum entre deux
        /// pressions de cette touche est écoulé.</returns>
        private bool DelaiDuplicationExpire(Keys touche, int delai)
        {
            // Premièrement s'assurer que la touche est pressée.
            if (!this.etatClavier.IsKeyDown(touche))
            {

                return false;
            }

            // Vérifier si le délai minimum entre deux pressions de la touche est expiré.
            DateTime now = DateTime.Now;        // heure courante
            if ((now - this.heureDernierePression[(int)touche]).TotalMilliseconds < delai)
            {
                return false;
            }

            // Le délai étant expiré, on note l'heure de la pression de la touche.
            this.heureDernierePression[(int)touche] = now;
            return true;
        }

        /// <summary>
        /// Récupère l'état d'une touche du clavier tout en considérant son état précédent
        /// de façon à s'assurer que c'est une nouvelle pression de la touche.
        /// </summary>
        /// <param name="touche">Touche du clavier à considérer.</param>
        /// <returns>Vrai si la touche visée est nouvellement pressée.</returns>
        private bool NouvellePression(Keys touche)
        {
            bool etaitPressee = this.etatPrecedent[(int)touche];      // stocker l'état précédent
            bool estPressee = this.etatClavier.IsKeyDown(touche);     // état actuel de la touche

            // Mettre à jour le tableau d'états précédents.
            this.etatPrecedent[(int)touche] = estPressee;

            // La touche est pressée seulement si c'est une nouvelle pression.
            return !etaitPressee && estPressee;
        }

    }
}
