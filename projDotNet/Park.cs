namespace projDotNet
{
    public class Park
    {
        public int zoneNumber { get; set; }
        public DateTime[] spots { get; set; }
        public int centsPerHour { get; set; }
        public int maxTimeSeconds { get; set; }

        public Park(int zoneNumber, DateTime[] spots, int centsPerHour, int maxTimeSeconds) 
        {
            this.zoneNumber = zoneNumber;
            this.spots = spots;
            this.centsPerHour = centsPerHour;
            this.maxTimeSeconds = maxTimeSeconds;
        }

        public void setupParkingZone()
        {
            for (int i = 0; i < spots.Length; i++) //prencher arrays do park com lugares vazios 
            {
                spots[i] = new DateTime(); // é igual 01/01/0001 00:00:00
            }
        }

        public void displayAllParkingSpots()
        {
            // Ver número de vagas/lugares ocupados em cada zona e carros que exederam o tempo de estacionamento permitido.
            var dateTimeNow = DateTime.Now;

            Console.WriteLine();
            Console.WriteLine("Zona " + zoneNumber + ":");
            for (int i = 0; i < spots.Length; i++)
            {
                if (spots[i] == new DateTime())
                    Console.WriteLine("[ Lugar " + (i + 1) + " - Disponível ]");
                else if (dateTimeNow > spots[i])
                    Console.WriteLine("[ Lugar " + (i + 1) +  " - Carro excede o tempo pago! -> " + spots[i] + " ]");
                else
                    Console.WriteLine("[ Lugar " + (i + 1) + " - Ocupado até " + spots[i] + " ]");
            }
        }

        public bool parkIsFull()
        {
            int occupiedSpots = 0;
            for (int i = 0; i < spots.Length; i++)
            {
                if (spots[i] != new DateTime())
                    occupiedSpots++;
            }

            if (occupiedSpots >= spots.Length)
            {
                Console.WriteLine("Este parque está cheio.");
                return true;
            }
            return false;
        }

        public void parkCar(DateTime duration)
        {
            // Caso o utilizador confirme a operação, durante o tempo estipulado, o lugar deverá estar identificado como indisponível.
            for (int i = 0; i < spots.Length; i++)
            {
                if (spots[i] == new DateTime())
                {
                    spots[i] = duration;
                    Console.Clear();
                    // O programa deverá apresentar um ticket com aduração permitida.
                    Console.WriteLine("--------------------- Ticket ---------------------");
                    Console.WriteLine(" O seu carro está estacionado na ZONA " + zoneNumber);
                    Console.WriteLine(" O seu ID é " + (i + 1));
                    Console.WriteLine(" O estacionamento é válido até " + duration);
                    Console.WriteLine("--------------------------------------------------");
                }
            }
        }

        public bool removeCar(int id)
        {
            id = id - 1;
            if ((id > (spots.Length -1) || id < 0))
            {
                Console.WriteLine("ID Inválido.");
                return false;
            }
            else if (spots[id] == new DateTime())
            {
                Console.WriteLine("Não há nenhum carro estacionado neste lugar.");
                return false;
            }
            spots[id] = new DateTime();
            Console.Clear();
            Console.WriteLine("Obrigado pela sua preferência.");
            return true;
        }

        /*private int getSeconds(int insertedCents)
        {
            //  centsPerHour ------- 3600 seconds
            // insertedCents -------  𝑥 seconds

            // This function returns 𝑥

            return insertedCents * 3600 / centsPerHour;
        }
        */
    }
}
