//-----------------------------------------------------------------------
// <copyright file="Plateforme.cs" company="Marco Lavoie">
// Marco Lavoie, 2010. Tous droits réservés
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
/// Classe représentant une plateforme se déplaçant horizontalement dans le monde. Celle-ci
/// peut transporter des sprites.
/// </summary>
public class Plateforme : Sprite
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
        public Plateforme(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Plateforme(float x, float y) : base(x, y)
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
            float vitesseH;

            // Faire bouger la plateforme seulement lorsque le sprite joueur est dessu.
            if (passagers.Count < 1)
            {
                vitesseH = 0.0f;
            }

            else
            {
                vitesseH = 0.3f;
            }
               


            int  deltaX = -(int)(gameTime.ElapsedGameTime.Milliseconds * vitesseH);
            

            // Repositionner la plateforme selon le déplacement horizontal calculé.
            this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y);

            // Déplacer aussi tous les sprites transportés par la plateforme.
            foreach (Sprite sprite in this.passagers)
            {
               
                    sprite.Position = new Vector2(sprite.Position.X + deltaX, sprite.Position.Y);
                
               
            }
        }
    }
}
