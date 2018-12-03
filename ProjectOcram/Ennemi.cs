using IFM20884;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ProjectOcram
{
    public class Ennemi : SpriteAnimation
    {
        /// <summary>
        /// Attribut statique contenant la palette d'animation de l'enemi Slime deplacement vers la gauche.
        /// </summary>
        private static Palette SlimeG;

        /// <summary>
        /// Attribut statique contenant la palette d'animation de l'enemi Slime deplacement vers la gauche.
        /// </summary>
        private static Palette SlimeD;
        /// <summary>
        /// Attribut statique contenant la palette d'animation de l'enemi Slime deplacement vers la gauche.
        /// </summary>
        private static Palette MiroyrG;

        /// <summary>
        /// Attribut statique contenant la palette d'animation de l'enemi Slime deplacement vers la gauche.
        /// </summary>
        private static Palette MiroyrD;

        /// <summary>
        /// Attribut statique fournissant un générateur de nombres aléatoires commun à toutes
        /// les instances.
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Vitesse de déplacement verticale du sprite.
        /// </summary>
        private float vitesseDeplacement;

        /// <summary>
        /// Direction du sprite.
        /// </summary>
        private int directionEnnemi = 0;

        /// <summary>
        /// Attribut indiquant le type de monstre.
        /// </summary>
        private CategoriesMonstre ennemi;

        /// <summary>
        /// Initialise une nouvelle instance de la classe Ennemi.
        /// </summary>
        /// <param name="x">Position en x du sprite.</param>
        /// <param name="y">Position en y du sprite.</param>
        public Ennemi(float x, float y)
            : base(x, y)
        {
            this.vitesseDeplacement = 0.1f;     // vitesse de déplacement vertical par défaut
            this.VitesseAnimation = 0.07f;      // vitesse d'animation pour fluidité

            // Choisir le matériau au hasard.
            switch (random.Next(2))
            {
                case 0:
                    this.ennemi = CategoriesMonstre.Slime;
                    break;
                case 1:
                    this.ennemi = CategoriesMonstre.Miroyr;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Ennemi.
        /// </summary>
        /// <param name="position">Position du sprite.</param>
        public Ennemi(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Enumération des Categorie de monstre disponibles.
        /// </summary>
        public enum CategoriesMonstre
        {
            /// <summary>
            /// Monstre crocy.
            /// </summary>>
            Slime,

            /// <summary>
            /// Monstre crocy.
            /// </summary>>
            Miroyr,
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
                switch (this.ennemi)
                {
                    case CategoriesMonstre.Slime:
                        if (this.directionEnnemi < 0)
                        {
                            return SlimeG;
                        }
                        else return SlimeD;
                    case CategoriesMonstre.Miroyr:
                        if (this.directionEnnemi < 0)
                        {
                            return MiroyrG;
                        }
                        else return MiroyrD;
                    default:
                        if (this.directionEnnemi< 0)
                        {
                            return MiroyrG;
                        }
                        else return MiroyrD;
                }
                   
            }
        }

        /// Fonction membre chargeant les ressources associées au sprite. Nous chargeons la palette
        /// d'animation d'enemis.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Charger les différentes palettes du personnage selon les directions.
            SlimeG = new Palette(content.Load<Texture2D>(@"Ennemi\Slime\SlimeLeft"), 80, 80);
            SlimeD = new Palette(content.Load<Texture2D>(@"Ennemi\Slime\SlimeRight"), 80, 80);
            MiroyrG = new Palette(content.Load<Texture2D>(@"Ennemi\Miroyr\MiroyrLeft"),80, 80);
            MiroyrD = new Palette(content.Load<Texture2D>(@"Ennemi\Miroyr\MiroyrRight"), 80,80);
        }

        /// <summary>
        /// Fonction membre mettant à jour la position du sprite.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if ((gameTime.TotalGameTime.Seconds / 1) % 2 == 0)
            {
                this.directionEnnemi = 1;
            }
            else
            {
                this.directionEnnemi= -1;
            }

            // Déplacer l'ennemi.
            this.Position = new Vector2(this.Position.X + (this.directionEnnemi * (gameTime.ElapsedGameTime.Milliseconds * (this.vitesseDeplacement - 0.1f))), this.Position.Y);

            // La classe de base gère l'animation.
            base.Update(gameTime, graphics);
        }
    }
}
