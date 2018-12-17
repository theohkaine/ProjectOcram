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
    using Microsoft.Xna.Framework.Media;
    using Microsoft.Xna.Framework.Audio;
    using System.IO;

    /// <summary>
    /// Classe principale du jeu.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {


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
        /// Attribut représentant la camera.
        /// </summary>
        private Camera camera;

        //Render the resolution variable
        public static RenderTarget2D nativeRenderTarget;

        private const int ScreenSizeH = 800;

        private const int ScreenSizeW = 1280;



        private Song music;
        private SoundEffect SlimeDeath;
        private SoundEffect MiroyrDeath;
        private SoundEffect DoorOpening;
        private SoundEffect GettingHit;
        private SoundEffect CharacterDeath;

        private List<Key> keys;
        private List<BoulePiqueObstacle> boulepiques;
        private List<Boss> boss;
        private List<JumpingItem> jumpingitems;


        /// <summary>
        /// Liste des sprites que la plateforme transporte avec elle (voir Update).
        /// </summary>
        private List<Plateforme> plateformes;

        /// <summary>
        /// Liste des sprites que la plateforme transporte avec elle (voir Update).
        /// </summary>
        private List<PlateformeDescendante> plateformesD;

        private List<Door> door;


        /// <summary>
        /// Attribut représentant la liste de goblins dans le jeu.
        /// </summary>
        private List<Slime> slimes;

        /// <summary>
        /// Attribut représentant la liste de goblins dans le jeu.
        /// </summary>
        private List<Miroyr> miroyrs;

        /// <summary>
        /// Liste des sprites représentant des obus.
        /// </summary>
        private List<Obus> listeObus;

        /// <summary>
        /// Attribut indiquant l'état du jeu
        /// </summary>
        private Etats etatJeu;

        /// <summary>
        /// Etat dans lequel état le jeu avant que la dernière pause ne soit activée.
        /// </summary>
        private Etats prevEtatJeu;

        /// <summary>
        /// Attribut fournissant la police d'affichage pour les messages
        /// </summary>
        private SpriteFont policeMessages;

        /// <summary>
        /// Liste de tous les menus du jeu (chargés dans LoadContent()).
        /// </summary>
        private List<Menu> listeMenus = new List<Menu>();

        /// <summary>
        /// Menu présentement affiché.
        /// </summary>
        private Menu menuCourant = null;

        /// <summary>
        /// Police exploitée pour afficher le titre des menus.
        /// </summary>
        private SpriteFont policeMenuTitre;

        /// <summary>
        /// Police exploitée pour afficher les items de menus.
        /// </summary>
        private SpriteFont policeMenuItem;

        /// <summary>
        /// Couleur de la police exploitée pour afficher le titre des menus.
        /// </summary>
        private Color couleurMenuTitre = Color.White;

        /// <summary>
        /// Couleur de la police exploitée pour afficher les items des menus lorsqu'ils ne sont 
        /// pas actifs.
        /// </summary>
        private Color couleurMenuItem = Color.White;

        /// <summary>
        /// Couleur de la police exploitée pour afficher les items des menus lorsqu'ils sont 
        /// actifs.
        /// </summary>
        private Color couleurMenuItemSelectionne = Color.Yellow;

        private float HpHitCouldown = 0f;

        bool Manette;

        bool deadslime;
        Texture2D Menu;

        int slimeHP_1 = 5;
        int slimeHP_2 = 5;
        int slimeHP_3 = 5;
        int slimeHP_4 = 5;

        bool deletedKey;

        
        int MiniBossHP = 50;

        bool minibossdeath;

        bool instantdeath;


        //FOR SOUND EFFECT
        float volume = 0.9f;
        float pitch = 0.0f;
        float pan = 0.0f;
        //Texture2D Boss;
        //Vector2 BossPosition = new Vector2(1700, 1720);

        /// <summary>
        /// Constructeur par défaut de la classe. Cette classe est générée automatiquement
        /// par Visual Studio lors de la création du projet.
        /// </summary>
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
        /// États disponibles du jeu.
        /// </summary>
        public enum Etats
        {
            /// <summary>
            /// En cours de démarrage.
            /// </summary>
            Demarrer,

            /// <summary>
            /// En cours de jeu.
            /// </summary>
            Jouer,

            /// <summary>
            /// En cours de fin de jeu.
            /// </summary>
            Quitter,

            /// <summary>
            /// En suspension temporaire.
            /// </summary>
            Pause
        }

        /// <summary>
        /// Propriété (accesseur pour etatJeu) retournant ou changeant l'état du jeu.
        /// </summary>
        /// <value>État courant du jeu.</value>
        public Etats EtatJeu
        {
            get { return this.etatJeu; }
            set { this.etatJeu = value; }
        }

        /// <summary>
        /// Propriété activant et désactivant l'état de pause du jeu. Cette propriété doit être utilisée
        /// pour mettre le jeu en pause (plutôt que EtatJeu) car elle stocke l'état précédent (i.e. avant 
        /// la pause) du jeu afin de le restaurer lorsque la pause est terminée.
        /// </summary>
        /// <value>Le jeu est en pause ou pas.</value>
        public bool Pause
        {
            get
            {
                return this.etatJeu == Etats.Pause;
            }

            set
            {
                // S'assurer qu'il y a changement de statut de pause
                if (value && this.EtatJeu != Etats.Pause)
                {
                    // Stocker l'état courant du jeu avant d'activer la pause
                    this.prevEtatJeu = this.EtatJeu;
                    this.EtatJeu = Etats.Pause;
                }
                else if (!value && this.EtatJeu == Etats.Pause)
                {
                    // Restaurer l'état du jeu à ce qu'il était avant la pause
                    this.EtatJeu = this.prevEtatJeu;
                }

                // Suspendre les effets sonores au besoin
                this.SuspendreEffetsSonores(this.Pause);
            }
        }

        /// <summary>
        /// Propriété (accesseur pour menuCourant) retournant ou changeant le menu affiché. Lorsque
        /// sa valeur est null, aucun menu n'est affiché.
        /// </summary>
        /// <value>Menu présentement affiché.</value>
        public Menu MenuCourant
        {
            get
            {
                return this.menuCourant;
            }
            set
            {
                this.menuCourant = value;

                // Mettre le jeu en pause si un menu est affiché
                this.Pause = this.menuCourant != null;
            }
        }

        

        /// <summary>
        /// Fonction retournant le niveau de résistance aux déplacements en fonction de la couleur du pixel de tuile
        /// à la position donnée.
        /// </summary>
        /// <param name="position">Position du pixel en coordonnées du monde.</param>
        /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
        public float CalculerResistanceAuMouvement(Vector2 position)
        {
            Color pixColor = Color.White;

            foreach (Plateforme plateforme in this.plateformes)
            {
                if (position.X >= plateforme.Position.X - (plateforme.Width / 2) &&
                    position.Y >= plateforme.Position.Y - (plateforme.Height / 2) &&
                    position.X <= plateforme.Position.X + (plateforme.Width / 2) &&
                    position.Y <= plateforme.Position.Y + (plateforme.Height / 2))
                {

                    pixColor = Color.Black;
                    break;
                }


            }

            foreach (PlateformeDescendante plateforme in this.plateformesD)
            {
                if (position.X >= plateforme.Position.X - (plateforme.Width / 2) &&
                    position.Y >= plateforme.Position.Y - (plateforme.Height / 2) &&
                    position.X <= plateforme.Position.X + (plateforme.Width / 2) &&
                    position.Y <= plateforme.Position.Y + (plateforme.Height / 2))
                {

                    pixColor = Color.Black;
                    break;
                }


            }
            foreach (Door door in this.door)
            {
                if(position.X >= door.Position.X - (door.Width) &&
                    position.Y >= door.Position.Y - (door.Height-20) &&
                    position.X <= door.Position.X + (door.Width) &&
                    position.Y <= door.Position.Y + (door.Height))
                {
                    if (deletedKey == false)
                    {
                        pixColor = Color.Black;
                        break;
                    }
                }
            }

          

                // Extraire la couleur du pixel correspondant à la position.
                if (pixColor != Color.Black)
                try
                {
                    pixColor = this.monde.CouleurDeCollision(position);
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.Exit();
                }

            if (pixColor == Color.Red)
            {

                instantdeath = true;
            }

            // Déterminer le niveau de résistance en fonction de la couleur.
            if (pixColor != Color.Black)
                return 0.0f;
            else
                return 1.0f;

            
            

            
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
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (gamepadState.IsConnected)
            {
                this.Components.Add(new ManetteService(this));
                Manette = true;
            }
            else
            {
                this.Components.Add(new ClavierService(this));
            }

            // 1280 450
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, this.graphics.GraphicsDevice.Viewport.Width, 450);

            // Initialiser la vue de la caméra à la taille de l'écran.
            this.camera = new Camera(new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, 450));

            

            // Créer les attributs de gestion des obus.
            this.listeObus = new List<Obus>();

            // Le jeu est en cours de démarrage. Notez qu'on évite d'exploiter la prorpiété EtatJeu
            // car le setter de cette dernière manipule des effets sonores qui ne sont pas encore
            // chargées par LoadContent()
            this.etatJeu = Etats.Demarrer;

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
            // Charger le sprite de personnages du joueur (statique).
            JoueurSprite.LoadContent(this.Content, this.graphics);
            JoueurObus.LoadContent(this.Content, this.graphics);

            // Au départ, le monde de jour est exploité.
            this.monde = new MondeOcram();
            this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur + (ScreenSizeW / 3), this.monde.Hauteur + (116));

            foreach (Obus listeObus in this.listeObus)
            {
                listeObus.ObusCollision = new Rectangle((int)listeObus.Position.X - (listeObus.Width / 2), (int)listeObus.Position.Y - (listeObus.Height / 2), listeObus.Width, listeObus.Height);
                //slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

                // slimes.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }

            //1746, 1280

            // Créer et initialiser le sprite du joueur.



            this.joueur = new JoueurSprite(650, 1070);

            joueur.PlayerCollision = new Rectangle((int)joueur.Position.X - (joueur.Width / 2), (int)joueur.Position.Y - (joueur.Height / 2), joueur.Width, joueur.Height);

            this.joueur.BoundsRect = new Rectangle(0, 0, this.monde.Largeur , this.monde.Hauteur);
            



            // Imposer la palette de collisions au déplacement du joueur.
            this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;

            // Charger la musique de fond du jeu.
            this.music = Content.Load<Song>(@"Music\MonogameFinalSong");
            this.SlimeDeath = Content.Load<SoundEffect>(@"SoundFX\SimeDeath");
            this.MiroyrDeath = Content.Load<SoundEffect>(@"SoundFX\MiryorDeath");
            this.DoorOpening = Content.Load<SoundEffect>(@"SoundFX\DoorOpening");
            this.GettingHit = Content.Load<SoundEffect>(@"SoundFX\MainCharacterHurt");
            this.CharacterDeath = Content.Load<SoundEffect>(@"SoundFX\MainCharacterDeath");

            // Paramétrer la musique de fond et la démarrer.
            MediaPlayer.Volume = 0.3f;         // valeur entre 0.0 et 1.0
            MediaPlayer.IsRepeating = true;    // jouer en boucle

            MediaPlayer.Play(this.music);

            // Associer la déléguée de gestion des obus du vaisseau à son sprite.
            this.joueur.GetLancerObus = this.LancerObus;

            Door.LoadContent(Content, this.graphics);
            this.door = new List<Door>();
            this.door.Add(new Door(1747, 1365));

            Key.LoadContent(Content, this.graphics);
            this.keys = new List<Key>();
            this.keys.Add(new Key(33, 1330));

            JumpingItem.LoadContent(Content, this.graphics);
            this.jumpingitems = new List<JumpingItem>();
            this.jumpingitems.Add(new JumpingItem(740, 1097));
            this.jumpingitems.Add(new JumpingItem(837, 1145));
            this.jumpingitems.Add(new JumpingItem(1270, 1145));

            Boss.LoadContent(Content, this.graphics);
            this.boss = new List<Boss>();
            this.boss.Add(new Boss(1757, 1805));

            // Créer les plateformes.
            Plateforme.LoadContent(Content, this.graphics);
            this.plateformes = new List<Plateforme>();
            this.plateformes.Add(new Plateforme(1835, 1565));
            //this.plateformes.Add(new Plateforme(200, 76));

            // Créer les plateformes.
            PlateformeDescendante.LoadContent(Content, this.graphics);
            this.plateformesD = new List<PlateformeDescendante>();
            this.plateformesD.Add(new PlateformeDescendante(1635, 1565));
            //this.plateformes.Add(new Plateforme(200, 76)); 

            //Créer les BoulePiques
            BoulePiqueObstacle.LoadContent(Content, this.graphics);
            this.boulepiques = new List<BoulePiqueObstacle>();
            this.boulepiques.Add(new BoulePiqueObstacle(1520, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(1020, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(800, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(600, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(200, 1535));
            // Charger le sprite représentant des ogres.
            Slime.LoadContent(this.Content, this.graphics);

            // Créer les slimes.
            this.slimes = new List<Slime>();
            this.slimes.Add(new Slime(350, 77));
            this.slimes.Add(new Slime(900, 77));
            this.slimes.Add(new Slime(1500, 77));

            
            //this.slimes.Add(new Slime(175, 605));
            this.slimes.Add(new Slime(1200, 605));

            //this.slimes.Add(new Slime(1700, 1920));
            //this.slimes.Add(new Slime(1400, 1920));
            //this.slimes.Add(new Slime(1300, 1920));


            Menu = this.Content.Load<Texture2D>(@"MenuImage\menuV2");

           

            // Configurer les ogres de sorte qu'ils ne puissent se déplacer
            // hors de la mappe monde et initialiser la détection de collision de tuiles.
            foreach (Slime slimes in this.slimes)
                {
                slimes.SlimeCollision = new Rectangle((int)slimes.Position.X-(slimes.Width/2), (int)slimes.Position.Y-(slimes.Height/2), slimes.Width,slimes.Height);
                slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

               // slimes.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }


            // Charger le sprite représentant des ogres.
            Miroyr.LoadContent(this.Content, this.graphics);

            // Créer les ogres.
            this.miroyrs = new List<Miroyr>();
            this.miroyrs.Add(new Miroyr(600, 1120));



            // Configurer les ogres de sorte qu'ils ne puissent se déplacer
            // hors de la mappe monde et initialiser la détection de collision de tuiles.
            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.MiroyrCollision  = new Rectangle((int)miroyr.Position.X-(miroyr.Width/2), (int)miroyr.Position.Y-(miroyr.Height/2), miroyr.Width, miroyr.Height);
                miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
              // miroyr.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }

            // Charger les polices
            this.policeMessages = Content.Load<SpriteFont>(@"Polices\MessagesPolice");
            this.policeMenuTitre = Content.Load<SpriteFont>(@"Polices\MenuTitresPolice");
            this.policeMenuItem = Content.Load<SpriteFont>(@"Polices\MenuItemsPolice");

            // Charger tous les menus disponibles et les stocker dans la liste des menus.
            // Obtenir d'abord une liste des fichiers XML de définition de menu.
            string[] fichiersDeMenu = Directory.GetFiles(Content.RootDirectory + @"\Menus\");



            // Itérer pour chaque fichier XML trouvé.
            foreach (string nomFichier in fichiersDeMenu)
            {
                // Créer un nouveau menu.
                Menu menu = new Menu();

                // Configurer le nouveau menu à partir de son fichier XML.
                menu.Load(nomFichier);

                // Assigner la fonction déléguée de Game au nouveau menu (pour gestion des
                // sélections de l'usager lors de l'affichage du menu).
                menu.SelectionItemMenu = this.SelectionItemMenu;

                // Ajouter le nouveau menu à la liste des menus du jeu.
                this.listeMenus.Add(menu);
            }
        }

        /// <summary>
        /// Fonction déléguée fournie à tous les menus du jeu pour traiter les sélections 
        /// de l'usager.
        /// </summary>
        /// <param name="nomMenu">Nom du menu d'où provient la sélection.</param>
        /// <param name="item">Item de menu sélectionné.</param>
        protected void SelectionItemMenu(string nomMenu, ItemDeMenu item)
        {
            // Est-ce le menu pour quitter le jeu?
            if (nomMenu == "QuitterMenu")
            {


                // Deux sélections possibles : Oui ou Non
                switch (item.Nom)
                {
                    case "Oui":         // L'usager veut quitter le jeu
                        this.Exit();
                        break;

                    case "Non":         // L'usager ne veut pas quitter le jeu
                        this.MenuCourant = null;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Trouve le menu dont le nom est fourni dans la liste des menus gérée par le jeu.
        /// </summary>
        /// <param name="nomMenu">Nom du menu d'où provient la sélection.</param>
        /// <returns>Menu recherché; null si non trouvé.</returns>
        protected Menu TrouverMenu(string nomMenu)
        {
            // Itérer parmi la liste des menus disponibles
            foreach (Menu menu in this.listeMenus)
            {
                // Si le menu recherché est trouvé, le retourner
                if (menu.Nom == nomMenu)
                {
                    return menu;
                }
            }

            return null;    // aucun menu correspondant au fourni
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
            // Si le jeu est en cours de démarrage, passer à l'état de jouer
            if (this.EtatJeu == Etats.Demarrer)
            {
                this.EtatJeu = Etats.Jouer;
            }

            // Un menu est-il affiché?
            if (this.MenuCourant != null)
            {
                // Oui, alors gérer les inputs pour ce menu.
                this.MenuCourant.GetInput(gameTime);

                // Lorsqu'un menu est affiché, le jeu est en pause alors il n'y a rien d'autre.
                // à faire dans Update()
                base.Update(gameTime);
                return;
            }

            // Permettre de quitter le jeu via la manette.
            if (ServiceHelper.Get<IInputService>().Quitter(1))
            {

                this.MenuCourant = this.TrouverMenu("QuitterMenu");
            }

            // Est-ce que le bouton de pause a été pressé?
            if (ServiceHelper.Get<IInputService>().Pause(1))
            {
                this.Pause = !this.Pause;
            }

            // Si le jeu est en pause, interrompre la mise à jour
            if (this.Pause)
            {
                base.Update(gameTime);
                return;
            }



            // Mettre à jour le sprite du joueur puis centrer la camera sur celui-ci.
            this.joueur.Update(gameTime, this.graphics);

            // for(int i = 0; i<deathLaser.Count; i++)
            // {
            //  this.deathLaser[i].Update(gameTime, this.graphics);


            //}


            
            foreach (Boss boss in this.boss)
            {
                boss.Update(gameTime, this.graphics);
            }

            this.UpdateObus(gameTime);


            //UpdateCollisionObus(gameTime);

            // Est-on en processus de fin de jeu dû à une collision du vaisseau avec un astéroïde?
            if (this.EtatJeu == Etats.Quitter)
            {
                this.Exit();
            }

            // Recentrer la caméra sur le sprite du joueur.
            this.camera.Centrer(this.joueur.Position);

            // Mettre à jour les Slimes.
            foreach (Slime slime in this.slimes)
            {
                slime.Update(gameTime, this.graphics);
            }

            if (minibossdeath == false)
            {
                // Mettre à jour le Miroyr.
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    miroyr.Update(gameTime, this.graphics);
                }
            }
             
            foreach(Key key in this.keys)
            {
                key.Update(gameTime, this.graphics);
            }

            foreach (JumpingItem jumpingitems in this.jumpingitems)
            {
                jumpingitems.Update(gameTime, this.graphics);
            }

            foreach (Door door in this.door)
            {
                door.Update(gameTime, this.graphics);
               
            }

            foreach (BoulePiqueObstacle boulepique in this.boulepiques)
            {
                boulepique.Update(gameTime, this.graphics);
            }

            //collisionEntre la cle et le Sprite
            UpdateCollisionKeyJoueur(gameTime);
            UpdateCollisionJoueurMonster(gameTime);
            UpdateCollisionJumpingItemJoueur(gameTime);

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
                    plateforme.RetirerPassager(this.joueur);
                
            }

            // Mettre à jour les plateformes et déterminer si le sprite du jour est sur une
            // plateforme, et si c'est le cas, alors indiquer à celle-ci qu'elle transporte 
            // ce sprite.
            foreach (PlateformeDescendante plateforme in this.plateformesD)
            {
                plateforme.Update(gameTime, this.graphics);  // mettre à jour la position

                // Activer/désactiver la composition selon la plateforme et la position du joueur.
                if (this.joueur.SurPlateforme(plateforme))
                {
                    plateforme.AjouterPassager(this.joueur);
                }
                else
                    plateforme.RetirerPassager(this.joueur);

            }


            joueur.PlayerCollision = new Rectangle((int)joueur.Position.X - (joueur.Width / 2), (int)joueur.Position.Y - (joueur.Height / 2), joueur.Width, joueur.Height);

            foreach (Slime slimes in this.slimes)
            {
                slimes.SlimeCollision = new Rectangle((int)slimes.Position.X - (slimes.Width / 2), (int)slimes.Position.Y - (slimes.Height / 2), slimes.Width, slimes.Height);
                slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

                // slimes.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }

            

            // Configurer les ogres de sorte qu'ils ne puissent se déplacer
            // hors de la mappe monde et initialiser la détection de collision de tuiles.
            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.MiroyrCollision = new Rectangle((int)miroyr.Position.X - (miroyr.Width / 2), (int)miroyr.Position.Y - (miroyr.Height / 2), miroyr.Width, miroyr.Height);
                miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                // miroyr.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }

            foreach (Obus listeObus in this.listeObus)
            {
                listeObus.ObusCollision = new Rectangle((int)listeObus.Position.X - (listeObus.Width / 2), (int)listeObus.Position.Y - (listeObus.Height / 2), listeObus.Width, listeObus.Height);
                //slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

                // slimes.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
            }


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

           


            if (this.EtatJeu != Etats.Quitter)
            {
                this.joueur.Draw(this.camera, this.spriteBatch);   // afficher le sprite du joueur
            }

            foreach (Obus obus in this.listeObus)
            {

                obus.Draw(this.camera, this.spriteBatch);

            }

            foreach (JumpingItem jumpingitems in this.jumpingitems)
            {
                jumpingitems.Draw(this.camera, this.spriteBatch);
            }

            if (deletedKey == false)
            {
                foreach (Key key in this.keys)
                {
                    key.Draw(this.camera, this.spriteBatch);
                }
            }

            if (deletedKey == false)
            {
                foreach (Door door in this.door)
                {
                    door.Draw(this.camera, this.spriteBatch);
                }
            }

            foreach (BoulePiqueObstacle boulepique in this.boulepiques)
            {
                boulepique.Draw(this.camera, this.spriteBatch);
            }

            // Afficher les plateformes.
            foreach (Plateforme plateforme in this.plateformes)
            {
                plateforme.Draw(this.camera, this.spriteBatch);
            }

            // Afficher les plateformes.
            foreach (PlateformeDescendante plateforme in this.plateformesD)
            {
                plateforme.Draw(this.camera, this.spriteBatch);
            }

            this.joueur.Draw(this.camera, this.spriteBatch);   // afficher le sprite du joueur


            foreach (Boss boss in this.boss)
            {
                boss.Draw(this.camera, this.spriteBatch);
            }
           

            if (deadslime == false)
            {
                // Afficher les  slimes
                foreach (Slime slime in this.slimes)
                {
                    slime.Draw(this.camera, this.spriteBatch);
                }
            }
            


            if(minibossdeath==false){
                // Afficher le Miroyr
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    miroyr.Draw(this.camera, this.spriteBatch);
                }
            }
            

            

            // Afficher les messages selon l'état du jeu
            this.DrawMessages(this.spriteBatch);

           

            // Afficher le menu courant s'il y en a un sélectionné
            if (this.MenuCourant != null)
            {
                spriteBatch.Draw(Menu, new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height), Color.White);
                // Dessiner le menu
                this.MenuCourant.Draw(
                    this.spriteBatch,
                    this.policeMenuTitre,
                    this.policeMenuItem,
                    this.couleurMenuTitre,
                    this.couleurMenuItem,
                    this.couleurMenuItemSelectionne);
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
        /// Suspend temporairement (pause) ou réactive les effets sonores du jeu.
        /// </summary>
        /// <param name="suspendre">Indique si les effets sonores doivent être suspendus ou réactivés.</param>
        protected void SuspendreEffetsSonores(bool suspendre)
        {
            // Suspendre au besoin les effets sonores du vaisseau
            this.joueur.SuspendreEffetsSonores(suspendre);

            // Suspendre ou réactiver le bruitage de fond
            if (suspendre)
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Pause();
                }
            }
            else
            {
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
            }
        }

        /// <summary>
        /// Routine d'affichage de message (centré à l'écran) correspondant à l'état courant du jeu.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        protected void DrawMessages(SpriteBatch spriteBatch)
        {
            string output = string.Empty;      // message à afficher

            // Déterminer le message à afficher selon l'état du jeu
            switch (this.EtatJeu)
            {
                case Etats.Pause:
                    if (this.MenuCourant == null)
                    {
                        if (Manette)
                        {
                            output = "Pause (Pressez Start pour continuer...)";
                        }
                        else
                        {
                            output = "Pause (Pressez P pour continuer...)";
                        }
                            
                    }

                    break;

                default:
                    output = string.Empty;
                    break;
            }

            // Afficher le message s'il y a lieu
            if (output.Length > 0)
            {
                // L'origine d'affichage du message est son point central
                Vector2 centrePolice = this.policeMessages.MeasureString(output) / 4;

                // L'origine du message sera positionnée au centre de l'écran
                Vector2 centreEcran = new Vector2(
                    this.graphics.GraphicsDevice.Viewport.Width / 4,
                    this.graphics.GraphicsDevice.Viewport.Height / 4);

                Vector2 centreEcranImage = new Vector2(0,0);

                spriteBatch.Draw(Menu, centreEcranImage, Color.White);
                // Afficher le message centré à l'écran
                spriteBatch.DrawString(
                    this.policeMessages,        // police d'affichge
                    output,                     // message à afficher
                    centreEcran,                // position où afficher le message
                    Color.White,               // couleur du texte
                    0,                          // angle de rotation
                    centrePolice,               // origine du texte (centrePolice positionné à centreEcran)
                    1.0f,                       // échelle d'affichage
                    SpriteEffects.None,         // effets
                    1.0f);                      // profondeur de couche (layer depth)
            }
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
                    obus.Position.Y - obus.Height > this.monde.Hauteur )
                {
                    obusFini.Add(obus);
                }
            }

            foreach (Obus obus in this.listeObus)
            {
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    if (miroyr.MiroyrCollision.Contains(obus.ObusCollision))
                    {

                        MiniBossHP--;
                        obusFini.Add(obus);

                        if (MiniBossHP == 0)
                        {
                            MiroyrDeath.Play(volume, pan, pitch);
                            minibossdeath = true;
                        }

                    }
                }
            }
            
            foreach (Obus obus in this.listeObus)
            {
                
                if (slimes[0].SlimeCollision.Contains(obus.ObusCollision))
                {
                    slimeHP_1--;
                    obusFini.Add(obus);
                    if(slimeHP_1==0){
                        SlimeDeath.Play(volume, pan, pitch);
                        this.slimes[0].Position = new Vector2(9999, 99999);
                    }
                   
                }
                if (slimes[1].SlimeCollision.Contains(obus.ObusCollision))
                {
                    slimeHP_2--;
                    obusFini.Add(obus);

                    if (slimeHP_2 == 0){
                        SlimeDeath.Play(volume, pan, pitch);
                        this.slimes[1].Position = new Vector2(9999, 99999);
                    }
                }

               if (slimes[2].SlimeCollision.Contains(obus.ObusCollision))
                {
                    slimeHP_3--;
                    obusFini.Add(obus);

                    if (slimeHP_3 == 0){
                        SlimeDeath.Play(volume, pan, pitch);
                        this.slimes[2].Position = new Vector2(9999, 99999);
                    }
                }
                if (slimes[3].SlimeCollision.Contains(obus.ObusCollision))
                {
                    slimeHP_4--;
                    obusFini.Add(obus);

                    if (slimeHP_4 == 0)
                    {
                        SlimeDeath.Play(volume, pan, pitch);
                        this.slimes[3].Position = new Vector2(9999, 99999);
                        deadslime = true;
                    }
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

        
    protected void UpdateCollisionKeyJoueur(GameTime gameTime)
        {
            
            for (int i = 0; i < keys.Count; i++)
            {
                
                //Vector2 tempPositionSlime = this.slimes[i].Position;
                if (keys[i].Collision(joueur))
                {

                    deletedKey = true;

                    DoorOpening.Play(volume, pan, pitch);
                }
            }
        }

        protected void UpdateCollisionJumpingItemJoueur(GameTime gameTime)
        {

            for (int i = 0; i < jumpingitems.Count; i++)
            {

                //Vector2 tempPositionSlime = this.slimes[i].Position;
                if (jumpingitems[0].Collision(joueur))
                {

                    this.joueur.VitesseVerticale -= 0.39f;
                }
                else if (jumpingitems[1].Collision(joueur))
                {
                    this.joueur.VitesseVerticale -= 0.45f;


                    /////Make falling more slower if the character vertical speed goes too high
                    if (this.joueur.VitesseVerticale > 0.4f)
                    {
                        this.joueur.VitesseVerticale = 0.35f;
                    }
                }
            }
        }

        protected void UpdateCollisionJoueurMonster(GameTime gameTime)
        {  
            for (int i = 0; i < slimes.Count; i++)
            {
                HpHitCouldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (HpHitCouldown > 3f)
                {
                    //Vector2 tempPositionSlime = this.slimes[i].Position;
                    if (slimes[i].SlimeCollision.Contains(joueur.PlayerCollision))
                    {
                        this.joueur.PlayerHPP -= 1;
                        this.GettingHit.Play(this.volume, this.pan, this.pitch);
                        HpHitCouldown = 0f;
                    }
                }
            }
            
            this.Reset();
        }

        private void Reset()
        {
            if (this.joueur.PlayerHPP == 0 || instantdeath==true)
            {
                this.CharacterDeath.Play(this.volume, this.pan, this.pitch);
                instantdeath = false;
                this.joueur.PlayerHPP = 6;
                deadslime = false;
                slimeHP_1 = 5;
                slimeHP_2 = 5;
                slimeHP_3 = 5;
                slimeHP_4 = 5;
                MiniBossHP = 50;
                deletedKey = false;

                // Au départ, le monde de jour est exploité.
                this.monde = new MondeOcram();
                this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur + (ScreenSizeW / 3), this.monde.Hauteur + (116));

                foreach (Obus listeObus in this.listeObus)
                {
                    listeObus.ObusCollision = new Rectangle((int)listeObus.Position.X - (listeObus.Width / 2), (int)listeObus.Position.Y - (listeObus.Height / 2), listeObus.Width, listeObus.Height);
                    //slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

                    // slimes.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
                }
                
                //1746, 1280

                // Créer et initialiser le sprite du joueur.
                this.joueur = new JoueurSprite(60,60);
                joueur.PlayerCollision = new Rectangle((int)joueur.Position.X - (joueur.Width / 2), (int)joueur.Position.Y - (joueur.Height / 2), joueur.Width, joueur.Height);
                this.joueur.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);




                // Imposer la palette de collisions au déplacement du joueur.
                this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
                this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;

                // Associer la déléguée de gestion des obus du vaisseau à son sprite.
                this.joueur.GetLancerObus = this.LancerObus;

               
                this.door = new List<Door>();
                this.door.Add(new Door(1847, 1365));

              
                this.keys = new List<Key>();
                this.keys.Add(new Key(33, 1330));

                this.jumpingitems = new List<JumpingItem>();
                this.jumpingitems.Add(new JumpingItem(100, 0));

                // Créer les plateformes.

                this.plateformes = new List<Plateforme>();
                this.plateformes.Add(new Plateforme(1835, 1575));
                //this.plateformes.Add(new Plateforme(200, 76));

                // Charger le sprite représentant des ogres.
               

                // Créer les slimes.
                this.slimes = new List<Slime>();
                this.slimes.Add(new Slime(350, 77));
                this.slimes.Add(new Slime(900, 77));
                this.slimes.Add(new Slime(1500, 77));


                //this.slimes.Add(new Slime(175, 605));
                this.slimes.Add(new Slime(1200, 605));

                // Charger le sprite représentant des ogres.
                Miroyr.LoadContent(this.Content, this.graphics);

                // Créer les ogres.
                this.miroyrs = new List<Miroyr>();
                this.miroyrs.Add(new Miroyr(600, 1120));



                // Configurer les ogres de sorte qu'ils ne puissent se déplacer
                // hors de la mappe monde et initialiser la détection de collision de tuiles.
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    miroyr.MiroyrCollision = new Rectangle((int)miroyr.Position.X - (miroyr.Width / 2), (int)miroyr.Position.Y - (miroyr.Height / 2), miroyr.Width, miroyr.Height);
                    miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                    // miroyr.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
                }

            }
        }
    }
}
