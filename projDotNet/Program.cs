using System.Text;

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
			Console.WriteLine("INSERIR PASS");
			string pass = Console.ReadLine();
			bool resultado = isPasswordCorrect(pass);
			if (resultado)
            {
				displayAdminMenu();
			} 
			else
            {
				Console.WriteLine("password incorreta");
				// falta timer
				displayMainMenu();
            }
			break;
		case 2:
			displayClientMenu();
			break;
	}
}

void displayAdminMenu() 
{
	var adminMenuOptions = new string[] { "Ver Zonas", "Ver Histórico", "Ver Máquinas", "Voltar" };
	displayMenu("Menu Admin", adminMenuOptions);
	int option = checkIfValidOption(adminMenuOptions.Length);
	switch (option)
	{
		case 4:
			displayMainMenu();
			break;
	}
};

bool isPasswordCorrect(string password)
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

void displayClientMenu() 
{
	var clientMenuOptions = new string[] { "Ver Zonas", "Voltar" };
	displayMenu("Menu Cliente", clientMenuOptions);

	int option = checkIfValidOption(clientMenuOptions.Length);
	switch (option)
	{
		case 1:
			displayParkingZones();
			break;
	}

};

void displayParkingZones()
{
	var clientMenuOptions = new string[] { "Zona 1", "Zona 2", "Zona 3" };
	displayMenu("Zonas", clientMenuOptions);

	int option = checkIfValidOption(clientMenuOptions.Length);
	switch (option)
	{
		case 1:
			displayZone(1);
			break;

		case 2:
			displayZone(2);
			break;

		case 3:
			displayZone(3);
			break;
	}
};

void displayZone(int zoneNumber)
{
	var zoneoptions = new string[] { "estacionar", "remover carro" };
	displayMenu("zona" + zoneNumber, zoneoptions);

	int option = checkIfValidOption(zoneoptions.Length);
	switch (option)
	{
		case 1:
			inserirMoedas();
			break;

	}
}



void inserirMoedas()
{
	var coins = new string[] {
		"0.5€", "0.10€", "0.20€", "0.50€", "1€", "2€"
    };

	displayMenu("Inserir moedas", coins);

}



bool parkIsFull(DateTime[] parkingSpots)
{
	int occupiedSpots = 0;
	for (int i = 0; i < parkingSpots.Length; i++)
	{
		if (parkingSpots[i] != new DateTime())
			occupiedSpots++;
	}

	if (occupiedSpots >= parkingSpots.Length)
		return true;

	return false;
}

void calculateCentsPerSecond(int cents)
{
	int seconds = 0;
	seconds = 1 * 3600 / cents;

	Console.WriteLine(seconds);
}

var rand = new Random();
var parkingOneSpots = new DateTime[rand.Next(1, 10)];

displayMainMenu();
 /*
for (int i = 0; i < parkingOneSpots.Length; i++)
{
	Console.WriteLine(parkingOneSpots[i]);
}
*/ 

// Add time to DateTime
DateTime now = DateTime.Now;
DateTime total = now.AddSeconds(60);

/*
// DateTime comparision is possible
if (total > now) {
	Console.WriteLine(true);
}
*/
/*
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

/*
int age = 69;
Console.WriteLine(getAge());

void addToAge(int add) {
	age = age + add;
}

int getAge() 
{
	return age;
}

addToAge(12);

Console.WriteLine(getAge());
*/

