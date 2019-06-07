using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Flurl;
using Flurl.Http;
using Xamarin.Forms;

namespace MultPlatProject
{
    public class BooksViewModel :
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ErrorResponse> RequestFailed;

        IEnumerable<Book> mBooks;
        public IEnumerable<Book> Books
        {
            get => mBooks;
            set
            {
                if (!Equals(mBooks, value))
                {
                    mBooks = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Books"));
                }
            }
        }

        bool _IsLoading = false;

        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                if (!Equals(_IsLoading, value))
                {
                    _IsLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLoading"));
                }
            }
        }

        public ICommand GetCommand { get; private set; }

        public string TitleEntry { get; set; }

        public BooksViewModel()
        {
            async void execute()
            {
                try
                {
                    IsLoading = true;

                    Books = await

                        Constants.BaseServiceUrl

                                 .AppendPathSegment(typeof(Book).Name)

                                 .GetJsonAsync<List<Book>>();
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<ErrorResponse>();

                    RequestFailed?.Invoke(this, error);
                }
                finally
                {
                    IsLoading = false;
                }
            }

            GetCommand = new Command(execute);
        }
    }
}
