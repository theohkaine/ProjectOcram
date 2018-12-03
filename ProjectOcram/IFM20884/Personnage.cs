//-----------------------------------------------------------------------
// <copyright file="Personnage.cs" company="Marco Lavoie">
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
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
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
    /// Classe implantant le sprite représentant un personnage pouvant être stationnairte, marcher 
    /// et courir dans les huit directions.
    /// </summary>
    public abstract class Personnage : SpriteAnimation
    {
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
        /// Vitesse de marche du personnage, avec valeur par défaut.
        /// </summary>
        private float vitesseMaximum = 0.35f;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Direction directionDeplacement;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Etats etat;

        /// <summary>
        /// Instance de bruitage des moteurs en cours de sonorisation durant le jeu.
        /// </summary>
        private SoundEffectInstance bruitActif;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Personnage(float x, float y)
            : base(x, y)
        {
            // Par défaut, le sprite est celui faisant face au personnage.
            this.directionDeplacement = Direction.Gauche;
            this.etat = Etats.Stationnaire;
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Personnage(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Enumération des directions potentielles de déplacement du personnage.
        /// </summary>
        public enum Direction
        {
            Gauche,

            Droite
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
        }

        /// <summary>
        /// Accesseur pour attribut vitesseMaximum.
        /// </summary>
        public float VitesseMaximum
        {
            get { return this.vitesseMaximum; }
            set { this.vitesseMaximum = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut direction.
        /// </summary>
        public Direction DirectionDeplacement
        {
            get { return this.directionDeplacement; }
            set { this.directionDeplacement = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut etat.
        /// </summary>
        public Etats Etat
        {
            get
            {
                return this.etat;
            }

            // Le setter modifie les attributs (hérités) d'animation du sprite afin que les tuiles d'animation
            // correspondant au nouvel état du personnage soient exploitées.
            set
            {
                bool resetBruit = this.etat != value;     // change-t-on d'état?

                // Si l'état change, arrêter le bruit de fond actif (s'il y en a un).
                if (resetBruit && this.bruitActif != null)
                {
                    this.bruitActif.Stop();
                }

                this.etat = value;      // enregistrer le nouvel état.

                // Sélectionner et paramétrer le bruitage de fond correspondant à l'état de déplacement.
                if (resetBruit)
                {
                    // Paramétrer le nouveau bruit de fond.
                    if (this.EffetsSonores != null && this.EffetsSonores[(int)this.etat] != null)
                    {
                        this.bruitActif = this.EffetsSonores[(int)this.etat].CreateInstance();

                        this.bruitActif.Volume = 1.0f;
                        this.bruitActif.IsLooped = true;
                    }
                    else
                    {
                        this.bruitActif = null;
                    }
                }
            }
        }

        /// <summary>
        /// Propriété (accesseur de lecture seulement) retournant la position des pattes du sprite.
        /// Cette position est utilisée pour déterminer si le sprite est debout sur une tuile solide.
        /// </summary>
        public virtual Vector2 PositionPourCollisions
        {
            get
            {
                int dx = 0, dy = (this.Height / 2) - 1;

                // La position considérée est celle des pattes devant le personnage,
                // ce qui dépend de la direction de déplacement
                if (this.directionDeplacement == Direction.Droite)
                {
                    dx += (this.Width / 2) - 1;
                }
                else if (this.directionDeplacement == Direction.Gauche)
                {
                    dx -= (this.Width / 2) - 1;
                }

                return new Vector2(this.Position.X + dx, this.Position.Y + dy);
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
        /// Propriété accesseur retournant la liste des palettes associées au personnage 
        /// selon son état et sa direction. Cette propriété doit obligatoirement être 
        /// surchargée dans les classes dérivées devant être instanciées.
        /// </summary>
        protected abstract List<Palette> Palettes
        {
            get;
        }

        /// <summary>
        /// Propriété accesseur retournant la liste des effets sonores associée au personnage
        /// selon son état. Cette propriété doit obligatoirement être surchargée dans les 
        /// classes dérivées devant être instanciées.
        /// </summary>
        protected abstract List<SoundEffect> EffetsSonores
        {
            get;
        }

        /// <summary>
        /// Lire de  l'input les vitesses de déplacement directionnels.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        /// <param name="vitesseVerticale">Retourne la vitesse de déplacement verticale.</param>
        /// <param name="vitesseDroite">Retourne la vitesse de déplacement vers le est.</param>
        /// <param name="vitesseGauche">Retourne la vitesse de déplacement vers le ouest.</param>
        /// <returns>Vrai si des vitesses furent lues; faux sinon.</returns>
        public abstract bool LireVitesses(
            GameTime gameTime,
            out float vitesseDroite,
            out float vitesseGauche);

        /// <summary>
        /// Ajuste la position du sprite en fonction de l'input.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {


            // Obtenir les vitesses de déplacements (toutes entre 0.0 et 1.0) de l'input
            float vitesseDroite, vitesseGauche;
            if (!this.LireVitesses(gameTime, out vitesseDroite, out vitesseGauche))
            {
                base.Update(gameTime, graphics);
                return;
            }

            // Éviter les directions contradictoires
            if (vitesseGauche > 0.0)
            {
                vitesseDroite = 0.0f;
            }

            // Changer le sprite selon la direction 
            if (vitesseDroite > 0.0)
            {
                this.directionDeplacement = Direction.Droite;
            }
            else if (vitesseGauche > 0.0)
            {
                this.directionDeplacement = Direction.Gauche;
            }


            // Calcul de la vitesse de déplacement du personnage en fonction de l'input
            float vitesse = this.vitesseMaximum;

            if (this.directionDeplacement == Direction.Droite)
            {
                vitesse *= vitesseDroite;
            }
            else if (this.directionDeplacement == Direction.Gauche)
            {
                vitesse *= vitesseGauche;
            }


            // Mettre à jour l'état du personnage selon sa vitesse de déplacement
            if (vitesse >= 0.6 * this.vitesseMaximum)
            {
                this.Etat = Etats.Marche;
            }
            else
            {
                this.Etat = Etats.Stationnaire;
            }

            // Rendre la vitesse indépendante du matériel
            vitesse *= gameTime.ElapsedGameTime.Milliseconds;

            // Calculer le déplacement du sprite selon la direction indiquée. Notez que
            // deux directions opposées s'annulent
            int deltaX = 0, deltaY = 0;

            if (vitesseGauche > 0.0)
            {
                deltaX = (int)-vitesse;
            }

            if (vitesseDroite > 0.0)
            {
                deltaX = (int)vitesse;
            }

            

            // Si une fonction déléguée est fournie pour valider les mouvements sur les tuiles
            // y faire appel pour valider la position résultante du mouvement
            if (this.getValiderDeplacement != null && (deltaX != 0.0 || deltaY != 0.0))
            {
                // Déterminer le déplacement maximal permis vers la nouvelle position en fonction
                // de la résistance des tuiles. Une résistance maximale de 0.95 est indiquée afin de
                // permettre au sprite de traverser les tuiles n'étant pas complètement solides.
                this.getValiderDeplacement(this.PositionPourCollisions, ref deltaX, ref deltaY, 0.95f);
            }

            // Si une fonction déléguée est fournie pour autoriser les mouvements sur les tuiles
            // y faire appel pour valider la position résultante du mouvement.
            if (this.getResistanceAuMouvement != null && (deltaX != 0.0 || deltaY != 0.0))
            {
                // Déterminer les coordonnées de destination et tenant compte que le sprite est
                // centré sur Position, alors que ses mouvements doivent être autorisés en fonction
                // de la position de ses pieds.
                Vector2 newPos = this.PositionPourCollisions;
                newPos.X += deltaX;
                newPos.Y += deltaY;

                // Calculer la résistance à la position du sprite.
                float resistance = this.getResistanceAuMouvement(newPos);

                // Appliquer le facteur de résistance obtenu au déplacement
                deltaX = (int)(deltaX * (1.0f - resistance));
                deltaY = (int)(deltaY * (1.0f - resistance));
            }

            // Activer les effets sonores associés au personnage lorsque ceux-ci sont actifs
            if (this.bruitActif != null)
            {
                if (this.bruitActif.State != SoundState.Playing)
                {
                    this.bruitActif.Play();
                }
            }

            // Modifier la position du sprite en conséquence (on exploite le setter
            // de _position afin d'appliquer boundsRect)
            this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y + deltaY);

            // La fonction de base s'occupe de l'animation.
            base.Update(gameTime, graphics);
        }

        /// <summary>
        /// Suspend temporairement (pause) ou réactive les effets sonores du personnage.
        /// </summary>
        /// <param name="suspendre">Indique si les effets sonores doivent être suspendus ou réactivés.</param>
        public void SuspendreEffetsSonores(bool suspendre)
        {
            // Premièrement s'assurer qu'on a un bruit de fond actif.
            if (this.bruitActif == null)
            {
                return;
            }

            // On en a un, alors le suspendre ou le réactiver, selon l'argument.
            if (suspendre)
            {
                // Suspendre au besoin les effets sonores associés au personnage.
                if (this.bruitActif.State == SoundState.Playing)
                {
                    this.bruitActif.Pause();
                }
            }
            else
            {
                // Réactiver au besoin les effets sonores associés aux moteurs
                if (this.bruitActif.State == SoundState.Paused)
                {
                    this.bruitActif.Play();
                }
            }
        }

        /// <summary>
        /// Charge les images associées au sprite du personnage. Cette fonction invoque sa
        /// surcharge plus générale en lui fournissant des arguments vides (null et string.Empty) 
        /// comme paramètres d'effets sonores.
        /// Il faut invoquer cette fonction pour charger les palettes des classes dérivées
        /// n'exploitant pas d'effets sonores pour leur personnage.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        /// <param name="palettes">Liste où doivent être stockées les palettes chargées.</param>
        /// <param name="largeurTuiles">Largeur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="hauteurTuiles">Hauteur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="repertoirePalettes">Sous-répertoire de Content qui contient les répertoires
        /// contenant les palettes selon l'état du personnage.</param>
        protected static void LoadContent(
            ContentManager content,
            GraphicsDeviceManager graphics,
            List<Palette> palettes,
            int largeurTuiles,
            int hauteurTuiles,
            string repertoirePalettes)
        {
            LoadContent(content, graphics, palettes, null, largeurTuiles, hauteurTuiles, repertoirePalettes, string.Empty);
        }

        /// <summary>
        /// Charge les images et les effets sonores associés au sprite du personnage.
        /// Il faut invoquer cette fonction dans LoadContent des classes dérivées pour 
        /// charger leurs palettes et effets sonores.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        /// <param name="palettes">Liste où doivent être stockées les palettes chargées.</param>
        /// <param name="effetsSonores">Liste où sont stockées les effets sonores chargés.</param>
        /// <param name="largeurTuiles">Largeur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="hauteurTuiles">Hauteur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="repertoirePalettes">Sous-répertoire de Content qui contient les répertoires
        /// contenant les palettes selon l'état du personnage.</param>
        /// <param name="repertoireEffetsSonores">Sous-répertoire de Content qui contient les effets 
        /// sonores selon l'état du personnage.</param>
        protected static void LoadContent(
            ContentManager content,
            GraphicsDeviceManager graphics,
            List<Palette> palettes,
            List<SoundEffect> effetsSonores,
            int largeurTuiles,
            int hauteurTuiles,
            string repertoirePalettes,
            string repertoireEffetsSonores)
        {
            // Puisque les palettes sont répertoriées selon l'état, on procède ainsi,
            // chargeant les huit palettes directionnelles un état à la fois.
            foreach (Etats etat in Enum.GetValues(typeof(Etats)))
            {
                // Déterminer le répertoire contenant les palettes selon l'état, ainsi que
                // les effets sonores (sans égard à l'état).
                string repertoireEtat;     // répertoire des palettes
                switch (etat)
                {
                    case Etats.Stationnaire:
                        repertoireEtat = @"\Marche";
                        break;
                    case Etats.Marche:
                        repertoireEtat = @"\Marche";
                        break;
                    default:
                        repertoireEtat = @"\Stationnaire";
                        break;
                }

                // Charger les différentes palettes du personnage selon les directions. Notez que le
                // répertoire fourni doit OBLIGATOIREMENT contenir une palette pour chaque direction
                // de déplacement, et ce pour chaque état.
                string repertoire = repertoirePalettes + repertoireEtat;
                palettes.Add(new Palette(content.Load<Texture2D>(repertoire + @"\Gauche"), largeurTuiles, hauteurTuiles));
                palettes.Add(new Palette(content.Load<Texture2D>(repertoire + @"\Droite"), largeurTuiles, hauteurTuiles));

                // Charger les bruitages de fond du personnage pour différents états. On utilise
                // un try-catch car il n'est pas requis par la classe dérivée de fournir un effet
                // sonore pour chaque état.
                if (effetsSonores != null)
                {
                    try
                    {
                        effetsSonores.Add(content.Load<SoundEffect>(repertoireEffetsSonores + repertoireEtat));
                    }
                    catch (ContentLoadException)
                    {
                        // Ajouter null à la liste pour indiquer que cet état ne dispose pas
                        // d'effet sonore associé.
                        effetsSonores.Add(null);
                    }
                }
            }
        }
    }
}

