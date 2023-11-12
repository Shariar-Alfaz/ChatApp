using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Feature.Services.Email.Template
{
    public partial class ForgotPasswordEmail
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public ForgotPasswordEmail(string name,string url)
        {
            Name = name;
            Url = url;
        }
    }
}
