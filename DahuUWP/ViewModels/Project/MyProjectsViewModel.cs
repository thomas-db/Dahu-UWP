﻿using DahuUWP.DahuTech.Inputs;
using DahuUWP.Models;
using DahuUWP.Models.ModelManager;
using DahuUWP.Services;
using DahuUWP.Views.Project;
using DahuUWP.Views.Project.Managing;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DahuUWP.ViewModels.Project
{
    public class MyProjectsViewModel : DahuViewModelBase
    {
        public ICommand OnPageLoadedCommand { get; private set; }

        public MyProjectsViewModel(IDataService service)
        {
            dataService = service;
            InitManageProjectButtonBindings();
            OnPageLoadedCommand = new RelayCommand(OnPageLoaded);
        }

        private async void OnPageLoaded()
        {
            ((HomePageViewModel)ViewModelLocator.HomePageViewModel).DahuSpecMenuOptions.SwitchOrActiveCurrentTopBarNodeMenu(typeof(MyProjects));
            UserProjects = null;
            LoadUserProjects();
        }

        private async void LoadUserProjects()
        {
            UserManager userManager = (UserManager)dataService.GetUserManager();

            Dictionary<string, object> userDicoCharge = new Dictionary<string, object>
            {
                { "_token", AppStaticInfo.Account.Token }
            };
            List<Models.Project> projectList = await userManager.ChargeProjects(AppStaticInfo.Account.Uuid, null);
            if (projectList != null)
                UserProjects = new ObservableCollection<Models.Project>(projectList);
            Models.Project proj = new Models.Project()
            {
                Name = "Last!!!",
                Description = "Desc",
                Uuid = "ezf"

            };

            UserProjects.Add(proj);
        }

        private void InitManageProjectButtonBindings()
        {
            ManageProjectButtonBindings = new DahuButtonBindings()
            {
                IsBusy = false,
                Name = "Gérer",
                RedirectedLink = typeof(ManageProject)
            };
        }

        private DahuButtonBindings _manageProjectButtonBindings;
        public DahuButtonBindings ManageProjectButtonBindings
        {
            get { return _manageProjectButtonBindings; }
            set
            {
                NotifyPropertyChanged(ref _manageProjectButtonBindings, value);
            }
        }

        private ObservableCollection<DahuUWP.Models.Project> _userProjects;
        public ObservableCollection<DahuUWP.Models.Project> UserProjects
        {
            get { return _userProjects; }
            set
            {
                NotifyPropertyChanged(ref _userProjects, value);
            }
        }

        private bool NotifyPropertyChanged<T>(ref T variable, T valeur, [CallerMemberName] string nomPropriete = null)
        {
            if (object.Equals(variable, valeur)) return false;

            variable = valeur;
            RaisePropertyChanged(nomPropriete);
            return true;
        }
    }
}
