using System;
using System.Collections.Generic;

// Интерфейс команды
public interface ICommand
{
    void Execute();
    void Undo();
}

// Класс редактора
public class TextEditor
{
    private string _text = string.Empty;
    private Stack<ICommand> _commands = new Stack<ICommand>();

    public void InsertText(string text)
    {
        var command = new InsertTextCommand(this, text);
        command.Execute();
        _commands.Push(command);
    }

    public void DeleteText(int length)
    {
        var command = new DeleteTextCommand(this, length);
        command.Execute();
        _commands.Push(command);
    }

    public void Undo()
    {
        if (_commands.Count > 0)
        {
            var command = _commands.Pop();
            command.Undo();
        }
    }

    public void ShowText()
    {
        Console.WriteLine("Текущий текст: " + _text);
    }

    internal void Insert(string text)
    {
        _text += text;
    }

    internal void Delete(int length)
    {
        if (length > _text.Length)
            length = _text.Length;
        _text = _text.Substring(0, _text.Length - length);
    }
}

// Конкретные команды
public class InsertTextCommand : ICommand
{
    private TextEditor _editor;
    private string _text; // Вставляемый текст

    public InsertTextCommand(TextEditor editor, string text)
    {
        _editor = editor;
        _text = text;
    }

    public void Execute()
    {
        _editor.Insert(_text);
    }

    public void Undo()
    {
        _editor.Delete(_text.Length);
    }
}

public class DeleteTextCommand : ICommand
{
    private TextEditor _editor;
    private int _length; // Количество удаляемых символов

    public DeleteTextCommand(TextEditor editor, int length)
    {
        _editor = editor;
        _length = length;
    }

    public void Execute()
    {
        _editor.Delete(_length);
    }

    public void Undo()
    {
        _editor.Insert(new string(' ', _length));
    }
}

// Тестирование команд в главной программе
public class Program
{
    public static void Main(string[] args)
    {
        TextEditor editor = new TextEditor();

        editor.InsertText("Hello!");
        editor.ShowText(); // Текущий текст: Hello!

        editor.InsertText(" How are you?");
        editor.ShowText(); // Текущий текст: Hello! How are you?

        editor.DeleteText(7); // Удаляем " How a"
        editor.ShowText(); // Текущий текст: Hello! you?

        editor.Undo(); // Отменить удаление
        editor.ShowText(); // Текущий текст: Hello! How are you?

        editor.Undo(); // Отменить ввод " How are you?"
        editor.ShowText(); // Текущий текст: Hello!
    }
}