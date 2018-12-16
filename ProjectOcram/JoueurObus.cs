//-----------------------------------------------------------------------
// <copyright file="JoueurObus.cs" company="Tristan Araujo & Dominik Desjardins">
// Tristan Araujo & Dominik Desjardins, 2018. Tous droits réservés
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
    using System.Threading.Tasks;
    using IFM20884;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe implantant l'obus tirer par le joueur et du
    /// téléchargement du sprite obus.
    /// </summary>
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
            this.VitessesPropulsion = vitesses / 1.5f;
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
            ////laserV2
            ////lol2
         
            bombe = content.Load<Texture2D>(@"Joueur\ObusDuJoueur\laserV2");           
        }
    }
}