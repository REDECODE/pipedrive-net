﻿using System;
using System.Linq.Expressions;
using PipedriveNet.Dto;
using PipedriveNet.Endpoints;

namespace PipedriveNet
{

	public class PipedriveClient : PipedriveClient<PersonDto, PipelineDto, StageDto, DealDto>
	{
		public PipedriveClient(string apiKey) : base(apiKey)
		{
		}
	}

    public class PipedriveClient<TPerson, TPipeline, TStage, TDeal> 
        where TPerson : PersonDto 
        where TPipeline : PipelineDto
        where TStage : StageDto
        where TDeal : DealDto
    {
		private readonly ContractResolver _resolver = new ContractResolver();

		public PersonsEndpoint<TPerson> Persons { get; private set; }
        public PipelinesEndpoint<TPipeline> Pipelines { get; private set; }
        public StagesEndpoint<TStage> Stages { get; private set; }
        public DealsEndpoint<TDeal> Deals { get; private set; } 

	    public PipedriveClient(string apiKey)
	    {
		    var client = new ApiClient(apiKey, _resolver);
		    Persons = new PersonsEndpoint<TPerson>(client);
            Pipelines = new PipelinesEndpoint<TPipeline>(client);
            Stages = new StagesEndpoint<TStage>(client);
	        Deals = new DealsEndpoint<TDeal>(client);
	    }

        public class CustomFieldConfigurator<T>
        {
            private readonly ContractResolver _resolver;

            internal CustomFieldConfigurator(ContractResolver resolver)
            {
                _resolver = resolver;
            }

            public CustomFieldConfigurator<T> Field<TField>(Expression<Func<T, TField>> field, string key)
            {
                _resolver.Register(field.ExtractProperty(), key);
                return this;
            }
        }

        public CustomFieldConfigurator<T> Configure<T>()
        {
            return new CustomFieldConfigurator<T>(_resolver);
        }
    }
}