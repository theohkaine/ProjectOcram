//-----------------------------------------------------------------------
// <copyright file="IInputService.cs" company="Marco Lavoie">
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

    /// <summary>
    /// Interface énumérant les fonctions à implanter par une classe héritant
    /// de cet interface.
    /// </summary>
    public interface IInputService
    {
        /// <summary>
        /// Retourne un entier entre 0.0 et 1.0 indiquant le facteur de vitesse à appliquer
        /// durant le jeu pour diriger le sprite du joueur vers la gauche : 0.0 = aucun 
        /// déplacement, 1.0 = vitesse maximale.
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Valeurs entre 0.0 (aucun mouvement) et 1.0 (vitesse maximale).</returns>
        float DeplacementGauche(int device);      // pour tourner vers la gauche

        /// <summary>
        /// Retourne un entier entre 0.0 et 1.0 indiquant le facteur de vitesse à appliquer
        /// durant le jeu pour diriger le sprite du joueur vers la droite : 0.0 = aucun 
        /// déplacement, 1.0 = vitesse maximale.
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Valeurs entre 0.0 (aucun mouvement) et 1.0 (vitesse maximale).</returns>
        float DeplacementDroite(int device);      // pour tourner vers la droite

        /// <summary>
        /// Retourne un booléen indiquant si un saut du personnage doit être effectue.
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Vrai si le personnage doit sauter; faux sinon.</returns>
        bool Sauter(int device);

        /// <summary>
        /// Retourne un booléen indiquant si la partie doit être terminée.
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Valeurs entre 0.0 (aucun mouvement) et 1.0 (vitesse maximale).</returns>
        bool Quitter(int device);                 // pour quitter la partie

        /// <summary>
        /// Indique si un obus doit être tiré.
        /// </summary>
        /// <param name="device">Le périphérique à lire.</param>
        /// <returns>Vrai si un obus doit être tiré; faux sinon.</returns>
        bool TirerObus(int device);

        float DashingDroite(int device);
        float DashingGauche(int device);
       
    }
}
