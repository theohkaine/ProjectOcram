//-----------------------------------------------------------------------
// <copyright file="SpriteAnimation.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Classe implantant un sprite animée à partir de textures contenues dans une palette de tuile.
    /// La classe PaletteTuiles est exploitée pour gérer les textures servant à l'animation.
    /// </summary>
    public abstract class SpriteAnimation : Sprite
    {
        /// <summary>
        /// Facteur de vitesse de défilement des tuiles d'animation.
        /// </summary>
        private float vitesseAnimation = 0.2f;

        /// <summary>
        /// Attribut et ses accesseurs indiquant la tuile courante affichée lors de l'animation. À noter que
        /// l'index de tuile courante est un flottant car la fonction Update() l'incrémente de façon 
        /// fractionnelle selon le temps de jeu. La propriété moule cette valeur sous forme d'entier afin
        /// que Draw() puisse y référer comme tel.
        /// </summary>
        private float indexTuile = 0.0f;      // index de tuile courante à afficher

        /// <summary>
        /// Constructuer paramétré recevant la position du sprite. 
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public SpriteAnimation(float x, float y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Constructuer paramétré recevant la position du sprite. L'attribut destRect est initialisé.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public SpriteAnimation(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Accesseur retournant une instance de Texture2D correspondant à la tuile courante
        /// dans la palette.
        /// *** IMPORTANT: Cette fonction est TRÈS exigeante en CPU. Donc évitez de 
        /// ***            l'utiliser à mon que ce soit ABSOLUMENT requis. La classe
        /// ***            n'utilise pas cette fonction puisque Draw() délègue à la
        /// ***            palette le soin de dessiner la tuile courante.
        /// </summary>
        public override Texture2D Texture
        {
            get
            {
                // Créer une nouvelle instance de Texture2D aux dimensions d'une tuile de la palette. Le
                // GraphicDevice de la palette est exploité pour que la tuile ait les mêmes attributs
                // graphiques que ceux de la palette.
                Texture2D tuile = new Texture2D(this.PaletteAnimation.Tuiles.GraphicsDevice, this.PaletteAnimation.LargeurTuile, this.PaletteAnimation.HauteurTuile);

                // Extraire les pixels de la palette.
                Color[] donneesPalette = new Color[this.PaletteAnimation.Tuiles.Width * this.PaletteAnimation.Tuiles.Height];
                this.PaletteAnimation.Tuiles.GetData<Color>(donneesPalette);

                // Créer un bloc pour stocker les pixels de la tuile.
                int numPixels = tuile.Width * tuile.Height;     // nombre de pixels dans la tuile
                Color[] donneesTuile = new Color[numPixels];    // les données de la tuile

                // Obtenir les coordonnées de la tuile dans sa palette.
                Rectangle tuileRect = this.PaletteAnimation.SourceRect(this.IndexTuile);

                // Récupérer de la palette les données pour la tuile courante, un pixel à la fois .
                for (int py = 0; py < tuile.Height; py++)
                {
                    for (int px = 0; px < tuile.Width; px++)
                    {
                        donneesTuile[px + (py * tuile.Width)] = donneesPalette[(tuileRect.X + px) + ((tuileRect.Y + py) * this.PaletteAnimation.Tuiles.Width)];
                    }
                }

                // Stocker les données de pixels dans la tuile, puis la retourner.
                tuile.SetData<Color>(donneesTuile);
                return tuile;
            }
        }

        /// <summary>
        /// Retourne la largeur du sprite. Puisque l'attribut texture héritée de Sprite n'est pas exploitée
        /// dans SpriteAnimation, l'accesseur Width doit être surchargé afin de retourné la largeur des
        /// tuiles de palette exploitées dans l'animation.
        /// </summary>
        public override int Width
        {
            get { return this.PaletteAnimation.LargeurTuile; }
        }

        /// <summary>
        /// Retourne la hauteur du sprite. Puisque l'attribut texture héritée de Sprite n'est pas exploitée
        /// dans SpriteAnimation, l'accesseur Height doit être surchargé afin de retourné la hauteur des
        /// tuiles de palette exploitées dans l'animation.
        /// </summary>
        public override int Height
        {
            get { return this.PaletteAnimation.HauteurTuile; }
        }

        /// <summary>
        /// Accesseur pour attribut vitesseAnimation.
        /// </summary>
        public float VitesseAnimation
        {
            get { return this.vitesseAnimation; }
            set { this.vitesseAnimation = value; }
        }

        /// <summary>
        /// Accesseur de l'attribut indexTuile. On s'assure que la valeur de l'attribut est topujours
        /// valide selon la taille de la palette.
        /// </summary>
        protected int IndexTuile               // accesseur pour indexTuile, moulé en entier
        {
            get { return (int)Math.Round(this.indexTuile); }
            set { this.indexTuile = (float)MathHelper.Clamp(value, 0, this.PaletteAnimation.NombreDeTuiles - 1); }
        }

        /// <summary>
        /// Palette de textures permettant l'animation. Doit être surchargée dans les classes 
        /// dérivées afin de manipuler une instance de PaletteTuiles.
        /// </summary>
        protected abstract Palette PaletteAnimation
        {
            get;
        }

        /// <summary>
        /// Animation du sprite: l'index de tuile à afficher est mis à jour en fonction du temps.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // Calcul de la vitesse d'animation (indépendante du matériel).
            float vitesse = gameTime.ElapsedGameTime.Milliseconds * this.vitesseAnimation;

            // Animer le sprite en changeant l'index de tuile à afficher. À noter que l'accesseur
            // de indexTuile fait une incrémentation cyclique de sa valeur au besoin.
            if (this.PaletteAnimation.NombreDeTuiles != 0)
            {
                // Facteur de changement choisi en fonction du nombre de tuiles pour l'animation
                // afin de changer de tuile à chaque 1/10 de seconde.
                float indexSuivant = this.indexTuile + (vitesse * 0.01f * this.PaletteAnimation.NombreDeTuiles);
                if (Math.Round(indexSuivant) < this.PaletteAnimation.NombreDeTuiles)
                {
                    this.indexTuile = indexSuivant;
                }
                else
                {
                    this.indexTuile = 0;
                }
            }
        }

        /// <summary>
        /// Affiche à l'écran le sprite en fonction de la position de la camera. L'affichage est
        /// déléguée à palette afin d'afficher la tuile courante d'animation.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public override void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Calculer les coordonnées du sprite dans le monde.
            Rectangle destRect = new Rectangle(0, 0, this.Width, this.Height);
            destRect.X = (int)Position.X - (this.Width / 2);
            destRect.Y = (int)Position.Y - (this.Height / 2);

            // Si une caméra est fournie, on s'assure que le sprite y est visible.
            if (camera == null || camera.EstVisible(destRect))
            {
                // Décaler la destination en fonction de la caméra. Ceci correspond à transformer destRect 
                // de coordonnées logiques (i.e. du monde) à des coordonnées physiques (i.e. de l'écran).
                camera.Monde2Camera(ref destRect);

                // Afficher la tuile courante
                this.PaletteAnimation.Draw(this.IndexTuile, destRect, spriteBatch);
            }
        }
    }
}
