using AmadonBlazor.Classes;
using Amadon.Resources.Styles;
using UbStandardObjects;
using UbStandardObjects.Objects;
using static Amadon.Views.PapersView;

namespace Amadon;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        EventsControl.FatalError += EventsControl_FatalErrorAsync;
        EventsControl.Error += EventsControl_Error;
        EventsControl.SendMessage += EventsControl_SendMessage;
    }

    #region Show messages
    private async Task ErrorAsync(string message)
    {
        await DisplayAlert("Error", message, "Ok");
    }

    //private async Task<bool> FatalErrorAsync(string message, Exception ex)
    private bool FatalErrorAsync(string message, Exception ex)
    {
        Exception ex2 = ex;
        while (ex2 != null)
        {
            message += ex2.Message;
            ex2 = ex2.InnerException;
        }
        //string action = await DisplayActionSheet($"Fatal error: {message}. \n\nClose the application?", "Cancel", "Close");
        return true;
    }

    private void EventsControl_SendMessage(string Message)
    {
        //LabelMessages.Text = Message;
    }

    private void EventsControl_Error(string message)
    {
        Task.Run(async () => await ErrorAsync(message)).ConfigureAwait(false).GetAwaiter().GetResult();
    }


    private bool EventsControl_FatalErrorAsync(string message, Exception ex)
    {
        Exception ex2 = ex;
        while (ex2 != null)
        {
            message += ex2.Message;
            ex2 = ex2.InnerException;
        }

        return FatalErrorAsync(message, ex);

        //bool ret = Task.Run(async () => await FatalErrorAsync(message, ex)).ConfigureAwait(false).GetAwaiter().GetResult();
        //if (ret)
        //{
        //    // Show log and exit
        //}
        //return ret;
    }

    #endregion

    #region Themme
    /// <summary>
    /// Chage the current themme
    /// <see href="https://learn.microsoft.com/en-us/dotnet/maui/user-interface/theming?view=net-maui-7.0"/>
    /// </summary>
    private void ChangeThemme()
    {
        ICollection<ResourceDictionary> mergedDictionaries = Microsoft.Maui.Controls.Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            bool useDark = true;
            ResourceDictionary existingDic = null;
            foreach (ResourceDictionary dic in mergedDictionaries)
            {
                if (dic is DarkTheme || (dic.Source != null && dic.Source.OriginalString != null && dic.Source.OriginalString.IndexOf("DarkTheme") > 0))
                {
                    useDark = false;
                    existingDic = dic;
                }
            }
            //if(existingDic != null) mergedDictionaries.Remove(existingDic);
            mergedDictionaries.Clear();
            mergedDictionaries.Add(useDark ? new DarkTheme() : new LightTheme());
        }
    }
    #endregion



    private void ContentPage_Loaded(object sender, EventArgs e)
    {
    }

    private void ContentPage_SizeChanged(object sender, EventArgs e)
    {

    }
}

