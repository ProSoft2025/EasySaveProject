using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EasySaveV1;
using EasySaveV2.Services;

namespace EasySaveV2
{
    public partial class JobProcess : UserControl
    {
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;

        public JobProcess(EasySaveApp manager, MessageService messageService)
        {
            this.manager = manager;
            this.messageService = messageService;
            InitializeComponent();
            LoadProcesses();
            manager.MonitorProcesses();
        }

        private async void OnAddProcessClick(object sender, RoutedEventArgs e)
        {
            var processTextBox = this.FindControl<TextBox>("ProcessTextBox");
            if (processTextBox == null)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "TextBox not find");
                return;
            }
            var process = processTextBox.Text;
            if (string.IsNullOrEmpty(process))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Process invalid");
                return;
            }
            else if (manager.ProcessesToMonitor.Contains(process))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Process exist");
                return;
            }

            manager.AddProcess(process);
            LoadProcesses();
            processTextBox.Clear();
        }

        private void LoadProcesses()
        {
            var processListBox = this.FindControl<ListBox>("ProcessListBox");
            if (processListBox != null)
            {
                processListBox.ItemsSource = manager.ProcessesToMonitor.ToList();
            }
        }

       

        private async void OnRemoveProcessClick(object sender, RoutedEventArgs e)
        {
            var processTextBox = this.FindControl<TextBox>("ProcessTextBox");
            if (processTextBox == null)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "TextBox non trouvé");
                return;
            }
            var process = processTextBox.Text;
            if (string.IsNullOrEmpty(process) || !manager.ProcessesToMonitor.Contains(process))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Process invalid");
                return;
            }

            manager.RemoveProcess(process);
            LoadProcesses();
            processTextBox.Clear();
        }

        private void ProcessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var processListBox = this.FindControl<ListBox>("ProcessListBox");
            var processTextBox = this.FindControl<TextBox>("ProcessTextBox");

            if (processListBox.SelectedItem != null)
            {
                processTextBox.Text = processListBox.SelectedItem.ToString();
            }
        }

    
    }
}
