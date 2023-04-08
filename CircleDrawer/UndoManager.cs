using System.Collections.Generic;

namespace CircleDrawer;

public class UndoManager<T>
{
    private readonly Stack<T> undoStack = new();
    private readonly Stack<T> redoStack = new();

    public UndoManager(T initialState)
    {
        undoStack.Push(initialState);
        redoStack.Push(initialState);
    }

    public T Record(T item)
    {
        undoStack.Push(item);
        return Current();
    }

    public T Undo()
    {
        redoStack.Push(undoStack.Pop());
        return Current();
    }

    public T Redo()
    {
        undoStack.Push(redoStack.Pop());
        return Current();
    }

    public T Current() => undoStack.Peek();

    public bool IsUndoAvailable() => undoStack.Count > 1;

    public bool IsRedoAvailable() => redoStack.Count > 1;
}