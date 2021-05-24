using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Question : BindableBase
    {
        public int Id { get; set; }
        public string Interrogation { get; set; }
        public Survey Survey { get; set; }
        public List<Answer> Answers { get; set; }
        public int Quantity { get; set; }
        
        public string QuestionType { get; set; }

        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set => SetProperty(ref _checked, value);
        }
        public Answer SelectedAnswer { get; set; }
    }
}
