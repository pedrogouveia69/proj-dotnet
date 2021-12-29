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
int totalDinheiroAcumulado = 0;

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
	if ((dateTimeNow.DayOfWeek == DayOfWeek.Sunday) || (dateTimeNow.DayOfWeek == DayOfWeek.Saturday && dateTimeNow.Hour < 9 && dateTimeNow.Hour > 14) || (dateTimeNow.Hour < 9 && dateTimeNow.Hour > 20))
	{
		Console.WriteLine("O parque está fechado");
		Console.WriteLine("Horário de funcionamento");
		Console.WriteLine("Dias de semana 9:00 - 20:00");
		Console.WriteLine("Sabados das 9:00 - 14:00");
		Console.WriteLine("Domingos Encerrado");		
		//falta encerrar programa
	}
	else
	{
		Console.Clear();
		Console.WriteLine("----- " + title + " -----");
		for (int i = 0; i < options.Length; i++)
		{
			Console.WriteLine("   " + (i + 1) + " - " + options[i]);
		}

		Console.WriteLine("------------------");
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
			Console.WriteLine("Insira a palavra-passe: ");
			string passsword = Console.ReadLine();
			if (!passwordIsCorrect(passsword) || string.IsNullOrEmpty(passsword))
			{
				Console.WriteLine("Password Incorreta");
				Thread.Sleep(3000);
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
			Console.ReadLine();
			displayMainMenu();
			break;
		case 2:
			Console.WriteLine("Total Acumulado: " + totalDinheiroAcumulado);
			Console.ReadLine();
			displayMainMenu();
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
	displayMenu("Selecione uma zona: ", zoneOptions);
	int option = selectOption(zoneOptions.Length);

	if (userIsParking)
	{
		switch (option)
		{
			case 1:
				if (parkIsFull(parkingOne))
				{
					Console.WriteLine("Este parque está cheio.");
					Thread.Sleep(3000);
					displayParkingZones(true);
				}
				else
					insertCoins(parkingOne, parkingOneCentsPerHour, parkingOneMaxSeconds);
				break;

			case 2:
				if (parkIsFull(parkingTwo))
				{
					Console.WriteLine("Este parque está cheio.");
					Thread.Sleep(3000);
					displayParkingZones(true);
				}
				else
					insertCoins(parkingTwo, parkingTwoCentsPerHour, parkingTwoMaxSeconds);
				break;

			case 3:
				if (parkIsFull(parkingThree))
				{
					Console.WriteLine("Este parque está cheio.");
					Thread.Sleep(3000);
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
				Console.WriteLine("Introduza o ID");
				// needs empty/null verification +  ID
				int id = int.Parse(Console.ReadLine());
				removeCar(parkingOne, id);
				break;
			case 2:
				Console.WriteLine("Introduza o ID");
				// needs empty/null verification +  ID
				int id2 = int.Parse(Console.ReadLine());
				removeCar(parkingTwo, id2);
				// falta parque 2
				break;
			case 3:
				Console.WriteLine("Introduza o ID");
				// needs empty/null verification +  ID
				int id3 = int.Parse(Console.ReadLine());
				removeCar(parkingThree, id3);
				// falta parque 3
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

	Console.WriteLine("\nCusto por hora: " + (float)centsPerHour/100 + "€");
	if (maxTimeSeconds != -1) 
	{
		Console.WriteLine("Tempo máximo permitido: " + maxTimeSeconds/60 + " minutos");
		Console.WriteLine();
	}

	if (totalCents != 0) { 
		Console.WriteLine("Total: " + (float)totalCents/100 + "€");     //faltam casas decimais
		Console.WriteLine("Duração: " + duration);
		Console.WriteLine();
	}

	if (duration > dateTimeNow.AddSeconds(maxTimeSeconds) && maxTimeSeconds != -1)
	{
		Console.WriteLine("Excedeu o tempo máximo permitido.");
		totalCents = 0;
		Thread.Sleep(3000);
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
		Thread.Sleep(3000);
		insertCoins(parkingZone, centsPerHour, maxTimeSeconds);
	}
	else if (option == 7)
    {
		totalDinheiroAcumulado += totalCents;
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
		return true;

	return false;
}

int getSecondsPerSingleCent(int centsPerHour)
{
	return 1 * 3600 / centsPerHour;
}

void displayAllParkingSpots(int zoneNumber, DateTime[] parkingZone) 
{
	var dateTimeNow = DateTime.Now;

	Console.WriteLine();
	Console.WriteLine("Zona " + zoneNumber + ": ");
	for (int i = 0; i < parkingZone.Length; i++) 
	{
		if (parkingZone[i] == new DateTime())
			Console.WriteLine("[ Lugar disponível ]");
		else if (dateTimeNow > parkingZone[i])
			Console.WriteLine("[ Carro execede o tempo pago! " + parkingZone[i] + "]");
		else
			Console.WriteLine("[ Lugar ocupado até: " + parkingZone[i] + "]");
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
			Console.WriteLine("ID=" + i + "; DURACAO=" + duration);	//alterar
			totalCents = 0;
			Thread.Sleep(5000);
			displayMainMenu();
        }
	}
}

void removeCar(DateTime[] parkingZone, int id)
{
	if ((id > (parkingZone.Length -1) || id < 0))
	{
		Console.WriteLine("ID Inválido.");
		Thread.Sleep(3000);
		displayClientMenu();
	}
	else if (parkingZone[id] == new DateTime()) 
	{
		Console.WriteLine("Não há nenhum carro estacionado neste lugar.");
		Thread.Sleep(3000);
		displayClientMenu();
	}
	else
	{
		parkingZone[id] = new DateTime();
		Console.Clear();
		Console.WriteLine("Obrigado pela sua preferência.");
		Thread.Sleep(3000);
		displayMainMenu();
	}
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

var password = "YmFgZw==";

if (cryptString(Console.ReadLine()) == password)
{
	Console.WriteLine(true);
}
*/