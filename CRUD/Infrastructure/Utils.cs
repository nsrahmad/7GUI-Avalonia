using System.Collections.Generic;
using System.IO;

using Avalonia;
using Avalonia.Platform;

using CRUD.Models;

namespace CRUD.Infrastructure;
internal static class Utils
{
    public static List<ObservableContact> LoadData()
    {
        IAssetLoader? assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        using Stream contactsOnDisk = assets!.Open(new System.Uri("avares://CRUD/Assets/MOCK_DATA.csv"));
        using StreamReader reader = new(contactsOnDisk);
        List<ObservableContact> contacts = new();
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] contact = line.Split(',');
            contacts.Add(new ObservableContact(contact[0], contact[1]));
        }
        return contacts;
    }
}
