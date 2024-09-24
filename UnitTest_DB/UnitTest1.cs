
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
//        "����"
//        });
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Register,
//            FromName = "���"
//        });
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Message,
//            FromName = "���",
//            ToName = "����",
//            Text = "�� ���"
//        });
//        messages.Enqueue(new MessageUDP
//        {
//            Command = Command.Message,
//            FromName =
//            "����",
//            ToName = "���",
//            Text = "�� ����"
//        });
//        messages.Enqueue(new MessageUDP // ��������� ��������� � �������
//        {
//            Command = Command.Message,
//            FromName =
//            "����",
//            ToName = "0",// �������������������� ������������
//            Text = "�� ����"
//        });
//        messages.Enqueue(new MessageUDP // ��������� ��������� � �������
//        {
//            Command = Command.Register,
//            FromName =
//         "0"// �������������������� ������������
         
//        });
//    }
//    public MessageUDP RecieveMessage(ref IPEndPoint iPEndPoint) // �������/��������� ������
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
    //    ClassicAssert.AreEqual(result.FromName, "����");
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
    //    // ��������, ��� ������ ����� ���������
    //    ClassicAssert.AreEqual(5, count); 
    //    ClassicAssert.AreEqual(result.ToName,"0"); 

    //    // �������� ������� �� 0 ���������. ���� ��, �� �� ���� �������� ����� ��������� � ������ ������� ���������. ������ �-����� ���������� �������������� ���������


    //}


    [Test]
    public void ReplyTest()
    {
        // �������� ��������� ������ �� ���������
        Client client_Sanya = new Client("����", _source, new IPEndPoint(IPAddress.Any, 12345));
        Client client_Vova = new Client("����", _source, new IPEndPoint(IPAddress.Any, 12345));


        client_Sanya.ClientListener();
        MessageUDP msg= new MessageUDP() { Command = Command.Message, ToName = "����", Text = "������" };
        client_Vova.ClientListener();
        // ����������� ���������

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
    //        Assert.IsTrue(ctx.Users.Count() == 2, "������������ �� �������");
    //        var user1 = ctx.Users.FirstOrDefault(x => x.Name == "����");
    //        var user2 = ctx.Users.FirstOrDefault(x => x.Name == "���");
    //        Assert.IsNotNull(user1, "������������ �� �������");
    //        Assert.IsNotNull(user2, "������������ �� �������");
    //        Assert.IsTrue(user1.FromMessages.Count == 1);
    //        Assert.IsTrue(user2.FromMessages.Count == 1);
    //        Assert.IsTrue(user1.ToMessages.Count == 1);
    //        Assert.IsTrue(user2.ToMessages.Count == 1);
    //        var msg1 = ctx.Messages.FirstOrDefault(x => x.FromUser == user1 &&
    //        x.ToUser == user2);
    //        var msg2 = ctx.Messages.FirstOrDefault(x => x.FromUser == user2 &&
    //        x.ToUser == user1);
    //        Assert.AreEqual("�� ���", msg2.Text);
    //        Assert.AreEqual("�� ����", msg1.Text);
    //    }
    //}
