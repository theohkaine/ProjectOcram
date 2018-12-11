//-----------------------------------------------------------------------
// <copyright file="ItemDeMenu.cs" company="Marco Lavoie">
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
    using System.Threading;

    /// <summary>
    /// Classe implantant un item de menu. Cette classe dispose seulement d'Attributs
    /// membres car elle sert uniquement à stocker de l'information relative à un item
    /// de menu.
    /// </summary>
    public class ItemDeMenu
    {
        /// <summary>
        /// Attribut indiquant si l'item est la sélection courante d'un menu.
        /// </summary>
        private bool selection = false;

        /// <summary>
        /// Attribut indiquant le nom de l'item (pour fins d'identification).
        /// </summary>
        private string nom = string.Empty;

        /// <summary>
        /// Attribut indiquant le titre de l'item (pour fins d'affichage).
        /// </summary>
        private string titre = string.Empty;

        /// <summary>
        /// Attribut indiquant l'indentation horizontale de l'item (en pixels) en rapport
        /// à la position du menu dans lequel l'item est affiché). Noter que c'est au menu
        /// de gérer l'indentation lors de l'affichage du menu.
        /// </summary>
        private int indentation = 0;

        /// <summary>
        /// Propriété (accesseur de titre) retournant et modifiant le titre de l'item 
        /// (pour fins d'affichage).
        /// </summary>
        public string Titre
        {
            get { return this.titre; }
            set { this.titre = value; }
        }

        /// <summary>
        /// Propriété (accesseur de nom) retournant et modifiant le nom de l'item 
        /// (pour fins d'identification).
        /// </summary>
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }

        /// <summary>
        /// Propriété (accesseur de selection) retournant et modifiant l'indication si 
        /// l'item est la sélection courante d'un menu.
        /// </summary>
        public bool Selection
        {
            get { return this.selection; }
            set { this.selection = value; }
        }

        /// <summary>
        /// Propriété (accesseur de indentation) retournant et modifiant l'indentation 
        /// horizontale de l'item (voir description de indentation).
        /// </summary>
        public int Indentation
        {
            get { return this.indentation; }
            set { this.indentation = value; }
        }
    }
}
