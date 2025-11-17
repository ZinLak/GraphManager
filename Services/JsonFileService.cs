using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using GraphManager.Models;
using System.Net.Http.Json;

namespace GraphManager.Services
{
    public class JsonFileService
    {
        public void Save(TaskProject project)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "GraphProject (*.json)|*.json";
            if (dlg.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(project, Formatting.Indented);
                File.WriteAllText(dlg.FileName, json);

            }
        }
        public TaskProject Load()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "GraphProject (*.json)|*.json";
            if (dlg.ShowDialog() == true)
            {
                var json = File.ReadAllText(dlg.FileName);
                var project = JsonConvert.DeserializeObject<TaskProject>(json);
                return project ?? new TaskProject();
            }
            return new TaskProject();
        }
    }
}
