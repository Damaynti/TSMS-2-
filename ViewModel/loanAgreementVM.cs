using System;
using System.Collections.Generic; // Используем List
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;

namespace TSMS_2_.ViewModel
{
    public class loanAgreementVM : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly loanAgreementModel _loanAgreementModel = new loanAgreementModel();
        private List<loanAgreementDTO> _loanAgreements; // Изменено на List
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
        public DateTime? End { get; set; }

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
            RefreshLoanAgreements();
            LoadLoanAg();
        }
        private void LoadLoanAg()
        {
            var loanAgreements = _tableModel.GetLoanAgreementDTOs();
            LoanAgreements = loanAgreements.ToList();
            OnPropertyChanged(nameof(LoanAgreements));
        }
        // List of loan agreements
        public List<loanAgreementDTO> LoanAgreements
        {
            get => _loanAgreements;
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
                SelectedLoanAgreement = new loanAgreementDTO(SelectedLoanAgreement);
                _windowService.OpenWindow("ADDLoanAgreement", this, 2);
            }
        }

        // Refresh the list of loan agreements
        public void RefreshLoanAgreements()
        {
            var loanAgreements = _tableModel.GetLoanAgreementDTOs();

            // Update statuses for expired loan agreements
            foreach (var loan in loanAgreements)
            {
                if (loan.end.HasValue && loan.end.Value < DateTime.Now && loan.status_id!=1)
                {
                    loan.status_id = 1;
                    _loanAgreementModel.UpdateLoanAgreementStatus(loan);
                }
                else if(loan.end.HasValue && loan.end.Value > DateTime.Now && loan.status_id != 2)
                {
                    loan.status_id = 2;
                    _loanAgreementModel.UpdateLoanAgreementStatus(loan);
                }
            }

            // Assign new data to the list and manually notify the UI
            LoanAgreements = loanAgreements.ToList();
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
                    sup_id = SupplierId,
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
            if (SelectedLoanAgreement != null)
            {
                _loanAgreementModel.UpdateLoanAgreement(new loanAgreementDTO
                {
                    id = SelectedLoanAgreement.id,
                    sup_id = SelectedLoanAgreement.sup_id,
                    sum = SelectedLoanAgreement.sum,
                    percent = SelectedLoanAgreement.percent,
                    status_id = SelectedLoanAgreement.status_id,
                    start = SelectedLoanAgreement.start,
                    end = SelectedLoanAgreement.end
                });
                RefreshLoanAgreements();
                LoadLoanAg();
                var currentWindow = Application.Current.Windows.OfType<ADDLoanAgreement>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
        }

        // Validation method for loan agreement data
        private bool ValidateLoanAgreement()
        {
            if (Sum <= 0)
                return false;

            if (Start == null || End == null || Start >= End)
                return false;

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
