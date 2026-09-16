using System;

namespace OOPAssignment04
{
    #region Part 01: Theoretical Questions
    // =========================================================================
    // PART 01: THEORETICAL QUESTIONS
    // =========================================================================
    //
    // Q1: Static Binding vs Dynamic Binding:
    // - Static Binding (Early Binding): Happens at compile time. Used for overloaded methods and non-virtual methods.
    // - Dynamic Binding (Late Binding): Happens at runtime based on the actual object type using vtables (virtual methods).
    //
    // Q2: Method Overloading vs Method Overriding:
    // - Method Overloading: Same method name with different parameters in the same class (compile-time polymorphism).
    // - Method Overriding: Redefining a base class virtual method in a derived class with exact same signature (runtime polymorphism).
    //
    // Q3: Keywords for Method Overriding:
    // - 'virtual': Declares a method in the base class that can be overridden.
    // - 'override': Extends/modifies the virtual method in the derived class.
    // - 'base': Calls the base class implementation from the derived class.
    // - 'new': Explicitly hides/shadows an inherited method without polymorphic dispatch.
    #endregion

    #region Part 02: Practical (Polymorphism in Movie Ticket Booking System)
    public class Ticket
    {
        private static int counter = 0;
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
            TicketId = ++counter;
            MovieName = movieName;
            Price = price;
        }

        // Method Overloading
        public void SetPrice(decimal newPrice)
        {
            Price = newPrice;
        }

        public void SetPrice(decimal basePrice, decimal multiplier)
        {
            Price = basePrice * multiplier;
        }

        // Virtual Method for Polymorphism
        public virtual void PrintTicket()
        {
            Console.WriteLine($"Ticket #{TicketId} | {MovieName} | Price: {Price} EGP | After Tax: {PriceAfterTax:F2} EGP");
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

        public override void PrintTicket()
        {
            base.PrintTicket();
            Console.WriteLine($"  Seat: {SeatNumber}");
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

        public override void PrintTicket()
        {
            base.PrintTicket();
            Console.WriteLine($"  Lounge: {(LoungeAccess ? "Yes" : "No")} | Service Fee: {ServiceFee} EGP");
        }
    }

    public class IMAXTicket : Ticket
    {
        public bool Is3D { get; set; }

        public IMAXTicket(string movieName, decimal price, bool is3D)
            : base(movieName, price)
        {
            Is3D = is3D;
        }

        public override void PrintTicket()
        {
            base.PrintTicket();
            Console.WriteLine($"  IMAX 3D: {(Is3D ? "Yes" : "No")}");
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
        private Projector projector = new Projector();
        private Ticket[] tickets = new Ticket[20];
        private int count = 0;

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
            if (count < tickets.Length)
            {
                tickets[count++] = t;
                return true;
            }
            return false;
        }

        public void PrintAllTickets()
        {
            Console.WriteLine("\n========== All Tickets ==========");
            for (int i = 0; i < count; i++)
            {
                tickets[i].PrintTicket();
            }
        }
    }

    class Program
    {
        public static void ProcessTicket(Ticket t)
        {
            t.PrintTicket();
        }

        static void Main(string[] args)
        {
            Cinema cinema = new Cinema("Grand Cinema");
            cinema.OpenCinema();

            StandardTicket st = new StandardTicket("Inception", 120m, "A-5");
            VIPTicket vt = new VIPTicket("Avengers", 200m, true);
            IMAXTicket it = new IMAXTicket("Dune", 180m, false);

            Console.WriteLine("\n========== SetPrice Test ==========");
            Console.WriteLine("Setting price directly: 150");
            st.SetPrice(150m);
            Console.WriteLine("Setting price with multiplier: 100 x 1.5 = 150");
            st.SetPrice(100m, 1.5m);

            cinema.AddTicket(st);
            cinema.AddTicket(vt);
            cinema.AddTicket(it);

            cinema.PrintAllTickets();

            Console.WriteLine("\n========== Process Single Ticket ==========");
            ProcessTicket(vt);

            cinema.CloseCinema();
            Console.WriteLine("\nAssignment 04 completed successfully!");
        }
    }
    #endregion
}
