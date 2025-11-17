using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager.Models
{
    public class TaskProject
    {
        public ObservableCollection<TaskBlock> Blocks { get; set; } = new ObservableCollection<TaskBlock>();
        public ObservableCollection<TaskLink> Links { get; set; } = new ObservableCollection<TaskLink>();

        public string NameProject { get; set; } = "Без названия";
    }
}
