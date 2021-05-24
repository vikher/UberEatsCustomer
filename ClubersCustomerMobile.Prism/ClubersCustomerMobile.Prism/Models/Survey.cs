using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Survey : BindableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Evaluation Evaluation { get; set; }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }
    }
}
