using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IFM20884;

namespace ProjectOcram
{
    public class JoueurObus : Obus
    {

        /// <summary>
        /// Attribut statique contenant la texture de l'obus.
        /// </summary>
        private static Texture2D bombe;

        /// <summary>
        /// Initialise une nouvelle instance de la classe JoueurObus.
        /// </summary>
        /// <param name="x">Position en x du sprite.</param>
        /// <param name="y">Position en y du sprite.</param>
        /// <param name="vitesses">Vitesses (horizontale et verticale) de propultion de l'obus.</param>
        public JoueurObus(float x, float y, Vector2 vitesses)
            : base(x, y, vitesses)
        {
            this.VitessesPropulsion = vitesses/1.5f;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe JoueurObus.
        /// </summary>
        /// <param name="position">Position du sprite.</param>
        /// <param name="vitesses">Vitesses (horizontale et verticale) de propultion de l'obus.</param>
        public JoueurObus(Vector2 position, Vector2 vitesses)
            : this(position.X, position.Y, vitesses)
        {
        }

        /// <summary>
        /// On doit surcharger l'accesseur texture en conséquence (toute classe à instancier dérivée 
        /// de Sprite doit surcharger cet accesseur).
        /// </summary>
        public override Texture2D Texture
        {
            get { return bombe; }
        }

        /// <summary>
        /// Fonction membre chargeant les ressources associées au sprite.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            //laserV2
            //lol2
            bombe = content.Load<Texture2D>(@"Joueur\ObusDuJoueur\laserV2");
           
        }
    }
}
