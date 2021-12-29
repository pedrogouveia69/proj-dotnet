using System.Text;
Console.OutputEncoding = Encoding.UTF8;

var rand = new Random();
var parkingOne = new DateTime[rand.Next(1, 10)];
var parkingTwo = new DateTime[rand.Next(1, 10)];
var parkingThree = new DateTime[rand.Next(1, 10)];

int parkingOneCentsPerHour = 115;
int parkingTwoCentsPerHour = 100;
int parkingThreeCentsPerHour = 62;

int parkingOneMaxSeconds = 2700;
int parkingTwoMaxSeconds = 7200;
int parkingThreeMaxSeconds = -1;

int totalCents = 0;
int totalAccumulatedMoney = 0;

string adminPassword = "1234";

setupParkingZone(parkingOne);
setupParkingZone(parkingTwo);
setupParkingZone(parkingThree);

displayMainMenu();

void setupParkingZone(DateTime[] parkingZone)
{
	for (int i = 0; i < parkingZone.Length; i++)
	{
		parkingZone[i] = new DateTime();
	}
}

void displayMenu(string title, string[] options)
{
	var dateTimeNow = DateTime.Now;
	//if ((dateTimeNow.DayOfWeek == DayOfWeek.Sunday) || (dateTimeNow.DayOfWeek == DayOfWeek.Saturday && dateTimeNow.Hour < 9 || dateTimeNow.Hour > 14) || (dateTimeNow.Hour < 9 || dateTimeNow.Hour > 20))
	if (false)
	{
		Console.WriteLine("////  O parque está encerrado.  ////");
		Console.WriteLine();
		Console.WriteLine("----- Horário de funcionamento -----");
		Console.WriteLine("|    Dias Úteis: 9:00 - 20:00      |");
		Console.WriteLine("|    Sábados:    9:00 - 14:00      |");
		Console.WriteLine("|    Domingos:   Encerrado         |");
		Console.WriteLine("------------------------------------");
		Environment.Exit(0);
	}
	else
	{
		Console.Clear();
		Console.WriteLine("------- " + title + " -------");
		for (int i = 0; i < options.Length; i++)
		{
			Console.WriteLine("   " + (i + 1) + " - " + options[i]);
		}
		string hyphens = "----------------";
		for (int j = 0; j < title.Length; j++){ hyphens += "-"; }
		Console.WriteLine(hyphens);
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

void displayMainMenu()
{
	string[] mainMenuOptions = { "Menu Cliente" , "Menu Admin" };
	displayMenu("Bem-vindo", mainMenuOptions);
	int option = selectOption(mainMenuOptions.Length);
	switch (option)
	{
		case 2:
			Console.Write("Insira a palavra-passe: ");
			string passsword = Console.ReadLine();
			if (!passwordIsCorrect(passsword) || string.IsNullOrEmpty(passsword))
			{
				Console.WriteLine("Password Incorreta");
				pressKeyToContinue();
				displayMainMenu();
			}
			else
			{
				displayAdminMenu();
			}
			break;
		case 1:
			displayClientMenu();
			break;
	}
}

void displayAdminMenu()
{
	string[] adminMenuOptions = { "Ver Zonas", "Ver Total Dinheiro" , "Voltar" };
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
			Console.WriteLine("\nTotal Acumulado: " + (float)totalAccumulatedMoney/100 + "€");
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 3:
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
		switch (option)
		{
			case 1:
				if (parkIsFull(parkingOne))
				{
					displayParkingZones(true);
				}
				else
					insertCoins(parkingOne, parkingOneCentsPerHour, parkingOneMaxSeconds);
				break;

			case 2:
				if (parkIsFull(parkingTwo))
				{
					displayParkingZones(true);
				}
				else
					insertCoins(parkingTwo, parkingTwoCentsPerHour, parkingTwoMaxSeconds);
				break;

			case 3:
				if (parkIsFull(parkingThree))
				{
					displayParkingZones(true);
				}
				else
					insertCoins(parkingThree, parkingThreeCentsPerHour, parkingThreeMaxSeconds);
				break;
			case 4:
				displayClientMenu();
				break;
		}
	}
	else
	{
		switch (option)
		{
			case 1:				
				removeCar(parkingOne, getID());
				break;
			case 2:
				removeCar(parkingTwo, getID());
				break;
			case 3:
				removeCar(parkingThree, getID());
				break;
			case 4:
				displayClientMenu();
				break;
		}
	}
};

void insertCoins(DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds)
{
	var dateTimeNow = DateTime.Now;
	var duration = dateTimeNow.AddSeconds(getSecondsPerSingleCent(centsPerHour) * totalCents);

	string[] coinsForDisplay = { "0.05€", "0.10€", "0.20€", "0.50€", "1.00€", "2.00€", "Confirmar", "Cancelar" };
	int[] coinsForCalc = { 5, 10, 20, 50, 100, 200 };

	displayMenu("Insira uma Moeda", coinsForDisplay);

	Console.WriteLine("Custo por hora: " + (float)centsPerHour/100 + "€");
	if (maxTimeSeconds != -1) 
	{
		Console.WriteLine("Tempo máximo permitido: " + maxTimeSeconds/60 + " minutos");
	}

	if (totalCents != 0) { 
		Console.WriteLine("\nTotal: " + (float)totalCents/100 + "€");     //faltam casas decimais
		Console.WriteLine("Duração: " + duration);
		Console.WriteLine();
		Console.WriteLine("Insira outra moeda ou escolha '7' para Confirmar.");
	}

	if (duration > dateTimeNow.AddSeconds(maxTimeSeconds) && maxTimeSeconds != -1)
	{
		Console.WriteLine("\nExcedeu o tempo máximo permitido.");
		totalCents = 0;
		pressKeyToContinue();
		insertCoins(parkingZone, centsPerHour, maxTimeSeconds);
	}

	int option = selectOption(coinsForDisplay.Length);

	while (option != 7 && option != 8)
	{
		for (int i = 0; i < coinsForCalc.Length; i++)
		{
			if (option - 1 == i)
			{
				totalCents += coinsForCalc[i];
			}
		}
		insertCoins(parkingZone, centsPerHour, maxTimeSeconds);
	}

	if (option == 7 && totalCents == 0)
    {
		Console.WriteLine("Não é possível Confirmar sem introduzir dinheiro.");
		pressKeyToContinue();
		insertCoins(parkingZone, centsPerHour, maxTimeSeconds);
	}
	else if (option == 7)
    {
		totalAccumulatedMoney += totalCents;
		parkCar(parkingZone, duration);
    }	
	else if (option == 8)
    {
		totalCents = 0;
		displayMainMenu();
	}
}

bool passwordIsCorrect(string password)
{
	if (password == adminPassword)
		return true;

	return false;
}

bool parkIsFull(DateTime[] parkingZone)
{
	int occupiedSpots = 0;
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] != new DateTime())
			occupiedSpots++;
	}

	if (occupiedSpots >= parkingZone.Length)
	{
		Console.WriteLine("Este parque está cheio.");
		pressKeyToContinue();
		return true;
	}
	return false;
}

