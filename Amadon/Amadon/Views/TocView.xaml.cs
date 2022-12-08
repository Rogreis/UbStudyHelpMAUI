using System.Reflection.PortableExecutable;

namespace Amadon.Views;

public partial class TocTableView : ContentView
{
	public TocTableView()
	{
		InitializeComponent();
    }

    private void FillTocTable()
    {
        Label header = new Label()
        {
            Text = "Part I"
        };
        TocExpander.Header = header;
        TocExpander.Content= header;



    }


}