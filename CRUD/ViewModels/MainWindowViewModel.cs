using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CRUD.Models;

using DynamicData;

using static CRUD.Infrastructure.Utils;

namespace CRUD.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject, IDisposable
{
    public MainWindowViewModel()
    {
        backingContacts.AddRange(LoadData());

        list = backingContacts.Connect()
            .Filter(filter)
            .Bind(out contacts)
            .DisposeMany()
            .Subscribe();

        filter.OnNext(CreateFilter());
    }

    private readonly IDisposable list;
    private readonly Subject<Func<ObservableContact, bool>> filter = new();
    private readonly SourceList<ObservableContact> backingContacts = new();

    private Func<ObservableContact, bool> CreateFilter() => c => c.SurName.StartsWith(FilterString, StringComparison.OrdinalIgnoreCase);

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

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnFilterStringChanged(string value) => filter.OnNext(CreateFilter());

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
        filter.Dispose();
        backingContacts.Dispose();
    }
}
