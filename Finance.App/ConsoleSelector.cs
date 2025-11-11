using System;
using System.Collections.Generic;

public static class ConsoleSelector
{
    public static T Select<T>(List<T> items, Func<T, string> display)
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Список пуст.");

        int index = 0;
        ConsoleKey key;

        Console.CursorVisible = false;

        do
        {
            Console.Clear();
            Console.WriteLine("Используйте ↑ ↓ для выбора, Enter для подтверждения:\n");

            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                }

                Console.WriteLine(display(items[i]));
                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;
            index = key switch
            {
                ConsoleKey.UpArrow => (index == 0 ? items.Count - 1 : index - 1),
                ConsoleKey.DownArrow => (index + 1) % items.Count,
                _ => index
            };

        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        Console.Clear();
        return items[index];
    }
}
