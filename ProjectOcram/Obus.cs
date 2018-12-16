//-----------------------------------------------------------------------
// <copyright file="Obus.cs" company="Tristan Araujo & Dominik Desjardins">
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
    using System.Threading.Tasks;
    using IFM20884;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe implantant les variables importantes pour
    /// de l'apparition de l'obus dans le monde (ex; vitesse de l'obus).
    /// Pour sa collision
    /// </summary>
    public abstract class Obus : Sprite
    {
        /// <summary>
        /// Vitesses de la source de l'obus lors du tir.
        /// </summary>
        private Vector2 vitessesSource;

        /// <summary>
        /// Vitesses de propulsion de l'obus lors du tir.
        /// </summary>
        private Vector2 vitessesPropulsion;

        /// <summary>
        /// Sprite à l'origine du tir.
        /// </summary>
        private Sprite source = null;

        /// <summary>
        /// Initialise une nouvelle instance de la classe Obus.
        /// </summary>
        /// <param name="x">Position en x du sprite.</param>
        /// <param name="y">Position en y du sprite.</param>
        /// <param name="vitesses">Vitesses (horizontale et verticale) de propultion de l'obus.</param>
        public Obus(float x, float y, Vector2 vitesses)
            : base(x, y)
        {
            this.vitessesSource = Vector2.Zero;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Obus.
        /// </summary>
        /// <param name="position">Position du sprite.</param>
        /// <param name="vitesses">Vitesses (horizontale et verticale) de propultion de l'obus.</param>
        public Obus(Vector2 position, Vector2 vitesses)
            : this(position.X, position.Y, vitesses)
        {
        }

        /// <summary>
        /// Accesseur et mutateur (attribut vitessesSource) retournant ou changeant les vitesses de la source de l'obus.
        /// </summary>
        /// <value>Vitesses de la source de l'obus lors du tir.</value>
        public Vector2 VitessesSource
        {
            get { return this.vitessesSource; }
            set { this.vitessesSource = value; }
        }

        /// <summary>
        /// Accesseur et mutateur (attribut vitessesPropulsion) retournant ou changeant les vitesses donnés à l'obus lors du tir.
        /// </summary>
        /// <value>Vitesses de la source de l'obus lors du tir.</value>
        public Vector2 VitessesPropulsion
        {
            get { return this.vitessesPropulsion; }
            set { this.vitessesPropulsion = value; }
        }

        /// <summary>
        /// Accesseur et mutateur (attribut source) retournant ou changeant la source du tir de l'obus.
        /// </summary>
        /// <value>Vitesses de la source de l'obus lors du tir.</value>
        public Sprite Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        /// <summary>
        /// Rectangle pour l'implémentassion de la collision sur la texture de l'obus.
        /// </summary>
        public Rectangle ObusCollision { get; set; }

        /// <summary>
        /// Fonction membre mettant à jour la position de l'obus en fonction de sa vitesse.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // Déplacer le vaisseau en fonction des vitesses latérales et frontales
            this.Position = new Vector2(
                this.Position.X + (gameTime.ElapsedGameTime.Milliseconds * (this.vitessesSource.X + this.vitessesPropulsion.X)),
                this.Position.Y + (gameTime.ElapsedGameTime.Milliseconds * (this.vitessesSource.Y + this.vitessesPropulsion.Y)));
        }
    }
}
