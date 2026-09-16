using System;

namespace OOPAssignment05
{
    #region Part 01: Theoretical Questions
    // =========================================================================
    // PART 01: THEORETICAL QUESTIONS
    // =========================================================================
    //
    // Q1: Interface in C#:
    // An interface is a contract that defines a set of method and property signatures without implementation.
    // Benefits:
    // 1. Enables multiple inheritance of types/contracts.
    // 2. Promotes Loose Coupling and Dependency Injection (DI).
    // 3. Facilitates Unit Testing and Mocking.
    //
    // Q2: Explicit Interface Implementation:
    // a) Problem: Both IEnglishSpeaker and IArabicSpeaker have Greet(). A single public method merges them.
    // b) Fix: Use Explicit Interface Implementation:
    //    void IEnglishSpeaker.Greet() => Console.WriteLine("Hello");
    //    void IArabicSpeaker.Greet() => Console.WriteLine("Ahlan");
    // c) You cannot call translator.Greet() directly. You must cast to the specific interface:
    //    ((IEnglishSpeaker)translator).Greet();
    //
    // Q3: Shallow Copy vs Deep Copy:
    // - Shallow Copy (MemberwiseClone): Copies value types, but copies reference pointers for reference types (both point to same memory).
    // - Deep Copy: Recursively duplicates all nested objects, creating completely independent instances.
    // - Risk of Shallow Copy: Mutating reference fields in the clone inadvertently corrupts the original object.
    //
    // Q4: Code Trace Output:
    // e2.Title = "QA";
    // e2.Dept.Name = "Testing";
    // Output:
    // Dev - Testing
    // QA - Testing
    // Explanation: Title is a string (value semantic), but Dept is a reference shared between e1 and e2.
    #endregion

    #region Part 02: Practical (Interfaces, Booking & Deep Cloning)
    public interface IPrintable
    {
        void Print();
    }

    public interface IBookable
    {
        bool IsBooked { get; }
        bool Book();
        bool Cancel();
    }

    public class Ticket : IPrintable, IBookable, ICloneable
    {
        private static int counter = 0;
        public int TicketId { get; protected set; }
        public string MovieName { get; set; }
        public decimal Price { get; set; }
        public bool IsBooked { get; private set; }

        public decimal PriceAfterTax => Price * 1.14m;

        public Ticket(string movieName, decimal price)
        {
            TicketId = ++counter;
            MovieName = movieName;
            Price = price;
            IsBooked = false;
        }

        public bool Book()
        {
            if (IsBooked) return false;
            IsBooked = true;
            return true;
        }

        public bool Cancel()
        {
            if (!IsBooked) return false;
            IsBooked = false;
            return true;
        }

        public virtual void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | Price: {Price} | After Tax: {PriceAfterTax:F1} | Booked: {(IsBooked ? "Yes" : "No")}");
        }

        public virtual object Clone()
        {
            Ticket copy = (Ticket)this.MemberwiseClone();
            copy.TicketId = ++counter; // New distinct ID
            copy.IsBooked = false;     // Reset booking status on clone
            return copy;
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

        public override void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | Standard | Seat: {SeatNumber} | Price: {Price} | After Tax: {PriceAfterTax:F1} | Booked: {(IsBooked ? "Yes" : "No")}");
        }

        public override object Clone()
        {
            StandardTicket copy = (StandardTicket)base.Clone();
            copy.SeatNumber = this.SeatNumber;
            return copy;
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

        public override void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | VIP | Lounge: {(LoungeAccess ? "Yes" : "No")} | Fee: {ServiceFee} | Price: {Price} | After Tax: {PriceAfterTax:F1} | Booked: {(IsBooked ? "Yes" : "No")}");
        }

        public override object Clone()
        {
            VIPTicket copy = (VIPTicket)base.Clone();
            copy.LoungeAccess = this.LoungeAccess;
            copy.ServiceFee = this.ServiceFee;
            return copy;
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

        public override void Print()
        {
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | IMAX | 3D: {(Is3D ? "Yes" : "No")} | Price: {Price} | After Tax: {PriceAfterTax:F1} | Booked: {(IsBooked ? "Yes" : "No")}");
        }

        public override object Clone()
        {
            IMAXTicket copy = (IMAXTicket)base.Clone();
            copy.Is3D = this.Is3D;
            return copy;
        }
    }

    public class Cinema
    {
        private Ticket[] tickets = new Ticket[20];
        private int count = 0;

        public void OpenCinema() => Console.WriteLine("=== Cinema Opened ===");
        public void CloseCinema() => Console.WriteLine("=== Cinema Closed ===");

        public void AddTicket(Ticket t)
        {
            if (count < tickets.Length)
                tickets[count++] = t;
        }

        public void PrintAll()
        {
            Console.WriteLine("--- All Tickets ---");
            for (int i = 0; i < count; i++)
            {
                tickets[i].Print();
            }
        }
    }

    public static class BookingHelper
    {
        public static void PrintAll(IPrintable[] items)
        {
            Console.WriteLine("--- BookingHelper.PrintAll ---");
            foreach (var item in items)
            {
                item?.Print();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cinema cinema = new Cinema();
            cinema.OpenCinema();

            StandardTicket t1 = new StandardTicket("Inception", 80m, "A5");
            VIPTicket t2 = new VIPTicket("Avengers", 200m, true);
            IMAXTicket t3 = new IMAXTicket("Dune", 130m, true);

            t1.Book();
            t2.Book();
            t3.Book();

            cinema.AddTicket(t1);
            cinema.AddTicket(t2);
            cinema.AddTicket(t3);

            cinema.PrintAll();

            Console.WriteLine("\n--- Clone Test ---");
            VIPTicket clone = (VIPTicket)t2.Clone();
            clone.MovieName = "Interstellar";
            Console.Write("Original : "); t2.Print();
            Console.Write("Clone    : "); clone.Print();

            Console.WriteLine("\n--- After Cancellation ---");
            t1.Cancel();
            t1.Print();

            Console.WriteLine();
            IPrintable[] printables = new IPrintable[] { t1, t2, t3 };
            BookingHelper.PrintAll(printables);

            cinema.CloseCinema();
            Console.WriteLine("\nAssignment 05 completed successfully!");
        }
    }
    #endregion
}
