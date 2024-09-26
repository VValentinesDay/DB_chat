using DB_chat.Abctraction;
using DB_chat;
using System.Net;
using NUnit.Framework.Legacy;

namespace Client_test
{
    public class Tests
    {
        public class MockMessageSource : IMessageSource
        {
            private Queue<MessageUDP> messages = new Queue<MessageUDP>();


            public MockMessageSource()
            {

                messages.Enqueue(new MessageUDP() { Command = Command.Register, FromName = "User1" });
                messages.Enqueue(new MessageUDP() { Command = Command.Register, FromName = "User2" });
                messages.Enqueue(new MessageUDP() { Command = Command.Message, FromName = "User1", ToName = "User2", Text = "Hello from User1" });
                messages.Enqueue(new MessageUDP() { Command = Command.Message, FromName = "User2", ToName = "User1", Text = "Hello from User2" });
            }


            public MessageUDP RecieveMessage(ref IPEndPoint iPEndPoint)
            {
                return messages.Dequeue();
            }

            public void Send(MessageUDP MessageUDP, IPEndPoint iPEndPoint)
            {
                messages.Enqueue(MessageUDP);
            }


            IMessageSource _messageSource;
            IPEndPoint _ipEndPoint;


            [SetUp]
            public void Setup()
            {
                _ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            }

            [Test]
            public void TestRecieveMessager()
            {
                _messageSource = new MockMessageSource();

                var result = _messageSource.RecieveMessage(ref _ipEndPoint);
                
                ClassicAssert.IsNotNull(result);
                ClassicAssert.IsNull(result.Text);
                ClassicAssert.AreEqual(result.FromName, "User1");
                ClassicAssert.That(Command.Register, Is.EqualTo(result.Command));
                ClassicAssert.Pass();
            }
            [Test]
            public void RegisterMeTest()
            {
                var name = "User1";
                var MessageUDP = new MessageUDP { Command = Command.Register, FromName = name };

                _messageSource = new MockMessageSource();
                _messageSource.Send(MessageUDP, _ipEndPoint);
                var result = _messageSource.RecieveMessage(ref _ipEndPoint);

                ClassicAssert.IsNotNull(result);
                ClassicAssert.IsNull(result.Text);
                ClassicAssert.That(result.FromName, Is.EqualTo(name));
                ClassicAssert.That(result.Command, Is.EqualTo(Command.Register));
                ClassicAssert.Pass();

            }
            [Test]
            public void ClientSenderTest()
            {
                string name = "User1";

                string message = "Buuuuuuu";

                ClassicAssert.IsNotNull(message);
                ClassicAssert.IsNotEmpty(message);

                string recipientTo = "muuuuuuuuu";

                ClassicAssert.IsNotNull(recipientTo);
                ClassicAssert.IsNotEmpty(recipientTo);

                var MessageUDP = new MessageUDP { Command = Command.Message, FromName = name, Text = message, ToName = recipientTo };

                _messageSource = new MockMessageSource();

                _messageSource.Send(MessageUDP, _ipEndPoint);




                ClassicAssert.IsNotNull(MessageUDP);
                ClassicAssert.That(MessageUDP.Text, Is.EqualTo(message));
                ClassicAssert.That(MessageUDP.FromName, Is.EqualTo(name));
                ClassicAssert.That(MessageUDP.ToName, Is.EqualTo(recipientTo));
                ClassicAssert.That(MessageUDP.Command, Is.EqualTo(Command.Message));
                ClassicAssert.Pass();

            }
            [Test]
            public void ClientListener()
            {
                RegisterMeTest();

                var ipEP = new IPEndPoint(_ipEndPoint.Address, _ipEndPoint.Port);


                var mesUdp = _messageSource.RecieveMessage(ref ipEP);

                ClassicAssert.IsNotNull(mesUdp);
                ClassicAssert.That(mesUdp.Text, Is.EqualTo(null));
                ClassicAssert.That(mesUdp.FromName, Is.EqualTo("User1"));
                ClassicAssert.That(mesUdp.ToName, Is.EqualTo(null));
                ClassicAssert.That(mesUdp.Command, Is.EqualTo(Command.Register));
                ClassicAssert.Pass();

            }
        }
    }
}