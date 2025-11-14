using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Downloaders;
using Application.Enums;
using Application.Interfaces.Downloaders;
using Application.Interfaces.Factories;
using Application.Interfaces.Parsers;
using Application.Parsers;

namespace Application.Factories
{
    internal class SimpleHtmlParserFactory : IHtmlInstrumentsFactory
    {
        Dictionary<string, Domens> _domens;
        SimpleHtmlParserFactory(Dictionary<string, Domens> domens)
        {
            _domens = domens;
        }
        #region Parsers
        private static IHtmlParser? _amazonParser;
        private static IHtmlParser AmazonParser
        {
            get
            {
                _amazonParser ??= new AmazonHtmlParser();

                return _amazonParser;
            }
        }

        Dictionary<Domens, IHtmlParser> _parsersForDomens = new()
        {
            { Domens.Amazon, AmazonParser}
        };
        #endregion
        #region Downloaders
        private static IHtmlDownloader? _amazonDownloader;
        private static IHtmlDownloader AmazonDownloader
        {
            get
            {
                _amazonDownloader ??= new AmazonHtmlDownloader();

                return _amazonDownloader;
            }
        }
        Dictionary<Domens, IHtmlDownloader> _downloadersForDomens = new()
        {
            { Domens.Amazon, AmazonDownloader}
        };
        #endregion

        public IHtmlDownloader CreateDownloader(string url)
        {
            Domens domen = SelectAndCheckDomen(url);

            return GetHtmlDownloader(domen);
        }
        public IHtmlParser CreateParser(string url)
        {
            Domens domen = SelectAndCheckDomen(url);

            return GetHtmlParser(domen);
        }

        private IHtmlDownloader GetHtmlDownloader(Domens domen)
        {
            return _downloadersForDomens[domen];
        }
        private IHtmlParser GetHtmlParser(Domens domen)
        {
            return _parsersForDomens[domen];
        }

        private Domens SelectAndCheckDomen(string url)
        {
            Domens domen = SelectDomen(url);

            if (domen is Domens.None)
            {
                string message = GetInvalidDomenMessage(url);

                throw new ArgumentException(message);
            }

            return domen;
        }
        private Domens SelectDomen(string url)
        {
            var domen = Domens.None;

            var predicate = 
                (KeyValuePair<string, Domens> k)
                => url.Contains(k.Key);

            bool isValid = _domens.Any(predicate);

            if (isValid)
            {
                domen = _domens
                .FirstOrDefault
                (predicate).Value;
            }

            return domen;
        }

        private string GetInvalidDomenMessage(string url)
        {

            var message = new StringBuilder("Can`t scrape data from ");

            message = message.Append(url + ".");

            message.AppendLine("Available domens:");
            message.AppendLine();

            foreach (var key in _domens.Keys)
            {
                message.Append(key + ", ");
            }

            return message.ToString();
        }
    }
}
