//-----------------------------------------------------------------------
// <copyright file="ServiceHelper.cs" company="Marco Lavoie">
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
    /// Classe facilitant la gestion des services.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Attribut statique liant le gestionnaire de services à la partie à gérer.
        /// </summary>
        private static Game game;      // conserve l'accès à l'instance de Game

        /// <summary>
        /// Propriété statique liant le gestionnaire de services à la partie à gérer.
        /// </summary>
        public static Game Game
        {
            get { return ServiceHelper.game; }
            set { ServiceHelper.game = value; }
        }

        /// <summary>
        /// Ajoute le service fourni au services XNA.
        /// </summary>
        /// <typeparam name="T">Type du service à ajouter.</typeparam>
        /// <param name="service">Le service à ajouter au gestionnaire.</param>
        public static void Add<T>(T service) where T : class
        {
            game.Services.AddService(typeof(T), service);
        }

        /// <summary>
        /// Donne accès au services XNA indiqué.
        /// </summary>
        /// <typeparam name="T">Type du service demandé.</typeparam>
        /// <returns>Le service demandé.</returns>
        public static T Get<T>() where T : class
        {
            return game.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Retire le service de type fourni des services XNA.
        /// </summary>
        /// <typeparam name="T">Type du service à retirer.</typeparam>
        /// <param name="service">Le service à retirer du gestionnaire.</param>
        public static void Remove<T>(T service) where T : class
        {
            game.Services.RemoveService(typeof(T));
        }
    }
}
