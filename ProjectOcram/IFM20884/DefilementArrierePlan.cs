//-----------------------------------------------------------------------
// <copyright file="DefilementArrierePlan.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe abstraite permettant d'implanter un arrière pla à défilement vertical. Cette
    /// classe gère le défilement vertical d'une image fournie par la classe dérivée. Cette
    /// dernière doit surcharger l'accesseur Texture afin de retourner l'image à afficher en
    /// arrière plan de jeu.
    /// </summary>
    public abstract class DefilementArrierePlan : Sprite
    {
        /// <summary>
        /// Attribut permettant de contrôler la vitesse de défilement vertical de
        /// l'arrière plan.
        /// </summary>
        private float vitesseArrierePlan = 0.2f;       // vitesse de défilement de l'arrière-plan

        /// <summary>
        /// Constructeur paramétré recevant un GraphicsDeviceManager afin de centrer l'image dans l'écran.
        /// Ne pas oublier que Sprite.Draw centre la texture à la position indiquée.
        /// </summary>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public DefilementArrierePlan(GraphicsDeviceManager graphics)
            : base(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height)
        {
        }

        /// <summary>
        /// Propriété permettant de contrôler la vitesse de défilement vertical de
        /// l'arrière plan.
        /// </summary>
        public float VitesseArrierePlan
        {
            get { return this.vitesseArrierePlan; }
            set { this.vitesseArrierePlan = value; }
        }

        /// <summary>
        /// Fonction membre mettant à jour le sprite. Afin de donner l'illusion de défilement,
        /// la texture est décalée vers le bas de façon modulo en fonction de la hauteur de
        /// l'écran.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // Calculer le décalage de l'arrière-plan vers le bas selon sa vitesse modulée.
            float posY = Position.Y + (gameTime.ElapsedGameTime.Milliseconds * this.vitesseArrierePlan);

            // Si la texture n'est plus visible (i.e. passée sous l'écran), la ramener au haut de
            // l'écran afin de ne pas occasionner de gap dans le défilement de l'arrière plan.
            if ((posY - (this.Height / 2)) > graphics.GraphicsDevice.Viewport.Height)
            {
                posY -= this.Height;
            }

            // Décaler l'arrière plan.
            this.Position = new Vector2(this.Position.X, posY);
        }

        /// <summary>
        /// Fonction membre dessinant le sprite. Nous dessinons la texture deux fois verticalement (i.e. une fois
        /// au dessus de l'autre) pour  éviter les gaps lors du défilement.
        /// </summary>
        /// <param name="camera">Caméra indiquant la partie du monde présentement visible à l'écran.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public override void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // La texture est affichée deux fois : une première fois au dessus de sa position
            // actuelle (ne pas oublier que le sprite est centrée sur _position).
            this.Position = new Vector2(this.Position.X, this.Position.Y - Height);
            base.Draw(camera, spriteBatch);

            // Et une seconde fois à sa position actuelle
            this.Position = new Vector2(this.Position.X, this.Position.Y + Height);
            base.Draw(camera, spriteBatch);
        }
    }
}