int getID()
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

int getSecondsPerSingleCent(int centsPerHour)
{
	return 1 * 3600 / centsPerHour;
}

void displayAllParkingSpots(int zoneNumber, DateTime[] parkingZone) 
{
	var dateTimeNow = DateTime.Now;

	Console.WriteLine();
	Console.WriteLine("Zona " + zoneNumber + ":");
	for (int i = 0; i < parkingZone.Length; i++) 
	{
		if (parkingZone[i] == new DateTime())
			Console.WriteLine("[ Lugar " + i + " - Disponível ]");
		else if (dateTimeNow > parkingZone[i])
			Console.WriteLine("[ Lugar " + i + " - Carro excede o tempo pago! -> " + parkingZone[i] + " ]");
		else
			Console.WriteLine("[ Lugar " + i + " - Ocupado até " + parkingZone[i] + " ]");
	}
}

void parkCar(DateTime[] parkingZone, DateTime duration)
{
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
        {
			parkingZone[i] = duration;
			Console.Clear();
			Console.WriteLine("Isto é o ticket!!!");
			Console.WriteLine("ID=" + i + "; DURACAO=" + duration);	//falta a zona no ticket!!!
			totalCents = 0;
			pressKeyToContinue();
			displayMainMenu();
        }
	}
}

void removeCar(DateTime[] parkingZone, int id)
{
	if ((id > (parkingZone.Length -1) || id < 0))
	{
		Console.WriteLine("ID Inválido.");
		pressKeyToContinue();
		displayClientMenu();
	}
	else if (parkingZone[id] == new DateTime()) 
	{
		Console.WriteLine("Não há nenhum carro estacionado neste lugar.");
		pressKeyToContinue();
		displayClientMenu();
	}
	else
	{
		parkingZone[id] = new DateTime();
		Console.Clear();
		Console.WriteLine("Obrigado pela sua preferência.");
		pressKeyToContinue();
		displayMainMenu();
	}
}

void pressKeyToContinue()
{
	Console.WriteLine("\nPressione qualquer tecla para continuar.");
	Console.ReadKey(true);
}

/*
// Encrypt password
//https://stackoverflow.com/questions/38816004/simple-string-encryption-without-dependencies
string cryptString(string input)
{
	byte xorConstant = 0x53;

	byte[] data = Encoding.UTF8.GetBytes(input);
	for (int i = 0; i < data.Length; i++)
	{
		data[i] = (byte)(data[i] ^ xorConstant);
	}
	return Convert.ToBase64String(data);

}

if (cryptString(Console.ReadLine()) == password)
{
	Console.WriteLine(true);
}
*/