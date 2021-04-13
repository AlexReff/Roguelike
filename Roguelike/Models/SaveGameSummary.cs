using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Models
{
    internal class SaveGameSummary
    {
        /// <summary>
        /// The seed used to generate the world
        /// </summary>
        public string Seed { get; set; }
        /// <summary>
        /// Game release version (for future cross-version save compatibility)
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Current character's name
        /// </summary>
        public string CharacterName { get; set; }
        /// <summary>
        /// Current character's level
        /// </summary>
        public int CharacterLevel { get; set; }
        /// <summary>
        /// How many player characters have been made in this world
        /// </summary>
        public int CharacterCount { get; set; }
        
        //public DateTime

        public SaveGameSummary()
        {
            //
        }
    }
}
