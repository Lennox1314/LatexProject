using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatexProject.GUI.Core;
using LatexProject.GUI.MVVM.View;

namespace LatexProject.GUI.MVVM.ViewModel
{ /// <summary>
/// Uses the relaycommand found in GUI/Core to create instances of all the view models needed for radio button
/// On radio button click changes the view to be desired gui
/// </summary>
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand InstructionsViewCommand { get; set; }
        public RelayCommand CreateTableViewCommand { get; set; }
        public RelayCommand EquationCreationViewCommand { get; set; }


        public HomeViewModel HomeVM { get; set; }

        public InstructionsViewModel InstructionsVM { get; set; }

        public CreateTableViewModel CreateTableVM { get; set; }
        public EquationCreationViewModel EquationCreationVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                _currentView = value;
                onPropertyChanged();
            }
        }
        public MainViewModel()
        { // KDN - instantiates all the view models needed for gui and has them change on radio button click  
            HomeVM = new HomeViewModel();
            InstructionsVM= new InstructionsViewModel();
            CreateTableVM= new CreateTableViewModel();
            EquationCreationVM = new EquationCreationViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => { CurrentView= HomeVM; });
            InstructionsViewCommand = new RelayCommand(o => { CurrentView = InstructionsVM; });
            CreateTableViewCommand = new RelayCommand(o => { CurrentView = CreateTableVM; });
            EquationCreationViewCommand = new RelayCommand(o => { CurrentView=EquationCreationVM; });
        }
    }
}
