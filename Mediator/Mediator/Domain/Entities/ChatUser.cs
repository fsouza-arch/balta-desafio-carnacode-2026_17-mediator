using Mediator.Domain.Interfaces;

namespace Mediator.Domain.Entities;

public class ChatUser
{
    public string Name { get; }
    public bool IsMuted { get; set; }
    private IChatMediator _mediator;

    public ChatUser(string name)
    {
        Name = name;
    }

    public void SetMediator(IChatMediator mediator) => _mediator = mediator;

    // O usuário não conhece os outros, apenas o Mediador
    public void Send(string message) => _mediator.SendMessage(message, this);

    public void SendPrivate(string recipientName, string message) =>
        _mediator.SendPrivateMessage(message, this, recipientName);

    public void RequestMute(string targetName) => _mediator.MuteUser(this, targetName);

    public void Leave() => _mediator.UnregisterUser(this);

    // Métodos de recebimento (chamados pelo Mediador)
    public void ReceiveMessage(string sender, string message) =>
        Console.WriteLine($"  → [{Name}] Mensagem de {sender}: {message}");

    public void ReceivePrivateMessage(string sender, string message) =>
        Console.WriteLine($"  → [{Name}] 🔒 Privada de {sender}: {message}");

    public void ReceiveNotification(string notification) =>
        Console.WriteLine($"  → [{Name}] {notification}");
}
