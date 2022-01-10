namespace projDotNet
{
    public class Ticket
    {
        private int zoneNumber;
        private int parkingSpotId;
        private DateTime exitTime;

        public Ticket(int zoneNumber, int parkingSpotId, DateTime exitTime)
        {
            this.zoneNumber = zoneNumber;
            this.parkingSpotId = parkingSpotId;
            this.exitTime = exitTime;
        }

        public void showTicket()
        {
            Console.WriteLine("--------------------- Ticket ---------------------");
            Console.WriteLine("O seu carro está estacionado na ZONA " + zoneNumber);
            Console.WriteLine("O seu ID é " + parkingSpotId);
            Console.WriteLine("O estacionamento é válido até " + exitTime);
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
