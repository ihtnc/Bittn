using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using LiteDB;
using Bittn.Api.Config;

namespace Bittn.Api.Repositories.AzureRepository
{
    public interface ILiteDbProvider
    {
        LiteDatabase GetDatabase();
    }

    public class LiteDbProvider : ILiteDbProvider
    {
        private readonly BittnConfig _config;

        public LiteDbProvider(IOptions<BittnConfig> options)
        {
            _config = options.Value ?? throw new InvalidOperationException("Missing config.");
        }

        public LiteDatabase GetDatabase()
        {
            return new LiteDatabase(_config.Database);
        }
    }
}