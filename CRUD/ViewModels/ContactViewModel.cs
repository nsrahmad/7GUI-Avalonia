using CommunityToolkit.Mvvm.ComponentModel;

namespace CRUD.ViewModels;
public partial class ContactViewModel : ObservableObject
{
    public ContactViewModel(string name, string surName)
    {
        this.name = name;
        this.surName = surName;
    }
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? surName;
}