using DB_chat.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DB_chat
{
    class Server
    {
        // словарь для хранения адресов клиентов по их именам
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        // Объект для работы с UDP-сокетом
        UdpClient udpClient;

        // методя для регистрациии ноаого клиента
        void Register(MessageUDP message, IPEndPoint fromep)
        {
            Console.WriteLine("Message Register, name = " + message.FromName);
            // Добавление клиентов в словарь
            clients.Add(message.FromName, fromep);
            // Добавление пользователя в БД, если его ещё нет
            using (var ctx = new Context())
            {
                if (ctx.Users.FirstOrDefault(x => x.Name == message.FromName) != null) return;
                ctx.Add(new User { Name = message.FromName });
                ctx.SaveChanges();

            }
        }

        // метод для подтверждения получения сообщения
        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id = " + id);
            // изменение стутуса получения сообщения в БД
            using (var ctx = new Context())
            {
                var msg = ctx.Messages.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.Received = true;
                    ctx.SaveChanges();
                }
            }
        }

        // метод для пересылки сообщения
        void RelyMessage(MessageUDP message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out IPEndPoint ep))
            {
                //добавление сообщения в БД
                using (var ctx = new Context())
                {

                    var fromUser = ctx.Users.FirstOrDefault(x => x.Name == message.FromName);
                    var toUser = ctx.Users.FirstOrDefault(y => y.Name == message.ToName);
                    var msg = new Message
                    {
                        FromUser = fromUser,
                        ToUser = toUser,
                        Received = false,
                        Text = message.Text
                    };

                    ctx.Messages.Add(msg);

                    ctx.SaveChanges();

                    id = msg.Id;
                }

                // Подготовка сообщения к пересылке
                var forvardMessageJson = new MessageUDP()
                {
                    Id = id,
                    Command = Command.Message,
                    ToName = message.ToName,
                    FromName = message.FromName,
                    Text = message.Text
                }.ToJson();
                byte[] forvardBytes = Encoding.UTF8.GetBytes(forvardMessageJson);
                // отправление сообщения
                udpClient.Send(forvardBytes, forvardBytes.Length, ep);
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");

            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
        }
        // Метод для обработки полученного сообщения
        void ProcessMessage(MessageUDP message, IPEndPoint fromep)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой {message.Command}:");
            Console.WriteLine(message.Text);

            // Обработка в зависимости от типа сообщения

            if (message.Command == Command.Register)
            {
                Register(message, new IPEndPoint(fromep.Address, fromep.Port));
            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }

        }

        // Метод для запуска сервера 
        public void Work()
        {
            // Инициализация объекта для приёма данных по UDP
            IPEndPoint remoteEndPoint;
            udpClient = new UdpClient(12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("UDP Клиент ожидает сообщений...");
            // Бесконечный цикл приема сообщений
            while (true)
            {
                byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                string receivedData = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine(receivedData);
                try
                {
                    // Десериализация полученного сообщения
                    var message = MessageUDP.FromJson(receivedData);
                    // Обработка сообщения
                    ProcessMessage(message, remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }
    }
}
