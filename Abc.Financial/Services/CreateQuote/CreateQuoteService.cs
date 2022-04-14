using Abc.Financial.Models;
using Abc.Financial.Services.Shared;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abc.Financial.Services.CreateQuote
{
    public class CreateQuoteService : FinancialServiceBase<CreateQuoteRequest, CreateQuoteResponse>
    {
        private readonly IInstrumentReader instrumentReader;
        private readonly List<Instrument> instruments = new List<Instrument>();

        public CreateQuoteService(IValidator<CreateQuoteRequest> validator, IInstrumentReader instrumentReader)
            : base(validator)
        {
            this.instrumentReader = instrumentReader;
        }

        protected override async Task<CreateQuoteResponse> RunAsync()
        {
            foreach (var identifier in this.Request.Identifiers)
            {
                Instrument instrument = await instrumentReader.ReadAsync(identifier);
                if (instrument != null)
                {
                    this.instruments.Add(instrument);
                }
            }

            if (instruments.Count == 0)
            {
                return this.Fail(FinancialResponseCode.NoInstrumentsFound);
            }

            return new CreateQuoteResponse
            {
                Succeeded = true,
                Instruments = instruments
            };
        }
    }

    public class CreateQuoteResponse : FinancialResponse
    {
        public IEnumerable<Instrument> Instruments { get; set; }

    }

    public class CreateQuoteRequest : FinancialRequest
    {
        public string[] Identifiers { get; set; }
    }

    public class CreateQuoteRequestValidator : FinancialRequestValidator<CreateQuoteRequest>
    {
        public CreateQuoteRequestValidator(ITenantChecker tenanChecker) 
            : base(tenanChecker)
        {
        }
    }
}
