//-----------------------------------------------------------------------
// <copyright file="Camera.cs" company="Marco Lavoie">
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

    /// <summary>
    /// Classe implantant une caméra d'affichage dont les mouvements sont restreints
    /// à un monde dont les dimensions sont fournies.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Propriété permettant la gestion du Rectangle définissant la partie du monde
        /// visible par la caméra.
        /// </summary>
        private Rectangle cameraRect;    // attribut non public où stocker le rectangle de caméra

        /// <summary>
        /// Propriété permettant de définir les coordonnées du monde à l'intérieur desquels
        /// la caméra peut se déplacer.
        /// </summary>
        private Rectangle mondeRect;     // attribut non public où stocker le rectangle du monde

        /// <summary>
        /// Constructeur paramétré.
        /// </summary>
        /// <param name="camera">Rectangle des coordonnées de la caméra.</param>
        /// <param name="monde">Rectangle des dimensions du monde.</param>
        public Camera(Rectangle camera, Rectangle monde)
        {
            // Initialiser les coordonnées de la caméra ey du monde.
            this.cameraRect = camera;
            this.mondeRect = monde;
        }

        /// <summary>
        /// Constructeur paramétré. Puisque seules les coordonnées de la caméra sont fournies,
        /// on suppose qu'elles correspondent aussi aux coordonnées du monde.
        /// </summary>
        /// <param name="camera">Rectangle des coordonnées de la caméra.</param>
        public Camera(Rectangle camera)
            : this(camera, camera) 
        { 
        }

        /// <summary>
        /// Accesseur public de l'attribut privé cameraRect.
        /// </summary>
        public Rectangle CameraRect        // propriété public avec getter et setter
        {
            get
            {
                return this.cameraRect;
            }

            // S'assurer que la caméra est restreinte à l'intérieur du monde.
            set
            {
                this.cameraRect = value;
                this.RestreindreCameraAuMonde();
            }
        }

        /// <summary>
        /// Accesseur public de l'attribut privé mondeRect.
        /// </summary>
        public Rectangle MondeRect          // propriété public avec getter et setter
        {
            get
            {
                return this.mondeRect;
            }

            // S'assurer que la caméra est restreinte à l'intérieur du monde.
            set
            {
                this.mondeRect = value;
                this.RestreindreCameraAuMonde();
            }
        }

        /// <summary>
        /// Indique si le rectangle fourni est visible dans la caméra.
        /// </summary>
        /// <param name="bounds">Rectangle à vérifier la visibilité.</param>
        /// <returns>Vrai si bounds est visible dans cameraRect; faux sinon.</returns>
        public bool EstVisible(Rectangle bounds)
        {
            return bounds.Intersects(this.cameraRect);
        }

        /// <summary>
        /// Indique si la caméra est totalement au dessus du rectangle fourni (c.à.d. ce rectangle
        /// n'est pas visible dans la caméra car il est en dessous de celle-ci).
        /// </summary>
        /// <param name="rect">Rectangle à vérifier la visibilité.</param>
        /// <returns>Vrai si rect est totalement sous cameraRect; faux sinon.</returns>
        public bool EstAuDessus(Rectangle rect)
        {
            return rect.Top > this.cameraRect.Bottom;
        }

        /// <summary>
        /// Indique si la caméra est totalement au dessous du rectangle fourni (c.à.d. ce rectangle
        /// n'est pas visible dans la caméra car il est en dessus de celle-ci).
        /// </summary>
        /// <param name="rect">Rectangle à vérifier la visibilité.</param>
        /// <returns>Vrai si rect est totalement au dessus de cameraRect; faux sinon.</returns>
        public bool EstAuDessous(Rectangle rect)
        {
            return rect.Bottom < this.cameraRect.Top;
        }

        /// <summary>
        /// Indique si la caméra est totalement à gauche du rectangle fourni (c.à.d. ce rectangle
        /// n'est pas visible dans la caméra car il est à droite de celle-ci).
        /// </summary>
        /// <param name="rect">Rectangle à vérifier la visibilité.</param>
        /// <returns>Vrai si rect est totalement à droite de cameraRect; faux sinon.</returns>
        public bool EstAGauche(Rectangle rect)
        {
            return rect.Left > this.cameraRect.Right;
        }

        /// <summary>
        /// Indique si la caméra est totalement à droite du rectangle fourni (c.à.d. ce rectangle
        /// n'est pas visible dans la caméra car il est à gauche de celle-ci).
        /// </summary>
        /// <param name="rect">Rectangle à vérifier la visibilité.</param>
        /// <returns>Vrai si rect est totalement à gauche de cameraRect; faux sinon.</returns>
        public bool EstADroite(Rectangle rect)
        {
            return rect.Right < this.cameraRect.Left;
        }

        /// <summary>
        /// Convertit le rectangle fourni de coordonnées du monde en coordonnées
        /// de la camérao (où 0:0 correspond au coin supérieur gauche de l'écran).
        /// </summary>
        /// <param name="rect">Rectangle de coordonnées à convertir.</param>
        public void Monde2Camera(ref Rectangle rect)
        {
            rect.X = rect.X - this.cameraRect.X;
            rect.Y = rect.Y - this.cameraRect.Y;
        }

        /// <summary>
        /// Convertit le rectangle fourni de coordonnées de la camérao (où 0:0 
        /// correspond au coin supérieur gauche de l'écran) en coordonnées du monde.
        /// </summary>
        /// <param name="rect">Rectangle de coordonnées à convertir.</param>
        public void Camera2Monde(ref Rectangle rect)
        {
            rect.X = rect.X + this.cameraRect.X;
            rect.Y = rect.Y + this.cameraRect.Y;
        }

        /// <summary>
        /// Centre la caméra aux coordonnées (du monde) fournies tout en s'assurant 
        /// qu'elle ne déborde pas du monde.
        /// </summary>
        /// <param name="pos">Nouvelle position du centre de la caméra.</param>
        public void Centrer(Vector2 pos)
        {
            this.cameraRect.X = (int)(pos.X - (this.cameraRect.Width / 2));
            this.cameraRect.Y = (int)(pos.Y - (this.cameraRect.Height / 2));

            this.RestreindreCameraAuMonde();
        }

        /// <summary>
        /// Repositionne la caméra à l'intérieur du monde.
        /// </summary>
        private void RestreindreCameraAuMonde()
        {
            // Restreindre en fonction du coin supérieur gauche du monde.
            if (this.cameraRect.Left < this.mondeRect.Left)
            {
                this.cameraRect.Offset(this.MondeRect.Left - this.CameraRect.Left, 0);
            }

            if (this.cameraRect.Top < this.mondeRect.Top)
            {
                this.cameraRect.Offset(0, this.MondeRect.Top - this.CameraRect.Top);
            }
 
            // Restreindre en fonction du coin inférieur droit du monde.
            if (this.cameraRect.Right > this.mondeRect.Right)
            {
                this.cameraRect.Offset(this.MondeRect.Right - this.CameraRect.Right, 0);
            }

            if (this.cameraRect.Bottom > this.mondeRect.Bottom)
            {
                this.cameraRect.Offset(0, this.MondeRect.Bottom - this.CameraRect.Bottom);
            }
        }
    }
}
