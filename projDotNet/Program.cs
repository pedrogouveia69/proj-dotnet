using projDotNet;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;

// 3 zonas de estacionamento (cada zona deverá ter um número aleatório de lugares de estacionamento).
var rand = new Random();
// in this program a parking zone is represented by an array of DateTimes
var parkOne = new DateTime[rand.Next(5, 10)];
var parkTwo = new DateTime[rand.Next(5, 10)];
var parkThree = new DateTime[rand.Next(5, 10)];

// Zona 1 tem o custo de 1.15€ / hora e a duração máxima de 45 minutos.
int parkOneCentsPerHour = 115;
int parkOneMaxSeconds = 2700;

// Zona 2 tem o curso de 1€ / hora e a duração máxima de 2 horas.
int parkTwoCentsPerHour = 100;
int parkTwoMaxSeconds = 7200;

// Zona 3 tem o custo de 0.62€ /hora e não possui duração máxima.
int parkThreeCentsPerHour = 62;
int parkThreeMaxSeconds = -1;    // -1 will be considered as not having a max time

int[] coins = { 1, 2, 5, 10, 20, 50, 100, 200 };

int insertedCents = 0;
int addedHours = 0;
int addedMinutes = 0;
int addedSeconds = 0;

var ticketHistory = new List<Ticket>();
int totalAccumulatedCents = 0;

string adminPassword = "1234";

displayMainMenu();

// generates a menu based on a given title and array of options
void displayMenu(string title, string[] options)
{
	Console.Clear();
	string topLine = "------------ " + title + " ------------";
	Console.WriteLine(topLine);

	for (int i = 0; i < options.Length; i++)
		Console.WriteLine(" " + (i + 1) + " - " + options[i]);

	for (int j = 0; j < topLine.Length; j++)
		Console.Write("-");
	Console.WriteLine();
}

void displayMainMenu() 
{
	string[] mainMenuOptions = { "Menu Cliente" , "Menu Admin" };
	displayMenu("Bem-vindo", mainMenuOptions);
	int option = selectOption(mainMenuOptions.Length);
	switch (option)
	{
		case 1:
			displayClientMenu();
			break;
		case 2:
			// access to admin menu is restricted by a password
			Console.Write("Insira a palavra-passe: ");
			string password = Console.ReadLine();
			if (password != adminPassword || string.IsNullOrEmpty(password))
			{
				Console.WriteLine("Password Incorreta");
				pressKeyToContinue();
				displayMainMenu();
			}
			displayAdminMenu();
			break;
	}
}

void displayAdminMenu() 
{
	string[] adminMenuOptions = 
	{
		"Ver Parques", "Ver Total de Dinheiro Acumulado", "Ver Histórico de Tickets", "Reset Total de Dinheiro Acumulado ", "Reset Histórico de Tickets", "Voltar" 
	};
	displayMenu("Menu Admin", adminMenuOptions);
	int option = selectOption(adminMenuOptions.Length);
	switch (option)
	{
		case 1:
			var parkList = new List<DateTime[]> { parkOne, parkTwo, parkThree };
            for (int i = 0; i < parkList.Count; i++)
				displayAllParkingSpots(i + 1, parkList[i]);
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 2:
			double totalAccumulatedEuros = (double)totalAccumulatedCents/100;
			Console.WriteLine("\nTotal Acumulado: " + totalAccumulatedEuros.ToString("n2") + "€");
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 3:
			if (!ticketHistory.Any())
			{
				Console.WriteLine("\nAinda não existe histórico.");
				pressKeyToContinue();
				displayAdminMenu();
			}
			Console.WriteLine("\nHistórico de Tickets:");
			foreach (var ticket in ticketHistory)
				ticket.showTicket();
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 4:
			totalAccumulatedCents = 0;
			Console.WriteLine("\nO reset foi feito com sucesso.");
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 5:
			ticketHistory.Clear();
			Console.WriteLine("\nO reset foi feito com sucesso.");
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 6:
			displayMainMenu();
			break;
	}
};

