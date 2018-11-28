//-----------------------------------------------------------------------
// <copyright file="JoueurSprite.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Définition de fonction déléguée permettant de calculer la résistance aux déplacements
    /// dans le monde à la position donnée.
    /// </summary>
    /// <param name="position">Position du pixel en coordonnées du monde.</param>
    /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
    public delegate float ResistanceAuMouvement(Vector2 position);

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
    public delegate void ValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax);

    /// <summary>
    /// Définition de fonction déléguée permettant de lancer un nouvel obus.
    /// </summary>
    /// <param name="obus">Nouvel obus à être géré par le jeu.</param>
    public delegate void LancerObus(Obus obus);

    /// <summary>
    /// Classe implantant le sprite représentant le soldat contrôlé par le joueur. Ce sprite
    /// animé peut être stationnaire, marcher et courir dans huit directions.
    /// </summary>
    public class JoueurSprite : SpriteAnimation
    {
        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du joueur.
        /// </summary>
        private static List<Palette> palettes = new List<Palette>();

        /// <summary>
        /// Fonction déléguée permettant d'obtenir la résistance aux déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ResistanceAuMouvement getResistanceAuMouvement;

        /// <summary>
        /// Fonction déléguée permettant de valider les déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ValiderDeplacement getValiderDeplacement;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Direction directionDeplacement;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Etats etat;

        /// <summary>
        /// Vitesse de marche du joueur, avec valeur par défaut.
        /// </summary>
        private float vitesseMarche = 0.2f;

        /// <summary>
        /// Vitesse verticale de déplacement, exploitée lors des sauts et lorsque le sprite tombe dans
        /// un trou.
        /// </summary>
        private float vitesseVerticale = 0.0f;

        /// <summary>
        /// Attribut indiquant l'index du périphérique contrôlant le sprite (voir
        /// dans Update (1 par défaut).
        /// </summary>
        private int indexPeripherique = 1;

        /// <summary>
        /// Fonction déléguée permettant de lancer un nouvel obus. Cette fonction est invoquée
        /// par this à chaque fois qu'un nouvel obus est lancé. C'est via cette fonction que
        /// le nouvel obus est remis à la classe ayant la responsabilité de gérer les obus.
        /// </summary>
        private LancerObus getLancerObus;

        // Texture servant à afficher un rectangle de vitalité au-dessus de this.
        private static Texture2D rectTexture;
        private static Texture2D rectTextureOnTop;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public JoueurSprite(float x, float y)
            : base(x, y)  
        {
            // Par défaut, le sprite est stationnaire et fait face au joueur.
            this.directionDeplacement = Direction.Droite;
            this.etat = Etats.Stationnaire;
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public JoueurSprite(Vector2 position)
            : this(position.X, position.Y) 
        {
        }

        /// <summary>
        /// Enumération des directions potentielles de déplacement du joueur.
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// Déplacement vers la droite de l'écran.
            /// </summary>
            Droite,

            /// <summary>
            /// Déplacement vers la gauche de l'écran.
            /// </summary>
            Gauche
        }

        /// <summary>
        /// Enumération des états disponibles du personnage.
        /// </summary>
        public enum Etats
        {
            /// <summary>
            /// Le personnage ne se déplace pas.
            /// </summary>
            Stationnaire,

            /// <summary>
            /// Le personnage se déplace lentement.
            /// </summary>
            Marche,

            /// <summary>
            /// Le personnage saute.
            /// </summary>
            Saut
        }

        /// <summary>
        /// Accesseur et mutateur pour attribut vitesseMarche.
        /// </summary>
        public float VitesseMarche
        {
            get { return this.vitesseMarche; }
            set { this.vitesseMarche = value; }
        }

        /// <summary>
        /// Accesseur et mutateur pour l'attribut directionDeplacement.
        /// </summary>
        public Direction DirectionDeplacement
        {
            get { return this.directionDeplacement; }
            set { this.directionDeplacement = value; }
        }

        /// <summary>
        /// Propriété (accesseur de etat) retournant et modifiant l'état du joueur.
        /// </summary>
        public Etats Etat
        {
            get { return this.etat; }
            // Le setter modifie les attributs (hérités) d'animation du sprite afin que les
            // tuiles d'animation correspondant au nouvel état du joueur soient exploitées.
            set
            {
                // Augmenter la vitesse de déplacement de 30% lors des sauts
                if (this.etat != Etats.Saut && value == Etats.Saut)
                    this.VitesseMarche *= 1.3f;
                else if (this.etat == Etats.Saut && value != Etats.Saut)
                    this.VitesseMarche /= 1.3f;
                this.etat = value;
            }
        }

        /// <summary>
        /// Propriété (accesseur pour getResistanceAuMouvement) retournant ou changeant la fonction déléguée 
        /// de calcul de résistance aux déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ResistanceAuMouvement GetResistanceAuMouvement
        {
            get { return this.getResistanceAuMouvement; }
            set { this.getResistanceAuMouvement = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour getValiderDeplacement) retournant ou changeant la fonction déléguée 
        /// de validation des déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ValiderDeplacement GetValiderDeplacement
        {
            get { return this.getValiderDeplacement; }
            set { this.getValiderDeplacement = value; }
        }

        /// <summary>
        /// Propriété indiquant l'index du périphérique contrôlant le sprite (1 à 4).
        /// </summary>
        public int IndexPeripherique
        {
            get { return this.indexPeripherique; }
            set { this.indexPeripherique = value; }
        }

        /// <summary>
        /// Propriété (accesseur de lecture seulement) retournant la position des pattes du sprite.
        /// Cette position est utilisée pour déterminer si le sprite est debout sur une tuile solide.
        /// </summary>
        public Vector2 PositionPourCollisions
        {
            get
            {
                int dx = 0, dy = (this.Height/ 3)-1;

               /* if (this.directionDeplacement)
                {
                    dx = -dy;
                }
                */
                // La position considérée est celle des pattes devant le personnage,
                // ce qui dépend de la direction de déplacement
                if (this.directionDeplacement == Direction.Droite)
                {
                    dx += (this.Width / 2)+3;
                }
                else if (this.directionDeplacement == Direction.Gauche)
                {
                    dx -= (this.Width/ 2)+3;
                }

                return new Vector2(this.Position.X + dx, this.Position.Y + dy);
            }
        }

        /// <summary>
        /// Surchargé afin de retourner la palette correspondant à la direction de 
        /// déplacement et l'état du joueur.
        /// </summary>
        protected override Palette PaletteAnimation
        {
            // Les palettes sont stockées dans la liste en groupes d'état (i.e.
            // 2 palettes de direction pour chaque état).
            get { return palettes[((int)this.etat * 2) + (int)this.directionDeplacement]; }
        }

        /// <summary>
        /// Propriété (accesseur pour getLancerObus) retournant ou changeant la fonction déléguée 
        /// gérant les nouveaux obus.
        /// </summary>
        /// <value>Fonction de gestion des nouveaux obus.</value>
        public LancerObus GetLancerObus
        {
            get { return this.getLancerObus; }
            set { this.getLancerObus = value; }
        }

        /// <summary>
        /// Charge les images associées au sprite du joueur.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Puisque les palettes sont répertoriées selon l'état, on procède ainsi,
            // chargeant les huit palettes directionnelles un état à la fois.
            foreach (Etats etat in Enum.GetValues(typeof(Etats)))
            {
                // Déterminer le répertoire contenant les palettes selon l'état.
                string repertoire;               
                switch (etat) 
                {
                    case Etats.Marche: 
                        repertoire = @"Joueur\Marche\"; 
                        break;
                    case Etats.Saut:
                        repertoire = @"Joueur\Saut\";
                        break;

                    default: 
                        repertoire = @"Joueur\Stationnaire\"; 
                        break;
                }

                // Charger les différentes palettes du personnage selon les directions.
                palettes.Add(new Palette(content.Load<Texture2D>(repertoire + "Sprite_Right"), 39, 39));
                palettes.Add(new Palette(content.Load<Texture2D>(repertoire + "Sprite_Left"), 39, 39));
            }
            // Charger la texture servant à l’affichage du rectangle de vitalité.
            rectTexture = content.Load<Texture2D>(@"Extra\tankHP");
            rectTextureOnTop = content.Load<Texture2D>(@"Extra\hp");
        }

        /// <summary>
        /// Ajuste la position du sprite en fonction de l'input.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {

            // Calcul de la vitesse de marche du joueur (indépendante du matériel)
            float vitesseH = gameTime.ElapsedGameTime.Milliseconds * this.vitesseMarche;
            float vitesseV = 0.0f;

            // Obtenir les vitesses de déplacements (toutes entre 0.0 et 1.0) de l'input
            float vitesseD = ServiceHelper.Get<IInputService>().DeplacementDroite(this.indexPeripherique);
            float vitesseG = ServiceHelper.Get<IInputService>().DeplacementGauche(this.indexPeripherique);

            // Éviter les directions contradictoires
            if (vitesseD > 0.0)
            {
                vitesseG = 0.0f;
            }

            // Changer le sprite selon la direction 
            if (vitesseD > 0.0)
            {
                
                this.directionDeplacement = Direction.Droite;
            }
            else if (vitesseG > 0.0)
            {
                this.directionDeplacement = Direction.Gauche;
            }

            // Calculer le déplacement horizontal du sprite selon la direction indiquée. Notez que deux directions
            // opposées s'annulent
            int deltaX = 0;
            if (this.directionDeplacement == Direction.Gauche)
            {
                deltaX = (int)(-vitesseH * vitesseG);
            }

            if (this.directionDeplacement == Direction.Droite)
            {
                deltaX = (int)(vitesseH * vitesseD);
            }

            // Déterminer si le sprite doit sauter. Si c'est le cas, une vitesse verticale négative (i.e. vers le
            // haut) est initiée.
            if (ServiceHelper.Get<IInputService>().Sauter(this.indexPeripherique) && this.etat != Etats.Saut)
            {
                this.Etat = Etats.Saut;

                // Vitesse initiale vers le haut de l'écran
                this.vitesseVerticale = -0.56f;
                
            }

            // Si le sprite est en état de saut, modifier graduellement sa vitesse verticale
            if (this.Etat == Etats.Saut)
            {
                this.vitesseVerticale += 0.037f;    // selon la constante de gravité (9.8 m/s2)
                

            }

            //Make falling more slower if the character vertical speed goes too high
            if(this.vitesseVerticale > 0.4f)
            {
                this.vitesseVerticale = 0.35f;
            }

            // Moduler la vitesse verticale en fonction du matériel
            vitesseV = gameTime.ElapsedGameTime.Milliseconds * this.vitesseVerticale;
            int deltaY = (int)vitesseV;

            // Si une fonction déléguée est fournie pour valider les mouvements sur les tuiles
            // y faire appel pour valider la position résultante du mouvement
            bool sautTermine = false;
            if (this.getValiderDeplacement != null && (deltaX != 0.0 || deltaY != 0.0))
            {


                // Déterminer le déplacement maximal permis vers la nouvelle position en fonction
                // de la résistance des tuiles. Une résistance maximale de 0.95 est indiquée afin de
                // permettre au sprite de traverser les tuiles n'étant pas complètement solides.
                this.getValiderDeplacement(this.PositionPourCollisions, ref deltaX, ref deltaY, 0.95f);
                


                // Si aucun déplacement verticale n'est déterminé lors d'un saut (parce que le sprite 
                // a rencontré une tuile solide), indiquer que le saut est terminé.
                sautTermine = (this.Etat == Etats.Saut) && (deltaY == 0);
                

            }

            // Si un saut est terminé, annuler la vitesse verticale et changer l'état du sprite
            if (sautTermine)
            {


                this.Etat = Etats.Stationnaire;  // le prochain Update() le remettra en état
                                                 // de marche au besoin
                this.vitesseVerticale = 0.0f;
            }

            // Vérifier si le sol est solide sous les pieds du sprite, sinon faire tomber ce dernier
            if (this.getResistanceAuMouvement != null && this.Etat != Etats.Saut)
            {
                // Déterminer les coordonnées de destination et tenant compte que le sprite est
                // centré sur Position, alors que ses mouvements doivent être autorisés en fonction
                // de la position de ses pieds.


                //CODE ALEX
                Vector2 newPos = this.PositionPourCollisions;
                newPos.Y += 1;

                Vector2 newPosition = this.PositionPourCollisions;
                newPosition.Y += 1;

                if(this.directionDeplacement == Direction.Droite)
                {
                    newPosition.X -= (this.Width) / 1.9f;
                }
                else if (this.directionDeplacement == Direction.Gauche)
                {
                    newPosition.X += (this.Width)/1.9f;
                }
                     
                // Calculer la résistance à la position du sprite.
                float resistance = this.getResistanceAuMouvement(newPos) + this.getResistanceAuMouvement(newPosition);

                // Déterminer si le sol est solide à la position du sprite. Sinon activer l'état de
                // saut pour simuler la chute du sprite
                if (resistance < 0.95f)
                {
                    this.Etat = Etats.Saut;
                    this.vitesseVerticale = 0.07f;
                }
            }

            // Modifier la position et l'état du sprite en conséquence
            if (deltaX != 0 || deltaY != 0)
            {
                if (this.Etat == Etats.Stationnaire)
                {
                    this.Etat = Etats.Marche;
                }

                this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y + deltaY);
            }
            else if (this.Etat != Etats.Saut)
            {
                this.Etat = Etats.Stationnaire;    // aucun mouvement: le joueur est stationnaire
            }

            // Déterminer si un obus doit être lancé
            if (ServiceHelper.Get<IInputService>().TirerObus(this.indexPeripherique) && this.getLancerObus != null)
            {
                
                
                if (this.directionDeplacement == Direction.Gauche)
                {

                   
                    // Créer le nouvel obus et le passer (via la déléguée) au gestionnaire d'obus
                    JoueurObus obusGauche = new JoueurObus(this.Position.X - (this.Width *1.25f), this.Position.Y - (this.Width / 9), new Vector2(-1.0f, 0f));
                    obusGauche.Source = this;

                    this.getLancerObus(obusGauche);
                }

                else if (this.directionDeplacement == Direction.Droite)
                {
                    // Créer le nouvel obus et le passer (via la déléguée) au gestionnaire d'obus
                    JoueurObus obusDroite = new JoueurObus(this.Position.X + (this.Width * 1.25f), this.Position.Y - (this.Width / 9), new Vector2(1.0f, 0f));
                    obusDroite.Source = this;
                    this.getLancerObus(obusDroite);

                }

            }

                // La fonction de base s'occupe de l'animation.
                base.Update(gameTime, graphics);
        }

        // Fonction dessinant un rectangle de vitalité au-dessus du sprite.
        private void DrawVitalite(Camera camera, SpriteBatch spriteBatch)
        {
            // Créer le rectangle à dessiner.
            Rectangle rect = new Rectangle((int)(this.Position.X - 45),
            (int)(this.Position.Y - this.Height / 2 - 23),
            this.Width+55, 35);

            // Créer le rectangle à dessiner.
            Rectangle rectOnTop = new Rectangle((int)(this.Position.X - 27),
            (int)(this.Position.Y - this.Height / 2 - 8),
            this.Width/2 + 40, 4);


            // Si nous avons une caméra, corriger le rectangle en conséquence.
            if (camera != null)
            {
                camera.Monde2Camera(ref rect);
                camera.Monde2Camera(ref rectOnTop);
            }
            // Afficher le rectangle.
            spriteBatch.Draw(rectTexture, rect,Color.Black);
            spriteBatch.Draw(rectTextureOnTop, rectOnTop, Color.MediumBlue);
        }



        // Surcharge afin d'afficher le rectangle de vitalité.
        public override void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Afficher le rectangle de vitalité.
            this.DrawVitalite(camera, spriteBatch);
            // Puis afficher le sprite.
            base.Draw(camera, spriteBatch);
        }
    }
}
