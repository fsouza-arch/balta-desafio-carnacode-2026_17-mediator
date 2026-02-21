using Mediator.Domain.Entities;
using Mediator.Domain.Interfaces;

namespace Mediator.Infrastructure.Mediators;

public class ChatRoom : IChatMediator
{
    private readonly List<ChatUser> _users = new List<ChatUser>();
    private readonly List<string> _forbiddenWords = new List<string> { "spam", "ofensa" };

    public void RegisterUser(ChatUser user)
    {
        if (!_users.Contains(user))
        {
            _users.Add(user);
            user.SetMediator(this);
            Notify($"ℹ️ {user.Name} entrou na sala", user);
        }
    }

    public void UnregisterUser(ChatUser user)
    {
        if (_users.Contains(user))
        {
            _users.Remove(user);
            Notify($"ℹ️ {user.Name} saiu da sala");
        }
    }

    public void SendMessage(string message, ChatUser sender)
    {
        // Lógica Centralizada: Filtro de Palavras
        foreach (var word in _forbiddenWords)
        {
            if (message.ToLower().Contains(word))
            {
                sender.ReceiveNotification("⚠️ Sua mensagem foi bloqueada pelo filtro de conteúdo.");
                return;
            }
        }

        // Lógica Centralizada: Verificação de Mute
        if (sender.IsMuted)
        {
            sender.ReceiveNotification("❌ Você está mutado e não pode enviar mensagens.");
            return;
        }

        // Distribuição da mensagem
        foreach (var user in _users)
        {
            if (user != sender)
            {
                user.ReceiveMessage(sender.Name, message);
            }
        }
    }

    public void SendPrivateMessage(string message, ChatUser sender, string recipientName)
    {
        var recipient = _users.FirstOrDefault(u => u.Name == recipientName);
        if (recipient != null)
        {
            recipient.ReceivePrivateMessage(sender.Name, message);
        }
        else
        {
            sender.ReceiveNotification($"❌ Usuário {recipientName} não encontrado.");
        }
    }

    public void MuteUser(ChatUser moderator, string targetName)
    {
        // Lógica Centralizada: Permissões (Apenas Alice é moderadora neste exemplo)
        if (moderator.Name != "Alice")
        {
            moderator.ReceiveNotification("❌ Você não tem permissão para mutar usuários.");
            return;
        }

        var target = _users.FirstOrDefault(u => u.Name == targetName);
        if (target != null)
        {
            target.IsMuted = true;
            Notify($"🚫 {target.Name} foi mutado por {moderator.Name}");
        }
    }
    public void Notify(string message, ChatUser excludeUser = null)
    {
        foreach (var user in _users)
        {
            if (user != excludeUser)
            {
                user.ReceiveNotification(message);
            }
        }
    }
}