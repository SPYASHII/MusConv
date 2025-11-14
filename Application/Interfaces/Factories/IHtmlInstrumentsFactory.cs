using Application.Interfaces.Downloaders;
using Application.Interfaces.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Interfaces.Factories
{
    internal interface IHtmlInstrumentsFactory
    {
        IHtmlParser CreateParser(string url);
        IHtmlDownloader CreateDownloader(string url);
    }
}
