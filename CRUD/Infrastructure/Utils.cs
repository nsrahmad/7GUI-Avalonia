using System.Collections.Generic;
using System.IO;

using Avalonia;
using Avalonia.Platform;

using CRUD.Models;

namespace CRUD.Infrastructure;
internal static class Utils
{
    public static IEnumerable<ObservableContact> LoadData()
    {
        using var contactsOnDisk = AssetLoader.Open(new System.Uri("avares://CRUD/Assets/MOCK_DATA.csv"));
        using StreamReader reader = new(contactsOnDisk);
        List<ObservableContact> contacts = new();

        while (reader.ReadLine() is { } line)
        {
            var contact = line.Split(',');
            contacts.Add(new ObservableContact(contact[0], contact[1]));
        }
        return contacts;
    }
}
