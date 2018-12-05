//-----------------------------------------------------------------------
// <copyright file="Menu.cs" company="Marco Lavoie">
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
    using System.Collections.Generic;
    using System.Xml;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Définition de fonction déléguée exécutée lorsqu'un item de menu est
    /// sélectionné.
    /// </summary>
    /// <param name="nomMenu">Nom du menu dans lequel un item est sélectionné.</param>
    /// <param name="nomItem">Nom de l'item sélectionné.</param>
    public delegate void TypeSelectionItemMenu(string nomMenu, ItemDeMenu nomItem);

    /// <summary>
    /// Classe implantant un menu constitué d'un ou plusieurs items de menu (instances
    /// de ItemDeMenu). La classe gère la sélection d'item via le périphérique d'entrée
    /// (i.e. le service de lecture d'entrées).
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Attribut conservant la fonction déléguée invoquée par l'instance de Menu lorsqu'un
        /// item de menu est sélectionné.
        /// </summary>
        private TypeSelectionItemMenu selectionItemMenu = null;

        /// <summary>
        /// Attribut gérant la liste des items associés au menu.
        /// </summary>
        private List<ItemDeMenu> items = new List<ItemDeMenu>();

        /// <summary>
        /// Attribut indiquant le nom du menu (pour fins d'identification).
        /// </summary>
        private string nom = string.Empty;

        /// <summary>
        /// Attribut indiquant le titre du menu (pour fins d'affichage).
        /// </summary>
        private string titre = string.Empty;

        /// <summary>
        /// Attribut contenant l'index de l'item du menu présentement actif pour
        /// sélection.
        /// </summary>
        private int indexSelection = 0;

        /// <summary>
        /// Attribut gérant la position du menu à l'écran. Le coin supérieur
        /// gauche du menu sera positionné à ces coordonnées.
        /// </summary>
        private Vector2 origine = new Vector2();

        /// <summary>
        /// Attribut indiquant le temps écoulé (en millisecondes) depuis que la sélection
        /// active fut sélectionnée. Cet attribut sert à insérer un délai entre les
        /// changements de sélection lorsque ceux-ci sont trop rapides (e.g. la touche du
        /// clavier est pressée continuellement).
        /// </summary>
        private double heureDernierChangementItem = 0.0;

        /// <summary>
        /// Propriété (accesseur de selectionItemMenu) retournant et modifiant la fonction 
        /// déléguée invoquée par l'instance de Menu lorsqu'un item de menu est sélectionné.
        /// </summary>
        public TypeSelectionItemMenu SelectionItemMenu
        {
            get { return this.selectionItemMenu; }
            set { this.selectionItemMenu = value; }
        }

        /// <summary>
        /// Propriété (accesseur de nom) retournant et modifiant le nom du menu (pour 
        /// fins d'identification).
        /// </summary>
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }

        /// <summary>
        /// Propriété retournant l'item de menu présentement actif pour sélection.
        /// </summary>
        private ItemDeMenu Selection
        {
            get
            {
                // L'instance de ItemDeMenu correspondant à la sélection active est
                // extraite (mais non retirée) de items.
                if (this.indexSelection >= 0 && this.indexSelection < this.items.Count)
                {
                    return this.items[this.indexSelection];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Charge le menu d'un fichier XML dont le nom est fourni en paramètre.
        /// </summary>
        /// <param name="nomMenu">Fichier XML d'où extraire le menu.</param>
        public void Load(string nomMenu)
        {
            // Initialiser un lexeur de fichier XML.
            XmlTextReader lecteurXML = new XmlTextReader(nomMenu);

            // Item de menu en construction lors de la lecture.
            ItemDeMenu itemCourant = null;

            // Lire les champs XML un à la fois, séquentiellement.
            while (lecteurXML.Read())
            {
                // Traiter le champ XML en fonction de son nom.
                switch (lecteurXML.Name)
                {
                    // Le nom du menu.
                    case "MenuNom":
                        this.nom = lecteurXML.ReadElementContentAsString();
                        break;

                    // Le titre du menu.
                    case "MenuTitre":
                        this.titre = lecteurXML.ReadElementContentAsString();
                        break;

                    // Un nouvel item de menu.
                    case "MenuItem":
                        // Vérifier premièrement si on était en processus de lecture
                        // d'un item de menu précédent. Si c'est le cas, ajouté cet
                        // item à la liste avant de commencer à lire le nouvel item.
                        if (itemCourant != null)
                        {
                            // Puisqu'un item de menu DOIT avoir un nom pour pouvoir le
                            // gérer, on conserve celui lu seulement s'il a un nom.
                            if (itemCourant.Nom != string.Empty)
                            {
                                this.items.Add(itemCourant);
                            }
                        }

                        // Créer un nouvel item de menu à construire.
                        itemCourant = new ItemDeMenu();
                        break;

                    // Le nom de l'item de menu en cours de lecture.
                    case "MenuItemNom":
                        itemCourant.Nom = lecteurXML.ReadElementContentAsString();
                        break;

                    // Le titre de l'item de menu en cours de lecture.
                    case "MenuItemTitre":
                        itemCourant.Titre = lecteurXML.ReadElementContentAsString();
                        break;

                    // L'indentation horizontale (en pixels) de l'item de menu en cours de lecture.
                    case "MenuItemIndent":
                        itemCourant.Indentation = lecteurXML.ReadElementContentAsInt();
                        break;

                    // L'item de menu actif par défaut (i.e. lorsque le menu est affiché, cet item
                    // sera celui actif).
                    case "IndexSelectionItem":
                        this.indexSelection = lecteurXML.ReadElementContentAsInt();
                        break;

                    // Position horizontale de l'origine du menu (coin supérieur gauche).
                    case "PositionX":
                        this.origine.X = lecteurXML.ReadElementContentAsInt();
                        break;

                    // Position verticale de l'origine du menu (coin supérieur gauche).
                    case "PositionY":
                        this.origine.Y = lecteurXML.ReadElementContentAsInt();
                        break;

                    default:
                        break;
                }
            }

            // Si un item de menu est actif pour sélection, l'en informer.
            if (this.Selection != null)
            {
                this.Selection.Selection = true;
            }
        }

        /// <summary>
        /// Met à jour l'item de menu actif (pour sélection) en fonction des inputs.
        /// La fonction exploite le service de lecture d'inputs pour gérer la sélection
        /// des items de menu. De plus, elle impose un délai minimum entre deux changements
        /// de sélection.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        public void GetInput(GameTime gameTime)
        {
            // Délai minimum imposé (en millisecondes) avant que la sélection active puisse 
            // être changée à nouveau.
            const double DelaiChangementItem = 150.0;

            // Vérifier si assez de temps s'est écoulé depuis le dernier changement de
            // sélection. On s'assure ainsi à l'utilisateur un meilleur contrôle du
            // changement de sélection (sinon les changements seraient beaucoup trop
            // rapides pour qu'il puisse choisir un item spécifique).
            if (this.heureDernierChangementItem > DelaiChangementItem)
            {
                // Doit-on changer la sélection active à l'item précédent du menu?
                if (ServiceHelper.Get<IInputService>().MenuItemPrecedent(1))
                {
                    this.SelectionItemPrecedent();

                    // Changement de sélection, remettre le compteur de temps écoulé à 0.
                    this.heureDernierChangementItem = 0.0;
                }
                else if (ServiceHelper.Get<IInputService>().MenuItemSuivant(1))
                {
                    // On doit changer la sélection active à l'item suivant du menu.
                    this.SelectionItemSuivant();

                    // Changement de sélection, remettre le compteur de temps écoulé à 0.
                    this.heureDernierChangementItem = 0.0;
                }
                else if (ServiceHelper.Get<IInputService>().MenuItemSelection(1))
                {
                    // Une sélection a été demandée alors on doit invoquer la fonction 
                    // déléguée fournie.
                    if (this.selectionItemMenu != null)
                    {
                        this.selectionItemMenu(this.nom, this.Selection);
                    }

                    // Changement de sélection, remettre le compteur de temps écoulé à 0.
                    this.heureDernierChangementItem = 0.0;
                }
            }
            else
            {
                // Mettre à jour le compteur de temps écoulé à 0.
                this.heureDernierChangementItem += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Fonction faisant passer la sélection active courante au prochain item de menu (i.e.
        /// celui en dessous de la sélection courante.
        /// </summary>
        public void SelectionItemSuivant()
        {
            // Désactiver l'item de menu actif précédent.
            this.items[this.indexSelection].Selection = false;

            // S'il y a un autre item en dessous.
            if (this.indexSelection < this.items.Count - 1)
            {
                // Mettre à jour l'index d'item actif de sorte qu'il indique
                // le prochain item de menu.
                this.indexSelection++;
            }
            else
            {
                // L'item courant est le dernier du menu, donc mettre à jour l'index d'item 
                // actif de sorte qu'il indique le premier item du menu.
                this.indexSelection = 0;
            }

            // Activer le nouvel item de menu.
            this.items[this.indexSelection].Selection = true;
        }

        /// <summary>
        /// Fonction faisant passer la sélection active courante à l'item de menu précédent 
        /// (i.e. celui au dessus de la sélection courante.
        /// </summary>
        public void SelectionItemPrecedent()
        {
            // Désactiver l'item de menu actif précédent.
            this.items[this.indexSelection].Selection = false;

            // S'il y a un autre item au dessus.
            if (this.indexSelection > 0)
            {
                // Mettre à jour l'index d'item actif de sorte qu'il indique
                // l'item de menu précédent.
                this.indexSelection--;
            }
            else
            {
                // L'item courant est le premier du menu, donc mettre à jour l'index 
                // d'item actif de sorte qu'il indique le dernier item du menu.
                this.indexSelection = this.items.Count - 1;
            }

            // Activer le nouvel item de menu.
            this.items[this.indexSelection].Selection = true;
        }

        /// <summary>
        /// Affiche le menu à l'écran.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        /// <param name="fontTitre">Police pour afficher le titre du menu.</param>
        /// <param name="fontItem">Police pour afficher les items du menu.</param>
        /// <param name="couleurTitre">Couleur du texte du titre du menu.</param>
        /// <param name="couleurItem">Couleur du texte des items du menu.</param>
        /// <param name="couleurItemSelectionne">Couleur pour afficher l'item présentement actif.</param>
        public void Draw(
            SpriteBatch spriteBatch,        // tampon d'affichage
            SpriteFont fontTitre,           // police pour afficher le titre du menu
            SpriteFont fontItem,            // police pour afficher les items du menu
            Color couleurTitre,             // couleur du texte du titre du menu
            Color couleurItem,              // couleur du texte des items du menu
            Color couleurItemSelectionne)   // couleur pour afficher l'item présentement actif
        {
            // Afficher le titre du menu.
            spriteBatch.DrawString(fontTitre, this.titre, this.origine, couleurTitre);

            // Calculer la hauteur du texte pour un titre d'item de menu.
            float hauteurTexte = fontItem.MeasureString(this.items[0].Nom).Y;

            // Calculer la hauteur du texte pour le titre de menu. On ajoute un espace
            // vertical supplémentaire pour distinguer le titre du menu de ses items.
            float titreHauteurTexte = fontTitre.MeasureString(this.titre).Y + 10.0f;

            // Où on en est rendu verticalement avec l'affichage des items.
            int noLigne = 0;

            // Afficher chaque item de menu. Chaque item est positionné en fonction de la hauteur
            // (en pixels) d'un item de menu et du nombre d'items affichés lors des itérations
            // précédentes de la boucle.
            foreach (ItemDeMenu item in this.items)
            {
                // Est-ce que l'item est présentement actif pour sélection?
                if (item.Selection)
                {
                    // Afficher l'item avec la couleur de sélection active.
                    spriteBatch.DrawString(fontItem, item.Titre, new Vector2(this.origine.X + item.Indentation, this.origine.Y + titreHauteurTexte + (hauteurTexte * noLigne)), couleurItemSelectionne);
                }
                else
                {
                    // Il n'est pas la sélection active, donc afficher l'item avec la couleur d'items non sélectionnés.
                    spriteBatch.DrawString(fontItem, item.Titre, new Vector2(this.origine.X + item.Indentation, this.origine.Y + titreHauteurTexte + (hauteurTexte * noLigne)), couleurItem);
                }

                // Ne pas oublier de compter les lignes.
                noLigne++;
            }
        }
    }
}
