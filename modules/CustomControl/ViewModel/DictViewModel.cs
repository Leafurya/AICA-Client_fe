using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utility.Data.Sentence;
using Utility.Data.Word;

namespace CustomControl.ViewModel
{
    public class DictViewModel : INotifyPropertyChanged
    {
        //private List<Meaning> _dictionaryMeans;
        //public List<Meaning> DictionaryMeans
        //{
        //    get => _dictionaryMeans;
        //    set
        //    {
        //        if (_dictionaryMeans != value)
        //        {
        //            _dictionaryMeans = value;
        //            OnPropertyChanged(nameof(DictionaryMeans));
        //        }
        //    }
        //}

        //private string _word;
        //public string Word
        //{
        //    get => _word;
        //    set
        //    {
        //        if (_word != value)
        //        {
        //            _word = value;
        //            OnPropertyChanged(nameof(Word));
        //        }
        //    }
        //}

        public DictViewModel()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
