using System.Text;
Console.OutputEncoding = Encoding.UTF8;

// 3 zonas de estacionamento (cada zona deverá ter um número aleatório de lugares de estacionamento).
var rand = new Random();
var parkingOne = new DateTime[rand.Next(5, 10)];
var parkingTwo = new DateTime[rand.Next(5, 10)];
var parkingThree = new DateTime[rand.Next(5, 10)];

// Zona 1 tem o custo de 1.15€ / hora e a duração máxima de 45 minutos.
int parkingOneCentsPerHour = 115;
int parkingOneMaxSeconds = 2700;

//Zona 2 tem o curso de 1€ / hora e a duração máxima de 2 horas.
int parkingTwoCentsPerHour = 100;
int parkingTwoMaxSeconds = 7200;

//Zona 3 tem o custo de 0.62€ /hora e não possui duração máxima.
int parkingThreeCentsPerHour = 62;
int parkingThreeMaxSeconds = -1;

int totalInsertedCents = 0;

var ticketHistory = new List<string>();
int totalAccumulatedCents = 0;

string adminPassword = "1234";

// Fill with parks with empty spots
fillWithEmptySpots(parkingOne);
fillWithEmptySpots(parkingTwo);
fillWithEmptySpots(parkingThree);

displayMainMenu();

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
		"Ver Zonas", "Ver Total de Dinheiro Acumulado", "Ver Histórico de Tickets", "Reset Total de Dinheiro Acumulado ", "Reset Histórico de Tickets", "Voltar" 
	};
	displayMenu("Menu Admin", adminMenuOptions);
	int option = selectOption(adminMenuOptions.Length);
	switch (option)
	{
		case 1:
			displayAllParkingSpots(1, parkingOne);
			displayAllParkingSpots(2, parkingTwo);
			displayAllParkingSpots(3, parkingThree);
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 2:
			float totalAccumulatedEuros = (float)totalAccumulatedCents/100;
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
				Console.WriteLine(ticket);
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
			Console.WriteLine("\nO histórico foi apagado com sucesso.");
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

void displayParkingZones(bool userIsParking) 
{
	string[] zoneOptions = { "Zona 1", "Zona 2", "Zona 3", "Voltar" };
	displayMenu("Selecione uma zona", zoneOptions);
	int option = selectOption(zoneOptions.Length);

	if (userIsParking)
	{
		switch (option) // ESTACIONAR CARRO
		{
			case 1:
				if (parkIsFull(parkingOne))
					displayParkingZones(true);
				displayParkingInfo(option, parkingOne, parkingOneCentsPerHour, parkingOneMaxSeconds);
				break;
			case 2:
				if (parkIsFull(parkingTwo))
					displayParkingZones(true);
				displayParkingInfo(option, parkingTwo, parkingTwoCentsPerHour, parkingTwoMaxSeconds);
				break;

			case 3:
				if (parkIsFull(parkingThree))
					displayParkingZones(true);
				displayParkingInfo(option, parkingThree, parkingThreeCentsPerHour, parkingThreeMaxSeconds);
				break;
			case 4:
				displayClientMenu();
				break;
		}
	}
	switch (option) // REMOVER CARRO
	{
		case 1:				
			removeCar(parkingOne, getIdFromUser());
			break;
		case 2:
			removeCar(parkingTwo, getIdFromUser());
			break;
		case 3:
			removeCar(parkingThree, getIdFromUser());
			break;
		case 4:
			displayClientMenu();
			break;
	}	
};

void displayParkingInfo(int zoneNumber, DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds) 
{
	string[] coinsForDisplay = { "0.05€", "0.10€", "0.20€", "0.50€", "1.00€", "2.00€", "Confirmar", "Cancelar" };

	int seconds = getSeconds(centsPerHour, totalInsertedCents);
	var dateTimeNow = DateTime.Now;
	var exitTime = dateTimeNow.AddSeconds(seconds);
	exitTime = checkExitTime(exitTime);

	displayMenu("Insira uma Moeda", coinsForDisplay);

	Console.WriteLine("Custo por hora: " + (float)centsPerHour/100 + "€");
	if (maxTimeSeconds != -1) 
		Console.WriteLine("Tempo máximo permitido: " + maxTimeSeconds/60 + " minutos");

	showOpenHours();

	if (totalInsertedCents != 0) {
		float totalEuros = (float)totalInsertedCents/100;
		Console.WriteLine("\nTotal: " + totalEuros.ToString("n2") + "€");

		if (seconds > maxTimeSeconds && maxTimeSeconds != -1)
		{
			Console.WriteLine("\nExcedeu o tempo máximo permitido.");
			exitTime = dateTimeNow.AddSeconds(maxTimeSeconds);
			exitTime = checkExitTime(exitTime);
			Console.WriteLine("O seu estacionamento será válido até: " + exitTime);
			Console.WriteLine("Irá receber o troco juntamente com o Ticket.");
			pressKeyToContinue();

			int maxCents = getMaxCents(centsPerHour, maxTimeSeconds);
			int change = totalInsertedCents - maxCents;
			totalAccumulatedCents += maxCents;
			parkCar(zoneNumber, parkingZone, exitTime, change);
		}

		Console.WriteLine("Duração: " + exitTime);
		Console.WriteLine("\nInsira outra moeda ou escolha '7' para Confirmar.");
	}

	manageOptions(coinsForDisplay.Length, zoneNumber, parkingZone, centsPerHour, maxTimeSeconds, exitTime);
}

void manageOptions(int coinsForDisplayLength, int zoneNumber, DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds, DateTime exitTime)
{
	int[] coinsForCalc = { 5, 10, 20, 50, 100, 200 };
	int option = selectOption(coinsForDisplayLength);

	if (option == 7 && totalInsertedCents == 0)
	{
		Console.WriteLine("Não é possível Confirmar sem introduzir dinheiro.");
		pressKeyToContinue();
		displayParkingInfo(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
	}
	if (option == 7)
	{
		totalAccumulatedCents += totalInsertedCents;
		parkCar(zoneNumber, parkingZone, exitTime, 0);
	}
	if (option == 8)
	{
		totalInsertedCents = 0;
		displayClientMenu();
	}

	for (int i = 0; i < coinsForCalc.Length; i++)
		if (option - 1 == i)
			totalInsertedCents += coinsForCalc[i];

	displayParkingInfo(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
}

void displayAllParkingSpots(int zoneNumber, DateTime[] parkingZone)
{
	var dateTimeNow = DateTime.Now;

	Console.WriteLine();
	Console.WriteLine("Zona " + zoneNumber + ":");
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
			Console.WriteLine("[ Lugar " + (i + 1) + " - Disponível ]");
		else if (dateTimeNow > parkingZone[i])
			Console.WriteLine("[ Lugar " + (i + 1) +  " - Carro excede o tempo pago! -> " + parkingZone[i] + " ]");
		else
			Console.WriteLine("[ Lugar " + (i + 1) + " - Ocupado até " + parkingZone[i] + " ]");
	}
}

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

DateTime checkExitTime(DateTime exitTime)
{
	DateTime dateTimeNow = DateTime.Now;

	if (exitTime.DayOfWeek == DayOfWeek.Sunday)
    {
		// add 24 hours to jump to monday
		// remove excess hours/minutes/seconds starting from 9:00
		return exitTime.AddHours(24).AddHours(9 - dateTimeNow.Hour).AddMinutes(-dateTimeNow.Minute).AddSeconds(-dateTimeNow.Second);
    }
	if (exitTime.DayOfWeek == DayOfWeek.Saturday && exitTime.Hour >= 14)
	{
		// add 43 hours to jump to monday
		// remove excess hours/minutes/seconds starting from 14:00
		return exitTime.AddHours(43).AddHours(14 - dateTimeNow.Hour).AddMinutes(-dateTimeNow.Minute).AddSeconds(-dateTimeNow.Second);

	}
	if (exitTime.Hour >= 20 || exitTime.Hour < 9)
	{
		// add 13 hours to jump to next day
		// remove excess hours/minutes/seconds starting from 20:00
		return exitTime.AddHours(13).AddHours(20 - dateTimeNow.Hour).AddMinutes(-dateTimeNow.Minute).AddSeconds(-dateTimeNow.Second);
	}
	return exitTime;
}

/*
DateTime checkExitTime(DateTime exitTime)
{
	DateTime dateTimeNow = DateTime.Now;

	if (dateTimeNow.DayOfWeek == DayOfWeek.Saturday && exitTime.Hour >= 14)
	{
		return exitTime.AddHours(43);
	}
	if (exitTime.Hour >= 20 || exitTime.Hour < 9)
	{
		if (exitTime.Hour + 13 >= 20 || exitTime.Hour < 9)
		{
			int aux = exitTime.Hour - 20;
			return exitTime.AddHours(13-aux);
		}
		else
			return exitTime.AddHours(13);
	}
	return exitTime;
}
*/

void showOpenHours()
{
	var dateTimeNow = DateTime.Now;
	if (
		(dateTimeNow.DayOfWeek == DayOfWeek.Sunday) ||
		(dateTimeNow.DayOfWeek == DayOfWeek.Saturday && (dateTimeNow.Hour < 9 || dateTimeNow.Hour >= 14)) ||
		(dateTimeNow.Hour < 9 || dateTimeNow.Hour >= 20)
	   )
	{
		Console.WriteLine("\nHorário de Funcionamento dos Parquímetros:");
		Console.WriteLine(" - Dias Úteis:  9:00 - 20:00");
		Console.WriteLine(" - Sábados:     9:00 - 14:00\n");
		Console.WriteLine("As suas moedas serão contabilizadas para o próximo período aberto.");
	}
}

void fillWithEmptySpots(DateTime[] parkingZone)
{
	for (int i = 0; i < parkingZone.Length; i++)
		parkingZone[i] = new DateTime();
}

bool parkIsFull(DateTime[] parkingZone) 
{
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
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
        {
			parkingZone[i] = exitTime;
			string ticket = "";
			ticket += "--------------------- Ticket ---------------------";
			ticket += "\n O seu carro está estacionado na ZONA " + zoneNumber;
			ticket += "\n O seu ID é " + (i + 1);
			ticket += "\n O estacionamento é válido até " + exitTime;
			ticket += "\n--------------------------------------------------";
			ticketHistory.Add(ticket);
			Console.Clear();
			Console.Write(ticket);
			if (change > 0)
				giveChange(change);
			totalInsertedCents = 0;
			pressKeyToContinue();
			displayMainMenu();
        }
	}
}

void removeCar(DateTime[] parkingZone, int id) 
{
	id = id - 1;
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

	parkingZone[id] = new DateTime();
	Console.Clear();
	Console.WriteLine("Obrigado pela sua preferência.");
	pressKeyToContinue();
	displayMainMenu();
}

void giveChange(int change)
{
	int[] coins = { 200, 100, 50, 20, 10, 5, 2, 1 };
	Console.WriteLine("\nTroco:");
	int countCoins(int change, int coinValue)
	{
		int numberOfCoins = 0;
		while (change >= coinValue)
		{
			numberOfCoins++;
			change -= coinValue;
		}
		if (numberOfCoins > 0)
		{
			float coinValueFl = (float)coinValue/100;
			Console.WriteLine(numberOfCoins + " moeda(s) de " + coinValueFl.ToString("n2") + "€");
		}
		return change;
	}

	int remainingChange = change;
	for (int i = 0; i < coins.Length; i++)
		remainingChange = countCoins(remainingChange, coins[i]);
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
	//       𝑥       ------- maxTimeSeconds

	return centsPerHour * maxTimeSeconds / 3600;
}

void pressKeyToContinue()
{
	Console.WriteLine("\nPressione qualquer tecla para continuar.");
	Console.ReadKey(true);
}

