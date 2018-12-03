//-----------------------------------------------------------------------
// <copyright file="Game.cs" company="Marco Lavoie">
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

    using IFM20884;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Classe principale du jeu.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Attribut indiquant l'état du jeu
        /// </summary>
        private Etats etatJeu;

        /// <summary>
        /// Attribut permettant d'obtenir des infos sur la carte graphique et l'écran.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Attribut gérant l'affichage en batch à l'écran.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Attribut représentant le monde à afficher durant le jeu.
        /// </summary>
        private Monde monde;

        /// <summary>
        /// Attribut représentant le personnage contrôlé par le joueur.
        /// </summary>
        private JoueurSprite joueur;


        /// <summary>
        /// Attribut représentant la liste d'ennemis dans le jeu.
        /// </summary>
        private List<Sprite> listeEnnemis;

        /// <summary>
        /// Attribut représentant la camera.
        /// </summary>
        private Camera camera;

        //Render the resolution variable
        public static RenderTarget2D nativeRenderTarget;

        private const int ScreenSizeH = 800;

        private const int ScreenSizeW = 1280;

        /// <summary>
        /// Liste des sprites représentant des obus.
        /// </summary>
        private List<Obus> listeObus;

        /// <summary>
        /// Liste des sprites que la plateforme transporte avec elle (voir Update).
        /// </summary>
        private List<Plateforme> plateformes;

        /// <summary>
        /// États disponibles du personnage.
        /// </summary>
        public enum Etats
        {
            /// <summary>
            /// En cours de démarrage.
            /// </summary>
            Demarrer,

        }

        /// <summary>
        /// Attribut représentant la liste de goblins dans le jeu.
        /// </summary>
        private List<Slime> slimes;

        /// <summary>
        /// Attribut représentant la liste de goblins dans le jeu.
        /// </summary>
        private List<Miroyr> miroyrs;

        public Game()
        {


            this.graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            //Resize the screen resolution
            graphics.PreferredBackBufferWidth = ScreenSizeW;
            graphics.PreferredBackBufferHeight = ScreenSizeH;
            //Code to make it fullscreen
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;

        }

        /// <summary>
        /// Fonction retournant le niveau de résistance aux déplacements en fonction de la couleur du pixel de tuile
        /// à la position donnée.
        /// </summary>
        /// <param name="position">Position du pixel en coordonnées du monde.</param>
        /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
        public float CalculerResistanceAuMouvement(Vector2 position)
        {
            Color pixColor = Color.Black;

            // Vérifier si la position donnée est dans une plateforme.
            foreach (Plateforme plateforme in this.plateformes)
            {
                if (position.X >= plateforme.Position.X - (plateforme.Width/2 ) &&
                    position.Y >= plateforme.Position.Y - (plateforme.Height/2) &&
                    position.X <= plateforme.Position.X + (plateforme.Width /2) &&
                    position.Y <= plateforme.Position.Y + (plateforme.Height /2))
                {
                    pixColor = Color.Black;
                    break;
                }
            }

            // Extraire la couleur du pixel correspondant à la position donnée.
            try
            {
             pixColor = this.monde.CouleurDeCollision(position);
            }            
            catch(System.IndexOutOfRangeException)
            {
                this.Exit();
            }
            // Déterminer le niveau de résistance en fonction de la couleur
            if (pixColor == Color.Black)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Définition de fonction déléguée permettant de valider un déplacement d'une position
        /// à une autre dans le monde. La fonction retourne le point le plus près de 
        /// (posSource.X+deltaX, posSource.Y+DeltaY) jusqu'où le personnage peut se rendre horizontalement 
        /// et verticalement sans rencontrer de résistance plus élévée que la limite donnée.
        /// </summary>
        /// <param name="posSource">Position du pixel de départ du déplacement, en coordonnées du monde.</param>
        /// <param name="deltaX">Déplacement total horizontal, en coordonnées du monde.</param>
        /// <param name="deltaY">Déplacement total vertical, en coordonnées du monde.</param>
        /// <param name="resistanceMax">Résistance maximale tolérée lors du déplacement.</param>
        public void SpriteValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax)
        {
            Vector2 dest = new Vector2(posSource.X, posSource.Y);

            // Premièrement considérer le déplacement horizontal. Incrémenter la distance horizontale
            // de déplacement jusqu'à deltaX ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.X != posSource.X + deltaX)
            {
                dest.X += Math.Sign(deltaX);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.X -= Math.Sign(deltaX);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            // Maintenant considérer le déplacement vertical. Incrémenter la distance verticale
            // de déplacement jusqu'à deltaY ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.Y != posSource.Y + deltaY)
            {
                dest.Y += Math.Sign(deltaY);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.Y -= Math.Sign(deltaY);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            // Déterminer le déplacement maximal dans les deux directions
            deltaX = (int)(dest.X - posSource.X);
            deltaY = (int)(dest.Y - posSource.Y);
        }

        /// <summary>
        /// Permet au jeu d'effectuer toute initialisation avant de commencer à jouer.
        /// Cette fonction membre peut demander les services requis et charger tout contenu
        /// non graphique pertinent. L'invocation de base.Initialize() itèrera parmi les
        /// composants afin de les initialiser individuellement.
        /// </summary>
        protected override void Initialize()
        {
            // Activer le service de gestion du clavier
            ServiceHelper.Game = this;
            this.Components.Add(new ClavierService(this));

            // 1280 450
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, this.graphics.GraphicsDevice.Viewport.Width, 450);


            //this.monde = new MondeOcram();   // créer le monde

            

            // Initialiser la vue de la caméra à la taille de l'écran.
            this.camera = new Camera(new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, 450));

            // Créer les attributs de gestion des obus.
            this.listeObus = new List<Obus>();

            //this.listeEnnemis = new List<Sprite>();
           /* ////Ajouter les enemis dans la liste avec leurs position.
            this.listeEnnemis.Add(new Ennemi(800, 75));
            this.listeEnnemis.Add(new Ennemi(200, 600));
            this.listeEnnemis.Add(new Ennemi(200, 1000));
            */
            //this.etatJeu = Etats.Demarrer;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent est invoquée une seule fois par partie et permet de
        /// charger tous vos composants.
        /// </summary>
        protected override void LoadContent()
        {
            // Créer un nouveau SpriteBatch, utilisée pour dessiner les textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);


            // Charger le monde.
            MondeOcram.LoadContent(this.Content);

            // Au départ, le monde de jour est exploité.
            this.monde = new MondeOcram();


            // Charger le sprite de personnages du joueur (statique).
            JoueurSprite.LoadContent(this.Content, this.graphics);


            this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur + (ScreenSizeW/3), this.monde.Hauteur);

            JoueurObus.LoadContent(this.Content, this.graphics);

               /*  if (this.listeEnnemis != null)
                    {
                    foreach (Ennemi ennemi in this.listeEnnemis)
                    {
                        Ennemi.LoadContent(this.Content, this.graphics);
                        this.joueur = new JoueurSprite(0, 0);
                    }
                }*/

            // Créer et initialiser le sprite du joueur.
            this.joueur = new JoueurSprite(0, 0);
            this.joueur.BoundsRect = new Rectangle(0, 0, this.monde.Largeur , this.monde.Hauteur);

            // Créer les plateformes.
            Plateforme.LoadContent(Content, this.graphics);
            this.plateformes = new List<Plateforme>();
            this.plateformes.Add(new Plateforme(200, 70));


            // Imposer la palette de collisions au déplacement du joueur.
            this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;

            // Associer la déléguée de gestion des obus du vaisseau à son sprite.
            this.joueur.GetLancerObus = this.LancerObus;

            // Charger le sprite représentant des ogres.
            Slime.LoadContent(this.Content, this.graphics);

            // Créer les slimes.
            this.slimes = new List<Slime>();
            this.slimes.Add(new Slime(900, 77));
            this.slimes.Add(new Slime(175, 605));
            this.slimes.Add(new Slime(350, 73));
            this.slimes.Add(new Slime(1500, 77));
            this.slimes.Add(new Slime(1200, 605));


            // Configurer les ogres de sorte qu'ils ne puissent se déplacer
            // hors de la mappe monde et initialiser la détection de collision de tuiles.
            foreach (Slime slimes in this.slimes)
            {
                slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                //slimes.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }

            // Charger le sprite représentant des ogres.
            Miroyr.LoadContent(this.Content, this.graphics);

            // Créer les ogres.
            this.miroyrs = new List<Miroyr>();
            this.miroyrs.Add(new Miroyr(900, 1120));
            //this.miroyrs.Add(new Miroyr(200, 77));
           
            // Configurer les ogres de sorte qu'ils ne puissent se déplacer
            // hors de la mappe monde et initialiser la détection de collision de tuiles.
            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                //miroyr.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }


        }

        /// <summary>
        /// UnloadContent est invoquée une seule fois par partie et permet de
        /// libérer vos composants.
        /// </summary>
        protected override void UnloadContent()
        {
            // À FAIRE: Libérez tout contenu de ContentManager ici
        }

        /// <summary>
        /// Permet d'implanter les comportements logiques du jeu tels que
        /// la mise à jour du monde, la détection de collisions, la lecture d'entrées
        /// et les effets audio.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Update(GameTime gameTime)
        {
            // Permettre de quitter le jeu via la manette ou le clavier.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
          

            // Mettre à jour le sprite du joueur puis centrer la camera sur celui-ci.
            this.joueur.Update(gameTime, this.graphics);

            /* foreach (Sprite ennemi in this.listeEnnemis)
             {
                 ennemi.Update(gameTime, this.graphics);
             }*/

            // Recentrer la caméra sur le sprite du joueur.
            this.camera.Centrer(this.joueur.Position);

            // Mettre à jour les Slimes.
            foreach (Slime slime in this.slimes)
            {
                slime.Update(gameTime, this.graphics);
            }

            // Mettre à jour le Miroyr.
            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.Update(gameTime, this.graphics);
            }

            // Mettre à jour les plateformes et déterminer si le sprite du jour est sur une 
            // plateforme, et si c'est le cas, alors indiquer à celle-ci qu'elle transporte 
            // ce sprite.
            foreach (Plateforme plateforme in this.plateformes)
            {
                plateforme.Update(gameTime, this.graphics);  // mettre à jour la position

                // Activer/désactiver la composition selon la plateforme et la position du joueur.
                if (this.joueur.SurPlateforme(plateforme))
                {
                    plateforme.AjouterPassager(this.joueur);
                }
                else
                {
                    plateforme.RetirerPassager(this.joueur);
                }
            }

            this.UpdateObus(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Cette fonction membre est invoquée lorsque le jeu doit mettre à jour son 
        /// affichage.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            // On débute avec un écran vierge (au cas où il y aurait des trous dans le monde de tuiles, on va les voir).
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRenderTarget(nativeRenderTarget);

            // Activer le blending alpha (pour la transparence des sprites).
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            

            this.monde.DrawArrierePlan(this.camera, this.spriteBatch);    // afficher le monde images

            // Afficher les plateformes.
            foreach (Plateforme plateforme in this.plateformes)
            {
                plateforme.Draw(this.camera, this.spriteBatch);
            }
            this.joueur.Draw(this.camera, this.spriteBatch);   // afficher le sprite du joueur
            
            // Afficher les obuss
            foreach (Obus obus in this.listeObus)
            {
                obus.Draw(this.camera, this.spriteBatch);
            }

            /*  if (this.listeEnnemis != null)
              {
                  foreach (Sprite ennemi in this.listeEnnemis)
                  {
                      ennemi.Draw(this.camera, this.spriteBatch);
                  }
              }*/
           
            // Afficher les  slimes
            foreach (Slime slime in this.slimes)
            {
                slime.Draw(this.camera, this.spriteBatch);
            }

            // Afficher le Miroyr
            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.Draw(this.camera, this.spriteBatch);
            }

            this.spriteBatch.End();

            // Resize the game to fit the monitor's resolution
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Fonction déléguée responsable de gérer les nouveaux obus lancés par le vaisseau du joueur.
        /// Nous ne faisons qu'ajouter le nouvel obus à la liste des obus à gérer.
        /// </summary>
        /// <param name="obus">Nouvel obus à gérer.</param>
        private void LancerObus(Obus obus)
        {
            // Ajouter l'obus à la liste des obus gérés par this.
            this.listeObus.Add(obus);
        }

        /// <summary>
        /// Routine mettant à jour les obus. Elle s'occupe de:
        ///   1 - Détruire les obus ayant quitté l'écran sans collision
        ///   2 - Déterminer si un des obus a frappé un sprite, et si c'est le cas
        ///       détruire les deux sprites (probablement un astéroïde)
        ///   3 - Mettre à jour la position des obus existants.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateObus(GameTime gameTime)
        {
            // Identifier les obus ayant quitté l'écran.
            List<Obus> obusFini = new List<Obus>();
            foreach (Obus obus in this.listeObus)
            {
                if (obus.Position.Y + obus.Height < 0 ||
                    obus.Position.Y - obus.Height > this.monde.Hauteur)
                {
                    obusFini.Add(obus);
                }
            }

            // Se débarasser des obus n'étant plus d'aucune utilité.
            foreach (Obus obus in obusFini)
            {
                this.listeObus.Remove(obus);
            }

            // Mettre à jour les obus existants.
            foreach (Obus obus in this.listeObus)
            {
                obus.Update(gameTime, this.graphics);
            }
        }

    }
}
