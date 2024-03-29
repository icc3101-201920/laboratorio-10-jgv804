﻿using Laboratorio_9_OOP_201920.Cards;
using Laboratorio_9_OOP_201920.Enums;
using Laboratorio_9_OOP_201920.Interfaces;
using Laboratorio_9_OOP_201920;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_9_OOP_201920
{
    [Serializable]
    public class Player: IAttackPoints
    {
        //EventHandlers
        public event EventHandler<PlayerEventArgs> CardPlayed;

        //Constantes
        private const int LIFE_POINTS = 2;
        private const int START_ATTACK_POINTS = 0;

        //Static
        private static int idCounter;

        //Atributos
        private int id;
        private int lifePoints;
        private Deck deck;
        private Hand hand;
        private Board board;
        private SpecialCard captain;
        private List<EnumType> cardEffects = new List<EnumType>() {
                EnumType.melee,
                EnumType.range,
                EnumType.longRange,
                EnumType.buffmelee,
                EnumType.buffrange,
                EnumType.bufflongRange,
        };
        //Constructor
        public Player()
        {
            LifePoints = LIFE_POINTS;
            Deck = new Deck();
            Hand = new Hand();
            Id = idCounter++;
        }

        //Propiedades
        public int Id { get => id; set => id = value; }
        public int LifePoints
        {
            get
            {
                return this.lifePoints;
            }
            set
            {
                this.lifePoints = value;
            }
        }
       
        public Deck Deck
        {
            get
            {
                return this.deck;
            }
            set
            {
                this.deck = value;
            }
        }
        public Hand Hand
        {
            get
            {
                return this.hand;
            }
            set
            {
                this.hand = value;
            }
        }
        public Board Board
        {
            get
            {
                return this.board;
            }
            set
            {
                this.board = value;
            }
        }
        public SpecialCard Captain
        {
            get
            {
                return this.captain;
            }
            set
            {
                this.captain = value;
            }
        }

        //Metodos
        public void DrawCard(int cardId = 0)
        {
            Card tempCard = CreateTempCard(cardId);
            hand.AddCard(tempCard);
            deck.DestroyCard(cardId);
            
            foreach(EnumType type in this.cardEffects)
            {
                ActivateEffectOfPlayedCards(type);
            }
            
        }
        public void DrawCard1(int cardId = 0)
        {
            Card tempCard = CreateTempCard(cardId);
            hand.AddCard(tempCard);
            deck.DestroyCard(cardId);


        }
        public void PlayCard(int cardId, EnumType buffRow = EnumType.None)
        {
            
            Card tempCard = CreateTempCard(cardId, false);

            if (tempCard is CombatCard)
            {
                board.AddCard(tempCard, this.Id);
            }
            else
            {
                if (tempCard.Type == EnumType.buff)
                {
                    board.AddCard(tempCard, this.Id, buffRow);
                }
                else
                {
                    board.AddCard(tempCard);
                }
            }
            hand.DestroyCard(cardId);
            foreach (EnumType type in this.cardEffects)
            {
                ActivateEffectOfPlayedCards(type);
            }
        }

        public void ChangeCard(int cardId)
        {
            Card tempCard = CreateTempCard(cardId, false);
            hand.DestroyCard(cardId);
            Random random = new Random();
            int deckCardId = random.Next(0, deck.Cards.Count);
            Card tempDeckCard = CreateTempCard(deckCardId);
            hand.AddCard(tempDeckCard);
            deck.DestroyCard(deckCardId);
            deck.AddCard(tempCard);
        }

        public void FirstHand()
        {
            Random random = new Random();
            for (int i = 0; i<10; i++)
            {
                DrawCard(random.Next(0, deck.Cards.Count));
            }
        }

        public void ChooseCaptainCard(SpecialCard captainCard)
        {
            Captain = captainCard;
            board.AddCard(new SpecialCard(Captain.Name, Captain.Type, Captain.CardEffect), Id);
        }

        public Card CreateTempCard(int cardId, bool useDeck = true)
        {
            Deck cardList = useDeck ? deck : hand;

            if (cardList.Cards[cardId] is CombatCard)
            {
                CombatCard card = cardList.Cards[cardId] as CombatCard;
                return new CombatCard(card.Name, card.Type, card.CardEffect, card.AttackPoints, card.Hero);
            }
            else
            {
                SpecialCard card = cardList.Cards[cardId] as SpecialCard;
                return new SpecialCard(card.Name, card.Type, card.CardEffect);
            }
        }

        public int[] GetAttackPoints(EnumType line = EnumType.None)
        {
            EnumType[] enums = new EnumType[3] { EnumType.melee, EnumType.range, EnumType.longRange };
            int attackPoints = 0;
            foreach (EnumType e in enums)
            {
                attackPoints += board.GetAttackPoints(e)[Id];
            }
            return new int[] { attackPoints };
        }

        public virtual void OnCardPlayed(Card card)
        {
            if (CardPlayed != null)
            {
                CardPlayed(this, new PlayerEventArgs() { Card = card, Player = this });
            }
        }

        private void ActivateEffectOfPlayedCards(EnumType type)
        {
            if (board.WeatherCards.Count>0)
            {
                foreach (Card card in board.WeatherCards)
                {
                    OnCardPlayed(card);
                }
                
            }
            if (board.PlayerCards[this.Id].ContainsKey(type))
            {
                foreach (Card card in board.PlayerCards[this.Id][type])
                {
                    OnCardPlayed(card);
                }
            }
            
        }
    }
}
