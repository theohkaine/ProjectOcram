//-----------------------------------------------------------------------
// <copyright file="MondeDeTuiles.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe représentant un monde constitué de tuiles étant extraites d'une palette de tuiles.
    /// En plus de cette palette, l'instance de cette classe dispose aussi d'un tableau bi-dimensionnel
    /// indiquant l'index de chaque tuile (de la palette) constituant le monde.
    /// </summary>
    public abstract class MondeDeTuiles : Monde
    {
        /// <summary>
        /// Accesseur retournant la largeur du monde en pixels
        /// </summary>
        public override int Largeur
        {
            get { return this.MappeMonde.GetLength(1) * this.PaletteDeTuiles.LargeurTuile; }
        }

        /// <summary>
        /// Accesseur retournant la hauteur du monde en pixels
        /// </summary>
        public override int Hauteur
        {
            get { return this.MappeMonde.GetLength(0) * this.PaletteDeTuiles.HauteurTuile; }
        }

        /// <summary>
        /// Accesseur à surcharger retournant la palette contenant les tuiles du monde.
        /// </summary>
        protected abstract Palette PaletteDeTuiles
        {
            get;
        }

        /// <summary>
        /// Accesseur à surcharger retournant la palette de tuiles permettant de gérer les collisions des 
        /// sprites avec les tuiles (aucune détection de collision par défaut).
        /// </summary>
        protected virtual Palette PaletteDeCollisions
        {
            get { return null; }
        }

        /// <summary>
        /// Accesseur à surcharger retournant la matrice de numéros de tuiles du monde.
        /// </summary>
        protected abstract int[,] MappeMonde
        {
            get;
        }

        /// <summary>
        /// Retourne l'index de la tuile localisée à la position donnée (en coordonnées du monde).
        /// </summary>
        /// <param name="position">Position (en coordonnées du monde) à convertir en index de tuile.</param>
        /// <returns>Index de la tuile contenant la position fournie.</returns>
        public int MondeXY2TuileIdx(Vector2 position)
        {
            int row = (int)(position.Y / this.PaletteDeTuiles.HauteurTuile);
            int col = (int)(position.X / this.PaletteDeTuiles.LargeurTuile);

     
            return this.MappeMonde[row, col];
        }

        /// <summary>
        /// Fonction retournant la couleur du pixel aux coordonnées du monde (position) selon la 
        /// palette de collisions. La palette de collisions (si fournie) sert généralement à 
        /// indiquer les zones du monde où les sprites peuvent se déplacer. Similairement, elle
        /// peut aussi servir à indiquer le type de terrain à la position donnée.
        /// </summary>
        /// <param name="position">Position du pixel en coordonnées du monde.</param>
        /// <returns>Couleur dans la palette de collision aux coordonnées du monde fournies. Si
        /// aucune palette de collision n'est fournie, une exception est lancée.</returns>
        public override Color CouleurDeCollision(Vector2 position)
        {
            // S'assurer qu'on dispose d'une palette de gestion de collisions.
            if (this.PaletteDeCollisions == null)
            {
                throw new NullReferenceException("Aucune palette de gestion de collisions fournie.");
            }

            // Extraire la couleur du pixel correspondant à la position donnée dans privTuilesCollisions.
            Color pixColor = this.PaletteDeCollisions.CouleurDePixel(
                this.MondeXY2TuileIdx(position),
                (int)position.X % this.PaletteDeCollisions.LargeurTuile,
                (int)position.Y % this.PaletteDeCollisions.HauteurTuile);

            return pixColor;
        }

        /// <summary>
        /// Affiche à l'écran la partie de la mappe monde visible par la camera.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public override void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Initialiser le rectangle de destination aux dimensions d'une tuile
            Rectangle destRect = new Rectangle(0, 0, this.PaletteDeTuiles.LargeurTuile, this.PaletteDeTuiles.HauteurTuile);

            // Afficher une rangée à la fois
            for (int row = 0; row < this.MappeMonde.GetLength(0); row++)
            {
                for (int col = 0; col < this.MappeMonde.GetLength(1); col++)
                {
                    // Calculer la position de la tuile à l'écran
                    destRect.X = col * this.PaletteDeTuiles.LargeurTuile;
                    destRect.Y = row * this.PaletteDeTuiles.HauteurTuile;

                    // Afficher la tuile si elle est visible
                    if (camera == null || camera.EstVisible(destRect))
                    {
                        // Décaler la destination en fonction de la caméra. Ceci correspond à transformer destRect 
                        // de coordonnées logiques (i.e. du monde) à des coordonnées physiques (i.e. de l'écran).
                        if (camera != null)
                        {
                            camera.Monde2Camera(ref destRect);
                        }

                        this.PaletteDeTuiles.Draw(this.MappeMonde[row, col], destRect, spriteBatch);
                    }
                }
            }
        }
    }
}
