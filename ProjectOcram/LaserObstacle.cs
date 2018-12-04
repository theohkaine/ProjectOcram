using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IFM20884;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace ProjectOcram
{
    public class LaserObstacle : SpriteAnimation
    {

        /// <summary>
        /// Attribut statique contenant la palette d'animation de l'astéroïde composé d'argent.
        /// </summary>
        private static Palette palette;

        /// <summary>
        /// Vitesse de déplacement verticale du sprite.
        /// </summary>
        private float vitesseDeplacement;

        Rectangle insideZone { get; set; }

        



        /// <summary>
        /// Initialise une nouvelle instance de la classe AsteroideSprite.
        /// </summary>
        /// <param name="x">Position en x du sprite.</param>
        /// <param name="y">Position en y du sprite.</param>
        public LaserObstacle(float x, float y)
            : base(x, y)
        {
            this.vitesseDeplacement = 0.2f;     // vitesse de déplacement vertical par défaut
            this.VitesseAnimation = 0.07f;      // vitesse d'animation pour fluidité

           
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe AsteroideSprite.
        /// </summary>
        /// <param name="position">Position du sprite.</param>
        public LaserObstacle(Vector2 position)
            : this(position.X, position.Y)
        {
        }




       
        /// <summary>
        /// Propriété (accesseur pour vitesseDeplacement) retournant ou changeant la vitesse de déplacement 
        /// verticale du sprite.
        /// </summary>
        /// <value>Position du sprite.</value>
        public float VitesseDeplacement
        {
            get { return this.vitesseDeplacement; }
            set { this.vitesseDeplacement = value; }
        }

        /// <summary>
        /// On doit surcharger l'accesseur PaletteAnimation en conséquence (toute classe à instancier dérivée 
        /// de Sprite doit surcharger cet accesseur).
        /// </summary>
        protected override Palette PaletteAnimation
        {
            get
            {
                
             return palette;
                
            }
        }


        /// <summary>
        /// Fonction membre chargeant les ressources associées au sprite. Nous chargeons la palette
        /// d'animation de l'astéroïde.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Créer la palette d'animation des trois types d'astéroïde.
            palette = new Palette(content.Load<Texture2D>(@"Extra\SpriteLazerDown"), 50, 200);
           
        }

        /// <summary>
        /// Fonction membre mettant à jour la position du sprite.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // Déplacer l'astériode vers le bas en fonction de sa vitesse.
            this.Position = new Vector2(this.Position.X, this.Position.Y + (gameTime.ElapsedGameTime.Milliseconds * this.vitesseDeplacement));
           



            // La classe de base gère l'animation.
            base.Update(gameTime, graphics);
        }



        public override void Draw(Camera camera, SpriteBatch spriteBatch)
        {


            base.Draw(camera, spriteBatch);
        }
    }
}