void displayClientMenu() 
{
	string[] clientMenuOptions = { "Estacionar", "Remover Carro", "Voltar" };
	displayMenu("Menu Cliente", clientMenuOptions);

	int option = selectOption(clientMenuOptions.Length);
	switch (option)
	{
		case 1:
			displayParkingZones(true);
			break;
		case 2:
			displayParkingZones(false);
			break;
		case 3:
			displayMainMenu();
			break;
	}
}

// reutilizes the same function for parking and unparking
// a user is parking if userIsParking == true
void displayParkingZones(bool userIsParking) 
{
	string[] zoneOptions = { "Zona 1", "Zona 2", "Zona 3", "Voltar" };
	displayMenu("Selecione uma zona", zoneOptions);
	int option = selectOption(zoneOptions.Length);

	if (userIsParking)
	{
		switch (option)
		{
			case 1:
				if (parkIsFull(parkOne))
					displayParkingZones(true);
				displayParkingInfo(option, parkOne, parkOneCentsPerHour, parkOneMaxSeconds);
				break;
			case 2:
				if (parkIsFull(parkTwo))
					displayParkingZones(true);
				displayParkingInfo(option, parkTwo, parkTwoCentsPerHour, parkTwoMaxSeconds);
				break;

			case 3:
				if (parkIsFull(parkThree))
					displayParkingZones(true);
				displayParkingInfo(option, parkThree, parkThreeCentsPerHour, parkThreeMaxSeconds);
				break;
			case 4:
				displayClientMenu();
				break;
		}
	}
	switch (option)
	{
		case 1:				
			removeCar(parkOne, getIdFromUser());
			break;
		case 2:
			removeCar(parkTwo, getIdFromUser());
			break;
		case 3:
			removeCar(parkThree, getIdFromUser());
			break;
		case 4:
			displayClientMenu();
			break;
	}	
};

