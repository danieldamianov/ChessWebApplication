﻿namespace ChessSystem.Application.Games.Commands.CreateGame
{
    using System.Threading;
    using System.Threading.Tasks;
    using ChessSystem.Application.Common.Interfaces;
    using ChessSystem.Domain.Entities;
    using MediatR;

    /// <summary>
    /// Command that handles creating a new game.
    /// </summary>
    public class CreateGameCommand : IRequest<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGameCommand"/> class.
        /// </summary>
        /// <param name="whitePlayerId">The Id of the white player.</param>
        /// <param name="blackPlayerId">The Id of the black player.</param>
        public CreateGameCommand(string whitePlayerId, string blackPlayerId)
        {
            this.WhitePlayerId = whitePlayerId;
            this.BlackPlayerId = blackPlayerId;
        }

        /// <summary>
        /// Gets or sets the White Player Id.
        /// </summary>
        public string WhitePlayerId { get; set; }

        /// <summary>
        /// Gets or sets the Black Player Id.
        /// </summary>
        public string BlackPlayerId { get; set; }

        /// <summary>
        /// Handler for the command.
        /// </summary>
        public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, string>
        {
            private readonly IChessApplicationData chessApplicationData;

            /// <summary>
            /// Initializes a new instance of the <see cref="CreateGameCommandHandler"/> class.
            /// </summary>
            /// <param name="chessApplicationData">DI generetad.</param>
            public CreateGameCommandHandler(IChessApplicationData chessApplicationData)
            {
                this.chessApplicationData = chessApplicationData;
            }

            /// <summary>
            /// Handler.
            /// </summary>
            /// <param name="request">request.</param>
            /// <param name="cancellationToken">cancellationToken.</param>
            /// <returns>result.</returns>
            public async Task<string> Handle(CreateGameCommand request, CancellationToken cancellationToken)
            {
                ChessGame chessGame = new ChessGame(request.WhitePlayerId, request.BlackPlayerId);
                this.chessApplicationData.ChessGames.Add(chessGame);

                await this.chessApplicationData.SaveChanges(cancellationToken);

                return chessGame.Id;
            }
        }
    }
}
