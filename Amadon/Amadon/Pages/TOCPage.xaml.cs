using AmadonBlazor.Classes;
using UbStandardObjects;

namespace Amadon;

public partial class TOCPage : ContentPage
{

    TocTreeviewBuilder tocTreeview= new TocTreeviewBuilder();

    public TOCPage()
	{
		InitializeComponent();
        ProcessTreeView();

    }

    private void ProcessTreeView()
    {
        var xamlItemGroups = tocTreeview.GroupData(StaticObjects.Book.RightTranslation);
        var rootNodes = TheTreeView.ProcessXamlItemGroups(xamlItemGroups);
        TheTreeView.RootNodes = rootNodes;
    }

}