using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class DeckManager : MonoBehaviour
    {
        public List<Spell> playerDeck;
        public List<Spell> discardPile;

        /// <summary>
        /// Draws a card from the player deck and returns it, if the player deck is empty, shuffle cards from the discard pile into the player deck and then draw a card
        /// </summary>
        /// <returns>The spell drawn from the player deck</returns>
        public Spell GetSpell()
        {
            if (playerDeck.Count == 0)
            {
                DrawFromDiscard();
            }

            if (playerDeck.Count != 0)
            {
                Spell spell = playerDeck[Random.Range(0, playerDeck.Count)];

                playerDeck.Remove(spell);

                return spell;
            }

            return null;
        }

        private void Start()
        {
            playerDeck.Sort(HelperFunctions.RandomSort);
        }

        /// <summary>
        /// Adds card to the discard pile
        /// </summary>
        /// <param name="spell"></param>
        public void ReturnCard(Spell spell)
        {
            discardPile.Add(spell);
        }

        /// <summary>
        /// Shuffled cards in the discard pile and then adds them to the player deck
        /// </summary>
        void DrawFromDiscard()
        {
            discardPile.Sort(HelperFunctions.RandomSort);

            foreach (Spell spell in discardPile)
            {
                playerDeck.Add(spell);
            }

            discardPile.Clear();
        }
    }
}