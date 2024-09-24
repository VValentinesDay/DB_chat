
using System.Net;
using DB_chat;
using DB_chat.Abctraction;

using NUnit.Framework.Legacy;

namespace UnitTest_DB;
//public class MockMessageSource : IMessageSource
//{
//    private Queue<MessageUDP> messages = new();

//    public MockMessageSource()
//    {
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Register,
//            FromName =
//        "Вася"
//        });
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Register,
//            FromName = "Юля"
//        });
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Message,
//            FromName = "Юля",
//            ToName = "Вася",
//            Text = "От Юли"
//        });
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Message,
//            FromName =
//            "Вася",
//            ToName = "Юля",
//            Text = "От Васи"
//        });
//        messages.Enqueue(new MessageUDP // последнее сообщение в очереди
//        {
//            Command = Command.Message,
//            FromName =
//            "Вася",
//            ToName = "0",// незарегистрированный пользователь
//            Text = "От Васи"
//        });
//        messages.Enqueue(new MessageUDP // последнее сообщение в очереди
//        {
//            Command = Command.Register,
//            FromName =
//         "0"// незарегистрированный пользователь
         
//        });
//    }
//    public MessageUDP RecieveMessage(ref IPEndPoint iPEndPoint) // подмена/иммитация метода
//    { return messages.Dequeue(); }

//    //public MessageUDP ModerRecieveMessage(ref IPEndPoint iPEndPoint)
//    //{ return messages.Dequeue(); }



//    public void Send(MessageUDP message, IPEndPoint iPEndPoint)
//    {
//        messages.Enqueue(message);
//    }


//}



public class Tests
{
    IMessageSource _source;
    IPEndPoint _endPoint;

    [SetUp]

    public void Setup()
    {
        //_source = new MockMessageSource();
        //_endPoint = new IPEndPoint(IPAddress.Any, 0);
    }
    
    //[Test]
    //public void TestRecieve()
    //{
    //   var result = _source.RecieveMessage(ref _endPoint);
    //    Assert.That(result, Is.Not.Null);
    //    ClassicAssert.IsNull(result.Text);
    //    ClassicAssert.IsNotNull(result.FromName);
    //    ClassicAssert.AreEqual(result.FromName, "Вася");
    //    Assert.That(Command.Register, Is.EqualTo(result.Command));

    //}


    //[Test]
    //public void TestRegistration()
    //{
    //    MessageUDP result = new MessageUDP();
    //    int count = 0;

    //    for (int i = 0; i < 5; i++)
    //    {
    //        count++;
    //        result = _source.RecieveMessage(ref _endPoint);
    //    }
    //    // проверка, что пришло пятое сообщение
    //    ClassicAssert.AreEqual(5, count); 
    //    ClassicAssert.AreEqual(result.ToName,"0"); 

    //    // проверка получил ли 0 ссобщение. Если да, то от него сработал метол прослушки и выслал обратно сообщение. Однако ю-тесты занимаются изолированными объектами


    //}


    [Test]
    public void ReplyTest()
    {
        // проверка получения ответа на сообщение
        Client client_Sanya = new Client("саня", _source, new IPEndPoint(IPAddress.Any, 12345));
        Client client_Vova = new Client("Вова", _source, new IPEndPoint(IPAddress.Any, 12345));


        client_Sanya.ClientListener();
        MessageUDP msg= new MessageUDP() { Command = Command.Message, ToName = "Вова", Text = "Привет" };
        client_Vova.ClientListener();
        // отправление сообщения

        client_Vova.ClientSendler(msg);
        Assert.Equals(client_Sanya.ClientListener(true), "Ok");


    }




}













    //[Test]
    //public void Test1()
    //{
    //    var mock = new MockMessageSource();
    //    var srv = new Server(mock);
    //    mock.AddServer(srv);
    //    srv.Work();
    //    using (var ctx = new Lection15Program6.Model.TestContext())
    //    {
    //        Assert.IsTrue(ctx.Users.Count() == 2, "Пользователи не созданы");
    //        var user1 = ctx.Users.FirstOrDefault(x => x.Name == "Вася");
    //        var user2 = ctx.Users.FirstOrDefault(x => x.Name == "Юля");
    //        Assert.IsNotNull(user1, "Пользователь не созданы");
    //        Assert.IsNotNull(user2, "Пользователь не созданы");
    //        Assert.IsTrue(user1.FromMessages.Count == 1);
    //        Assert.IsTrue(user2.FromMessages.Count == 1);
    //        Assert.IsTrue(user1.ToMessages.Count == 1);
    //        Assert.IsTrue(user2.ToMessages.Count == 1);
    //        var msg1 = ctx.Messages.FirstOrDefault(x => x.FromUser == user1 &&
    //        x.ToUser == user2);
    //        var msg2 = ctx.Messages.FirstOrDefault(x => x.FromUser == user2 &&
    //        x.ToUser == user1);
    //        Assert.AreEqual("От Юли", msg2.Text);
    //        Assert.AreEqual("От Васи", msg1.Text);
    //    }
    //}
