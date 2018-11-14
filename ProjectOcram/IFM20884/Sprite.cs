//-----------------------------------------------------------------------
// <copyright file="Sprite.cs" company="Marco Lavoie">
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
    /// Classe abstraite de base des sprites du jeu.
    /// </summary>
    public abstract class Sprite
    {
        /// <summary>
        /// Attribut stockant la position du centre du sprite.
        /// </summary>
        private Vector2 position;         // position du sprite en 2D

        /// <summary>
        /// Attribut statique contenant le rectangle confinant les mouvements du sprite.
        /// </summary>
        private Rectangle boundsRect;      // rectangle confinant les mouvements du sprite

        /// <summary>
        /// Attribut contrôlant le rayon appliquée au sprite pour la détection approximative de collisions.
        /// </summary>
        private float rayonDeCollision;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Sprite(float x, float y)
        {
            this.Position = new Vector2(x, y);

            // Aucun rayon, car le sprite n'a peut-être pas encore de texture.
            this.rayonDeCollision = 0.0f;
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Sprite(Vector2 position)
            : this(position.X, position.Y) 
        { 
        }

        /// <summary>
        /// Propriété abstraite pour manipuler la texture du sprite. Doit être
        /// surchangée dans les classes dérivées afin de manipuler une Texture2D.
        /// </summary>
        public abstract Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Accesseur de l'attribut privé position contrôlant la position centrale du
        /// sprite. Le mutateur s'assure que la position fournie est confinée aux bornes
        /// du monde (en fonction de l'attribut BoundsRect).
        /// </summary>
        public Vector2 Position             // accesseur pour position
        {
            get 
            {
                return this.position; 
            }

            // Le setter s'assure que la nouvelle position n'excède pas les bornes de mouvements
            // si elles sont fournies.
            set
            {
                this.position = value;

                // Limiter le mouvement si un boundsRect est fourni.
                this.ClampPositionToBoundsRect();
            }
        }

        /// <summary>
        /// Accesseur de l'attribut privé boundsRect contrôlant les bornes de positionnement
        /// du sprite. Le mutateur s'assure que la position courante du sprite est confinée
        /// aux nouvelles bornes du monde.
        /// </summary>
        public Rectangle BoundsRect          // accesseur pour boundsRect
        {
            get 
            {
                return this.boundsRect; 
            }

            // Le setter s'assurer que la position courante est confinée au nouvelles bornes.
            set
            {
                this.boundsRect = value;
                this.Position = this.position;         // exploiter le setter de position 
            }
        }

        /// <summary>
        /// Accesseur retournant le rectangle couvert par le sprite en fonction de sa
        /// position et de sa taille.
        /// </summary>
        public Rectangle PositionRect
        {
            get
            {
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                rect.X = (int)this.Position.X - (this.Width / 2);
                rect.Y = (int)this.Position.Y - (this.Height / 2);

                return rect;
            }
        }

        /// <summary>
        /// Accesseur surchargeable pour obtenir la largeur du sprite.
        /// </summary>
        public virtual int Width
        {
            get { return this.Texture.Width; }
        }

        /// <summary>
        /// Accesseur surchargeable pour obtenir la hauteur du sprite.
        /// </summary>
        public virtual int Height
        {
            get { return this.Texture.Height; }
        }

        /// <summary>
        /// Accesseur pour attribut contrôlant le rayon appliqué au sprite pour la détection 
        /// approximative de collisions.
        /// </summary>
        public virtual float RayonDeCollision
        {
            // Si aucun rayon n'est explicitement fourni, calculer un implicitement
            // de façon à inclure la totalité de la texture.
            get
            {
                if (this.rayonDeCollision > 0.0f)
                {
                    return this.rayonDeCollision;
                }
                else
                {
                    return (float)Math.Sqrt((this.Width * this.Width) + (this.Height * this.Height)) / 2.0f;
                }
            }

            set
            {
                this.rayonDeCollision = value;
            }
        }

        /// <summary>
        /// Fonction membre abstraite (doit être surchargée) mettant à jour le sprite.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public abstract void Update(GameTime gameTime, GraphicsDeviceManager graphics);

        /// <summary>
        /// Fonction membre à surcharger pour dessiner le sprite. Par défaut la this.texture est
        /// affichée à sa position.
        /// </summary>
        /// <param name="camera">Caméra indiquant la partie du monde présentement visible à l'écran (peut être nulle).</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public virtual void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // On doit travailler avec une copie de PositionRect car on va mapper ses coordonnées
            // en celles du monde si on nous a fourni une caméra.
            Rectangle destRect = this.PositionRect;

            // Si une caméra est fournie, on s'assure que le sprite y est visible.
            if (camera == null || camera.EstVisible(destRect))
            {
                // Décaler la destination en fonction de la caméra avant de dessiner.
                if (camera != null)
                {
                    camera.Monde2Camera(ref destRect);
                }

                // Afficher le sprite.
                spriteBatch.Draw(this.Texture, destinationRectangle: destRect);
            }
        }

        /// <summary>
        /// Fonction extrayant l'information de couleur pour chacun de ses pixels. Ces informations
        /// sont retournées dans un tableau de couleurs créé à cet effet.
        /// </summary>
        /// <returns>Tableau de couleurs, une couleur par pixel.</returns>
        public virtual Color[] ExtraireCouleurs()
        {
            // Extraire les données de couleurs dans un nouveau tableau
            Color[] data = new Color[this.Width * this.Height];
            this.Texture.GetData<Color>(data);

            return data;   // retourner le tableau de couleurs
        }

        /// <summary>
        /// Fonction vérifiant si this est en collision avec le sprite fourni en paramètre. La routine
        /// effectue deux tests : le premier, rapide, consiste à effectuer une détection de collision
        /// par forme englobante (i.e. un cercle). Si ce premier test indique un potentiel de collision,
        /// un second test plus précis, la détection de collisions par superposition de pixels, est
        /// appliqué.
        /// </summary>
        /// <param name="cible">Sprite à vérifier s'il y a collision avec this.</param>
        /// <returns>Vrai si this est en collision avec cible.</returns>
        public virtual bool Collision(Sprite cible)
        {
            // Appliquer premièrement la détection par forme englobante
            float distance = (float)Math.Sqrt(Math.Pow(this.Position.X - cible.Position.X, 2f) + Math.Pow(this.Position.Y - cible.Position.Y, 2f));
            if (distance > (this.RayonDeCollision + cible.RayonDeCollision))
            {
                return false;
            }

            // Il y a risque de collision, donc on applique la détection par superposition de pixels
            // Premièrement, déterminer le rectangle de coordonnées pour chaque sprite (ne pas oublié 
            // que le sprite est centré à Position, donc il faut compenser pour son origine)
            Rectangle rectangleA = new Rectangle(
                                        (int)(this.Position.X - (this.Width / 2)),
                                        (int)(this.Position.Y - (this.Height / 2)),
                                        this.Width,
                                        this.Height);
            Rectangle rectangleB = new Rectangle(
                                        (int)(cible.Position.X - (cible.Width / 2)),
                                        (int)(cible.Position.Y - (cible.Height / 2)),
                                        cible.Width,
                                        cible.Height);

            // Obtenir les données de couleur pour chaque sprite
            Color[] dataA = this.ExtraireCouleurs();
            Color[] dataB = cible.ExtraireCouleurs();

            // Déterminer les coordonnées du rectangle résultant de l'intersection des deux sprite
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Comparer chaque pixel dans le rectangle d'intersection : si deux pixels correspondants ne sont
            // pas transparents, alors il y a collision
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Identifier la couleur du pixel de chaque sprite
                    Color colorA = dataA[(x - rectangleA.Left) + ((y - rectangleA.Top) * rectangleA.Width)];
                    Color colorB = dataB[(x - rectangleB.Left) + ((y - rectangleB.Top) * rectangleB.Width)];

                    // Effectuer un ET logique des deux couleurs
                    if ((colorA.A & colorB.A) != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Fonction restreignant _position à l'intérieur des limites fournies par boundsRect si
        /// de telles limites sont fournies.
        /// </summary>
        protected virtual void ClampPositionToBoundsRect()
        {
            // Limiter le mouvement si un boundsRect est fourni.
            if (!this.boundsRect.IsEmpty)
            {
                // On divise la taille du sprite par 2 car _position indique le centre du sprite.
                this.position.X = MathHelper.Clamp(this.position.X, this.boundsRect.Left + (this.Width / 2), this.boundsRect.Right - (this.Width / 2));
                this.position.Y = MathHelper.Clamp(this.position.Y, this.boundsRect.Top + (this.Height / 2), this.boundsRect.Bottom - (this.Height / 2));
            }
        }
    }
}
