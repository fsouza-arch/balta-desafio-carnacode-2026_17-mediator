using Mediator.Domain.Entities;
using Mediator.Infrastructure.Mediators;

Console.WriteLine("=== Chat Room (Padrão Mediator) ===\n");

var chatRoom = new ChatRoom();

var alice = new ChatUser("Alice");
var bob = new ChatUser("Bob");
var carlos = new ChatUser("Carlos");

chatRoom.RegisterUser(alice);
chatRoom.RegisterUser(bob);
chatRoom.RegisterUser(carlos);

Console.WriteLine("\n--- Conversa Pública ---");
alice.Send("Olá a todos!");
bob.Send("Oi Alice!");

Console.WriteLine("\n--- Filtro de Conteúdo ---");
carlos.Send("Isso é um spam!"); // Será bloqueado pelo mediador

Console.WriteLine("\n--- Mensagem Privada ---");
alice.SendPrivate("Bob", "Você viu o novo padrão de projeto?");

Console.WriteLine("\n--- Moderação Centralizada ---");
alice.RequestMute("Carlos");
carlos.Send("Ainda posso falar?"); // Bloqueado por estar mutado

Console.WriteLine("\n--- Saída de Usuário ---");
bob.Leave();
alice.Send("O Bob saiu?");