// See https://aka.ms/new-console-template for more information
using Orca6OutPatch;

if (args.Length == 0)
{
    Console.WriteLine("Укажите файл или директорию для конвертации");
    Console.ReadKey();
    return;
}

foreach (string arg in args)
{
    Patcher patcher = new Patcher(arg);
    patcher.Patch();
}

Console.ReadKey();
