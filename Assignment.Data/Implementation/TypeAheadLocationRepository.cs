using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Assignment.Core;
using RestSharp;

namespace Assignment.Data
{
    public class TypeAheadLocationRepository: ITypeAheadLocationRepository
    {
        private readonly RestClient _client;
        public TypeAheadLocationRepository()
        {
            _client = new RestClient("https://sprint-api.newhomesource.com/api/v2");
        }

        public async Task<ICollection<Location>> GetAllLocationsAsync(LocationParameters param)
        {
            _client.ThrowOnDeserializationError = true;
            var request = new RestRequest("Typeahead/Locations", Method.GET ,DataFormat.Json);

            request.AddParameter("partnerid", param.PartnerId);
            request.AddParameter("searchTerm", param.SearchTerm);
            if(!string.IsNullOrEmpty(param.Types))
            {
                request.AddParameter("types", param.Types);
            }
            if(param.IncludeAll.HasValue)
            {
                request.AddParameter("includeAll", param.IncludeAll);
            }
            
            var response = await _client.ExecuteAsync<List<Location>>(request);
            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.Content);
            }
            return response.Data;
        }
    }
}