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
int totalAccumulatedCents = 0;

string adminPassword = "1234";

int hoursAdded = 0;

// Fill with parks with empty spots
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
	// O horário de funcionamento dos parquímetros é das 9h às 20h durante os dia úteis e das 9h às 14h nos sábados.
	/*
	var dateTimeNow = DateTime.Now;
	if ((dateTimeNow.DayOfWeek == DayOfWeek.Sunday) || (dateTimeNow.DayOfWeek == DayOfWeek.Saturday && (dateTimeNow.Hour < 9 || dateTimeNow.Hour >= 14)) || (dateTimeNow.Hour < 9 || dateTimeNow.Hour >= 20))
	//if (false)
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
	*/
	Console.Clear();
	Console.WriteLine("------- " + title + " -------");
	for (int i = 0; i < options.Length; i++)
	{
		Console.WriteLine("   " + (i + 1) + " - " + options[i]);
	}
	string hyphens = "----------------";
	for (int j = 0; j < title.Length; j++){ hyphens += "-"; }
	Console.WriteLine(hyphens);
	//}
}

// MENU DE OPÇOES
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

// MENU PRINCIPAL
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
			// Hide user password input
			// https://stackoverflow.com/questions/23433980/c-sharp-console-hide-the-input-from-console-window-while-typing
			string password = null;
			while (true)
			{
				var key = System.Console.ReadKey(true);
				if (key.Key == ConsoleKey.Enter)
					break;
				password += key.KeyChar;
			}
			// --------------------------
			if (password != adminPassword || string.IsNullOrEmpty(password))
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
	}
}

// MENU DE ADMNISTRADOR
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
			// Visualizar o total(€) que foi obtido durante o dia.
			float totalAccumulatedEuros = (float)totalAccumulatedCents/100;
			Console.WriteLine("\nTotal Acumulado: " + totalAccumulatedEuros.ToString("n2") + "€");
			pressKeyToContinue();
			displayAdminMenu();
			break;
		case 3:
			displayMainMenu();
			break;
	}
};

// OPÇOES DE ESTACIONAR E REMOVER CARRO
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

// OPÇOES DE MOSTRAR ZONAS
void displayParkingZones(bool userIsParking) 
{
	string[] zoneOptions = { "Zona 1", "Zona 2", "Zona 3", "Voltar" };
	displayMenu("Selecione uma zona", zoneOptions);
	int option = selectOption(zoneOptions.Length);

	if (userIsParking)
	{
		switch (option) // INTRODUZIR CARRO
		{
			case 1:
				if (parkIsFull(parkingOne))
					displayParkingZones(true);
				else
					insertCoins(option, parkingOne, parkingOneCentsPerHour, parkingOneMaxSeconds);
				break;

			case 2:
				if (parkIsFull(parkingTwo))
					displayParkingZones(true);
				else
					insertCoins(option, parkingTwo, parkingTwoCentsPerHour, parkingTwoMaxSeconds);
				break;

			case 3:
				if (parkIsFull(parkingThree))
					displayParkingZones(true);
				else
					insertCoins(option, parkingThree, parkingThreeCentsPerHour, parkingThreeMaxSeconds);
				break;
			case 4:
				displayClientMenu();
				break;
		}
	}
	else
	{
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
	}
};

