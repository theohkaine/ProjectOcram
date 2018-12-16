using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOcram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using IFM20884;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// Classe représentant une plateforme se déplaçant horizontalement dans le monde. Celle-ci
    /// peut transporter des sprites.
    /// </summary>
    public class PlateformeDescendante : Sprite
    {
        /// <summary>
        /// Texture représentant la plateforme dans la console.
        /// </summary>
        private static Texture2D texture;

        /// <summary>
        /// Vitesse verticale de déplacement, exploitée lors des sauts et lorsque le sprite tombe dans
        /// un trou.
        /// </summary>
        private float vitesseVerticale = 0.0f;

        float vitesseV = 0.0f;



        /// <summary>
        /// Liste des sprites que la plateforme doit déplacer avec elle.
        /// </summary>
        private List<Sprite> passagers;

        /// <summary>
        /// Accesseur et mutateur pour attribut vitesseVerticale.
        /// </summary>
        public float VitesseVerticale
        {
            get { return this.vitesseVerticale; }
            set { this.vitesseVerticale = value; }
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public PlateformeDescendante(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public PlateformeDescendante(float x, float y) : base(x, y)
        {
            // Créer la liste où seront stockés les sprites transportés par la plateforme.
            this.passagers = new List<Sprite>();
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
            texture = content.Load<Texture2D>(@"GameObject\Platform");
        }

        /// <summary>
        /// Ajoute le sprite donné à la liste des sprites que la plateforme doit 
        /// déplacer avec elle. On suppose que le sprite donné est "debout" sur la
        /// plateforme mais on ne valide pas cette hypothèse ici.
        /// </summary>
        /// <param name="sprite">Le sprite à ajouter à la liste des sprites transportés.</param>
        public void AjouterPassager(Sprite sprite)
        {
            if (!this.passagers.Contains(sprite))
            {
                this.passagers.Add(sprite);
            }
        }

        /// <summary>
        /// Retire le sprite donné de la liste des sprites que la plateforme doit 
        /// déplacer avec elle.
        /// </summary>
        /// <param name="sprite">Le sprite à retirer de la liste des sprites transportés.</param>
        public void RetirerPassager(Sprite sprite)
        {
            if (this.passagers.Contains(sprite))
            {
                this.passagers.Remove(sprite);
            }
        }

        /// <summary>
        /// Fonction membre abstraite (doit être surchargée) mettant à jour le sprite.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {


            // Faire bouger la plateforme seulement lorsque le sprite joueur est dessu.

            if (passagers.Count > 0)
                vitesseV = 0.4f;


            int deltaY = +(int)(gameTime.ElapsedGameTime.Milliseconds * vitesseV);


            // Repositionner la plateforme selon le déplacement horizontal calculé.
            this.Position = new Vector2(this.Position.X , this.Position.Y+ deltaY);

            // Déplacer aussi tous les sprites transportés par la plateforme.
            foreach (Sprite sprite in this.passagers)
            {

                sprite.Position = new Vector2(sprite.Position.X , sprite.Position.Y+deltaY);


            }
        }
    }
}

