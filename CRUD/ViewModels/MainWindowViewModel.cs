﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;

using Avalonia;
using Avalonia.Platform;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CRUD.Models;

using DynamicData;

namespace CRUD.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
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
    private string? tbName;

    [ObservableProperty]
    private string? tbSurName;

    [ObservableProperty]
    private string filterString = string.Empty;

    partial void OnFilterStringChanged(string value) => filter.OnNext(CreateFilter());

    [RelayCommand]
    private void OnDelete()
    {
        if (SelectedContact != null)
        {
            _ = backingContacts.Remove(SelectedContact);
        }
    }

    [RelayCommand]
    private void OnUpdate()
    {
        if (SelectedContact != null && TbName != null && TbSurName != null)
        {
            SelectedContact.Name = TbName;
            SelectedContact.SurName = TbSurName;
        }
    }

    [RelayCommand]
    private void OnCreate()
    {
        if (TbName != null && TbSurName != null)
        {
            ObservableContact c = new(TbName, TbSurName);
            backingContacts.Add(c);
        }
    }

    private static ObservableContact[] LoadData()
    {
        IAssetLoader? assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        using Stream contactsOnDisk = assets!.Open(new System.Uri("avares://CRUD/Assets/MOCK_DATA.csv"));
        using StreamReader reader = new(contactsOnDisk);
        string? line;
        ObservableContact[] contacts = new ObservableContact[1000];
        int idx = 0;
        while ((line = reader.ReadLine()) != null)
        {
            string[] contact = line.Split(',');
            contacts[idx] = new ObservableContact(contact[0], contact[1]);
            idx++;
        }
        return contacts;
    }

    public void Dispose()
    {
        list.Dispose();
        filter.Dispose();
        backingContacts.Dispose();
    }
}
