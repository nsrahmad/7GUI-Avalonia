using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CRUD.Models;

using DynamicData;
using DynamicData.Binding;

namespace CRUD.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject, IDisposable
{
    public MainWindowViewModel()
    {
        backingContacts.AddRange(Infrastructure.Utils.LoadData());

        IObservable<Func<ObservableContact, bool>> dynamicFilter = this.WhenValueChanged(@this => @this.FilterString).Select(CreateFilter!);

        list = backingContacts.Connect()
            .Filter(dynamicFilter)
            .Sort(SortExpressionComparer<ObservableContact>.Ascending(c => c.SurName))
            .Bind(out contacts)
            .DisposeMany()
            .Subscribe();
    }

    private readonly IDisposable list;
    private readonly SourceList<ObservableContact> backingContacts = new();

    private static Func<ObservableContact, bool> CreateFilter(string filter) => c => c.SurName.StartsWith(filter, StringComparison.OrdinalIgnoreCase);

    private readonly ReadOnlyObservableCollection<ObservableContact> contacts;
    public ReadOnlyObservableCollection<ObservableContact> Contacts => contacts;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    private ObservableContact? selectedContact;

    partial void OnSelectedContactChanged(ObservableContact? value)
    {
        if (value != null)
        {
            TbName = value.Name;
            TbSurName = value.SurName;
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
    private string? tbName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
    private string? tbSurName;

    private bool IsNameNotNull => !string.IsNullOrWhiteSpace(TbName) && !string.IsNullOrWhiteSpace(TbSurName);

    [ObservableProperty]
    private string filterString = string.Empty;

    private bool IsSelectedContact => SelectedContact != null;

    [RelayCommand(CanExecute = nameof(IsSelectedContact))]
    private void OnDelete() => _ = backingContacts.Remove(SelectedContact!);

    [RelayCommand(CanExecute = nameof(IsSelectedContact))]
    private void OnUpdate()
    {
        if (!string.IsNullOrWhiteSpace(TbName) && !string.IsNullOrWhiteSpace(TbSurName))
        {
            SelectedContact!.Name = TbName;
            SelectedContact.SurName = TbSurName;
        }
    }

    [RelayCommand(CanExecute = nameof(IsNameNotNull))]
    private void OnCreate() => backingContacts.Add(new ObservableContact(TbName!, TbSurName!));

    public void Dispose()
    {
        list.Dispose();
        backingContacts.Dispose();
    }
}
