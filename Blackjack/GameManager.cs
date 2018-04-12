﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackjack.Enums;
using Blackjack.Exceptions;
using Blackjack.Interfaces;

namespace Blackjack {
    public class GameManager
    {
        /// <summary>
        /// 
        /// </summary>
        private IInputProvider iinputProvider;

        private IOutputProvider ioutputProvider;

        private PlayerManager playerManager;

        private ICard card;

        IMoveProvider moveProvider;
        int currentPlayerIndex;

        GameState State;

        IPlayer dealer;



        public GameManager(IOutputProvider output, IInputProvider input, PlayerManager pm, BlackjackMoveProvider moveProvider)
        {

            iinputProvider = input;
            ioutputProvider = output;
            playerManager = pm;
            this.moveProvider = moveProvider;

            State = GameState.INITIATING_ROUND;

        }

        /// <summary>
        /// Initiates a reocurring loop unitl the game state is set to TERMINATING
        /// </summary>
        public void InitiateLoop()
        {

            while (State != GameState.TERMINATING)
            {
                InitiateNewRound();
                InitiatePlayerTurn();
                CheckForWin();
            }
        }
        /// <summary>
        /// Resets the players hands to empty hands and deals each player two new cards :D
        /// </summary>
        void InitiateNewRound()
        {
            if(State == GameState.INITIATING_ROUND)
            {
                foreach (var player in playerManager.Players)
                {
                    player.Hand = new Hand();
                    playerManager.AddCardToPlayerPlayer(player);
                    playerManager.AddCardToPlayerPlayer(player);
                }
                State = GameState.INITIATING_TURN;
            }
        }



        /// <summary>
        /// Loops through players and applies 
        /// </summary>
        void InitiatePlayerTurn()
        {
            if(State == GameState.INITIATING_TURN)
            {
                foreach (var player in playerManager.Players)
                {
                    moveProvider.SetCurrentplayer(player);
                    moveProvider.InitiateLoop();
                    State = GameState.WIN_CHECKING;
                }
            }
        }

        void CheckForWin()
        {
            if(State == GameState.WIN_CHECKING)
            {

            }
        }




        public void AssignInitialCardsToPlayer()
        {
            players[0].Hand.Cards.Add(deck.Draw());
            players[0].Hand.Cards.Add(deck.Draw());
        } 


        public void AssignInitialCardsToDealer()
        {
            players[1].Hand.Cards.Add(deck.Draw());
            players[1].Hand.Cards.Add(deck.Draw());
        }

        /// <summary>
        /// After typing Deal, HumanPlayer is assigned 2 cards which are displayed along with their total on the screen
        /// </summary>
        public void TypeDealAndShowCards()
        {
            ioutputProvider.WriteLine("Type Deal to have cards dealt");
            iinputProvider.Read();

            AssignInitialCardsToPlayer();
            AssignInitialCardsToDealer();

            ioutputProvider.WriteLine($"Dealer: [{players[1].Hand.Cards[0].GetIntValue()}] [{players[1].Hand.Cards[1].GetIntValue()}]");
            ioutputProvider.WriteLine($"{players[0].Name}: [{players[0].Hand.Cards[0].GetIntValue()}] [{players[0].Hand.Cards[1].GetIntValue()}] - {players[0].Hand.GetTotalValue(players[0].Hand.Cards)}");

        }

    }
}
