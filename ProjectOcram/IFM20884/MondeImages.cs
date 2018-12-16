//-----------------------------------------------------------------------
// <copyright file="MondeImages.cs" company="Marco Lavoie">
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
    /// Classe représentant un monde constitué de textures disposées côte-à-côte afin
    /// de constituer un monde.
    /// </summary>
    public abstract class MondeImages : Monde
    {
        /// <summary>
        /// Constructeur par défaut s'asurant de la validité des tableaux de textures
        /// fournies par les classes dérivées.
        /// </summary>
        public MondeImages()
        {
            ////float scale = .5f; //50% smaller
            // On s'assure d'avoir des images à afficher.
            if (this.Textures == null || this.Textures.GetLength(0) == 0 || this.Textures.GetLength(1) == 0)
            {
                throw new System.Exception("un tableau bidimensionnel d'images doit être disponible");
            }

            // On s'assure que toutes les images ont les mêmes dimensions.
            ValiderDimensionsDeTextures(this.Textures);

            // Si des textures sont fournies pour la détection de collisions,
            // on valide aussi leurs dimensions.
            try
            {
                ValiderDimensionsDeTextures(this.TexturesCollisions);
            }
            catch (NullReferenceException)
            {
                // C'est OK, aucune textures de collisions ne sont fournies.
            }
        }

        /// <summary>
        /// Tableau d'images constituant le monde. La première dimension est le nombre de
        /// rangées d'image, et la seconde dimension est le nombre de colonnes d'images.
        /// Par exemple, voici la disposition d'un monde avec textures[2,3]:
        ///        +--------------+--------------+--------------+
        ///        | texture[0,0] | texture[0,1] | texture[0,2] |
        ///        +--------------+--------------+--------------+
        ///        | texture[1,0] | texture[1,1] | texture[1,2] |
        ///        +--------------+--------------+--------------+
        /// Toutes les images doivent être de mêmes dimensions.
        /// Cet accesseur est abstrait et doit conséquemment être surchargé dans les
        /// classes dérivées afin de fournir un tableau de textures à afficher.
        /// </summary>
        public abstract Texture2D[,] Textures
        {
            get;
        }

        /// <summary>
        /// Tableau d'images servant à la détection de collision. Le format de
        /// du tableau de textures est le même que la propriété Textures (soit
        /// les rangées en première dimension et les colonnes en seconde).
        /// Cette propriété doit être surchargée dans les classes dérivées pour 
        /// fournir des textures de collisions. Si la détection de collisions
        /// n'est pas requise, vous n'avez pas à surcharger cette propriété dans
        /// la classe dérivée (en autant que vous vous assurez de ne pas invoquer
        /// CouleurDeCollision pour cette classe dérivée).
        /// Cet accesseur lance une exception car la classe MondeImages ne fournie
        /// pas de détection de collision, n'ayant pas de textures à cet effet. 
        /// </summary>
        public virtual Texture2D[,] TexturesCollisions
        {
            get { throw new NullReferenceException("aucune texture fournie pour la détection de collisions"); }
        }

        /// <summary>
        /// Accesseur retournant la largeur d'une image en pixels.
        /// </summary>
        public int LargeurImage
        {
            get { return this.Textures[0, 0].Width; }
        }

        /// <summary>
        /// Accesseur retournant la hauteur d'une image en pixels.
        /// </summary>
        public int HauteurImage
        {
            get { return this.Textures[0, 0].Height; }
        }

        /// <summary>
        /// Accesseur retournant la largeur du monde en pixels.
        /// </summary>
        public override int Largeur
        {
            get { return this.Textures.GetLength(1) * this.LargeurImage; }
        }

        /// <summary>
        /// Accesseur retournant la hauteur du monde en pixels.
        /// </summary>
        public override int Hauteur
        {
            get { return this.Textures.GetLength(0) * this.HauteurImage; }
        }

        /// <summary>
        /// Retourne la couleur du pixel dont la position est fournie dans les
        /// textures de collisions (propriété TexturesCollisions). Cette fonction
        /// est opérationnelle seulement si la classe dérivée fournie des textures
        /// de collisions, sinon une exception est lancée.
        /// </summary>
        /// <param name="position">Coordonnées du pixel dans le monde.</param>
        /// <returns>La couleur du pixel dans les textures de collisions.</returns>
        public override Color CouleurDeCollision(Vector2 position)
        {
            // S'assurer que les coordonnées fournies sont dans le monde.
            if (position.X < 0 || position.X > this.Largeur || position.Y < 0 || position.Y > this.Hauteur)
            {
                return Color.Black;
            }

            // Déterminer dans quelle image est le pixel visé.
            int row = (int)position.Y / this.HauteurImage;
            int col = (int)position.X / this.LargeurImage;

            // Déterminer le pixel d'intérêt dans l'image.
            int x = (int)position.X % this.LargeurImage;
            int y = (int)position.Y % this.HauteurImage;

            // Déclarer un tableau juste assez grand pour stocker la couleur d'UN SEUL PIXEL
            Color[] colorData = new Color[1];

            // Calculer un rectangle d'un seul pixel positionné au pixel d'intérêt dans l'image.
            Rectangle targetRect = new Rectangle(x, y, 1, 1);

            // Extraire la couleur du pixel      
            this.TexturesCollisions[row, col].GetData<Color>(0, targetRect, colorData, 0, 1);

            return colorData[0];
        }

        /// <summary>
        /// Affiche à l'écran la partie de l'arrière plan du monde visible par la caméra.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public override void DrawArrierePlan(Camera camera, SpriteBatch spriteBatch)
        {
            this.Draw(camera, spriteBatch);
        }

        /// <summary>
        /// Affiche à l'écran la partie de la mappe monde visible par la caméra.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        protected void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Initialiser le rectangle de destination aux dimensions d'une image.
            Rectangle destRect = new Rectangle(0, 0, this.LargeurImage, this.HauteurImage);

            // Afficher une rangée à la fois.
            for (int row = 0; row < this.Textures.GetLength(0); row++)
            {
                for (int col = 0; col < this.Textures.GetLength(1); col++)
                {
                    // Calculer la position de l'image à l'écran.
                    destRect.X = col * this.LargeurImage;
                    destRect.Y = row * this.HauteurImage;

                    // Afficher l'image si elle est visible dans la caméra.
                    if (camera == null || camera.EstVisible(destRect))
                    {
                        // Décaler la destination en fonction de la caméra. Ceci correspond à transformer destRect 
                        // de coordonnées logiques (i.e. du monde) à des coordonnées physiques (i.e. de l'écran).
                        if (camera != null)
                        {
                            camera.Monde2Camera(ref destRect);
                        }

                        // Afficher l'image courante.
                        spriteBatch.Draw(this.Textures[row, col], destinationRectangle: destRect);   // afficher la tuile
                    }
                }
            }
        }

        /// <summary>
        /// Cette fonction s'assure que les textures contenues dans le tableau de
        /// textures fourni en argument (paramètre tex) aient toutes les mêmes
        /// dimensions. Si ce n'est pas le cas, une exception est lancée.
        /// </summary>
        /// <param name="tex">Tableau des textures à valider.</param>
        private static void ValiderDimensionsDeTextures(Texture2D[,] tex)
        {
            // On s'assure que toutes les images ont les mêmes dimensions.
            int largeur = tex[0, 0].Width;
            int hauteur = tex[0, 0].Height;
            for (int row = 0; row < tex.GetLength(0); row++)
            {
                for (int col = 0; col < tex.GetLength(1); col++)
                {
                    if (tex[row, col].Width != largeur || tex[row, col].Height != hauteur)
                    {
                        throw new System.Exception("les images doivent être de dimensions uniformes");
                    }
                }
            }
        }
    }
}
