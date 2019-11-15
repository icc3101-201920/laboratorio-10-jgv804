using Laboratorio_9_OOP_201920.Cards;
using Laboratorio_9_OOP_201920.Enums;
using System.Collections.Generic;
using System;

using System.Text;

namespace Laboratorio_9_OOP_201920
{
    public static class Effect
    {
        private static Dictionary<EnumEffect, string> effectDescriptions = new Dictionary<EnumEffect, string>()
        {
            { EnumEffect.bitingFrost, "Sets the strength of all melee cards to 1 for both players" },
            { EnumEffect.impenetrableFog, "Sets the strength of all range cards to 1 for both players" },
            { EnumEffect.torrentialRain, "Sets the strength of all longRange cards to 1 for both players" },
            { EnumEffect.clearWeather, "Removes all Weather Card (Biting Frost, Impenetrable Fog and Torrential Rain) effects" },
            { EnumEffect.moraleBoost, "Adds +1 to all units in the row (excluding itself)" },
            { EnumEffect.spy, "Place on your opponent's battlefield (counts towards opponent's total) and draw 2 cards from your deck" },
            { EnumEffect.tightBond, "Place next to a card with the same name to double the strength of both cards" },
            { EnumEffect.buff, "Doubles the strength of all unit cards in that row. Limited to 1 per row" },
            { EnumEffect.none, "None" },
        };

        private static List<EnumType> buffLines = new List<EnumType>() { EnumType.buffmelee, EnumType.buffrange, EnumType.bufflongRange };
        private static List<EnumType> lines = new List<EnumType>() { EnumType.melee, EnumType.range, EnumType.longRange };

        public static string GetEffectDescription(EnumEffect e)
        {
            return effectDescriptions[e];
        }

        public static void ApplyEffect(Card playedCard, Player activePlayer, Player opponent, Board board)
        {
            switch (playedCard.CardEffect)
            {
                case EnumEffect.bitingFrost:
                    for (int i = 0; i<2; i++)
                    {
                        EnumType type = EnumType.melee;
                        if (board.PlayerCards[i].ContainsKey(type))
                        {
                            foreach (CombatCard card in board.PlayerCards[i][type])
                            {
                                if (!card.Hero) card.AttackPoints = 1;
                            }
                        }
                    }
                    break;
                case EnumEffect.impenetrableFog:
                    for (int i = 0; i < 2; i++)
                    {
                        EnumType type = EnumType.range;
                        if (board.PlayerCards[i].ContainsKey(type))
                        {
                            foreach (CombatCard card in board.PlayerCards[i][type])
                            {
                                if (!card.Hero) card.AttackPoints = 1;
                            }
                        }
                    }
                    break;
                case EnumEffect.torrentialRain:
                    for (int i = 0; i < 2; i++)
                    {
                        EnumType type = EnumType.longRange;
                        if (board.PlayerCards[i].ContainsKey(type))
                        {
                            foreach (CombatCard card in board.PlayerCards[i][type])
                            {
                                if (!card.Hero) card.AttackPoints = 1;
                            }
                        }
                    }
                    break;
                case EnumEffect.clearWeather:
                    for (int i = 0; i < 2; i++)
                    {
                        board.WeatherCards = new List<SpecialCard>();
                        foreach (EnumType type in new List<EnumType>(){ EnumType.melee, EnumType.range, EnumType.longRange})
                        {
                            if (board.PlayerCards[i].ContainsKey(type))
                            {
                                foreach (CombatCard card in board.PlayerCards[i][type])
                                {
                                    if (!card.Hero) card.AttackPoints = card.OriginalAttackPoints;
                                }
                            }  
                        }

                    }
                    break;
                case EnumEffect.buff:

                    for (int i=0;i<1;i++)
                    {
                        
                        if (board.PlayerCards[activePlayer.Id].ContainsKey(EnumType.buffmelee))
                        {
                            foreach (CombatCard card in board.PlayerCards[activePlayer.Id][EnumType.melee])
                            {
                               
                                if (!card.Hero) card.AttackPoints = card.AttackPoints+card.AttackPoints;
                            }
                        }
                        if (board.PlayerCards[activePlayer.Id].ContainsKey(EnumType.buffrange))
                        {
                            foreach (CombatCard card in board.PlayerCards[activePlayer.Id][EnumType.range])
                            {
                                
                                if (!card.Hero) card.AttackPoints = card.AttackPoints + card.AttackPoints;
                            }
                        }
                        if (board.PlayerCards[activePlayer.Id].ContainsKey(EnumType.bufflongRange))
                        {
                            foreach (CombatCard card in board.PlayerCards[activePlayer.Id][EnumType.longRange])
                            {
                                
                                if (!card.Hero) card.AttackPoints = card.AttackPoints + card.AttackPoints;
                            }
                        }
                    }
                    




                    break;

                case EnumEffect.moraleBoost:
                    foreach (EnumType type in lines)
                    {

                        if (board.PlayerCards[activePlayer.Id].ContainsKey(type))
                        {
                            foreach (CombatCard card in board.PlayerCards[activePlayer.Id][type])
                            {
                                if (!card.Hero && card.Name != playedCard.Name) card.AttackPoints += 1;
                            }
                        }
                    }
                    break;
                case EnumEffect.tightBond:
                    foreach (EnumType type in lines)
                    {

                        if (board.PlayerCards[activePlayer.Id].ContainsKey(type))
                        {
                            foreach (CombatCard card in board.PlayerCards[activePlayer.Id][type])
                            {
                                if (!card.Hero && card.Name == playedCard.Name) card.AttackPoints =card.AttackPoints+card.AttackPoints;
                            }
                        }
                    }
                    


                    break;
                case EnumEffect.spy:
                    if (activePlayer.Id == 1)
                    {
                        board.AddCard(playedCard, 0);
                        Console.WriteLine(activePlayer.Deck.Cards.Count);
                        Console.ReadKey();
                        Random random = new Random();
                        for (int i = 0; i < 2; i++)
                        {
                            activePlayer.DrawCard1(random.Next(0, activePlayer.Deck.Cards.Count));
                        }
                        
                        
                        //activePlayer.DrawCard();//
                        
                    }
                    else
                    {
                        board.AddCard(playedCard, 1);
                        Console.WriteLine(activePlayer.Deck.Cards.Count);
                        Console.ReadKey();
                        Random random = new Random();
                        for (int i = 0; i < 2; i++)
                        {
                            activePlayer.DrawCard1(random.Next(0, activePlayer.Deck.Cards.Count));
                        }
                        
                        //activePlayer.DrawCard();//
                    }
                    break;

           
                default:
                    break;
                
            }
        }
        
    }
}
