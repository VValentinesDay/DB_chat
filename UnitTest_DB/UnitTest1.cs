
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
        "����"
        });
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Register,
            FromName = "���"
        });
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Message,
            FromName = "���",
            ToName = "����",
            Text = "�� ���"
        });
        messages.Enqueue(new MessageUDP
        {
            Command = Command.Message,
            FromName =
            "����",
            ToName = "���",
            Text = "�� ����"
        });
    }
    //public MessageUDP RecieveMessage(ref IPEndPoint iPEndPoint) // �������/��������� ������
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
        ClassicAssert.AreEqual(result.FromName, "����");
        Assert.That(Command.Register, Is.EqualTo(result.Command));

    }

    [Test]
    public void Register()
    {
        // �������� ���������������� �� ����
        _source = new MockMessageSource();
        
        var result = _source.RecieveMessage(ref _endPoint);

        ClassicAssert.IsNotNull(result);
        ClassicAssert.IsNull(result.Text);
        ClassicAssert.That(result.FromName, Is.EqualTo("����"));
        ClassicAssert.That(result.Command, Is.EqualTo(Command.Register));
        ClassicAssert.Pass();
    }
}