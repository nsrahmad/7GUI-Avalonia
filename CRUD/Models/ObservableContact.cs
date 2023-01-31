using CommunityToolkit.Mvvm.ComponentModel;

namespace CRUD.Models;

public partial class ObservableContact : ObservableObject
{
    public ObservableContact(string name, string surName)
    {
        this.name = name;
        this.surName = surName;
    }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string surName;
}