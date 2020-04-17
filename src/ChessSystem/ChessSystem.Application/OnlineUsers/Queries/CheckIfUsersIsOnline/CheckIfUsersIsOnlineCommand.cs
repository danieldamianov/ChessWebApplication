﻿using ChessSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessSystem.Application.OnlineUsers.Queries.CheckIfUserIsOnline
{
    public class CheckIfUsersIsOnlineCommand : IRequest<bool>
    {
        public CheckIfUsersIsOnlineCommand(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }

        public class CheckIfUserIsOnlineCommandHandler : IRequestHandler<CheckIfUsersIsOnlineCommand, bool>
        {
            private readonly IChessApplicationData chessApplicationData;

            public CheckIfUserIsOnlineCommandHandler(IChessApplicationData chessApplicationData)
            {
                this.chessApplicationData = chessApplicationData;
            }

            public async Task<bool> Handle(CheckIfUsersIsOnlineCommand request, CancellationToken cancellationToken)
                => await this.chessApplicationData.LogedInUsers.AnyAsync(user => user.UserId == request.UserId);
        }
    }
}