// MENU DE CALCULO DE MOEDAS
void insertCoins(int zoneNumber, DateTime[] parkingZone, int centsPerHour, int maxTimeSeconds) 
{
	int seconds = getSeconds(centsPerHour, totalInsertedCents);

	var dateTimeNow = DateTime.Now;
	var exitTime = dateTimeNow.AddSeconds(seconds);

	string[] coinsForDisplay = { "0.05€", "0.10€", "0.20€", "0.50€", "1.00€", "2.00€", "Confirmar", "Cancelar" };
	int[] coinsForCalc = { 5, 10, 20, 50, 100, 2400 };

	// Não pode ser cobrada nenhuma tarifa fora do horário de funcionamento do parquímetro.
	// Jump to next open day
	// Zona 3 bug

	exitTime = checkExitTime(exitTime, seconds);

	/*
	if (exitTime.DayOfWeek == DayOfWeek.Saturday && exitTime.Hour >= 14)
    {
		exitTime = dateTimeNow.AddSeconds(seconds).AddHours(19).AddDays(1);
		hoursAdded += 19 + 24;
	} 
	else if (exitTime.Hour >= 20 || exitTime.Hour < 9)
    {
		exitTime = dateTimeNow.AddSeconds(seconds).AddHours(13);
		hoursAdded += 13;
	}
	*/

	displayMenu("Insira uma Moeda", coinsForDisplay);

	
	// Informação acerca da zona e do preço a pagar por hora.
	Console.WriteLine("Custo por hora: " + (float)centsPerHour/100 + "€");
	if (maxTimeSeconds != -1) 
	{
		Console.WriteLine("Tempo máximo permitido: " + maxTimeSeconds/60 + " minutos");
	}

	// O utilizador deverá ver a informação mencionada e inserir uma ou várias moedas, cujo total é mostrado no visor.
	// O utilizador poderá fazer reset ou confirmar a operação.
	// O tempo de estacionamento deverá ser calculado com base no montante recebido e deverá ser mostrado ao utilizador.
	if (totalInsertedCents != 0) {
		float totalEuros = (float)totalInsertedCents/100;
		Console.WriteLine("\nTotal: " + totalEuros.ToString("n2") + "€");
		Console.WriteLine("Duração: " + exitTime);
		Console.WriteLine();
		Console.WriteLine("Insira outra moeda ou escolha '7' para Confirmar.");
	}

	if (exitTime > dateTimeNow.AddSeconds(maxTimeSeconds) && maxTimeSeconds != -1)
	{
		// Falta funcao troco
		/*
		Console.WriteLine("Excedeu o tempo máximo permitido.");
		totalInsertedCents = 0;
		pressKeyToContinue();
		insertCoins(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
		*/
	}

	int option = selectOption(coinsForDisplay.Length);

	// OPCOES DE CONFIRMAR E CANCELAR
	while (option != 7 && option != 8) 
	{
		for (int i = 0; i < coinsForCalc.Length; i++)
		{
			if (option - 1 == i)
				totalInsertedCents += coinsForCalc[i];
		}
		insertCoins(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
	}

	if (option == 7 && totalInsertedCents == 0)
    {
		Console.WriteLine("Não é possível Confirmar sem introduzir dinheiro.");
		pressKeyToContinue();
		insertCoins(zoneNumber, parkingZone, centsPerHour, maxTimeSeconds);
	}
	// CONFIRMAR
	else if (option == 7) 
    {
		totalAccumulatedCents += totalInsertedCents;
		parkCar(zoneNumber, parkingZone, exitTime);
    }
	// CANCELAR
	else if (option == 8) 
    {
		totalInsertedCents = 0;
		displayClientMenu();
	}
}

DateTime checkExitTime(DateTime exitTime, int seconds) 
{
	DateTime dateTimeNow = DateTime.Now;

	if (dateTimeNow.DayOfWeek == DayOfWeek.Saturday && exitTime.Hour >= 14)
	{
		return exitTime.AddHours(19).AddDays(1);
	}
	else if (exitTime.Hour >= 20 || exitTime.Hour < 9)
	{		
		if (exitTime.Hour + 13 >= 20 || exitTime.Hour < 9) {
			int aux = exitTime.Hour - 20;
			return exitTime.AddHours(13-aux);
		} 
		else
			return exitTime.AddHours(13);	
	}

	return exitTime;
}

//PARK CHEIO
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

//GET ID PARA REMOVER CARRO
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

// Regra de 3 simples para obter segundos p/cêntimo
int getSeconds(int centsPerHour, int insertedCents)
{
	//  centsPerHour ------- 3600 seconds
	// insertedCents -------  𝑥 seconds

	// This function returns 𝑥

	return insertedCents * 3600 / centsPerHour;
}

// MENU DE MOSTRAR OS LUGARES
void displayAllParkingSpots(int zoneNumber, DateTime[] parkingZone)  
{
	// Ver número de vagas/lugares ocupados em cada zona e carros que exederam o tempo de estacionamento permitido.
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

// FUNÇAO DE ESTACIONAR CARRO
void parkCar(int zoneNumber, DateTime[] parkingZone, DateTime exitTime)  
{
	// Caso o utilizador confirme a operação, durante o tempo estipulado, o lugar deverá estar identificado como indisponível.
	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
        {
			parkingZone[i] = exitTime;
			Console.Clear();
			// O programa deverá apresentar um ticket com aduração permitida.
			Console.WriteLine("--------------------- Ticket ---------------------");
			Console.WriteLine(" O seu carro está estacionado na ZONA " + zoneNumber);
			Console.WriteLine(" O seu ID é " + (i + 1));
			Console.WriteLine(" O estacionamento é válido até " + exitTime);
			Console.WriteLine("--------------------------------------------------");
			totalInsertedCents = 0;
			pressKeyToContinue();
			displayMainMenu();
        }
	}
}

//REMOVER CARRO
void removeCar(DateTime[] parkingZone, int id) 
{
	id = id - 1;
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

int giveChange(int totalInsertedCents)
{
	int change = 0;

	// ...

	return change;
}

