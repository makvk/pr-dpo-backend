using System.ComponentModel.DataAnnotations;
using HelpDesk.Results.Models;

namespace HelpDesk.Results.Services;

public class InMemoryTicketRepository : ITicketRepository
{
    private readonly List<Ticket> tickets = [
        new Ticket(1, "Bug", "Open", 5, DateTime.Now),
        new Ticket(2, "Bug", "Open", 4, DateTime.Now),
        new Ticket(3, "Bug", "Open", 6, DateTime.Now),
    ]; 

    public IEnumerable<Ticket> GetAll()
    {
        return tickets; 
    }

    public Ticket? GetById(int id)
    {
        foreach (var t in tickets)
        {
            if (t.Id == id)
            {
                return t;
            }
        }
        return null;
    }

    public Ticket Create(string title, int priority)
    {
        int id = 0;
        if (tickets.Count > 0) {
            id = tickets.Last().Id + 1;
        }
        Ticket currentTicket = new(id, title, "Open", priority, DateTime.Now);

        tickets.Add(currentTicket);
        return currentTicket;
    }
}