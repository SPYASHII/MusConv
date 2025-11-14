using Application.Interfaces.Downloaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Downloaders
{
    internal class AmazonHtmlDownloader : IHtmlDownloader
    {
        public async Task<string> DownloadHtmlAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}
