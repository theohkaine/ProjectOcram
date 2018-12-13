using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFM20884;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOcram
{
   public class Door : Sprite
    {


        public Rectangle DoorCollision { get; set; }


        /// <summary>
        /// Texture représentant la plateforme dans la console.
        /// </summary>
        private static Texture2D texture;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Door(Vector2 positions)
            : this(positions.X, positions.Y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Door(float x, float y) : base(x, y)
        {
        }

        /// <summary>
        /// Propriété pour manipuler la texture du sprite. Celle-ci est commune à toutes les
        /// instances.
        /// </summary>
        public override Texture2D Texture
        {
            get { return texture; }
        }


        /// <summary>
        /// Charge l'image de la plateforme.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Charger la texture associée à la plateforme.
            texture = content.Load<Texture2D>(@"GameObject\wall");
        }

        /// <summary>
        /// Fonction membre abstraite (doit être surchargée) mettant à jour le sprite.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {

        }

    }
}
