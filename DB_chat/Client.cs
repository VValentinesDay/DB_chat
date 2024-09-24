using DB_chat.Abctraction;
using System.Net;


namespace DB_chat
{
    public class Client
    {
        // для полей private readonly - нижнее подчёркивание и маленькая буква
        private readonly string _name;
        private readonly IMessageSource _messageSource;
        private readonly IPEndPoint _IPEndPoint;

        public Client(string name, IMessageSource messageSource, IPEndPoint iPEndPoint)
        {
            _messageSource = messageSource;
            _IPEndPoint = iPEndPoint;
            _name = name;
        }

        public void ClientSendler()
        {


            while (true)
            {
                Console.WriteLine("Введите сообщение: ");
                string text = Console.ReadLine();
                Console.WriteLine("Введите имя получателя: ");
                string toUser = Console.ReadLine();

                // если то что в скобках выполняется то (true)
                if (string.IsNullOrEmpty(toUser))
                {
                    continue; // пропускает текущую итерацию, начинает цикл заново
                }


                MessageUDP messageUDP = new MessageUDP();
                messageUDP.FromName = _name;
                messageUDP.ToName = toUser;
                messageUDP.Text = text;
                _messageSource.Send(messageUDP, _IPEndPoint);

            }

        }

        // перегрузка метода для тестирования
        public void ClientSendler(MessageUDP message)
        {
            if (message.Command == Command.Message)
            {
                _messageSource.Send(message, _IPEndPoint);
            }
            else
            {
                Console.WriteLine("Error");
            }
        }



        public void ClientListener()
        {
           Register(); // принимать сообщения могу только если я зареган
            IPEndPoint ep = new IPEndPoint
                (_IPEndPoint.Address,
                _IPEndPoint.Port);

            while (true)
            {
                MessageUDP recivedMessage = _messageSource.RecieveMessage(ref ep);
                Console.WriteLine
                    ($"Время: {DateTime.Now} \n" +
                    $"Получено сообщение от: {recivedMessage.FromName} \n" +
                    $"Текст: {recivedMessage.Text}");
            }

        }

        // перегрузка метода для тестирования

        public string ClientListener(bool test)
        {
  
            IPEndPoint ep = new IPEndPoint
                (_IPEndPoint.Address,
                _IPEndPoint.Port);

            while (true)
            {
                MessageUDP recivedMessage = _messageSource.RecieveMessage(ref ep);
                return "Ok";
            }

        }

        private void Register()
        {
            var msg = new MessageUDP()
            { Command = Command.Register, FromName = _name };

            // отпрвка
            _messageSource.Send(msg, _IPEndPoint);
        }
    }
}
