﻿namespace ChessWebApplication.Hubs.Game
{
    using System;
    using System.Threading.Tasks;

    using ChessSystem.Application.Games.Commands.SaveCastlingMove;
    using ChessSystem.Application.Games.Commands.SaveMove;
    using ChessSystem.Domain.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;

    public class GameHub : BaseMediatorHub
    {
        public GameHub(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        public async Task UserHasMadeCastlingMove(
            string userThatHasMadeCastlingId,
            string opponentId,
            string kingPositionHorizontal,
            string kingPositionVertical,
            string rookPositionHorizontal,
            string rookPositionVertical,
            string figureColor)
        {
            await this.Clients.User(opponentId).SendAsync(
                "OpponentHasMadeCastlingMove",
                opponentId,
                kingPositionHorizontal,
                kingPositionVertical,
                rookPositionHorizontal,
                rookPositionVertical,
                figureColor);

            if (figureColor == "White")
            {
                await this.Mediator.Send(new SaveCastlingMoveCommand(
                    userThatHasMadeCastlingId,
                    opponentId,
                    new SaveCastlingMoveInputModel(
                        Enum.Parse<ChessFigureColor>(figureColor),
                        kingPositionHorizontal[0],
                        int.Parse(kingPositionVertical),
                        rookPositionHorizontal[0],
                        int.Parse(rookPositionVertical))));
            }
            else
            {
                await this.Mediator.Send(new SaveCastlingMoveCommand(
                    opponentId,
                    userThatHasMadeCastlingId,
                    new SaveCastlingMoveInputModel(
                        Enum.Parse<ChessFigureColor>(figureColor),
                        kingPositionHorizontal[0],
                        int.Parse(kingPositionVertical),
                        rookPositionHorizontal[0],
                        int.Parse(rookPositionVertical))));
            }
        }

        public async Task UserHasMadeMove(
            string userThatHasMadeTheMove,
            string opponentId,
            string initialPositionHorizontal,
            string initialPositionVertical,
            string targetPositionHorizontal,
            string targetPositionVertical,
            string figureType,
            string figureColor)
        {
            if (figureColor == "White")
            {
                await this.Mediator.Send(new SaveNormalMoveCommand(
                    userThatHasMadeTheMove,
                    opponentId,
                    new SaveNormalMoveInputModel(
                        initialPositionHorizontal[0],
                        int.Parse(initialPositionVertical),
                        targetPositionHorizontal[0],
                        int.Parse(targetPositionVertical),
                        Enum.Parse<ChessFigureType>(figureType),
                        Enum.Parse<ChessFigureColor>(figureColor))));
            }
            else
            {
                await this.Mediator.Send(new SaveNormalMoveCommand(
                    opponentId,
                    userThatHasMadeTheMove,
                    new SaveNormalMoveInputModel(
                        initialPositionHorizontal[0],
                        int.Parse(initialPositionVertical),
                        targetPositionHorizontal[0],
                        int.Parse(targetPositionVertical),
                        Enum.Parse<ChessFigureType>(figureType),
                        Enum.Parse<ChessFigureColor>(figureColor))));
            }

            await this.Clients.User(opponentId).SendAsync(
                "OpponentHasMadeMove",
                opponentId,
                initialPositionHorizontal,
                initialPositionVertical,
                targetPositionHorizontal,
                targetPositionVertical,
                figureType,
                figureColor);
        }
    }
}
