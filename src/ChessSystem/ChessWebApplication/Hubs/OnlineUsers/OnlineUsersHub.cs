﻿namespace ChessWebApplication.Hubs.OnlineUsers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChessSystem.Application.Common.Interfaces;
    using ChessSystem.Application.OnlineUsers.Commands.AddOnlineUser;
    using ChessSystem.Application.OnlineUsers.Commands.RemoveOnlineUser;
    using ChessSystem.Application.OnlineUsers.Queries.GetAllOnlineUsers;
    using ChessWebApplication.Controllers.Game.InputModels;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Hub That manages which users are online and searching for opponents.
    /// </summary>
    public class OnlineUsersHub : Hub
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;
        private IMediator mediator;

        public OnlineUsersHub(
            IHttpContextAccessor httpContextAccessor,
            ICurrentUser currentUser,
            IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        protected IMediator Mediator
            => this.mediator ??= (IMediator)this.httpContextAccessor.HttpContext
                .RequestServices
                .GetService(typeof(IMediator));

        public async override Task OnConnectedAsync()
        {
            await this.Mediator.Send(new AddOnlineUserCommand(this.currentUser.UserId, this.Context.User.Identity.Name));

            await this.Clients.Others.SendAsync("NewUser", new OnlineUserSocketModel(this.Context.User.Identity.Name, this.currentUser.UserId));

            GetAllOnlineUsersOutputModel getAllOnlineUsersOutputModel = await this.mediator.Send(new GetAllOnlineUsersQuery());
            foreach (var client in getAllOnlineUsersOutputModel.OnlineUsers)
            {
                if (client.UserId != this.currentUser.UserId)
                {
                    await this.Clients.Caller.SendAsync("NewUser", new OnlineUserSocketModel(client.Username, client.UserId));
                }
            }
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await this.Mediator.Send(new RemoveOnlineUserCommand(this.currentUser.UserId));

            await this.Clients.All.SendAsync("UserDisconnected", new OnlineUserSocketModel(this.Context.User.Identity.Name, this.currentUser.UserId));

        }

        [Authorize]
        public async Task InviteUserToPlay(string invitedId)
        {
            await this.Clients.User(invitedId).SendAsync(
                "HandleInvitation",
                new OnlineUserSocketModel(this.Context.User.Identity.Name, this.currentUser.UserId));
        }

        [Authorize]
        public async Task AcceptGame(string opponentId)
        {
            var modelStartGame = new PlayInputModel()
            {
                BlackPlayerId = this.currentUser.UserId,
                WhitePlayerId = opponentId,
            };

            await this.Clients.Caller.SendAsync("StartGameAsBlack", modelStartGame);
            await this.Clients.User(opponentId).SendAsync("StartGameAsWhite", modelStartGame);
        }
    }
}
