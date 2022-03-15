using Sitecore.Shell.Framework.Commands;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ItemPathLogger
{
    public class ItemPathLogger : Command
    {
        public override void Execute(CommandContext context)
        {
            // Sitecore.Context.ClientPage.ClientResponse.Alert("This is my custom button!");
            try
            {
                if (context.Items.Length > 0)
                {
                    var item = context.Items[0];

                    string datafolder = Sitecore.Configuration.Settings.DataFolder;
                    string rootFolder = System.Web.HttpContext.Current.Server.MapPath("~") + datafolder?.Substring(1, datafolder.Length - 1) + "\\" + Sitecore.Configuration.Settings.GetSetting("itemLogFolder");

                    if (Directory.Exists(rootFolder))
                    {
                        string filePath = rootFolder + "\\" + Sitecore.Configuration.Settings.GetSetting("itemLogFileName") + "." + System.DateTime.Now.ToString("dd-MM-yyyy") + ".txt"; // path to file
                        string content = "Item " + item.Paths.FullPath + " changed on timestamp:" + System.DateTime.Now.ToString();
                        var task = WriteFileAsync(filePath, content);

                        Sitecore.Context.ClientPage.ClientResponse.Alert("Entry saved for item: " + item.Paths.FullPath);
                    }
                    else
                    {
                        Sitecore.Context.ClientPage.ClientResponse.Alert("Root folder does not exist");
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception while updating item change log" + ex.Message, this);
            }
        }

        static async Task WriteFileAsync(string file, string content)
        {
            using (StreamWriter outputFile = new StreamWriter(file, true))
            {
                await outputFile.WriteAsync(content);
                await outputFile.WriteLineAsync();
            }
        }
    }
}
