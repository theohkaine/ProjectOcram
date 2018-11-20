//-----------------------------------------------------------------------
// <copyright file="MondeDarkBros.cs" company="Marco Lavoie">
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
    /// Classe concrétisant un monde d'image de Mario Bros.
    /// </summary>
    public class MondeOcram : MondeImages
    {
        /// <summary>
        /// Attribut fournissant les textures d'affichage à la propriété Textures.
        /// </summary>
        private static Texture2D[,] textures;

        /// <summary>
        /// Attribut fournissant les textures de détection de collisions à la 
        /// propriété TexturesCollisions.
        /// </summary>
        private static Texture2D[,] texturesCollisions;

        /// <summary>
        /// Surcharge de la propriété accesseur aux textures d'affichage. Cette
        /// propriété est exploitée par la classe de base pour afficher le
        /// monde d'images.
        /// </summary>
        public override Texture2D[,] Textures
        {
            get { return textures; }
        }

        /// <summary>
        /// Surcharge de la propriété accesseur aux textures de détection de
        /// collisions. Cette propriété est exploitée par la classe de base pour 
        /// extraire les couleurs de collisions.
        /// </summary>
        public override Texture2D[,] TexturesCollisions
        {
            get { return texturesCollisions; }
        }

        /// <summary>
        /// Surcharghe de la propriété ccesseur retournant la position initiale du sprite 
        /// du joueur dans le monde.
        /// </summary>
        public override Vector2 PositionInitiale
        {
            get { return new Vector2(2, 2); }
        }

        /// <summary>
        /// Charge les images d'affichage et de détection de collisions.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        public static void LoadContent(ContentManager content)
        {
            // Créer les deux tableaux de textures.
            textures = new Texture2D[1, 1];
            texturesCollisions = new Texture2D[1, 1];

            // Charger les textures d'affichage, rangée par rangée
            textures[0, 0] = content.Load<Texture2D>(@"Monde\MondeOcram\map01");
           



            // Charger les textures de collisions, rangée par rangée
            texturesCollisions[0, 0] = content.Load<Texture2D>(@"Monde\MondeOcram\map02");
            

        }
    }
}
