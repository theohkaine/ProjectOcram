//-----------------------------------------------------------------------
// <copyright file="Monde.cs" company="Marco Lavoie">
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
    /// Classe abstraite servant de classe de base à celles représentant un monde
    /// dans lequel évolue les sprites du jeu.
    /// </summary>
    public abstract class Monde
    {
        /// <summary>
        /// Accesseur retournant la largeur du monde en pixels.
        /// </summary>
        public abstract int Largeur
        {
            get;
        }

        /// <summary>
        /// Accesseur retournant la hauteur du monde en pixels.
        /// </summary>
        public abstract int Hauteur
        {
            get;
        }

        /// <summary>
        /// Retourne la couleur du pixel dont la position donnée dans le monde.
        /// </summary>
        /// <param name="position">Coordonnées du pixel dans le monde.</param>
        /// <returns>La couleur du pixel dans les textures de collisions.</returns>
        public abstract Color CouleurDeCollision(Vector2 position);

        /// <summary>
        /// Affiche à l'écran la partie du monde visible par la caméra.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public abstract void Draw(Camera camera, SpriteBatch spriteBatch);
    }
}
