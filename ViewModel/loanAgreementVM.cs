using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.EF;

namespace TSMS_2_.ViewModel
{
    public class loanAgreementVM : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly loanAgreementModel _loanAgreementModel = new loanAgreementModel();
        private List<loanAgreementDTO> _loanAgreements;
        private readonly IWindowService _windowService;
        private loanAgreementDTO _selectedLoanAgreement;
        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand DeleteObjCommand { get; }
        public ICommand RefreshObjCommand { get; }
        public ICommand EditObjCommand { get; }

        // Properties for Loan Agreement details
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public long Sum { get; set; }
        public long Percent { get; set; }
        public long StatusId { get; set; }
        public DateTime? Start { get; set; }
        public TimeSpan? End { get; set; }

        public loanAgreementVM()
        {
            _loanAgreements = new List<loanAgreementDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateLoanAgreement);
            UpdObjInDBCommand = new RelayCommand(UpdateLoanAgreement);
            AddObjCommand = new RelayCommand(OpenAddLoanAgreement);
            EditObjCommand = new RelayCommand(OpenEditLoanAgreement);
            DeleteObjCommand = new RelayCommand(DeleteSelectedLoanAgreement);
            RefreshObjCommand = new RelayCommand(RefreshLoanAgreements);
        }

        // List of loan agreements
        public List<loanAgreementDTO> LoanAgreements
        {
            get
            {
                if (_loanAgreements.Count == 0)
                {
                    _loanAgreements = _tableModel.GetLoanAgreementDTOs();
                    OnPropertyChanged(nameof(LoanAgreements));
                }
                return _loanAgreements;
            }
            set
            {
                if (_loanAgreements != value)
                {
                    _loanAgreements = value;
                    OnPropertyChanged(nameof(LoanAgreements));
                }
            }
        }

        public loanAgreementDTO SelectedLoanAgreement
        {
            get => _selectedLoanAgreement;
            set
            {
                _selectedLoanAgreement = value;
                OnPropertyChanged(nameof(SelectedLoanAgreement));
            }
        }

        // Open the window to add a new loan agreement
        public void OpenAddLoanAgreement()
        {
            SelectedLoanAgreement = new loanAgreementDTO(); // Reset the selected loan agreement
            _windowService.OpenWindow("ADDLoanAgreement", this, 1);
        }

        // Open the window to edit an existing loan agreement
        public void OpenEditLoanAgreement()
        {
            if (SelectedLoanAgreement != null)
            {
                SelectedLoanAgreement = new loanAgreementDTO(SelectedLoanAgreement); // Create a copy for editing
                _windowService.OpenWindow("EDITLoanAgreement", this, 2);
            }
        }

        // Refresh the list of loan agreements
        public void RefreshLoanAgreements()
        {
            _loanAgreements = _tableModel.GetLoanAgreementDTOs();
            OnPropertyChanged(nameof(LoanAgreements));
        }

        // Delete the selected loan agreement
        private void DeleteSelectedLoanAgreement()
        {
            if (SelectedLoanAgreement != null)
            {
                _loanAgreementModel.DeleteLoanAgreement(SelectedLoanAgreement.id);
                RefreshLoanAgreements();
            }
        }

        // Create a new loan agreement in the database
        private void CreateLoanAgreement()
        {
            if (SelectedLoanAgreement != null && ValidateLoanAgreement())
            {
                _loanAgreementModel.CreateLoanAgreement(new loanAgreementDTO
                {
                    supplier_id = SupplierId,
                    sum = Sum,
                    percent = Percent,
                    status_id = StatusId,
                    start = Start,
                    end = End
                });
                RefreshLoanAgreements();
            }
        }

        // Update an existing loan agreement in the database
        private void UpdateLoanAgreement()
        {
            if (SelectedLoanAgreement != null && ValidateLoanAgreement())
            {
                _loanAgreementModel.UpdateLoanAgreement(new loanAgreementDTO
                {
                    id = Id,
                    supplier_id = SupplierId,
                    sum = Sum,
                    percent = Percent,
                    status_id = StatusId,
                    start = Start,
                    end = End
                });
                RefreshLoanAgreements();
            }
        }
        // Validation method for loan agreement data
        private bool ValidateLoanAgreement()
        {
            // Implement your validation logic here
            return true;
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}