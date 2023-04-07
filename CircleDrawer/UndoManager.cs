using System.Collections.Generic;

namespace CircleDrawer;

public class UndoManager<T> where T : class
{
    private readonly Stack<T> undoStack;
    private readonly Stack<T> redoStack;

    public UndoManager(T initialState)
    {
        undoStack = new Stack<T>();
        undoStack.Push(initialState);
        redoStack = new Stack<T>();
        redoStack.Push(initialState);
    }

    public void Record(T item)
    {
        undoStack.Push(item);
    }

    public void Undo()
    {
        if (undoStack.Count > 0 && undoStack.TryPop(out var item))
        {
            redoStack.Push(item);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0 && redoStack.TryPop(out var item))
        {
            undoStack.Push(item);
        }
    }

    public T? Current()
    {
        return undoStack.TryPeek(out var item) ? item : null;
    }
}