void buildMenu(string title, string[] options) 
{
    Console.WriteLine(title);
    for (int i = 0; i < options.Length; i++) {
        Console.WriteLine(i + 1 + " - " + options[i]);
    } 
}

buildMenu("Bem-vindo", new string[] { "Opção 1", "Opção 2" });