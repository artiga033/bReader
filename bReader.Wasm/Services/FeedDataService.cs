using Blazor.Extensions.Storage;
using bReader.Shared.Models;
using bReader.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bReader.Wasm.Services
{
    public class FeedDataService //: IFeedService
    {
        private readonly LocalStorage _localStorage;

        const string KEY_LINKS = "subscribeLinks";

       

        public FeedDataService(LocalStorage localStorage)
        {
            this._localStorage = localStorage;
        }
    }
}
