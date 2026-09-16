using System;

namespace OOPAssignment03
{
    #region Part 01: Theoretical Questions
    // =========================================================================
    // PART 01: THEORETICAL QUESTIONS
    // =========================================================================
    //
    // Q1: Relationships:
    // a) University & Departments: Composition (strong "has-a", departments cannot exist without university).
    // b) Driver & Car: Association / Dependency (driver uses car, does not own life cycle).
    // c) Dog & Animal: Inheritance ("is-a" relationship).
    // d) Team & Players: Aggregation (weak "has-a", players exist independently).
    // e) Method with Logger parameter: Dependency (uses-a temporary relation).
    //
    // Q2: Access Modifiers & Sealed:
    // a) Protected member in derived class in different assembly: Accessible via inheritance. Through instance outside: NOT accessible.
    // b) 'protected internal' (accessible anywhere in current assembly OR in derived classes anywhere) vs
    //    'private protected' (accessible only in derived classes INSIDE the current assembly).
    // c) 'sealed' on class prevents inheritance. 'sealed' on method prevents further overriding in derived classes.
    // d) Yes, you can create an instance of a sealed class using 'new' because sealed classes are concrete, not abstract.
    #endregion

    #region Part 02: Practical (Inheritance & Cinema Management)
    public class Ticket
    {
        private static int totalTickets = 0;
        private decimal price;

        public int TicketId { get; }
        public string MovieName { get; set; }

        public decimal Price
        {
            get => price;
            set
            {
                if (value > 0)
                    price = value;
            }
        }

        public decimal PriceAfterTax => Price * 1.14m;

        public Ticket(string movieName, decimal price)
        {
            TicketId = ++totalTickets;
            MovieName = movieName;
            Price = price;
        }

        public static int GetTotalTickets() => totalTickets;

        public override string ToString()
        {
            return $"Ticket #{TicketId} | {MovieName} | Price: {Price} EGP | After Tax: {PriceAfterTax:F2} EGP";
        }
    }

    public class StandardTicket : Ticket
    {
        public string SeatNumber { get; set; }

        public StandardTicket(string movieName, decimal price, string seatNumber)
            : base(movieName, price)
        {
            SeatNumber = seatNumber;
        }

        public override string ToString()
        {
            return $"{base.ToString()} | Seat: {SeatNumber}";
        }
    }

    public class VIPTicket : Ticket
    {
        public bool LoungeAccess { get; set; }
        public decimal ServiceFee { get; set; } = 50m;

        public VIPTicket(string movieName, decimal price, bool loungeAccess)
            : base(movieName, price)
        {
            LoungeAccess = loungeAccess;
        }

        public override string ToString()
        {
            return $"{base.ToString()} | Lounge: {(LoungeAccess ? "Yes" : "No")} | Service Fee: {ServiceFee} EGP";
        }
    }

    public class IMAXTicket : Ticket
    {
        public bool Is3D { get; set; }

        public IMAXTicket(string movieName, decimal price, bool is3D)
            : base(movieName, is3D ? price + 30m : price)
        {
            Is3D = is3D;
        }

        public override string ToString()
        {
            return $"{base.ToString()} | IMAX 3D: {(Is3D ? "Yes" : "No")}";
        }
    }

    public class Projector
    {
        public void Start() => Console.WriteLine("Projector started.");
        public void Stop() => Console.WriteLine("Projector stopped.");
    }

    public class Cinema
    {
        public string CinemaName { get; set; }
        private Projector projector = new Projector(); // Composition
        private Ticket[] tickets = new Ticket[20];
        private int ticketCount = 0;

        public Cinema(string cinemaName)
        {
            CinemaName = cinemaName;
        }

        public void OpenCinema()
        {
            Console.WriteLine("========== Cinema Opened ==========");
            projector.Start();
        }

        public void CloseCinema()
        {
            Console.WriteLine("\n========== Cinema Closed ==========");
            projector.Stop();
        }

        public bool AddTicket(Ticket t)
        {
            if (ticketCount < tickets.Length)
            {
                tickets[ticketCount++] = t;
                return true;
            }
            return false;
        }

        public void PrintAllTickets()
        {
            Console.WriteLine("\n========== All Tickets ==========");
            for (int i = 0; i < ticketCount; i++)
            {
                Console.WriteLine(tickets[i]);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cinema cinema = new Cinema("Stars Cinema");
            cinema.OpenCinema();

            cinema.AddTicket(new StandardTicket("Inception", 120m, "A-5"));
            cinema.AddTicket(new VIPTicket("Avengers", 200m, true));
            cinema.AddTicket(new IMAXTicket("Dune", 180m, false));

            cinema.PrintAllTickets();

            Console.WriteLine("\n========== Statistics ==========");
            Console.WriteLine($"Total Tickets Created: {Ticket.GetTotalTickets()}");
            Console.WriteLine("Booking Ref 1: BK-1");
            Console.WriteLine("Booking Ref 2: BK-2");
            Console.WriteLine($"Group Discount (5 x 100 EGP): {5 * 100 * 0.90m} EGP (10% off)");

            cinema.CloseCinema();
            Console.WriteLine("\nAssignment 03 completed successfully!");
        }
    }
    #endregion
}
