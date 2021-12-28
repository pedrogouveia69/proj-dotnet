using System.Text;

int num; // = 0
DateTime date; // = 01-Jan-01 00:00:00


var rand = new Random();
var parkingOne = new DateTime[rand.Next(1, 10)];
var parkingTwo = new DateTime[rand.Next(1, 10)];
var parkingThree = new DateTime[rand.Next(1, 10)];

int totalInCents = 0;


setupParkingZone(parkingOne);
setupParkingZone(parkingTwo);
setupParkingZone(parkingThree);

void setupParkingZone(DateTime[] parkingZone)
{
	for (int i = 0; i < parkingZone.Length; i++)
	{
		parkingZone[i] = new DateTime();
	}
}

void displayMenu(string title, string[] options) 
{
    Console.Clear();
    Console.WriteLine(title);
    for (int i = 0; i < options.Length; i++) {
        Console.WriteLine(i + 1 + " - " + options[i]);
    }
	Console.Write("Escolha uma opção: ");
}

int checkIfValidOption(int optionsLength)
{
	string optionStr = Console.ReadLine();

	if (string.IsNullOrEmpty(optionStr))
	{
		Console.WriteLine("Não foi selecionada nenhuma opção!");
		Console.Write("Escolha uma opção: ");
		return checkIfValidOption(optionsLength);
	}

	int option;
	bool tryParse = int.TryParse(optionStr, out option);
	if (!tryParse || (option < 1 || option > optionsLength))
	{
		Console.WriteLine("Opção inválida!");
		Console.Write("Escolha uma opção: ");
		return checkIfValidOption(optionsLength);
	}

	return option;
}

void displayMainMenu() 
{
	var mainMenuOptions = new string[] { "Menu Admin", "Menu Cliente" };
	displayMenu("Bem-vindo", mainMenuOptions);
	int option = checkIfValidOption(mainMenuOptions.Length);
	switch (option)
	{
		case 1:
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
		case 2:
			displayClientMenu();
			break;
	}
}

void displayAdminMenu() 
{
	var adminMenuOptions = new string[] { "Ver Zonas", "Ver Histórico", "Ver Máquinas", "displayAllParkingSpots()", "Voltar" };
	displayMenu("Menu Admin", adminMenuOptions);
	int option = checkIfValidOption(adminMenuOptions.Length);
	switch (option)
	{
		case 4:
			Console.WriteLine("Zona 1");
			displayAllParkingSpots(parkingOne);
			Console.WriteLine();
			Console.WriteLine("Zona 2");
			displayAllParkingSpots(parkingTwo);
			Console.WriteLine();
			Console.WriteLine("Zona 3");
			displayAllParkingSpots(parkingThree);
			Console.WriteLine();
			Console.ReadLine();
			displayMainMenu();
			break;
		case 5:
			displayMainMenu();
			break;
	}
};

void displayClientMenu()
{
	var clientMenuOptions = new string[] { "Estacionar", "Remover Carro", "Voltar" };
	displayMenu("Menu Cliente", clientMenuOptions);

	int option = checkIfValidOption(clientMenuOptions.Length);
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

//receber argumento estacionar ou remover
void displayParkingZones(bool userIsParking)
{
	var zoneOptions = new string[] { "Zona 1", "Zona 2", "Zona 3" };
	displayMenu("Selecione uma zona: ", zoneOptions);
	int option = checkIfValidOption(zoneOptions.Length);

	//variaveis com preço p/hora e tempo maximo

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
					parkCar(parkingOne, 120);
				break;

			case 2:
				inserirMoedas();
				break;

			case 3:
				inserirMoedas();
				break;
		}
	}
	else
	{
		switch (option)
		{
			case 1:
				Console.WriteLine("Introduza o ID");
				// needs empty/null verification
				int id = int.Parse(Console.ReadLine());
				removeCar(parkingOne, id);
				break;
		}
	}
};

//no need for else
bool passwordIsCorrect(string password)
{
	string correctPassword = "1234";
	if (password == correctPassword)
    {
		return true;
    }
    else
    {
		return false;
    }
}

// ta tudo fodido
void inserirMoedas()
{
	int[] coinCalc = new int[] { 5, 10, 20, 50, 100, 200 };
	var coinDisplay = new string[] { "0.05€", "0.10€", "0.20€", "0.50€", "1.00€", "2.00€" , "Confirmar", "Reset"};
	
	displayMenu("Insira uma Moeda", coinDisplay);
	//falta casas decimais
	Console.WriteLine("\nTotal: " + (float)totalInCents/100 + "€");
	int option = checkIfValidOption(coinDisplay.Length);

	while (option != 7 || option != 8)
	{
		for (int i = 0; i < coinDisplay.Length; i++)
		{
			if (option - 1 == i)
			{
				totalInCents += coinCalc[i];
			}
		}
		inserirMoedas();
	}
	if ((option == 7 || option == 8) && totalInCents > 0)
    {
		Console.WriteLine("Não é possível confirmar/fazer reset sem introduzir dinheiro.");
		Thread.Sleep(3000);
		inserirMoedas();
	}
	else if (option == 7)
    {
		parkCar(parkingOne, getSecondsPerCent(115) * totalInCents);
    }	
	else if (option == 8)
    {
		totalInCents = 0;
		inserirMoedas();
    }
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

// remove uneeded variable
int getSecondsPerCent(int cents)
{
	return 1 * 3600 / cents;
}

void displayAllParkingSpots(DateTime[] parkingZone) 
{
	for (int i = 0; i < parkingZone.Length; i++) 
	{
		Console.WriteLine(parkingZone[i]);
	}
}

void parkCar(DateTime[] parkingZone, int seconds)
{
	DateTime now = DateTime.Now;
	DateTime totalTime = now.AddSeconds(seconds);

	for (int i = 0; i < parkingZone.Length; i++)
	{
		if (parkingZone[i] == new DateTime())
        {
			parkingZone[i] = totalTime;
			Console.WriteLine("Isto é o ticket!!!");
			Console.WriteLine("ID=" + i + "; DURACAO=" + totalTime);
			totalInCents = 0;
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
		Console.WriteLine("Obrigado pela sua preferência.");
		Thread.Sleep(3000);
		displayMainMenu();
	}
}

inserirMoedas();
//displayMainMenu();

/*
// DateTime comparision is possible
if (total > now) {
	Console.WriteLine(true);
}
*/


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