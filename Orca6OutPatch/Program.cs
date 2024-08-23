// See https://aka.ms/new-console-template for more information
using Orca6OutPatch;

if (args.Length == 0)
{
    Console.WriteLine("Укажите файл или директорию для конвертации");
    Console.ReadKey();
    return;
}

List<Task> tasks = new List<Task>();

foreach (string arg in args)
{
    Patcher patcher = new Patcher(arg);
    tasks.Add(Task.Factory.StartNew(() => patcher.Patch()));
}

Task.WaitAll(tasks.ToArray());
Console.WriteLine("");
Console.WriteLine("=== Все задачи завершены ===");
Console.ReadKey();
