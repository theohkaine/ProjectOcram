//-----------------------------------------------------------------------
// <copyright file="Palette.cs" company="Marco Lavoie">
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
    using System.Threading.Tasks;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Une palette de tuiles est essentiellement une texture contenant une série de tuiles.
    /// Celles-ci peuvent servir soit à constituer un monde de tuiles (voir la classe MondeDeTuiles)
    /// ou les différents mouvements d'animation d'un sprite (voir classe SpriteAnimation).
    /// </summary>
    public class Palette
    {
        /// <summary>
        /// Palette des tuiles.
        /// </summary>
        private Texture2D tuiles;

        /// <summary>
        /// Largeur d'une tuile en pixels.
        /// </summary>
        private int largeurTuile;

        /// <summary>
        /// Hauteur d'une tuile en pixels.
        /// </summary>
        private int hauteurTuile;

        /// <summary>
        /// Constructeur paramétré.
        /// </summary>
        /// <param name="tuiles">Texture contenant les tuiles.</param>
        /// <param name="largeurTuile">Largeur uniforme de chaque tuile, en pixels.</param>
        /// <param name="hauteurTuile">Hauteur uniforme de chaque tuile, en pixels.</param>
        public Palette(Texture2D tuiles, int largeurTuile, int hauteurTuile)
        {
            this.tuiles = tuiles;

            this.largeurTuile = largeurTuile;
            this.hauteurTuile = hauteurTuile;
        }

        /// <summary>
        /// Accesseur pour l'attribut tuiles.
        /// </summary>
        public Texture2D Tuiles
        {
            get { return this.tuiles; }
        }

        /// <summary>
        /// Accesseur pour l'attribut largeurTuile.
        /// </summary>
        public int LargeurTuile
        {
            get { return this.largeurTuile; }
        }

        /// <summary>
        /// Accesseur pour l'attribut hauteurTuile.
        /// </summary>
        public int HauteurTuile
        {
            get { return this.hauteurTuile; }
        }

        /// <summary>
        /// Accesseur retournant le nombre de tuiles dans la palette, calculée selon
        /// la taille de la palette et les dimensions uniformes des tuiles.
        /// </summary>
        public int NombreDeTuiles
        {
            get
            {
                int tuilesParRangee = this.tuiles.Width / this.LargeurTuile;   // nombre de tuiles dans une rangée de la palette
                int tuilesParColonne = this.tuiles.Height / this.HauteurTuile; // nombre de tuiles dans une colonne de la palette

                return tuilesParRangee * tuilesParColonne;
            }
        }

        /// <summary>
        /// Retourne la couleur du pixel à la position (x,y) de la tuile indiquée de la palette. Notez que
        /// (x,y) doit être relatif au coin supérieur gauche de la tuile et non de la palette.
        /// </summary>
        /// <param name="tuileIdx">Index de tuile à considérer.</param>
        /// <param name="x">Coordonnée x du pixel dans la tuile.</param>
        /// <param name="y">Coordonnée y du pixel dans la tuile.</param>
        /// <returns>Couleur du pixel.</returns>
        public Color CouleurDePixel(int tuileIdx, int x, int y)
        {
            // Calculer le nombre de tuiles dans une rangée de la palette.
            int tuilesParRangee = this.tuiles.Width / this.LargeurTuile;

            int paletteRow = tuileIdx / tuilesParRangee;   // rangée de la tuile visée
            int paletteCol = tuileIdx % tuilesParRangee;   // colonne de la tuile visée

            // Déclarer un tableau juste assez grand pour stocker la couleur d'UN SEUL PIXEL.
            Color[] colorData = new Color[1];

            // Calculer un rectangle d'un seul pixel positionné aux coordonnées corrigées en fonction
            // de l'origine de la palette.
            Rectangle targetRect = new Rectangle((paletteCol * this.LargeurTuile) + x, (paletteRow * this.HauteurTuile) + y, 1, 1);

            // Extraire la couleur du pixel.   
            this.tuiles.GetData<Color>(0, targetRect, colorData, 0, 1);

            return colorData[0];
        }

        /// <summary>
        /// Retourne un Rectangle aux coordonnées de la tuile (en pixels) indiquée en argument.
        /// </summary>
        /// <param name="tuileIdx">Index de la tuile dont on veut obtenir les coordonnées.</param>
        /// <returns>Rectangle contenant les coordonnées de la tuile (en pixels) dans la palette.</returns>
        public Rectangle SourceRect(int tuileIdx)
        {
            int tuilesParRangee = this.tuiles.Width / this.LargeurTuile; // nombre de tuiles dans une rangée de la palette

            int paletteRow = tuileIdx / tuilesParRangee;            // rangée de la tuile visée
            int paletteCol = tuileIdx % tuilesParRangee;            // colonne de la tuile visée

            // Créer un Rectangle aux coordonnées et dimensions de la tuile dans la palette.
            Rectangle sourceRect = new Rectangle(
                paletteCol * this.LargeurTuile,
                paletteRow * this.HauteurTuile,
                this.LargeurTuile,
                this.HauteurTuile);

            return sourceRect;
        }

        /// <summary>
        /// Affiche à l'écran la tuile indiquée dans le rectangle destinataire fourni
        /// </summary>
        /// <param name="tuileIdx">Index de la tuile à afficher.</param>
        /// <param name="destRect">Position où afficher la tuile à l'écran.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public void Draw(int tuileIdx, Rectangle destRect, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.tuiles, destinationRectangle: destRect, sourceRectangle: this.SourceRect(tuileIdx));   // afficher la tuile
        }
    }
}
