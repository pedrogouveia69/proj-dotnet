namespace projDotNet
{
    public class Ticket
    {
        public int zoneNumber { get; } //read-only // pode aceder, nao pode modificar
        public int parkingSpotId { get; }
        public DateTime exitTime { get; }
        public int valuePaid { get; }
        public int change { get; }

        public Ticket(int zoneNumber, int parkingSpotId, DateTime exitTime, int valuePaid, int change)
        {
            this.zoneNumber = zoneNumber;
            this.parkingSpotId = parkingSpotId;
            this.exitTime = exitTime;
            this.valuePaid = valuePaid;
            this.change = change;
        }
        public override string ToString()
        {
            string ticket = "";
            ticket += "--------------------- Ticket ---------------------";
            ticket += "\nO seu carro está estacionado na ZONA " + zoneNumber;
            ticket += "\nO seu ID é " + parkingSpotId;
            ticket += "\nO estacionamento é válido até " + exitTime;
            ticket += "\n\nTotal pago: " + ToEuros(valuePaid);
            if (change > 0)
                ticket += "\nTroco: " + ToEuros(change);
            ticket += "\n--------------------------------------------------";

            return ticket;
        }

        private string ToEuros(int cents) 
        { 
            double d = (double)cents / 100;
            return d.ToString("n2") + "€";
        }
    }
}
