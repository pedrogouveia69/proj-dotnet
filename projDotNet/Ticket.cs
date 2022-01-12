namespace projDotNet
{
    public class Ticket
    {
        private int zoneNumber;
        private int parkingSpotId;
        private DateTime exitTime;
        private int valuePaid;
        private int change;

        public Ticket(int zoneNumber, int parkingSpotId, DateTime exitTime, int valuePaid, int change)
        {
            this.zoneNumber = zoneNumber;
            this.parkingSpotId = parkingSpotId;
            this.exitTime = exitTime;
            this.valuePaid = valuePaid;
            this.change = change;
        }

        public void showTicket()
        {
            Console.WriteLine("--------------------- Ticket ---------------------");
            Console.WriteLine("O seu carro está estacionado na ZONA " + zoneNumber);
            Console.WriteLine("O seu ID é " + parkingSpotId);
            Console.WriteLine("O estacionamento é válido até " + exitTime);
            Console.WriteLine("\nTotal pago: " + ToDouble(valuePaid).ToString("n2") + "€");
            if (change > 0)
                Console.WriteLine("Troco: " + ToDouble(change).ToString("n2") + "€");
            Console.WriteLine("--------------------------------------------------");
        }

        private double ToDouble(int integerValue) { return (double)integerValue/100; }
    }
}
