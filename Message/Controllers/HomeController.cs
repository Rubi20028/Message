using Message.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Message.Controllers;

public class HomeController : Controller
{
    DAL.DAL.Message _message = new DAL.DAL.Message();
    private static int _sequenceCounter = 0;
    private readonly IHubContext<MessageHub> _hubContext;
    
    public HomeController(IHubContext<MessageHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Client1()
    {
        ViewData["Title"] = "Client1";
        return View();
    }
    
    public IActionResult Client2()
    {
        ViewData["Title"] = "Client2";
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> SubmitMessage([FromBody] MessageRequest.MessageRequest request)
    {
        if (request.Text == null || request.Text.Length > 128)
        {
            return BadRequest("Text must be between 1 and 128 characters.");
        }
        
        var message = new Domain.Models.Message
        {
            MessageId = 0,
            Text = request.Text,
            TimeStamp = DateTime.Now,
            SequenceNumber = Interlocked.Increment(ref _sequenceCounter)
        };
        
        _message.InsertMessage(message);
        
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", new 
        {
            sequenceNumber = message.SequenceNumber,
            text = message.Text,
            timeStamp = message.TimeStamp
        });
        
        return Ok();
    }

    [HttpGet]
    public IActionResult Client3()
    {
        var messagesList = _message.GetAllMessages();
        return View(messagesList);
    }
    
}