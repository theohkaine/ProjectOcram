//-----------------------------------------------------------------------
// <copyright file="Slime.cs" company="Tristan Araujo & Dominik Desjardins">
// Tristan Araujo & Dominik Desjardins, 2018. Tous droits réservés
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

namespace ProjectOcram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using IFM20884;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Classe implantant le sprite représentant un Slime. Ce sprite animé par intelligence
    /// artificielle peut être stationnaire, marcher et courir dans deux directions.
    /// </summary>
    public class Slime : Personnage
    {
        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du personnage.
        /// </summary>
        private static List<Palette> palettes = new List<Palette>();

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Slime(float x, float y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Slime(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Rectangle pour l'implémentassion de la collision sur la texture du slime.
        /// </summary>
        public Rectangle SlimeCollision { get; set; }

        /// <summary>
        /// Propriété accesseur retournant la liste des palettes associées au personnage 
        /// selon son état et sa direction. Ces palettes sont stockées dans l'attribut 
        /// static palettes.
        /// </summary>
        protected override List<Palette> Palettes
        {
            get { return Slime.palettes; }
        }

        /// <summary>
        /// Propriété accesseur retournant la liste des effets sonores associée au personnage
        /// selon son état. Aucun effet sonore n'est associé au slime. 
        /// </summary>
        protected override List<SoundEffect> EffetsSonores
        {
            get { return null; }
        }

        /// <summary>
        /// Surchargé afin de retourner la palette correspondant à la direction de 
        /// déplacement et l'état du personnage.
        /// </summary>
        protected override Palette PaletteAnimation
        {
            // Les palettes sont stockées dans la liste en groupes d'état (i.e.
            // 2 palettes de direction pour chaque état).
            get { return this.Palettes[((int)this.Etat * 12) + (int)this.DirectionDeplacement]; }
        }

        /// <summary>
        /// Charge les images associées au sprite du slime. Cette fonction static invoque
        /// la fonction static de la classe de base qui s'occupe de charger les textures
        /// et effets sonores que devraient avoir toute classe dérivée de Personnage.
        /// Notez l'absence d'effets sonores associés au slime.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du slime.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            LoadContent(
                content,            // gestionnaire de contenu à utiliser
                graphics,           // gestionnaire de périphériques à utiliser
                Slime.palettes,      // liste où doivent être stockées les palettes du slime
                80,                 // largeur de chaque tuile dans les palettes
                80,                 // hauteur de chaque tuile dans les palettes
                @"Ennemi\Slime");  // sous-répertoire de Content où sont stockées les palettes du slime
        }

        /// <summary>
        /// Lire de  l'input les vitesses de déplacement directionnels. Nous utilisons l'intelligence
        /// artificielle pour contrôler les mouvements de l'ogre.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        /// <param name="vitesseDroite">Retourne la vitesse de déplacement vers le est.</param>
        /// <param name="vitesseGauche">Retourne la vitesse de déplacement vers le ouest.</param>
        /// <returns>Vrai si des vitesses furent lues; faux sinon.</returns>
        public override bool LireVitesses(
            GameTime gameTime,
            out float vitesseDroite,
            out float vitesseGauche)
        {
            // Aucun périphérique d'inputs disponible, alors aucune vitesse lue.
            vitesseDroite = 0.0f;
            vitesseGauche = 0.0f;

            // Aucune intellignece : l'ogre marche de gauche a droite aux 3 secondes
            if ((gameTime.TotalGameTime.Seconds / 3) % 2 == 0)
            {
                this.Etat = Etats.Marche;
                vitesseDroite = 0.23f;
            }
            else
            {
                this.Etat = Etats.Marche;
                vitesseGauche = 0.23f;
            }

            return true;
        }
    }
}