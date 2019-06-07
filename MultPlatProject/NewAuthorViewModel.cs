using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MultPlatProject
{
    public class NewAuthorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ErrorResponse> RequestFailed;

        public event EventHandler AuthorAdded;

        string mName;

        public string Name
        {
            get => mName;
            set
            {
                if (!Equals(mName, value))
                {
                    mName = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        
        public ICommand PostCommand { get; private set; }

        public NewAuthorViewModel()
        {
            async void execute()
            {
                try
                {
                    var author = new Author() { Name = mName };
                    var httpResponse = await

                        Constants.BaseServiceUrl

                                 .AppendPathSegment(typeof(Author).Name)

                                 .PostJsonAsync(author);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        AuthorAdded?.Invoke(this, new EventArgs());
                    }
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<ErrorResponse>();

                    RequestFailed?.Invoke(this, error);
                }
            }

            PostCommand = new Command(execute);
        }
    }
}
