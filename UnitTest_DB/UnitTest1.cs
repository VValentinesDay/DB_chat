
using System.Net;
using DB_chat;
using DB_chat.Abctraction;

using NUnit.Framework.Legacy;

namespace UnitTest_DB;
public class MockMessageSource : IMessageSource
{
    private Queue<MessageUDP> messages = new();

    public MockMessageSource()
    {
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Register,
            FromName =
        "Вася"
        });
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Register,
            FromName = "Юля"
        });
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Message,
            FromName = "Юля",
            ToName = "Вася",
            Text = "От Юли"
        });
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Message,
            FromName =
            "Вася",
            ToName = "Юля",
            Text = "От Васи"
        });
    }
    //public MessageUDP RecieveMessage(ref IPEndPoint iPEndPoint) // подмена/иммитация метода
    //{ return messages.Peek(); }

    public MessageUDP RecieveMessage(ref IPEndPoint iPEndPoint)
    { return messages.Dequeue(); }



    public void Send(MessageUDP message, IPEndPoint iPEndPoint)
    {
        messages.Enqueue(message);
    }



}



public class Tests
{
    IMessageSource _source;
    IPEndPoint _endPoint;

    [SetUp]

    public void Setup()
    {
        _source = new MockMessageSource();
        _endPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    [Test]
    public void TestRecieve()
    {
        var result = _source.RecieveMessage(ref _endPoint);
        Assert.That(result, Is.Not.Null);
        ClassicAssert.IsNull(result.Text);
        ClassicAssert.IsNotNull(result.FromName);
        ClassicAssert.AreEqual(result.FromName, "Вася");
        Assert.That(Command.Register, Is.EqualTo(result.Command));

    }

    [Test]
    public void Register()
    {
        // Проверка зарегистрированн ли Вася
        _source = new MockMessageSource();
        
        var result = _source.RecieveMessage(ref _endPoint);

        ClassicAssert.IsNotNull(result);
        ClassicAssert.IsNull(result.Text);
        ClassicAssert.That(result.FromName, Is.EqualTo("Вася"));
        ClassicAssert.That(result.Command, Is.EqualTo(Command.Register));
        ClassicAssert.Pass();
    }
}