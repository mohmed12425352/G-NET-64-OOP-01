using System;

namespace OOPAssignment06
{
    public partial class Cinema
    {
        public void PrintAllTickets()
        {
            Console.WriteLine("\n--- All Tickets (from Cinema.Reporting) ---");
            for (int i = 0; i < count; i++)
            {
                tickets[i]?.Print();
            }
        }
    }
}
