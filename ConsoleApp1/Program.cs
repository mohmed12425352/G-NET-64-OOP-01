using System;

namespace OOPAssignment02
{
    #region Part 01: Theoretical Questions
    // =========================================================================
    // PART 01: THEORETICAL QUESTIONS
    // =========================================================================
    //
    // Q1: BankAccount encapsulation problems & fix:
    // a) Problems:
    //    1. Public fields (Owner, Balance) allow unrestricted outside mutation (e.g. setting negative balance).
    //    2. Withdraw method has no validation (withdrawing more than balance or negative amount).
    // b) Fix: Make fields private, create properties with validation, and add checks in Withdraw().
    // c) Bad practice: Exposing public fields breaks data hiding, allows corrupted states, and prevents future validation logic.
    //
    // Q2: Field vs Property:
    // - Field is a direct memory storage variable.
    // - Property is a wrapper with get/set accessors containing logic.
    // Example: public decimal PriceAfterTax => Price * 1.14m;
    //
    // Q3: Indexers:
    // a) 'this[int index]' is an Indexer. It allows instances of a class to be indexed just like arrays.
    // b) If index is out of bounds (10), an IndexOutOfRangeException occurs. Make it safe by validating bounds in get/set.
    // c) Yes, indexers can be overloaded (e.g. indexing by int index or by string name).
    //
    // Q4: Static Keyword:
    // a) 'static' means the member belongs to the type itself, shared across all instances, not duplicated per object.
    // b) Static methods cannot access instance fields (Item) directly because static methods have no 'this' instance context.
    #endregion

    #region Part 02: Practical (Extending Movie Ticket Booking System)
    public enum TicketType
    {
        Standard,
        VIP,
        IMAX
    }

    public struct SeatLocation
    {
        public char Row;
        public int Number;

        public SeatLocation(char row, int number)
        {
            Row = row;
            Number = number;
        }

        public override string ToString() => $"{Row}-{Number}";
    }

    public class Ticket
    {
        private static int ticketCounter = 0;
        private string movieName = "Unknown";
        private double price = 50.0;

        public int TicketId { get; private set; }

        public string MovieName
        {
            get => movieName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    movieName = value;
            }
        }

        public TicketType Type { get; set; }
        public SeatLocation Seat { get; set; }

        public double Price
        {
            get => price;
            set
            {
                if (value > 0)
                    price = value;
            }
        }

        public double PriceAfterTax => Price * 1.14;

        public Ticket(string movieName, TicketType type, SeatLocation seat, double price)
        {
            TicketId = ++ticketCounter;
            MovieName = movieName;
            Type = type;
            Seat = seat;
            Price = price;
        }

        public static int GetTotalTicketsSold() => ticketCounter;

        public override string ToString()
        {
            return $"Ticket #{TicketId} | {MovieName} | {Type} | Seat: {Seat} | Price: {Price} EGP | After Tax: {PriceAfterTax:F1} EGP";
        }
    }

    public class Cinema
    {
        private Ticket[] tickets = new Ticket[20];

        // Indexer by position
        public Ticket this[int index]
        {
            get
            {
                if (index >= 0 && index < tickets.Length)
                    return tickets[index];
                return null;
            }
            set
            {
                if (index >= 0 && index < tickets.Length)
                    tickets[index] = value;
            }
        }

        // Search by movie name
        public Ticket this[string movieName]
        {
            get
            {
                foreach (var t in tickets)
                {
                    if (t != null && t.MovieName.Equals(movieName, StringComparison.OrdinalIgnoreCase))
                        return t;
                }
                return null;
            }
        }

        public bool AddTicket(Ticket t)
        {
            for (int i = 0; i < tickets.Length; i++)
            {
                if (tickets[i] == null)
                {
                    tickets[i] = t;
                    return true;
                }
            }
            return false;
        }
    }

    public static class BookingHelper
    {
        private static int refCounter = 0;

        public static double CalcGroupDiscount(int numberOfTickets, double pricePerTicket)
        {
            double total = numberOfTickets * pricePerTicket;
            if (numberOfTickets >= 5)
            {
                return total * 0.90; // 10% discount
            }
            return total;
        }

        public static string GenerateBookingReference()
        {
            return $"BK-{++refCounter}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Ticket Booking ==========");
            Cinema cinema = new Cinema();

            Ticket t1 = new Ticket("Inception", TicketType.VIP, new SeatLocation('B', 5), 120);
            Ticket t2 = new Ticket("Avengers", TicketType.Standard, new SeatLocation('A', 3), 80);
            Ticket t3 = new Ticket("Dune", TicketType.IMAX, new SeatLocation('C', 10), 200);

            cinema.AddTicket(t1);
            cinema.AddTicket(t2);
            cinema.AddTicket(t3);

            Console.WriteLine("\n========== All Tickets ==========");
            for (int i = 0; i < 3; i++)
            {
                if (cinema[i] != null)
                    Console.WriteLine(cinema[i]);
            }

            Console.WriteLine("\n========== Search by Movie ==========");
            string search = "Avengers";
            Console.WriteLine($"Enter movie name to search: {search}");
            Ticket found = cinema[search];
            if (found != null)
                Console.WriteLine($"Found: {found}");
            else
                Console.WriteLine("Movie not found!");

            Console.WriteLine("\n========== Statistics ==========");
            Console.WriteLine($"Total Tickets Sold: {Ticket.GetTotalTicketsSold()}");
            Console.WriteLine($"Booking Reference 1: {BookingHelper.GenerateBookingReference()}");
            Console.WriteLine($"Booking Reference 2: {BookingHelper.GenerateBookingReference()}");
            Console.WriteLine($"Group Discount (5 tickets x 80 EGP): {BookingHelper.CalcGroupDiscount(5, 80)} EGP (10% off applied)");

            Console.WriteLine("\nAssignment 02 completed successfully!");
        }
    }
    #endregion
}
