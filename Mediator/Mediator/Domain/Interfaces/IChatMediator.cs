using Mediator.Domain.Entities;

namespace Mediator.Domain.Interfaces;

public interface IChatMediator
{
    void RegisterUser(ChatUser user);
    void UnregisterUser(ChatUser user);
    void SendMessage(string message, ChatUser sender);
    void SendPrivateMessage(string message, ChatUser sender, string recipientName);
    void MuteUser(ChatUser moderator, string targetName);
    void Notify(string message, ChatUser excludeUser = null);
}
