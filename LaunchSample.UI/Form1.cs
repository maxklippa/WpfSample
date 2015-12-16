using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using LaunchSample.BLL.Services;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.UI
{
    public partial class Form1 : Form
    {
        private readonly LaunchService _launchService;

        public Form1()
        {
            InitializeComponent();
            _launchService = new LaunchService();

            statusComboBox.Items.Add("All");
            foreach (var status in Enum.GetValues(typeof(LaunchStatus)))
            {
                statusComboBox.Items.Add(status.ToString());
            }
            statusComboBox.SelectedIndex = 0;

            var towns = _launchService.GetAll().Select(l => l.City);
            var cities = new HashSet<string>(towns);
            cityComboBox.Items.Add("All");
            foreach (var city in cities)
            {
                cityComboBox.Items.Add(city);
            }
            cityComboBox.SelectedIndex = 0;

            fromDateTimePicker.Value = DateTime.Now.AddYears(-5);

            filterGridView.Columns[0].Visible = false;
            filterGridView.AllowUserToAddRows = false;
            filterGridView.ReadOnly = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            launchBindingSource.DataSource = new ObservableCollection<LaunchDto>(_launchService.GetAll()).ToBindingList();
        }
        
        private void launchBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();

            var list = ((IEnumerable)launchDataGridView.DataSource).Cast<LaunchDto>().ToList();
            _launchService.SaveAll(list);

            launchDataGridView.Refresh();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            UpdateListingGrid();
        }

        private void UpdateListingGrid()
        {
            var city = cityComboBox.SelectedItem == "All" || cityComboBox.SelectedItem == null ? null : cityComboBox.SelectedItem.ToString();
            var from = fromDateTimePicker.Value;
            var to = toDateTimePicker.Value;
            var status = statusComboBox.SelectedItem == "All"
                ? (LaunchStatus?) null
                : (LaunchStatus)Enum.Parse(typeof(LaunchStatus), statusComboBox.SelectedItem.ToString(), true);
            filterGridView.DataSource =
                new ObservableCollection<LaunchDto>(_launchService.GetAll(city, @from, to, status)).ToBindingList();
        }

        private void cityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListingGrid();
        }

        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListingGrid();
        }

        private void fromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateListingGrid();
        }

        private void toDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateListingGrid();
        }


    }
}
