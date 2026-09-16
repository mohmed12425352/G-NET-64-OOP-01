using System;

namespace OOPAssignment06
{
    #region Part 01: Theoretical Questions
    // =========================================================================
    // PART 01: THEORETICAL QUESTIONS
    // =========================================================================
    //
    // Q1: Abstraction vs Encapsulation:
    // - Abstraction: Focuses on "WHAT" an object does rather than "HOW". Hides complexity behind high-level interfaces.
    // - Encapsulation: Focuses on data hiding and bundling data with methods to protect object integrity.
    // Real-world example: A smartphone screen/touch interface is Abstraction (you tap to call). The internal circuit board/battery protection is Encapsulation.
    //
    // Q2: Abstract Class vs Interface:
    // 1. State: Abstract class can have instance state/fields; Interface cannot have instance fields.
    // 2. Inheritance: Single inheritance for classes; Multiple inheritance for interfaces.
    // 3. Constructors: Abstract class has constructors; Interface cannot have constructors.
    // 4. Access Modifiers: Abstract class supports all access modifiers; Interface members are public by default.
    //
    // Q3: Appliance Code Analysis:
    // a) Appliance a = new Appliance("LG"); -> Compile error. Abstract classes cannot be directly instantiated.
    // b) PowerConsumption() is abstract (must be customized by child), Status() is virtual (has default, can override), Label() is concrete (shared logic).
    // c) Toaster.Status() returns "Standby" because it inherits the default virtual implementation.
    //
    // Q4: Partial Classes, Partial Methods & Extension Methods:
    // a) Partial Class splits class definition across multiple files for team organization and auto-generated code.
    // b) Partial Method allows optional hooks. If the implementation is deleted, the compiler removes calls at compile time with 0 overhead.
    // c) Extension method rules: Static class, static method, 'this' keyword on the first parameter.
    // d) Code trace output:
    //    Log: result = 20
    //    $20.00
    #endregion

    #region Part 02: Practical (Abstract Classes, Partial Cinema & Extension Methods)
    public abstract class Ticket
    {
        private static int counter = 0;
        public int TicketId { get; protected set; }
        public string MovieName { get; set; }
        public decimal Price { get; set; }
        public bool IsBooked { get; set; }

        public Ticket(string movieName, decimal price)
        {
            TicketId = ++counter;
            MovieName = movieName;
            Price = price;
            IsBooked = false;
        }

        // Abstract method enforced on all child types
        public abstract decimal CalculateFinalPrice();

        public virtual void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | Price: {Price} | Final: {CalculateFinalPrice():F2} | Booked: {(IsBooked ? "Yes" : "No")}");
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

        public override decimal CalculateFinalPrice() => Price * 1.14m;

        public override void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | Standard | Seat: {SeatNumber} | Price: {Price} | Final: {CalculateFinalPrice():F2} | Booked: {(IsBooked ? "Yes" : "No")}");
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

        public override decimal CalculateFinalPrice() => (Price + ServiceFee) * 1.14m;

        public override void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | VIP | Lounge: {(LoungeAccess ? "Yes" : "No")} | Fee: {ServiceFee} | Price: {Price} | Final: {CalculateFinalPrice():F2} | Booked: {(IsBooked ? "Yes" : "No")}");
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

        public override decimal CalculateFinalPrice() => (Price + (Is3D ? 30m : 0m)) * 1.14m;

        public override void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | IMAX | 3D: {(Is3D ? "Yes" : "No")} | Price: {Price} | Final: {CalculateFinalPrice():F2} | Booked: {(IsBooked ? "Yes" : "No")}");
        }
    }

    // Extension Methods
    public static class TicketExtensions
    {
        public static string GenerateReceipt(this Ticket ticket)
        {
            return $"========== RECEIPT ==========\n" +
                   $"  Movie    : {ticket.MovieName}\n" +
                   $"  Type     : {ticket.GetType().Name}\n" +
                   $"  Price    : {ticket.Price}\n" +
                   $"  Final    : {ticket.CalculateFinalPrice():F2}\n" +
                   $"  Status   : {(ticket.IsBooked ? "Booked" : "Available")}\n" +
                   $"=============================";
        }

        public static decimal CalculateTotalRevenue(this Ticket[] tickets)
        {
            decimal total = 0;
            foreach (var t in tickets)
            {
                if (t != null && t.IsBooked)
                    total += t.CalculateFinalPrice();
            }
            return total;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Cinema Opened ===");
            Console.WriteLine("Projector ON");

            // Demonstrating compiler protection:
            // Ticket plainTicket = new Ticket("Test", 100); // CS0144: Cannot create an instance of the abstract type or interface 'Ticket'

            StandardTicket t1 = new StandardTicket("Inception", 80m, "A5") { IsBooked = true };
            VIPTicket t2 = new VIPTicket("Avengers", 200m, true) { IsBooked = true };
            IMAXTicket t3 = new IMAXTicket("Dune", 130m, true) { IsBooked = true };

            Cinema cinema = new Cinema();
            cinema.AddTicket(t1);
            cinema.AddTicket(t2);
            cinema.AddTicket(t3);

            cinema.PrintAllTickets();

            Console.WriteLine("\n--- Polymorphism: Final Price per Ticket ---");
            Ticket[] tickets = new Ticket[] { t1, t2, t3 };
            foreach (var t in tickets)
            {
                Console.WriteLine($"{t.GetType().Name} => Final Price: {t.CalculateFinalPrice():F2}");
            }

            Console.WriteLine("\n--- Extension Method: Receipt ---");
            Console.WriteLine(t2.GenerateReceipt());

            Console.WriteLine("\n--- Extension Method: Total Revenue ---");
            Console.WriteLine($"Total Revenue: {tickets.CalculateTotalRevenue():F2}");

            Console.WriteLine("Projector OFF");
            Console.WriteLine("=== Cinema Closed ===");
            Console.WriteLine("\nAssignment 06 completed successfully!");
        }
    }
    #endregion
}
