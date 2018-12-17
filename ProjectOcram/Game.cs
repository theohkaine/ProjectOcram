﻿//-----------------------------------------------------------------------
// <copyright file="Game.cs" company="Tristan Araujo & Dominik Desjardins">
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
    using System.IO;
    using System.Linq;
    using IFM20884;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// Classe principale du jeu.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Changer la resolution horizontale du jeu.
        /// </summary>
        private const int ScreenSizeH = 800;

        /// <summary>
        /// Changer la resolution verticale du jeu.
        /// </summary>
        private const int ScreenSizeW = 1280;

        /// <summary>
        /// Render the resolution variable
        /// </summary>
        private static RenderTarget2D nativeRenderTarget;

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

        /// <summary>
        /// Music principale du jeu.
        /// </summary>
        private Song music;

        /// <summary>
        /// SoundEffect de la mort du slime.
        /// </summary>
        private SoundEffect slimeDeath;

        /// <summary>
        /// SoundEffect de la mort du monstre miroyr.
        /// </summary>
        private SoundEffect miroyrDeath;

        /// <summary>
        /// SoundEffect d'une porte qui ouvre.
        /// </summary>
        private SoundEffect doorOpening;

        /// <summary>
        /// SoundEffect quand le personnage recoit une attack d'un monstre.
        /// </summary>
        private SoundEffect gettingHit;

        /// <summary>
        /// SoundEffect de la mort du personnage.
        /// </summary>
        private SoundEffect characterDeath;

        /// <summary>
        /// Liste principale des sprites pour la classe Key.
        /// </summary>
        private List<Key> keys;

        /// <summary>
        /// Liste principale des sprites pour la classe BoulePiqueObstacle. 
        /// </summary>
        private List<BoulePiqueObstacle> boulepiques;

        /// <summary>
        /// Liste principale des sprites pour la classe Boss. 
        /// </summary>
        private List<Boss> boss;

        /// <summary>
        /// Liste principale des sprites pour la classe JumpingItem. 
        /// </summary>
        private List<JumpingItem> jumpingitems;

        /// <summary>
        /// Liste des sprites que la plateforme transporte avec elle (voir Update).
        /// </summary>
        private List<Plateforme> plateformes;

        /// <summary>
        /// Liste des sprites que la plateforme transporte avec elle (voir Update).
        /// </summary>
        private List<PlateformeDescendante> plateformesD;

        /// <summary>
        /// Liste principale des sprites pour la classe Door. 
        /// </summary>
        private List<Door> door;

        /// <summary>
        /// Liste principale des sprites pour la classe WallForMiniboss. 
        /// </summary>
        private List<WallForMiniboss> minibossWall;

        /// <summary>
        /// Liste principale des sprites pour la classe Slime. 
        /// </summary>
        private List<Slime> slimes;

        /// <summary>
        /// Liste principale des sprites pour la classe Miroyr. 
        /// </summary>
        private List<Miroyr> miroyrs;

        /// <summary>
        /// Liste principale des sprites pour la classe Obus. 
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

        /// <summary>
        /// Permet d'avoir un delai entre des coups d'attaque par les monstres envers
        /// le personnage.
        /// </summary>
        private float hithpCoolDown = 0f;

        /// <summary>
        /// Permet de savoir si une manette est connecté.
        /// </summary>
        private bool manette;

        /// <summary>
        /// Permet de savoir si le slime est mort.
        /// </summary>
        private bool deadslime;

        /// <summary>
        /// The menu for the game(Pause and quit).
        /// </summary>
        private Texture2D menu;

        /// <summary>
        /// La vie du slime.
        /// </summary>
        private int slimeHP1 = 5;

        /// <summary>
        /// La vie du slime.
        /// </summary>
        private int slimeHP2 = 5;

        /// <summary>
        /// La vie du slime.
        /// </summary>
        private int slimeHP3 = 5;

        /// <summary>
        /// La vie du slime.
        /// </summary>
        private int slimeHP4 = 5;

        /// <summary>
        /// Permet de savoir si la clé est prise par le personnage.
        /// </summary>
        private bool deletedKey;

        /// <summary>
        /// La vie du miniboss(miroyr).
        /// </summary>
        private int miniBossHP = 30;

        /// <summary>
        /// Permet de savoir si le miniboss(miroyr) est mort.
        /// </summary>
        private bool minibossdeath;

        /// <summary>
        /// Savoir si le personnage meurt d'un seul coup(spikes).
        /// </summary>
        private bool instantdeath;

        /// <summary>
        /// Instance du volume pour les Sound Effects.
        /// </summary>
        private float volume = 0.9f;

        /// <summary>
        /// Instance du pitch pour les Sound Effects.
        /// </summary>
        private float pitch = 0.0f;

        /// <summary>
        /// Instance du pan pour les Sound Effects.
        /// </summary>
        private float pan = 0.0f;

        /// <summary>
        /// Fond d'écran d'accueil.
        /// </summary>
        private Texture2D ecranAccueil;

        /// <summary>
        /// Constructeur par défaut de la classe. Cette classe est générée automatiquement
        /// par Visual Studio lors de la création du projet.
        /// </summary>
        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            ////Resize the screen resolution
            this.graphics.PreferredBackBufferWidth = ScreenSizeW;
            this.graphics.PreferredBackBufferHeight = ScreenSizeH;

            ////Code to make it fullscreen
            this.graphics.IsFullScreen = false;
            this.graphics.PreferMultiSampling = false;
            this.graphics.SynchronizeWithVerticalRetrace = true;
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
                //// S'assurer qu'il y a changement de statut de pause
                if (value && this.EtatJeu != Etats.Pause)
                {
                    //// Stocker l'état courant du jeu avant d'activer la pause
                    this.prevEtatJeu = this.EtatJeu;
                    this.EtatJeu = Etats.Pause;
                }
                else if (!value && this.EtatJeu == Etats.Pause)
                {
                    //// Restaurer l'état du jeu à ce qu'il était avant la pause
                    this.EtatJeu = this.prevEtatJeu;
                }

                //// Suspendre les effets sonores au besoin
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

                //// Mettre le jeu en pause si un menu est affiché
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
                if (position.X >= door.Position.X - door.Width &&
                    position.Y >= door.Position.Y - (door.Height - 20) &&
                    position.X <= door.Position.X + door.Width &&
                    position.Y <= door.Position.Y + door.Height)
                {
                    if (this.deletedKey == false)
                    {
                        pixColor = Color.Black;
                        break;
                    }
                }
            }

            foreach (WallForMiniboss minibossWall in this.minibossWall)
            {
                if (position.X >= minibossWall.Position.X - minibossWall.Width &&
                    position.Y >= minibossWall.Position.Y - (minibossWall.Height - 20) &&
                    position.X <= minibossWall.Position.X + minibossWall.Width &&
                    position.Y <= minibossWall.Position.Y + minibossWall.Height)
                {
                    if (this.minibossdeath == false)
                    {
                        pixColor = Color.Black;
                        break;
                    }
                }
            }

            //// Extraire la couleur du pixel correspondant à la position.
            if (pixColor != Color.Black)
            {
                try
                {
                    pixColor = this.monde.CouleurDeCollision(position);
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.Exit();
                }
            }

            if (pixColor == Color.Red)
            {
                this.instantdeath = true;
            }

            //// Déterminer le niveau de résistance en fonction de la couleur.
            if (pixColor != Color.Black)
            {
                return 0.0f;
            }
            else
            {
                return 1.0f;
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

            //// Premièrement considérer le déplacement horizontal. Incrémenter la distance horizontale
            //// de déplacement jusqu'à deltaX ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            //// soit rencontrée.
            while (dest.X != posSource.X + deltaX)
            {
                dest.X += Math.Sign(deltaX);        // incrémenter la distance horizontale

                //// Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.X -= Math.Sign(deltaX);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            //// Maintenant considérer le déplacement vertical. Incrémenter la distance verticale
            //// de déplacement jusqu'à deltaY ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            //// soit rencontrée.
            while (dest.Y != posSource.Y + deltaY)
            {
                dest.Y += Math.Sign(deltaY);        // incrémenter la distance horizontale

                //// Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.Y -= Math.Sign(deltaY);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            //// Déterminer le déplacement maximal dans les deux directions
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
            //// Activer le service de gestion du clavier
            ServiceHelper.Game = this;
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (gamepadState.IsConnected)
            {
                this.Components.Add(new ManetteService(this));
                this.manette = true;
            }
            else
            {
                this.Components.Add(new ClavierService(this));
            }

            //// 1280 450
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, this.graphics.GraphicsDevice.Viewport.Width, 450);

            //// Initialiser la vue de la caméra à la taille de l'écran.
            this.camera = new Camera(new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, 450));

            //// Créer les attributs de gestion des obus.
            this.listeObus = new List<Obus>();

            //// Le jeu est en cours de démarrage. Notez qu'on évite d'exploiter la prorpiété EtatJeu
            //// car le setter de cette dernière manipule des effets sonores qui ne sont pas encore
            //// chargées par LoadContent()
            this.etatJeu = Etats.Demarrer;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent est invoquée une seule fois par partie et permet de
        /// charger tous vos composants.
        /// </summary>
        protected override void LoadContent()
        {
            //// Créer un nouveau SpriteBatch, utilisée pour dessiner les textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            //// Charger le monde.

            MondeOcram.LoadContent(this.Content);
            JoueurSprite.LoadContent(this.Content, this.graphics);
            JoueurObus.LoadContent(this.Content, this.graphics);

            //// Au départ, le monde de jour est exploité.
            this.monde = new MondeOcram();
            this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur + (ScreenSizeW / 3), this.monde.Hauteur + 116);

            ////1746, 1280
            ////650, 1070
            this.joueur = new JoueurSprite(0, 0);
            this.joueur.PlayerCollision = new Rectangle((int)this.joueur.Position.X - (this.joueur.Width / 2), (int)this.joueur.Position.Y - (this.joueur.Height / 2), this.joueur.Width, this.joueur.Height);
            this.joueur.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

            //// Imposer la palette de collisions au déplacement du joueur.
            this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;

            //// Charger la musique de fond du jeu.
            this.music = Content.Load<Song>(@"Music\MonogameFinalSong");
            this.slimeDeath = Content.Load<SoundEffect>(@"SoundFX\SimeDeath");
            this.miroyrDeath = Content.Load<SoundEffect>(@"SoundFX\MiryorDeath");
            this.doorOpening = Content.Load<SoundEffect>(@"SoundFX\DoorOpening");
            this.gettingHit = Content.Load<SoundEffect>(@"SoundFX\MainCharacterHurt");
            this.characterDeath = Content.Load<SoundEffect>(@"SoundFX\MainCharacterDeath");

            //// Paramétrer la musique de fond et la démarrer.
            MediaPlayer.Volume = 0.3f;         // valeur entre 0.0 et 1.0
            MediaPlayer.IsRepeating = true;    // jouer en boucle

            // MediaPlayer.Play(this.music);

            //// Associer la déléguée de gestion des obus du personnage à son sprite.
            this.joueur.GetLancerObus = this.LancerObus;

            Door.LoadContent(this.Content, this.graphics);

            this.door = new List<Door>();
            this.door.Add(new Door(1847, 1365));

            WallForMiniboss.LoadContent(this.Content, this.graphics);
            this.minibossWall = new List<WallForMiniboss>();
            this.minibossWall.Add(new WallForMiniboss(1320, 980));

            Key.LoadContent(this.Content, this.graphics);
            this.keys = new List<Key>();
            this.keys.Add(new Key(33, 1330));

            JumpingItem.LoadContent(this.Content, this.graphics);
            this.jumpingitems = new List<JumpingItem>();
            this.jumpingitems.Add(new JumpingItem(740, 1097));
            this.jumpingitems.Add(new JumpingItem(837, 1145));
            this.jumpingitems.Add(new JumpingItem(1250, 1145));

            Boss.LoadContent(this.Content, this.graphics);
            this.boss = new List<Boss>();
            this.boss.Add(new Boss(1757, 1805));

            //// Créer les plateformes.
            Plateforme.LoadContent(this.Content, this.graphics);
            this.plateformes = new List<Plateforme>();
            this.plateformes.Add(new Plateforme(1835, 1565));

            // Créer les plateformes Descendantes.
            PlateformeDescendante.LoadContent(this.Content, this.graphics);
            this.plateformesD = new List<PlateformeDescendante>();
            this.plateformesD.Add(new PlateformeDescendante(100, 1645));
            this.plateformesD.Add(new PlateformeDescendante(400, 1765));
            this.plateformesD.Add(new PlateformeDescendante(580, 1805));
            this.plateformesD.Add(new PlateformeDescendante(780, 1830));

            ////Créer les BoulePiques
            BoulePiqueObstacle.LoadContent(this.Content, this.graphics);

            this.boulepiques = new List<BoulePiqueObstacle>();
            this.boulepiques.Add(new BoulePiqueObstacle(1520, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(1020, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(800, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(600, 1535));
            this.boulepiques.Add(new BoulePiqueObstacle(200, 1535));

            //// Créer les slimes.
            Slime.LoadContent(this.Content, this.graphics);
            this.slimes = new List<Slime>();
            this.slimes.Add(new Slime(350, 77));
            this.slimes.Add(new Slime(900, 77));
            this.slimes.Add(new Slime(1500, 77));
            this.slimes.Add(new Slime(1200, 605));

            //// Créer les miroyr.
            Miroyr.LoadContent(this.Content, this.graphics);
            this.miroyrs = new List<Miroyr>();
            this.miroyrs.Add(new Miroyr(890, 1120));

            foreach (Obus listeObus in this.listeObus)
            {
                listeObus.ObusCollision = new Rectangle((int)listeObus.Position.X - (listeObus.Width / 2), (int)listeObus.Position.Y - (listeObus.Height / 2), listeObus.Width, listeObus.Height);
            }

            foreach (Slime slimes in this.slimes)
            {
                slimes.SlimeCollision = new Rectangle((int)slimes.Position.X - (slimes.Width / 2), (int)slimes.Position.Y - (slimes.Height / 2), slimes.Width, slimes.Height);

                slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
            }

            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.MiroyrCollision = new Rectangle((int)miroyr.Position.X - (miroyr.Width / 2), (int)miroyr.Position.Y - (miroyr.Height / 2), miroyr.Width, miroyr.Height);
                miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
            }

            this.menu = this.Content.Load<Texture2D>(@"MenuImage\menuV2");

            //// Charger les polices
            this.policeMessages = Content.Load<SpriteFont>(@"Polices\MessagesPolice");
            this.policeMenuTitre = Content.Load<SpriteFont>(@"Polices\MenuTitresPolice");
            this.policeMenuItem = Content.Load<SpriteFont>(@"Polices\MenuItemsPolice");

            //// Charger tous les menus disponibles et les stocker dans la liste des menus.
            //// Obtenir d'abord une liste des fichiers XML de définition de menu.
            string[] fichiersDeMenu = Directory.GetFiles(Content.RootDirectory + @"\Menus\");

            //// Itérer pour chaque fichier XML trouvé.

            foreach (string nomFichier in fichiersDeMenu)
            {
                //// Créer un nouveau menu.
                Menu menu = new Menu();

                //// Configurer le nouveau menu à partir de son fichier XML.
                menu.Load(nomFichier);

                //// Assigner la fonction déléguée de Game au nouveau menu (pour gestion des
                //// sélections de l'usager lors de l'affichage du menu).
                menu.SelectionItemMenu = this.SelectionItemMenu;

                //// Ajouter le nouveau menu à la liste des menus du jeu.
                this.listeMenus.Add(menu);
            }

            // Charger les fonds d'écran d'accueil et de générique.
            this.ecranAccueil = Content.Load<Texture2D>(@"Textures\SplashDebut");
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
                //// Deux sélections possibles : Oui ou Non
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
            //// Itérer parmi la liste des menus disponibles
            foreach (Menu menu in this.listeMenus)
            {
                //// Si le menu recherché est trouvé, le retourner
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
            //// À FAIRE: Libérez tout contenu de ContentManager ici
        }

        /// <summary>
        /// Permet d'implanter les comportements logiques du jeu tels que
        /// la mise à jour du monde, la détection de collisions, la lecture d'entrées
        /// et les effets audio.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Update(GameTime gameTime)
        {
            //// Un menu est-il affiché?
            if (this.MenuCourant != null)
            {
                //// Oui, alors gérer les inputs pour ce menu.
                this.MenuCourant.GetInput(gameTime);

                //// Lorsqu'un menu est affiché, le jeu est en pause alors il n'y a rien d'autre.
                //// à faire dans Update()
                base.Update(gameTime);
                return;
            }

            //// Permettre de quitter le jeu via la manette.
            if (ServiceHelper.Get<IInputService>().Quitter(1))
            {
                this.MenuCourant = this.TrouverMenu("QuitterMenu");
            }

            //// Est-ce que le bouton de pause a été pressé?
            if (ServiceHelper.Get<IInputService>().Pause(1))
            {
                this.Pause = !this.Pause;
            }

            //// Si le jeu est en cours de démarrage, passer à l'état de jouer
            if (this.EtatJeu == Etats.Demarrer)
            {
                this.EtatJeu = Etats.Jouer;
            }

            //// Si le jeu est en pause, interrompre la mise à jour
            if (this.Pause)
            {
                base.Update(gameTime);
                return;
            }

            if (this.EtatJeu == Etats.Quitter)
            {
                this.Exit();
            }

            //// Mettre à jour le sprite du joueur puis centrer la camera sur celui-ci.
            this.joueur.Update(gameTime, this.graphics);

            this.UpdateObus(gameTime);

            // Recentrer la caméra sur le sprite du joueur.
            this.camera.Centrer(this.joueur.Position);
          
            this.UpdateCollisionKeyJoueur(gameTime);
            this.UpdateCollisionJoueurMonster(gameTime);
            this.UpdateCollisionJumpingItemJoueur(gameTime);
            this.UpdateCollisionJoueurBoulePique(gameTime);

            // Mettre à jour les Slimes.
            foreach (Slime slime in this.slimes)
            {
                slime.Update(gameTime, this.graphics);
            }

            foreach (Boss boss in this.boss)
            {
                boss.Update(gameTime, this.graphics);
            }

            if (this.minibossdeath == false)
            {
                // Mettre à jour le Miroyr.
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    miroyr.Update(gameTime, this.graphics);
                }
            }

            foreach (Key key in this.keys)
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

            if (this.minibossdeath == false)
            {
                foreach (WallForMiniboss minibossWall in this.minibossWall)
                {
                    minibossWall.Update(gameTime, this.graphics);
                }
            }

            foreach (BoulePiqueObstacle boulepiques in this.boulepiques)
            {
                boulepiques.Update(gameTime, this.graphics);
            }

            //// Mettre à jour les plateformes et déterminer si le sprite du jour est sur une 
            //// plateforme, et si c'est le cas, alors indiquer à celle-ci qu'elle transporte 
            //// ce sprite.
            foreach (Plateforme plateforme in this.plateformes)
            {
                plateforme.Update(gameTime, this.graphics);  // mettre à jour la position

                //// Activer/désactiver la composition selon la plateforme et la position du joueur.
                if (this.joueur.SurPlateforme(plateforme))
                {
                    plateforme.AjouterPassager(this.joueur);
                }
                else
                {
                    plateforme.RetirerPassager(this.joueur);
                }
            }

            //// Mettre à jour les plateformes et déterminer si le sprite du jour est sur une
            //// plateforme, et si c'est le cas, alors indiquer à celle-ci qu'elle transporte 
            //// ce sprite.
            foreach (PlateformeDescendante plateforme in this.plateformesD)
            {
                plateforme.Update(gameTime, this.graphics);  // mettre à jour la position

                //// Activer/désactiver la composition selon la plateforme et la position du joueur.
                if (this.joueur.SurPlateforme(plateforme))
                {
                    plateforme.AjouterPassager(this.joueur);
                }
                else
                {
                    plateforme.RetirerPassager(this.joueur);
                }
            }

            this.joueur.PlayerCollision = new Rectangle((int)this.joueur.Position.X - (this.joueur.Width / 2), (int)this.joueur.Position.Y - (this.joueur.Height / 2), this.joueur.Width, this.joueur.Height);

            foreach (Slime slimes in this.slimes)
            {
                slimes.SlimeCollision = new Rectangle((int)slimes.Position.X - (slimes.Width / 2), (int)slimes.Position.Y - (slimes.Height / 2), slimes.Width, slimes.Height);
                slimes.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
            }

            // Configurer le monstre miroyr de sorte qu'ils ne puissent se déplacer

            // hors de la mappe monde et initialiser la détection de collision de tuiles.
            foreach (Miroyr miroyr in this.miroyrs)
            {
                miroyr.MiroyrCollision = new Rectangle((int)miroyr.Position.X - (miroyr.Width / 2), (int)miroyr.Position.Y - (miroyr.Height / 2), miroyr.Width, miroyr.Height);
                miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
            }

            foreach (Obus listeObus in this.listeObus)
            {
                listeObus.ObusCollision = new Rectangle((int)listeObus.Position.X - (listeObus.Width / 2), (int)listeObus.Position.Y - (listeObus.Height / 2), listeObus.Width, listeObus.Height);
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
            //// On débute avec un écran vierge (au cas où il y aurait des trous dans le monde de tuiles, on va les voir).
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRenderTarget(nativeRenderTarget);

            //// Activer le blending alpha (pour la transparence des sprites).
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

            if (this.deletedKey == false)
            {
                foreach (Key key in this.keys)
                {
                    key.Draw(this.camera, this.spriteBatch);
                }
            }

            if (this.deletedKey == false)
            {
                foreach (Door door in this.door)
                {
                    door.Draw(this.camera, this.spriteBatch);
                }
            }

            if (this.minibossdeath == false)
            {
                foreach (WallForMiniboss minibossWall in this.minibossWall)
                {
                    minibossWall.Draw(this.camera, this.spriteBatch);
                }
            }

            foreach (BoulePiqueObstacle boulepiques in this.boulepiques)
            {
                boulepiques.Draw(this.camera, this.spriteBatch);
            }

            //// Afficher les plateformes.

            foreach (Plateforme plateforme in this.plateformes)
            {
                plateforme.Draw(this.camera, this.spriteBatch);
            }

            //// Afficher les plateformes.
            foreach (PlateformeDescendante plateforme in this.plateformesD)
            {
                plateforme.Draw(this.camera, this.spriteBatch);
            }

            this.joueur.Draw(this.camera, this.spriteBatch);   // afficher le sprite du joueur

            foreach (Boss boss in this.boss)
            {
                boss.Draw(this.camera, this.spriteBatch);
            }

            if (this.deadslime == false)
            {
                //// Afficher les  slimes
                foreach (Slime slime in this.slimes)
                {
                    slime.Draw(this.camera, this.spriteBatch);
                }
            }

            if (this.minibossdeath == false)
            {
                //// Afficher le Miroyr
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    miroyr.Draw(this.camera, this.spriteBatch);
                }
            }
            //// Afficher les messages selon l'état du jeu
            this.DrawMessages(this.spriteBatch);

            //// Afficher le menu courant s'il y en a un sélectionné           
            if (this.MenuCourant != null)
            {
                this.spriteBatch.Draw(this.menu, new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height), Color.White);
                //// Dessiner le menu
                this.MenuCourant.Draw(
                    this.spriteBatch,
                    this.policeMenuTitre,
                    this.policeMenuItem,
                    this.couleurMenuTitre,
                    this.couleurMenuItem,
                    this.couleurMenuItemSelectionne);
            }

            this.spriteBatch.End();

            //// Resize the game to fit the monitor's resolution
            GraphicsDevice.SetRenderTarget(null);
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            this.spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Suspend temporairement (pause) ou réactive les effets sonores du jeu.
        /// </summary>
        /// <param name="suspendre">Indique si les effets sonores doivent être suspendus ou réactivés.</param>
        protected void SuspendreEffetsSonores(bool suspendre)
        {
            //// Suspendre au besoin les effets sonores du vaisseau
            this.joueur.SuspendreEffetsSonores(suspendre);

            //// Suspendre ou réactiver le bruitage de fond
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

            //// Déterminer le message à afficher selon l'état du jeu
            switch (this.EtatJeu)
            {
                case Etats.Pause:
                    if (this.MenuCourant == null)
                    {
                        if (this.manette)
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

            //// Afficher le message s'il y a lieu
            if (output.Length > 0)
            {
                //// L'origine d'affichage du message est son point central
                Vector2 centrePolice = this.policeMessages.MeasureString(output) / 4;

                //// L'origine du message sera positionnée au centre de l'écran
                Vector2 centreEcran = new Vector2(
                    this.graphics.GraphicsDevice.Viewport.Width / 4,
                    this.graphics.GraphicsDevice.Viewport.Height / 4);

                Vector2 centreEcranImage = new Vector2(0, 0);

                spriteBatch.Draw(this.menu, centreEcranImage, Color.White);
                //// Afficher le message centré à l'écran
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
        /// Routine mettant à jour les obus. Elle s'occupe de:
        ///   1 - Détruire les obus ayant quitté l'écran sans collision
        ///   2 - Déterminer si un des obus a frappé un sprite, et si c'est le cas
        ///       détruire les deux sprites (probablement un astéroïde)
        ///   3 - Mettre à jour la position des obus existants.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateObus(GameTime gameTime)
        {
            //// Identifier les obus ayant quitté l'écran.
            List<Obus> obusFini = new List<Obus>();
            foreach (Obus obus in this.listeObus)
            {
                if (obus.Position.Y + obus.Height < 0 ||
                    obus.Position.Y - obus.Height > this.monde.Hauteur)
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
                        this.miniBossHP--;
                        obusFini.Add(obus);
                        if (this.miniBossHP == 0)
                        {
                            this.miroyrDeath.Play(this.volume, this.pan, this.pitch);
                            this.minibossdeath = true;
                        }
                    }
                }
            }

            foreach (Obus obus in this.listeObus)
            {
                if (this.slimes[0].SlimeCollision.Contains(obus.ObusCollision))
                {
                    this.slimeHP1--;
                    obusFini.Add(obus);

                    if (this.slimeHP1 == 0)
                    {
                        this.slimeDeath.Play(this.volume, this.pan, this.pitch);
                        this.slimes[0].Position = new Vector2(9999, 99999);
                    }
                }

                if (this.slimes[1].SlimeCollision.Contains(obus.ObusCollision))
                {
                    this.slimeHP2--;
                    obusFini.Add(obus);

                    if (this.slimeHP2 == 0)
                    {
                        this.slimeDeath.Play(this.volume, this.pan, this.pitch);
                        this.slimes[1].Position = new Vector2(9999, 99999);
                    }
                }

                if (this.slimes[2].SlimeCollision.Contains(obus.ObusCollision))
                {
                    this.slimeHP3--;
                    obusFini.Add(obus);

                    if (this.slimeHP3 == 0)
                    {
                        this.slimeDeath.Play(this.volume, this.pan, this.pitch);
                        this.slimes[2].Position = new Vector2(9999, 99999);
                    }
                }

                if (this.slimes[3].SlimeCollision.Contains(obus.ObusCollision))
                {
                    this.slimeHP4--;
                    obusFini.Add(obus);

                    if (this.slimeHP4 == 0)
                    {
                        this.slimeDeath.Play(this.volume, this.pan, this.pitch);
                        this.slimes[3].Position = new Vector2(9999, 99999);
                        this.deadslime = true;
                    }
                }
            }

            //// Se débarasser des obus n'étant plus d'aucune utilité.
            foreach (Obus obus in obusFini)
            {
                this.listeObus.Remove(obus);
            }

            //// Mettre à jour les obus existants.
            foreach (Obus obus in this.listeObus)
            {
                obus.Update(gameTime, this.graphics);
            }
        }

        /// <summary>
        /// Fonction qui permet d'avoir une collision entre le personnage et la clé.
        /// </summary>
        /// <param name="gameTime">gametime à gérer pour le sprite.</param>
        protected void UpdateCollisionKeyJoueur(GameTime gameTime)
        {
            for (int i = 0; i < this.keys.Count; i++)
            {
                ////Vector2 tempPositionSlime = this.slimes[i].Position;
                if (this.keys[i].Collision(this.joueur))
                {
                    this.deletedKey = true;

                    this.doorOpening.Play(this.volume, this.pan, this.pitch);
                }
            }
        }

        /// <summary>
        /// Fonction qui permet d'avoir une collision entre le personnage et la trampoline.
        /// </summary>
        /// <param name="gameTime">gametime à gérer pour le sprite.</param>
        protected void UpdateCollisionJumpingItemJoueur(GameTime gameTime)
        {
            foreach (JumpingItem jumpingitems in this.jumpingitems)
            {
                if (jumpingitems.Collision(this.joueur))
                {
                    this.joueur.VitesseVerticale -= 1.25f;
                }
            }
        }

        /// <summary>
        /// Fonction qui permet d'avoir une collision entre le personnage et les slimes.
        /// </summary>
        /// <param name="gameTime">gametime à gérer pour le sprite.</param>
        protected void UpdateCollisionJoueurMonster(GameTime gameTime)
        {
            for (int i = 0; i < this.slimes.Count; i++)
            {
                this.hithpCoolDown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.hithpCoolDown > 3f)
                {
                    ////Vector2 tempPositionSlime = this.slimes[i].Position;
                    if (this.slimes[i].SlimeCollision.Contains(this.joueur.PlayerCollision))
                    {
                        this.joueur.PlayerHPP -= 1;
                        this.gettingHit.Play(this.volume, this.pan, this.pitch);
                        this.hithpCoolDown = 0f;
                    }
                }
            }

            if (this.minibossdeath == false)
            {
                for (int i = 0; i < this.miroyrs.Count; i++)
                {
                    this.hithpCoolDown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (this.hithpCoolDown > 3f)
                    {
                        ////Vector2 tempPositionSlime = this.slimes[i].Position;
                        if (this.miroyrs[i].MiroyrCollision.Contains(this.joueur.PlayerCollision))
                        {
                            this.joueur.PlayerHPP -= 1;
                            this.gettingHit.Play(this.volume, this.pan, this.pitch);
                            this.hithpCoolDown = 0f;
                        }
                    }
                }
            }

            this.Reset();
        }

        /// <summary>
        /// Fonction qui permet d'avoir une collision entre le personnage et la boule de pique.
        /// </summary>
        /// <param name="gameTime">gametime à gérer pour le sprite.</param>
        protected void UpdateCollisionJoueurBoulePique(GameTime gameTime)
        {
            for (int i = 0; i < this.boulepiques.Count; i++)
            {
                this.hithpCoolDown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.hithpCoolDown > 3f)
                {
                    if (this.boulepiques[i].Collision(this.joueur))
                    {
                        ////float vitesseH = gameTime.ElapsedGameTime.Milliseconds * this.vitesseMarche;
                        this.joueur.PlayerHPP -= 1;
                        this.gettingHit.Play(this.volume, this.pan, this.pitch);
                        this.hithpCoolDown = 0f;
                    }
                }
            }

            this.Reset();
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
        /// Un reset complet du jeu quand le personnage meurt.
        /// </summary>
        private void Reset()
        {
            if (this.joueur.PlayerHPP == 0 || this.instantdeath == true)
            {
                this.characterDeath.Play(this.volume, this.pan, this.pitch);
                this.instantdeath = false;
                this.minibossdeath = false;
                this.joueur.PlayerHPP = 3;
                this.deadslime = false;
                this.slimeHP1 = 5;
                this.slimeHP2 = 5;
                this.slimeHP3 = 5;
                this.slimeHP4 = 5;
                this.miniBossHP = 30;
                this.deletedKey = false;

                //// Au départ, le monde de jour est exploité.
                this.monde = new MondeOcram();
                this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur + (ScreenSizeW / 3), this.monde.Hauteur + 116);

                foreach (Obus listeObus in this.listeObus)
                {
                    listeObus.ObusCollision = new Rectangle((int)listeObus.Position.X - (listeObus.Width / 2), (int)listeObus.Position.Y - (listeObus.Height / 2), listeObus.Width, listeObus.Height);
                }

                ////1746, 1280
                //// Créer et initialiser le sprite du joueur.
                this.joueur = new JoueurSprite(60, 60);
                this.joueur.PlayerCollision = new Rectangle((int)this.joueur.Position.X - (this.joueur.Width / 2), (int)this.joueur.Position.Y - (this.joueur.Height / 2), this.joueur.Width, this.joueur.Height);
                this.joueur.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

                //// Imposer la palette de collisions au déplacement du joueur.
                this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
                this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;

                //// Associer la déléguée de gestion des obus du vaisseau à son sprite.
                this.joueur.GetLancerObus = this.LancerObus;

                this.door = new List<Door>();
                this.door.Add(new Door(1847, 1365));

                this.minibossWall = new List<WallForMiniboss>();
                this.minibossWall.Add(new WallForMiniboss(1320, 980));

                this.keys = new List<Key>();
                this.keys.Add(new Key(33, 1330));

                this.jumpingitems = new List<JumpingItem>();
                this.jumpingitems.Add(new JumpingItem(740, 1097));
                this.jumpingitems.Add(new JumpingItem(837, 1145));
                this.jumpingitems.Add(new JumpingItem(1250, 1145));

                // Créer les plateformes
                this.plateformes = new List<Plateforme>();
                this.plateformes.Add(new Plateforme(1835, 1575));

                // Créer les slimes.
                this.slimes = new List<Slime>();
                this.slimes.Add(new Slime(350, 77));
                this.slimes.Add(new Slime(900, 77));
                this.slimes.Add(new Slime(1500, 77));

                this.slimes.Add(new Slime(1200, 605));

                this.miroyrs = new List<Miroyr>();
                this.miroyrs.Add(new Miroyr(890, 1120));

                this.boss = new List<Boss>();
                this.boss.Add(new Boss(1757, 1805));

                this.boulepiques = new List<BoulePiqueObstacle>();
                this.boulepiques.Add(new BoulePiqueObstacle(1520, 1535));
                this.boulepiques.Add(new BoulePiqueObstacle(1020, 1535));
                this.boulepiques.Add(new BoulePiqueObstacle(800, 1535));
                this.boulepiques.Add(new BoulePiqueObstacle(600, 1535));
                this.boulepiques.Add(new BoulePiqueObstacle(200, 1535));

                this.plateformesD = new List<PlateformeDescendante>();
                this.plateformesD.Add(new PlateformeDescendante(100, 1645));
                this.plateformesD.Add(new PlateformeDescendante(400, 1765));
                this.plateformesD.Add(new PlateformeDescendante(580, 1805));
                this.plateformesD.Add(new PlateformeDescendante(780, 1830));

                // Configurer les ogres de sorte qu'ils ne puissent se déplacer
                // hors de la mappe monde et initialiser la détection de collision de tuiles.
                foreach (Miroyr miroyr in this.miroyrs)
                {
                    miroyr.MiroyrCollision = new Rectangle((int)miroyr.Position.X - (miroyr.Width / 2), (int)miroyr.Position.Y - (miroyr.Height / 2), miroyr.Width, miroyr.Height);
                    miroyr.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                }
            }
        }
    }
}