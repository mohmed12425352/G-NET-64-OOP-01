namespace OOPAssignment06
{
    public partial class Cinema
    {
        private Ticket[] tickets = new Ticket[20];
        private int count = 0;

        public bool AddTicket(Ticket t)
        {
            if (count < tickets.Length)
            {
                tickets[count++] = t;
                return true;
            }
            return false;
        }

        public Ticket[] GetTickets() => tickets;
    }
}