void displayParkingInfo(int zoneNumber, DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds) 
{
	string[] coinOptions = { "0.05€", "0.10€", "0.20€", "0.50€", "1.00€", "2.00€", "Confirmar", "Cancelar" };
	displayMenu("Insira uma Moeda", coinOptions);

	Console.WriteLine("Custo por hora: " + (double)centsPerHour/100 + "€");
	if (maxTimeSeconds != -1) 
		Console.WriteLine("Tempo máximo permitido: " + maxTimeSeconds/60 + " minutos");

	showOpenHours();

	int seconds = getSeconds(centsPerHour, insertedCents);	
	var exitTime = calculateExitTime(seconds);

	if (insertedCents != 0) {
		double totalEuros = (double)insertedCents/100;
		Console.WriteLine("\nTotal: " + totalEuros.ToString("n2") + "€"); // ("n2") forces the converted double to have 2 decimal places
		if (seconds > maxTimeSeconds && maxTimeSeconds != -1)
		{
			parkAndGiveChange(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
		}
		Console.WriteLine("Duração: " + exitTime);
		Console.WriteLine("\nInsira outra moeda ou escolha '7' para Confirmar.");
	}

	int option = selectOption(coinOptions.Length);
	manageOption(option, zoneNumber, parkingZone, centsPerHour, maxTimeSeconds, exitTime);
}

void parkAndGiveChange(int zoneNumber, DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds)
{
	var exitTime = calculateExitTime( maxTimeSeconds);
	Console.WriteLine("\nExcedeu o tempo máximo permitido.");
	Console.WriteLine("O seu estacionamento será válido até: " + exitTime);
	Console.WriteLine("Irá receber o troco juntamente com o Ticket.");
	pressKeyToContinue();

	int maxCents = getMaxCents(centsPerHour, maxTimeSeconds);
	int change = insertedCents - maxCents;
	totalAccumulatedCents += maxCents;
	parkCar(zoneNumber, parkingZone, exitTime, change);
}

void manageOption(int option, int zoneNumber, DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds, DateTime exitTime)
{
	// options = { "0.05€", "0.10€", "0.20€", "0.50€", "1.00€", "2.00€", "Confirmar", "Cancelar" };
	if (option == 7 && insertedCents == 0)
	{
		Console.WriteLine("Não é possível Confirmar sem introduzir dinheiro.");
		pressKeyToContinue();
		displayParkingInfo(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
	}
	if (option == 7)
	{
		totalAccumulatedCents += insertedCents;
		parkCar(zoneNumber, parkingZone, exitTime, 0);
	}
	if (option == 8)
	{
		resetCounters();
		displayClientMenu();
	}

	// int i = 2 makes it so the first coin is 5
	for (int i = 2; i < coins.Length; i++)
		// option + 1 matches the option to the correct int[] coins index
		if (option + 1 == i)
			insertedCents += coins[i];

	displayParkingInfo(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
}

void displayAllParkingSpots(int zoneNumber, DateTime[] parkingZone)
{
	Console.WriteLine();
	Console.WriteLine("Zona " + zoneNumber + ":");
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
			Console.WriteLine("[ Lugar " + (i+1) + " - Disponível ]");
		else if (DateTime.Now > parkingZone[i])
			Console.WriteLine("[ Lugar " + (i+1) +  " - Carro excede o tempo pago! -> " + parkingZone[i] + " ]");
		else
			Console.WriteLine("[ Lugar " + (i+1) + " - Ocupado até " + parkingZone[i] + " ]");
	}
}

// for an option to be valid it has to be:
// an integer
// no lesser than 1
// no greater than the given length of an array of options
int selectOption(int optionsLength)
{
	Console.Write("Escolha uma opção: ");
	string optionStr = Console.ReadLine();
	if (string.IsNullOrEmpty(optionStr))
	{
		Console.WriteLine("Não foi selecionada nenhuma opção!");
		return selectOption(optionsLength);
	}

	int option;
	bool tryParse = int.TryParse(optionStr, out option);
	if (!tryParse || (option < 1 || option > optionsLength))
	{
		Console.WriteLine("Opção inválida!");
		return selectOption(optionsLength);
	}
	return option;
}

DateTime calculateExitTime(int seconds)
{
	var exitTime = DateTime.Now.AddSeconds(seconds + addedSeconds).AddHours(addedHours).AddMinutes(addedMinutes);

	if (exitTime.DayOfWeek == DayOfWeek.Sunday)
	{
		if (exitTime.Day == DateTime.Now.Day)
		{
			addedHours += 24 + 9 - DateTime.Now.Hour;
			addedMinutes = -DateTime.Now.Minute;
			addedSeconds = -DateTime.Now.Second;
		}
		else addedHours += 24;
	}
	else if (exitTime.DayOfWeek == DayOfWeek.Saturday && exitTime.Hour >= 14)
	{
		if (exitTime.Day == DateTime.Now.Day)
		{
			addedHours += 43 + 14 - DateTime.Now.Hour;
			addedMinutes = -DateTime.Now.Minute;
			addedSeconds = -DateTime.Now.Second;
		}
		else addedHours += 43;
			
	}
	else if (exitTime.Hour < 9)
	{
		if (exitTime.Day == DateTime.Now.Day)
		{
			addedHours = 9 - DateTime.Now.Hour;
			addedMinutes = -DateTime.Now.Minute;
			addedSeconds = -DateTime.Now.Second;
		}
        else addedHours += 9 - exitTime.Hour;
	}
	else if (exitTime.Hour >= 20)
	{
		if (exitTime.Day == DateTime.Now.Day)
        {
			addedHours = 9 + 20 - DateTime.Now.Hour;
			addedMinutes = -DateTime.Now.Minute;
			addedSeconds = -DateTime.Now.Second;
        }
        else addedHours += 13;
	}

	return DateTime.Now.AddSeconds(seconds + addedSeconds).AddHours(addedHours).AddMinutes(addedMinutes);
}

void showOpenHours()
{
	if 
	(
		(DateTime.Now.DayOfWeek == DayOfWeek.Sunday) ||
		(DateTime.Now.DayOfWeek == DayOfWeek.Saturday && (DateTime.Now.Hour < 9 || DateTime.Now.Hour >= 14)) ||
		(DateTime.Now.Hour < 9 || DateTime.Now.Hour >= 20)
	)
	{
		Console.WriteLine("\nHorário de Funcionamento dos Parquímetros:");
		Console.WriteLine(" - Dias Úteis:  9:00 - 20:00");
		Console.WriteLine(" - Sábados:     9:00 - 14:00\n");
		Console.WriteLine("As suas moedas serão contabilizadas para o próximo período aberto.");
	}
}

bool parkIsFull(DateTime[] parkingZone) 
{
	// a spot will be considered empty if equal to new DateTime()
	int occupiedSpots = 0;
	for (int i = 0; i < parkingZone.Length; i++)
		if (parkingZone[i] != new DateTime())
			occupiedSpots++;

	if (occupiedSpots >= parkingZone.Length)
	{
		Console.WriteLine("Este parque está cheio.");
		pressKeyToContinue();
		return true;
	}
	return false;
}

void parkCar(int zoneNumber, DateTime[] parkingZone, DateTime exitTime, int change)  
{
	// looks for the first empty spot
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
        {
			// fills it with an exitTime
			parkingZone[i] = exitTime;		

			//Ticket class example
			var ticket = new Ticket(zoneNumber, i + 1, exitTime);
			ticketHistory.Add(ticket);
			Console.Clear();
			ticket.showTicket();
			if (change > 0)
				giveChange(change);
			resetCounters();
			pressKeyToContinue();
			displayMainMenu();
        }
	}
}

void removeCar(DateTime[] parkingZone, int id) 
{
	// the +1 added for visual purposes is removed here before validations are made
	id = id - 1;

	// the id is invalid if it's not one of the array's indexes
	if ((id > (parkingZone.Length -1) || id < 0))
	{
		Console.WriteLine("ID Inválido.");
		pressKeyToContinue();
		displayClientMenu();
	}
	if (parkingZone[id] == new DateTime()) 
	{
		Console.WriteLine("Não há nenhum carro estacionado neste lugar.");
		pressKeyToContinue();
		displayClientMenu();
	}

	// empties the spot if the id is valid
	parkingZone[id] = new DateTime();
	Console.Clear();
	Console.WriteLine("Obrigado pela sua preferência.");
	pressKeyToContinue();
	displayMainMenu();
}

void giveChange(int change)
{
	Console.WriteLine("\nTroco:");
	int countCoins(int change, int coinValue)
	{
		int numberOfCoins = 0;
		while (change >= coinValue)
		{
			change -= coinValue;
			numberOfCoins++;
		}
		if (numberOfCoins > 0)
		{
			double coinValueDouble = (double)coinValue/100;
			Console.WriteLine(numberOfCoins + " moeda(s) de " + coinValueDouble.ToString("n2") + "€");
		}
		return change;	// returns remaining change
	}

	for (int i = coins.Length-1; i >= 0; i--)
	{
		if (change == 0)
			break;
		change = countCoins(change, coins[i]);
	}
}

int getIdFromUser()
{
	Console.Write("Introduza o seu ID: ");
	string idStr = Console.ReadLine();
	if (string.IsNullOrEmpty(idStr))
	{
		Console.WriteLine("ID Inválido.");
		pressKeyToContinue();
		displayClientMenu();
	}
	int id;
	bool tryParse = int.TryParse(idStr, out id);
	if (!tryParse)
	{
		Console.WriteLine("ID Inválido.");
		pressKeyToContinue();
		displayClientMenu();
	}
	return id;
}

// Regra de 3 simples para obter o total de segundos conforme os cêntimos introduzidos
int getSeconds(int centsPerHour, int insertedCents)
{
	//  centsPerHour ------- 3600 seconds
	// insertedCents -------  𝑥 seconds
	return insertedCents * 3600 / centsPerHour;
}

// Regra de 3 simples para obter o custo do tempo máximo em cêntimos
int getMaxCents(int centsPerHour, int maxTimeSeconds)
{
	//  centsPerHour ------- 3600 seconds
	//    𝑥 cents    ------- maxTimeSeconds
	return centsPerHour * maxTimeSeconds / 3600;
}

// waits for user input before displaying the next menu
void pressKeyToContinue()
{
	Console.WriteLine("\nPressione qualquer tecla para continuar.");
	Console.ReadKey(true);
	// bool determines whether to display the pressed key in the console window
	// true to not display the pressed key    
}

void resetCounters()
{
	insertedCents = 0;
	addedHours = 0;
	addedMinutes = 0;
	addedSeconds = 0;
}

