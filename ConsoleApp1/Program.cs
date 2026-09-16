using System;

namespace OOPAssignment01
{
    #region Part 01: Theoretical Questions
    // =========================================================================
    // PART 01: THEORETICAL QUESTIONS
    // =========================================================================
    //
    // Q1: Explain with code example how class and struct behave differently.
    // ANSWER:
    // - Class is a Reference Type stored on the Managed Heap. When copied or passed to another variable,
    //   only the reference pointer is copied. Modifying one variable modifies the underlying object.
    // - Struct is a Value Type stored on the Stack. When assigned or passed, a full independent copy of
    //   the data is made. Modifying one copy does NOT affect the original.
    //
    // Q2 & Q3: Describe the steps to create and use a Class Library in Visual Studio.
    // ANSWER:
    // 1. In Visual Studio, go to File -> New -> Project -> Choose "Class Library (.NET Core / .NET Standard / .NET 8/9)".
    // 2. Define public classes and methods inside the library.
    // 3. Build the Class Library solution (generates a .dll assembly).
    // 4. In your Console Application project, right-click "Dependencies" -> "Add Project Reference".
    // 5. Check the Class Library project and click OK.
    // 6. Add 'using YourLibraryNamespace;' at the top of your code to use the library classes.
    //
    // Q4: What is a class library? Why do we use class libraries?
    // ANSWER:
    // A Class Library is a compiled assembly (.dll) that contains reusable classes, interfaces, and methods
    // without an entry point (no Main method). We use them for:
    // 1. Code Reusability across multiple projects.
    // 2. Separation of Concerns and clean modular architecture.
    // 3. Maintainability and easier unit testing.
    #endregion

    #region Part 02: Practical (Movie Ticket Booking System)
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
        public string MovieName;
        public TicketType Type;
        public SeatLocation Seat;
        private double price;

        public double Price
        {
            get => price;
            set => price = value;
        }

        // Full Constructor
        public Ticket(string movieName, TicketType type, SeatLocation seat, double price)
        {
            MovieName = movieName;
            Type = type;
            Seat = seat;
            this.price = price;
        }

        // Constructor Chaining for default ticket
        public Ticket(string movieName) 
            : this(movieName, TicketType.Standard, new SeatLocation('A', 1), 50.0)
        {
        }

        public double CalcTotal(double taxPercent)
        {
            return price + (price * (taxPercent / 100.0));
        }

        public void ApplyDiscount(ref double discountAmount)
        {
            if (discountAmount > 0 && discountAmount <= price)
            {
                price -= discountAmount;
                discountAmount = 0; // consumed
            }
        }

        public void PrintTicket()
        {
            Console.WriteLine($"Movie: {MovieName} | Type: {Type} | Seat: {Seat} | Price: {price:F2} EGP");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("=== Assignment 01 OOP - Movie Ticket Booking ===");
            Console.WriteLine("==================================================");

            // Demonstration
            Ticket ticket1 = new Ticket("Inception", TicketType.VIP, new SeatLocation('B', 5), 120.0);
            Ticket ticket2 = new Ticket("Avatar"); // Default constructor

            Console.WriteLine("\n--- Ticket 1 (VIP) ---");
            ticket1.PrintTicket();
            Console.WriteLine($"Total with 14% Tax: {ticket1.CalcTotal(14):F2} EGP");

            double discount = 20.0;
            Console.WriteLine($"Applying discount of {discount} EGP...");
            ticket1.ApplyDiscount(ref discount);
            Console.WriteLine($"Price after discount: {ticket1.Price:F2} EGP (Remaining discount balance: {discount})");

            Console.WriteLine("\n--- Ticket 2 (Default Standard) ---");
            ticket2.PrintTicket();
            Console.WriteLine($"Total with 14% Tax: {ticket2.CalcTotal(14):F2} EGP");

            Console.WriteLine("\nAssignment 01 OOP completed successfully!");
        }
    }
    #endregion
}
